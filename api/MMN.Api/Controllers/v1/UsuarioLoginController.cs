using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Jwt;
using MMN.Util.Model;
using MMN.Util.Translation;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.UsuarioLoginViewModel;

namespace MMN.Api.Controllers.v1
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioLoginController : ControllerBase
    {
        private readonly IUsuarioNegocio _negocio;
        private readonly IGraduacaoNegocio _graduacoNegocio;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly IParceiroNegocio _parceiroNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly ICache _cache;
        private readonly ITokenUtil _token;
        //private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly TokenManagement _tokenManagement;
        public bool IsDev { get; set; }
        private readonly ILocation _location;

        public UsuarioLoginController(
            IUsuarioNegocio negocio,
            IOptions<AppSettings> appSettings,
            ILocation location,
            ICache cache,
            IConfiguracaoNegocio configNegocio,
            IParceiroNegocio parceiroNegocio,
            ITokenUtil token,
            IGraduacaoNegocio graduacoNegocio,
            IOptions<TokenManagement> tokenManagement,
            IHostingEnvironment env,
            ICredenciamentoNegocio credenciamentoNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio)
        {
            _negocio = negocio;
            _appSettings = appSettings.Value;
            _location = location;
            _cache = cache;
            _configNegocio = configNegocio;
            _parceiroNegocio = parceiroNegocio;
            _token = token;
            _tokenManagement = tokenManagement.Value;
            _graduacoNegocio = graduacoNegocio;
            IsDev = env.IsDevelopment();
            _credenciamentoNegocio = credenciamentoNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
        }

        private async Task<IActionResult> RetornoAutenticacao(UsuarioViewModel usuario, Parceiro parceiro, OrigemLoginEnum? origem)
        {
            bool comerciante = false;


            if (!usuario.EmailConfirmado)
            {
                throw new UnauthorizedException("email_nao_confirmado");
            }

            var produtoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (produtoAtivo == null)
            {
                _usuarioProdutoNegocio.Insert(new UsuarioProdutoViewModel
                {
                    IdProduto = 1,
                    IdUsuario = usuario.IdUsuario,
                    IdPedido = 1,
                    DataVinculo = DateTime.UtcNow,
                    Ativo = true,
                });
            }

            var tokenProperties = await _negocio.GenerateTokenAndRefreshToken(usuario, usuario.Grupo.Descricao == "Comerciante");

            if (usuario.Grupo.Descricao == "Comerciante")
            {
                comerciante = true;

                var credenciamento = usuario.Credenciamento;

                switch (credenciamento?.Status)
                {
                    case StatusCredenciamento.Aprovado:
                        // Ok
                        break;
                    case StatusCredenciamento.Reprovado:
                        throw new UnauthorizedException("credenciamento_reprovado");
                    default:
                        throw new UnauthorizedException("credenciamento_nao_aprovado");
                }
            }

            if (parceiro != null)
            {
                if (origem == OrigemLoginEnum.Aplicativo)
                {
                    usuario.Nome = parceiro.Nome;
                    usuario.Grupo.Descricao = "Parceiro";
                    usuario.Perfil = 'P';
                }
                else
                {
                    throw new UnauthorizedException("login_incorreto");
                }
            }

            return Ok(new
            {
                Id = parceiro?.IdParceiro.ToString() ?? usuario.IdUsuario.ToString(),
                Username = usuario.Nome,
                usuario.Login,
                usuario.Email,
                tokenProperties.Token,
                tokenProperties.RefreshToken,
                usuario.Grupo.Descricao,
                usuario.Cultura,
                usuario.UrlImg,
                usuario.TermosDeAceite,
                Admin = usuario.IdGrupo == 1 && usuario.Perfil != 'P',
                DadosCompletos = usuario.UsuarioEndereco.Count > 0,
                comerciante,
                usuario.Empreendedor,
                usuario.Perfil,
                usuario.Credenciamentos.FirstOrDefault(c => c.IdUsuario == usuario.IdUsuario)?.LogoUrl,
                produtoAtivo.AssinaturaHabilitada,
                produtoAtivo.Produto.IdProduto,
                habilitacao = produtoAtivo.Produto.Nome,
                usuario.PreCadastro,
                usuario.LinkAssistenteVirtual
                        });
        }

        [HttpPost]
        [Route("autenticacao")]
        [EnableRateLimiting("auth-limit")]
        public async Task<IActionResult> Autenticacao([FromBody] Logar login)
        {
            var usuario = _negocio.Autenticacao(login.Login, login.Senha, out var parceiro, !IsDev);

            return await RetornoAutenticacao(usuario, parceiro, login.Origem);
        }

        [HttpPost]
        [Route("autenticacaoGoogleCredential")]
        [EnableRateLimiting("auth-limit")]
        public async Task<IActionResult> AutenticacaoGoogleCredential([FromBody] LogarGoogleCredentialViewModel logar)
        {
            var (usuario, parceiro) = await _negocio.AutenticacaoGoogleCredentialAsync(logar.Credential);

            return await RetornoAutenticacao(usuario, parceiro, logar.Origem);
        }

        [HttpPost]
        [Route("autenticacaoAppleCredential")]
        [EnableRateLimiting("auth-limit")]
        public async Task<IActionResult> AutenticacaoAppleCredential([FromBody] LogarAppleCredentialViewModel logar)
        {
            var (usuario, parceiro) = await _negocio.AutenticacaoAppleCredentialAsync(
                logar.IdentityToken,
                logar.Email,
                logar.FullName);

            return await RetornoAutenticacao(usuario, parceiro, logar.Origem);
        }

        [HttpGet]
        [Route("esqueciMinhaSenha/{login}")]
        [EnableRateLimiting("auth-limit")]
        public IActionResult EsqueciMinhaSenha(string login)
        {
            var usuario = _negocio.BuscarLoginOuEmail(login);

            if (usuario == null)
            {
                throw new PadraoException("usuario_nao_encontrado");
            }

            if (usuario.Bloqueado && usuario.TentativasIncorretas == 0)
                throw new UnauthorizedException("usuario_bloqueado");

            var webToken = _token.ConstruirToken(usuario);

            EnviarEmailEsqueciMinhaSenha(usuario, webToken);

            return Ok(new
            {
                message = _location.GetTranslation("EnvioEmailEsqueciMinhaSenha")
            });
        }
        [HttpPost]
        [Route("abrirToken/{token}")]
        public IActionResult AbrirToken(string token)
        {
            var retorno = _token.ValidarToken(token);
            return Ok(retorno);
        }

        [HttpGet]
        [Route("redirect")]
        public async Task<IActionResult> RedirectTokenAsync()
        {
            var token = HttpContext.Request.Headers["Authorization"];

            var validToken = _token.ValidarToken(token);
            var usuario = _negocio.GetById(new Guid(validToken.IdUsuario), new[] {"Grupo", "Credenciamento"});

            if (usuario is null)
                throw new PadraoException("usuario_nao_encontrado");

            bool comerciante = false;

            var produtoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (produtoAtivo == null)
            {
                _usuarioProdutoNegocio.Insert(new UsuarioProdutoViewModel
                {
                    IdProduto = 1,
                    IdUsuario = usuario.IdUsuario,
                    IdPedido = 1,
                    DataVinculo = DateTime.UtcNow,
                    Ativo = true,
                });
            };

            var tokenProperties = await _negocio.GenerateTokenAndRefreshToken(usuario, usuario.Perfil == 'C');

            if (usuario.Perfil == 'C')
            {
                comerciante = true;

                var credenciamento = usuario.Credenciamento;

                switch (credenciamento?.Status)
                {
                    case StatusCredenciamento.Aprovado:
                        // Ok
                        break;
                    case StatusCredenciamento.Reprovado:
                        throw new UnauthorizedException("credenciamento_reprovado");
                    default:
                        throw new UnauthorizedException("credenciamento_nao_aprovado");
                }
            }


            return Ok(new
            {
                Id = usuario.IdUsuario.ToString(),
                Username = usuario.Nome,
                usuario.Login,
                usuario.Email,
                tokenProperties.Token,
                tokenProperties.RefreshToken,
                usuario.Grupo.Descricao,
                usuario.Cultura,
                usuario.UrlImg,
                usuario.TermosDeAceite,
                Admin = usuario.IdGrupo == 1 && usuario.Perfil != 'P',
                DadosCompletos = usuario.UsuarioEndereco.Count > 0,
                comerciante,
                usuario.Empreendedor,
                usuario.Perfil,
                usuario.Credenciamentos.FirstOrDefault(c => c.IdUsuario == usuario.IdUsuario)?.LogoUrl,
                produtoAtivo.AssinaturaHabilitada,
                usuario.PreCadastro,
                                usuario.LinkAssistenteVirtual
                        });            
        }

        [HttpPost]
        [Route("confirmarConta/{token}")]
        public IActionResult ConfirmarConta(string token)
        {
            var retorno = _token.ValidarToken(token);
            
            _negocio.ValidarContaUsuario(Guid.Parse(retorno.IdUsuario));
            
            var credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == Guid.Parse(retorno.IdUsuario));

            if (credenciamento != null)
            {
                credenciamento.Status = StatusCredenciamento.Pendente;
                _credenciamentoNegocio.Update(credenciamento);
            }

            // TODO: Enviar email ao indicador

            return Ok(retorno);
        }

        [HttpPost]
        [Route("alterarSenha")]
        public IActionResult AlterarSenha([FromBody] EsqueciMinhaSenha modelLogin)
        {
            EsqueciMinhaSenhaValidator validator = new EsqueciMinhaSenhaValidator();

            var result = validator.Validate(modelLogin);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            if (string.IsNullOrEmpty(modelLogin.Token))
            {
                throw new PadraoException("token_invalido");
            }

            var webToken = System.Net.WebUtility.UrlDecode(modelLogin.Token);
            var stream = webToken;
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            //var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
            var idUsuario = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;
            var dataExpira = tokenS.Claims.First(claim => claim.Type == "Exp").Value;

            if (DateTime.UtcNow.HorarioBrasilia() > Convert.ToDateTime(dataExpira).HorarioBrasilia())
            {
                throw new UnauthorizedException("token_invalido");
            }

            var guidId = new Guid(idUsuario);

            var usuario = _negocio.GetById(guidId);
            if (usuario.Bloqueado && usuario.TentativasIncorretas == 0)
                throw new UnauthorizedException("usuario_bloqueado");

            var retorno = _negocio.AlterarSenha(idUsuario, modelLogin.Senha, modelLogin.SenhaConfirmada);

            if (!retorno)
                throw new Exception();

            return Ok(new
            {
                message = _location.GetTranslation("SucessoAlterarSenha")
            });
        }

        [HttpPost]
        [Route("reenviarEmailConfirmacao")]
        public async Task<IActionResult> ReenviarEmailConfirmacao(UsuarioViewModel model)
        {
            await EnviarEmailConfirmacao(model.Login, $"Big Cash - Confirmação de email");
            return Ok(new
            {
                message = _location.GetTranslation("Cadastro_ReenviarEmailConfirmacao")
            });
        }

        [HttpPost, Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestViewModel request)
        {
            var authResponse = await _negocio.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                //throw new Exception($"ErrorType:{authResponse.ErrorType}\nError:{authResponse.Error}");
            }

            return Ok(new
            {
                authResponse.Token,
                authResponse.RefreshToken
            });
        }

        private void EnviarEmailEsqueciMinhaSenha(UsuarioViewModel usuario, string webToken)
        {
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = $"{_appSettings.FromName} - Solicitação de alteração de senha",
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var rootSite = _configNegocio.BuscarRootSite().Valor;

            var link = rootSite + _appSettings.RootSiteResetPassword + webToken;

            link = link.Replace("quantashop.com.br", "escritorio.quantashop.com.br");

            var body = new Dictionary<string, string>
            {
                { "#NOMECLIENTE#", objectEmail.DestinationName },
                { "#URL", link}
            };

            var ret = new EmailUtilitis().EnviarEmail(body, _appSettings.EsqueciMinhaSenha, _appSettings.TemplatePai, objectEmail);
        }

        private async Task EnviarEmailConfirmacao(string login, string titulo)
        {
            var usuario = _negocio.BuscarLoginOuEmail(login);
            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = titulo,
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var webToken = _token.ConstruirToken(usuario);
            var rootSite = _configNegocio.BuscarRootSite().Valor;
            var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;

            var body = new Dictionary<string, string>
                {
                    { "#NOMECLIENTE#", objectEmail.DestinationName },
                    { "#URL", link}
                };

            var emailUtil = new EmailUtilitis();
            await emailUtil.EnviarEmail(body, _appSettings.ConfirmarEmail, _appSettings.TemplatePai, objectEmail);
        }

        private async Task EnviarEmailAprovacao(string login, string titulo)
        {
            var usuario = _negocio.BuscarLoginOuEmail(login);
            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = titulo,
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var webToken = _token.ConstruirToken(usuario);
            var rootSite = _configNegocio.BuscarRootSite().Valor;
            var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;

            var body = new Dictionary<string, string>
                {
                    { "#NOMECLIENTE#", objectEmail.DestinationName },
                    { "#URL", link}
                };

            var emailUtil = new EmailUtilitis();
            await emailUtil.EnviarEmail(body, _appSettings.ConfirmarEmail, _appSettings.TemplatePai, objectEmail);
        }
    }
}