using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Translation;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UsuarioController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _negocio;
        private readonly IUsuarioEnderecoNegocio _negocioEndereco;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly ILocation _location;
        private readonly AppSettings _appSettings;
        private readonly ITokenUtil _token;
        private readonly ICache _cache;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IPreCadastroNegocio _preCadastroNegocio;
        private readonly WhatsAppService _whatsAppService;
        private readonly IBotConversaService _botConversa;
        private readonly IUsersService _usersService;
        private readonly ISaqueNegocio _saqueNegocio;

        public UsuarioController(IUsuarioNegocio negocio,
            ILocation location,
            ITokenUtil token,
            ICache cache,
            IConfiguracaoNegocio configNegocio,
            IUsuarioEnderecoNegocio negocioEndereco,
            ILancamentoNegocio lancamentoNegocio,
            IOptions<AppSettings> appSettings,
            IProceduresRepositorio proceduresRepositorio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IPreCadastroNegocio preCadastroNegocio,
            WhatsAppService whatsAppService,
            IUsersService usersService,
            ISaqueNegocio saqueNegocio,
            IBotConversaService botConversa)
        {
            _negocio = negocio;
            _location = location;
            _token = token;
            _cache = cache;
            _configNegocio = configNegocio;
            _negocioEndereco = negocioEndereco;
            _appSettings = appSettings.Value;
            _lancamentoNegocio = lancamentoNegocio;
            _proceduresRepositorio = proceduresRepositorio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _preCadastroNegocio = preCadastroNegocio;
            _whatsAppService = whatsAppService;
            _usersService = usersService;
            _saqueNegocio = saqueNegocio;
            _botConversa = botConversa;
        }

        [HttpPost]
        [Route("aceitarcookies")]
        public IActionResult AceitarCookies()
        {
            var usuario = _negocio.FirstNoTracking(u => u.IdUsuario == IdUsuarioLogado);

            usuario.TermosDeAceite = true;
            _negocio.Update(usuario);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("cadastrar")]
        [EnableRateLimiting("auth-limit")]
        public IActionResult Cadastrar(UsuarioCadastroPWAViewModel model)
        {
            try
            {
                var myHeaderValue = Request.Headers[_appSettings.HeaderKey];

                if (myHeaderValue.Count == 0) return Unauthorized();
                string myHeader = myHeaderValue.ToString();

                if (myHeader != _appSettings.HeaderSecret)
                    return Unauthorized(new
                    {
                        status = 401,
                        message = "Unauthorized",
                        errors = Array.Empty<string>()
                    });

                UsuarioCadastroPWAViewModelValidator validator = new UsuarioCadastroPWAViewModelValidator();
                var result = validator.Validate(model);

                // A Tayon não preenche o campo login no fluxo deles
                model.Login = model.Documento;

                if (model.Celular.StartsWith("55"))
                    model.Celular = model.Celular.Replace("55", "");

                if (!result.IsValid)
                {
                    // Para atender a necessidade da Tayon

                    //"email_requerido"
                    //"email_invalido"
                    //"email_nao_permitido"
                    //"email_requisito" 
                    //"senha_requisitos"
                    //"patrocinador_requerido"
                    //"cpf_cnpj_invalido"

                    #region Tayon
                    if (result.Errors.Any(x => x.ErrorMessage == "email_requerido"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " email_requerido",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "email_invalido"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " email_invalido",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "email_nao_permitido"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " patrocinador_nao_encontrado",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "email_requisito"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " email_requisito",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "senha_requisitos"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " senha_requisitos",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "patrocinador_requerido"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " patrocinador_nao_encontrado",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }

                    if (result.Errors.Any(x => x.ErrorMessage == "cpf_cnpj_invalido"))
                    {
                        return BadRequest(new
                        {
                            status = 400,
                            code = " patrocinador_nao_encontrado",
                            message = "Bad request",
                            errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message)
                        });
                    }
                    #endregion Tayon
                }

                var usuarioPai = _negocio.GetByLoginOrEmail(model.LoginPatrocinador);

                if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " patrocinador_nao_encontrado",
                        message = "Bad request",
                        errors = new[] { "Patrocinador não encontrado" }
                    });
                }

                if (model.Login.TemCaracterEspecial())
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " login_contem_caracteres_especiais",
                        message = "Bad request",
                        errors = new[] { "O login deve conter apenas letras" }
                    });
                }

                var usuarioLogin = _negocio.GetByLoginOrEmail(model.Login);

                if (usuarioLogin != null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " login_em_uso",
                        message = "Bad request",
                        errors = new[] { "Login indisponível" }
                    });
                }

                usuarioLogin = _negocio.GetByLoginOrEmail(model.Email);

                if (usuarioLogin != null)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " email_em_uso",
                        message = "Bad request",
                        errors = new[] { "Já existe uma conta cadastrada com este endereço de email" }
                    });
                }

                if (_negocio.Get(u => u.Celular == model.Celular).Any())
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " telefone_em_uso",
                        message = "Bad request",
                        errors = new[] { "Já existe uma conta cadastrada com este número de telefone" }
                    });
                }

                if (_negocio.Get(u => u.Documento == model.Documento).Any())
                {
                    return BadRequest(new
                    {
                        status = 400,
                        code = " cpf_cnpj_em_uso",
                        message = "Bad request",
                        errors = new[] { "Já existe uma conta cadastrada com este número de documento" }
                    });
                }

                var usuario = _negocio.CadastroPWA(model);

                EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email").Wait();

                //var phone = "55" + usuario.Celular;
                //var message = $"Olá, {usuario.Nome ?? usuario.Email}! \nBem-vindo ao Quanta Shop! 🙌 \nFicamos felizes que você tenha se juntado à nossa comunidade 💪 \nPara garantir o seu sucesso siga estes passos simples através da nossa plataforma 🖥 \n\n1️⃣ Confirme seu e-mail ✉ \n2️⃣ Complete seus dados 💯 \n3️⃣ Faça o tour pela plataforma e assista aos vídeos 🎞 \n4️⃣ Procure um parceiro online ou próximo de você, realize a primeira compra e receba seu primeiro cashback 🤑 \n5️⃣ Salve nosso contato na sua agenda  \n\nAbraços,\nEquipe Quanta Shop";

                //_whatsAppService.SendMessageAsync(phone, message).Wait();

                var phone = "+55" + usuario.Celular;
                var username = usuario.Nome.Split(' ');
                var firstName = username.Length > 0 ? username[0] : string.Empty;
                var lastName = username.Length > 1 ? username[1] : string.Empty;

                if (string.IsNullOrEmpty(firstName))
                    firstName = usuario.Email;

                if (string.IsNullOrEmpty(lastName))
                    lastName = usuario.Celular;

                var subscriber = _botConversa.CreateSubscriberAsync(phone, firstName, lastName).Result;
                _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                _botConversa.SendFlowAsync(subscriber.Id, 7109853); // QS Boas Vindas
                _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email); // Email
                
                string documento = Regex.Replace(usuario.Documento, @"\D", ""); // Remove tudo que não é dígito

                if (documento.Length == 11)
                    _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento); // CPF

                if (documento.Length == 14)
                    _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento); // CNPJ

                _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}"); // Link_Indicação_QS
                _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191); // Tag PF                

                return Ok(new
                {
                    status = 200,
                    code = usuario.Login,
                    userId = usuario.IdUsuario,
                    message = "Cadastro realizado com sucesso",
                    errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;

                return StatusCode(500, new { status = 500, message, errors = Array.Empty<string>() });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrar")]
        public IActionResult Registrar(UsuarioCadastroViewModel model)
         {
            UsuarioCadastroViewModelValidator validator = new UsuarioCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            var usuario = _negocio.Registrar(model);

            EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email").Wait();

            //var phone = "55" + usuario.Celular;
            //var message = $"Olá, {usuario.Nome ?? usuario.Email}! \nBem-vindo ao Quanta Shop! 🙌 \nFicamos felizes que você tenha se juntado à nossa comunidade 💪 \nPara garantir o seu sucesso siga estes passos simples através da nossa plataforma 🖥 \n\n1️⃣ Confirme seu e-mail ✉ \n2️⃣ Complete seus dados 💯 \n3️⃣ Faça o tour pela plataforma e assista aos vídeos 🎞 \n4️⃣ Procure um parceiro online ou próximo de você, realize a primeira compra e receba seu primeiro cashback 🤑 \n5️⃣ Salve nosso contato na sua agenda  \n\nAbraços,\nEquipe Quanta Shop";

            //_whatsAppService.SendMessageAsync(phone, message).Wait();

            var phone = "+55" + usuario.Celular;
            var username = usuario.Nome.Split(' ');
            var firstName = username.Length > 0 ? username[0] : string.Empty;
            var lastName = username.Length > 1 ? username[1] : string.Empty;

            if (string.IsNullOrEmpty(firstName))
                firstName = usuario.Email;

            if (string.IsNullOrEmpty(lastName))
                lastName = usuario.Celular;

            var subscriber = _botConversa.CreateSubscriberAsync(phone, firstName, lastName).Result;

            if (subscriber is not null)
            {
                _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                _botConversa.SendFlowAsync(subscriber.Id, 7109853); // QS Boas Vindas
                _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email); // Email

                string documento = Regex.Replace(usuario.Documento, @"\D", ""); // Remove tudo que não é dígito

                if (documento.Length == 11)
                    _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento); // CPF

                if (documento.Length == 14)
                    _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento); // CNPJ

                _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}"); // Link_Indicação_QS
                _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191); // Tag PF
            }

            return Ok(usuario.IdUsuario);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrarGoogle")]
        public async Task<IActionResult> RegistrarGoogleAsync(Oauth2CadastroViewModel model)
        {
            var validator = new OauthCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            var usuario = await _negocio.RegistrarGoogleAsync(model);

            return Ok(usuario.IdUsuario);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrarGoogleCredential")]
        public async Task<IActionResult> RegistrarGoogleCredentialAsync([FromBody] Oauth2CredentialCadastroViewModel model)
        {
            var validator = new OauthCredentialCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            var usuario = await _negocio.RegistrarGoogleCredentialAsync(model);

            return Ok(usuario.IdUsuario);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("verificarPatrocinador/{login}")]
        public IActionResult VerificarPatrocinador(string login)
        {
            var objUsuario = _negocio.BuscarLoginOuEmail(login);
            if (objUsuario != null && objUsuario.Ativo && !objUsuario.Master)
            {
                return Ok(new { patrocinador = true });
            }
            throw new PadraoException("patrocinador_nao_encontrado");
        }

        [HttpGet]
        [Route("reenviarativacao/{idUsuario}")]
        public IActionResult ReenviarAtivacao(string idUsuario)
        {
            _ = EnviarEmailConfirmacao(new Guid(idUsuario), "Quanta Shop - Confirmação de email");

            return Ok();
        }

        [HttpGet]
        [Route("diretosUsaurioLogado")]
        public async Task<IActionResult> DiretosUsuarioLogado()
        {
            var mesAnterior = ObterPrimeiroEUltimoDiaMesAnterior();
            var diretos = _negocio.ListaUsuarioDiretos(IdUsuarioLogado);

            if (!diretos.Any())
                return Ok(new List<object>());

            var idsUsuarios = diretos.Select(u => u.IdUsuario).ToList();
            
            // Buscar dados em lote (batch) - 2 queries ao invés de N * 2
            var dataUltimasComprasTask = _usersService.GetLastBuyBatch(idsUsuarios);
            var consumosMesAnteriorTask = _saqueNegocio.ObterConsumoSaqueBatch(
                idsUsuarios, 
                mesAnterior.primeiroDia, 
                mesAnterior.ultimoDia);

            // Aguardar ambas as queries em paralelo
            await Task.WhenAll(dataUltimasComprasTask, consumosMesAnteriorTask);

            var dataUltimasCompras = await dataUltimasComprasTask;
            var consumosMesAnterior = await consumosMesAnteriorTask;

            // Montar resultado final
            var result = diretos.Select(u => new
            {
                produtoAtivo = u.UsuarioProduto != null && u.UsuarioProduto.Count > 0 && u.UsuarioProduto.Any(p => p.Ativo) 
                    ? u.UsuarioProduto.FirstOrDefault(p => p.Ativo).Produto.Nome 
                    : "Baf Vision",
                u.IdUsuario,
                u.Nome,
                u.UrlImg,
                u.Login,
                u.Email,
                u.DataCadastro,
                u.Celular,
                Graduacao = new { u.Graduacao.Nome },
                TemFilhos = u.Filhos.Count > 0,
                Diretos = u.Filhos.Count,
                Pontuacao = _negocio.GetPontosFromCache(u.IdUsuario).TotalPontosUsuario,
                u.DataUltimoAcesso,
                dataUltimaCompra = dataUltimasCompras.ContainsKey(u.IdUsuario) ? dataUltimasCompras[u.IdUsuario] : null,
                consumoMesAnterior = consumosMesAnterior.ContainsKey(u.IdUsuario) 
                    ? consumosMesAnterior[u.IdUsuario].ToString("C2") 
                    : "R$ 0,00"
            }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("obterDistribuicao")]
        public IActionResult ObterDistribuicao()
        {
            var distribuicao = _negocio.ObterDistruibuicao(IdUsuarioLogado);
            var lancamentos = new List<object>();
            decimal total = 0;
            distribuicao.ForEach(d =>
            {
                lancamentos.Add(new
                {
                    descricao = d.Descricao,
                    valor = d.Valor,
                    validade = Math.Round((d.DataLancamento.AddDays(60) - DateTime.Now).TotalDays + 1)
                }); total += d.Valor;
            }
            );

            return Ok(new { lancamentos, total });
        }

        [HttpGet]
        [Route("obterUltimosDiretos")]
        public IActionResult ObterUltimosDiretos()
        {
            var lstUsuario = _negocio.ObterUltimosDiretos(IdUsuarioLogado);

            return Ok(lstUsuario.Select(u => new { u.Login, u.Email, u.DataCadastro, Graduacao = new { u.Graduacao.Nome } }));
        }

        [HttpGet]
        [Route("verificarQualificacao")]
        public IActionResult VerificarQualificacao()
        {
            var usuarioLogado = _negocio.GetById(IdUsuarioLogado);

            var pontuacao = new
            {
                qualificado = usuarioLogado.DataQualificacao.HasValue,
                usuarioLogado.DataQualificacao
            };

            return Ok(pontuacao);
        }

        private async Task EnviarEmailConfirmacao(Guid idUsuario, string titulo)
        {
            var usuario = _negocio.GetById(idUsuario);
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
            //var rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            var rootSite = _configNegocio.BuscarRootSite().Valor;
            if (string.IsNullOrEmpty(rootSite))
            {
                _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
                rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
            }
            var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;

            var body = new Dictionary<string, string>
            {
                { "{{ name }}", objectEmail.DestinationName },
                { "{{ confirmation_link }}", link}
            };

            var emailUtil = new EmailUtilitis();
            await emailUtil.EnviarEmail(body, _appSettings.ConfirmarEmail, null, objectEmail);
        }

        [HttpPost]
        [Route("atualizarDadosUsuario")]
        public async Task<IActionResult> AtualizarDadosUsuario([FromBody] UsuarioCadastroCompletoViewModel model)
        {
            var valid = new UsuarioCadastroCompletoViewModelValidator();
            var result = valid.Validate(model);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            };

            if (!string.IsNullOrEmpty(model.NovaSenha))
            {
                if (model.NovaSenha == model.NovaSenhaConfirma)
                {
                    var check = _negocio.VerificarSenha(model.IdUsuario, model.SenhaAntiga);

                    if (!check)
                    {
                        throw new UnauthorizedException("senha_incorreta");
                    }
                }
                else
                    throw new PadraoException("senha_nao_confere");

            }
            var image = string.Empty;

            if (!string.IsNullOrEmpty(model.Imagem))
                image = await AzureStorage.CreateBlob(model.Imagem, IdUsuarioLogado, _appSettings.StorageAccountConnectionString);

            var dadosUsuario = new UsuarioViewModel
            {
                IdUsuario = IdUsuarioLogado,
                Nome = model.Nome,
                Senha = model.NovaSenha,
                Celular = model.Celular,
                AssinaturaEletronica = model.AssinaturaEletronica,
                UrlImg = image,
                NomeSocial = model.NomeSocial,
                Login = model.Login,
                Genero = model.Genero,
                DataNascimento = model.DataNascimento,
            };
            var user = _negocio.AtualizarDados(dadosUsuario);

            var enderecoUsuario = new UsuarioEnderecoViewModel
            {
                IdCidade = model.IdCidade,
                IdUsuario = IdUsuarioLogado,
                Rua = model.Rua,
                Numero = model.Numero,
                Bairro = model.Bairro,
                Cep = model.Cep,
                Complemento = model.Complemento
            };
            var end = _negocioEndereco.CadastrarEndereco(enderecoUsuario);

            if (!end)
                throw new Exception(); ;

            return Ok(new
            {
                message = _location.GetTranslation("DadosUsuarioSalvo")
            });
        }

        [HttpGet]
        [Route("obterDadosPessoais")]
        public IActionResult ObterDadosPessoais()
        {
            return Ok(_negocio.ObterDadosPessoais(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("obterDadosUsuarioRede/{idUsuario}")]
        public IActionResult ObterDadosUsuarioRede(Guid idUsuario)
        {
            return Ok(_negocio.ObterDadosPessoais(idUsuario));
        }

        [HttpGet]
        [Route("obterFotoPerfil")]
        public IActionResult ObterFotoPerfil()
        {
            return Ok(_negocio.ObterFotoPerfil(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("obterPerfilPainel")]
        public IActionResult ObterPerfilPainel()
        {
            var dados = _proceduresRepositorio.spc_obterPerfilPainel(IdUsuarioLogado);

            return Ok(dados);
        }
        [HttpGet]
        [Route("checkCpfCnpj/{cpfCnpj}")]
        public IActionResult CheckCpfCnpj(string cpfCnpj)
        {
            var exists = _negocio.ObterDadosPessoaisPorCpfCnpj(cpfCnpj);
            return Ok(exists);
        }

        [HttpGet]
        [Route("obterPerformanceRede")]
        public IActionResult ObterPerfomanceRede(string nome = "", string login = "")
        {
            if (nome == null)
            {
                nome = "";
            }
            if (login == null)
            {
                login = "";
            }
            var performanceRede = _proceduresRepositorio.spc_PerformanceRede(IdUsuarioLogado, nome, login);

            var dadosRede = performanceRede
                .Select(async s => new
                {
                    s.IdUsuario,
                    s.Nome,
                    s.Login,
                    s.Direto,
                    s.ValorGerado,
                    s.NomeProduto,
                    s.URLIMG,
                    Pontuacao = _negocio.GetPontosFromCache(s.IdUsuario).TotalPontosUsuario,
                })
                .Select(s => s.Result);

            return Ok(dadosRede);
        }

        [HttpGet]
        [Route("obterPerformanceRedeDireto/{idUsuario}")]
        public IActionResult ObterPerfomanceRedeDireto(Guid idUsuario, string nome = "", string login = "")
        {
            if (nome == null)
            {
                nome = "";
            }
            if (login == null)
            {
                login = "";
            }
            var dadosRedeUsuario = _proceduresRepositorio.spc_PerformanceRede(idUsuario, nome, login);

            return Ok(dadosRedeUsuario);
        }

        [HttpGet]
        [Route("obterLancamentoRede")]
        public IActionResult ObterLancamentoRede()
        {
            var dadosLancamentoRede = _proceduresRepositorio.spc_LancamentoRedeUsuario(IdUsuarioLogado);

            return Ok(dadosLancamentoRede);
        }

        [HttpGet]
        [Route("assinaturaEletronicaAleatoria")]
        public IActionResult AssinaturaEletronicaAleatoria()
        {
            var pass = RandomPassword.Generate(8, 10);
            var usuario = _negocio.AssinaturaEletronicaAleatoria(IdUsuarioLogado, pass);

            if (usuario != null)
            {
                EnviarEmailAssinaturaEletronica(usuario, pass);
                return Ok(new { message = _location.GetTranslation("AssinaturaEletronoicaEnviadaPorEmail") });
            }
            else
                throw new Exception();
        }

        [HttpPost]
        [Route("editarUsuario")]
        public IActionResult EditarUsuario([FromBody] UsuarioEditarViewModel viewModel)
        {
            var valid = new UsuarioEditarViewModelValidator();
            var result = valid.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            viewModel.IdUsuario = IdUsuarioLogado;

            if (!string.IsNullOrEmpty(viewModel.NovaSenha))
            {
                if (viewModel.NovaSenha == viewModel.NovaSenhaConfirma)
                {
                    var check = _negocio.VerificarSenha(viewModel.IdUsuario, viewModel.SenhaAntiga);

                    if (!check)
                    {
                        throw new UnauthorizedException("senha_incorreta");
                    }
                }
                else
                    throw new PadraoException("senha_nao_confere");

            }

            var updateUsuario = _negocio.EditarDadosUsuario(viewModel);

            if (updateUsuario)
            {
                UsuarioViewModel user = _negocio.GetById(IdUsuarioLogado);

                string phone = Regex.Replace(user.Celular, @"\D", ""); // Remove tudo que não é dígito

                var subscriber = _botConversa.GetSubscriberByPhoneAsync($"55{phone}").Result;

                if (subscriber is not null)
                {
                    if (user.DataNascimento is not null)
                        _botConversa.SetCustomFieldAsync(subscriber.Id, 4166457, user.DataNascimento.Value.ToString("yyyy-MM-dd"));
                }

                return Ok(new { message = _location.GetTranslation("DadosUsuariosSalvo") });
            }
            else
                throw new Exception();

        }

        [HttpPost]
        [Route("updateImage")]
        public async Task<IActionResult> UpdateImage(UsuarioCadastroCompletoViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Imagem))
            {
                //var image = await new Azure().CreateBlob(viewModel.Imagem, viewModel.IdUsuario, _appSettings.StorageAccountConnectionString);
                var image = SalvaImagem(viewModel.Imagem, IdUsuarioLogado.ToString());
                var ret = _negocio.UpdateImage(IdUsuarioLogado, image);

                if (ret)
                    return Ok(new { message = _location.GetTranslation("ImageAtualizada"), url = image });
                else
                    throw new Exception();
            }
            else
                throw new PadraoException("imagem_requerida");
        }

        [HttpPost]
        [Route("updateAssinaturaEletronica")]
        public IActionResult UpdateAssinaturaEletronica(AssinaturaEletronicaViewModel viewModel)
        {
            var valid = new AssinaturaEletronicaViewModelValidator();
            var result = valid.Validate(viewModel);

            if (result.IsValid)
            {
                if (_negocio.VerificarAssinaturaEletronica(IdUsuarioLogado, viewModel.AssinaturaAtual))
                {
                    if (_negocio.UpdateAssinaturaEletronica(IdUsuarioLogado, viewModel.AssinaturaEletronica))
                        return Ok(new { message = _location.GetTranslation("AssinaturaEletronicaAtualizada") });
                }

                throw new Exception();
            }

            throw new PadraoException("senha_nao_confere");
        }
        
        private void EnviarEmailAssinaturaEletronica(UsuarioViewModel usuario, string assinaturaEletronica)
        {
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = _appSettings.FromName + " - Nova Assinatura Eletrônica",
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var body = new Dictionary<string, string>
            {
                { "#NOMECLIENTE#", objectEmail.DestinationName },
                { "#ASINATURAELETRONICA", assinaturaEletronica}
            };

            var ret = new EmailUtilitis().EnviarEmail(body, _appSettings.NovaAssinaturaEletronica, _appSettings.TemplatePai, objectEmail);
        }

        [HttpGet]
        [Route("getSaldo")]
        public IActionResult GetSaldo()
        {
            return Ok(new
            {
                saldo = _lancamentoNegocio
                        .Get(l => l.Ativo && l.IdUsuario == IdUsuarioLogado && l.IdStatus != 3 && !l.Bloqueado, "LancamentoRetido")
                        .Sum(l => l.Valor - l.LancamentoRetido.Where(w => w.Ativo).Sum(lt => lt.Valor))
            });
        }

        [HttpPost]
        [Route("verificarSaldo")]
        public IActionResult VerificarSaldo(VerificarSaldoViewModel venda)
        {
            var mensagens = new List<string>();
            var saldoSuficiente = false;
            //verificar se o cashback gerado é acima de 50 reais 
            var saldo = _lancamentoNegocio
                    .Get(l => l.Ativo && l.IdUsuario == IdUsuarioLogado && l.IdStatus != 3 && !l.Bloqueado, "LancamentoRetido")
                    .Sum(l => l.Valor - l.LancamentoRetido.Where(w => w.Ativo).Sum(lt => lt.Valor));
            var cashbackAcumulado = _lancamentoNegocio.Get(cm => cm.IdUsuario == IdUsuarioLogado && (cm.IdTipo == 33 || cm.IdTipo == 52)).Sum(v => v.Valor);
            var valorMinimo = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "VALOR_MINIMO_CONSUMO").Valor);
            var valorTarifa = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "TARIFA_PAGAMENTO_COM_SALDO_COMPRADOR").Valor);

            if (saldo >= venda.ValorCompra + valorTarifa)
            {
                saldoSuficiente = true;

                if (cashbackAcumulado >= valorMinimo)
                {
                    saldoSuficiente = true;
                }
                else
                {
                    saldoSuficiente = false;
                    mensagens.Add($"Seu cashback acumulado ainda não atingiu o valor mínimo de {valorMinimo:C2}. Faltam {valorMinimo - cashbackAcumulado:C2}.");

                }
            }
            else
            {
                if (valorTarifa > 0)
                    mensagens.Add($"Seu saldo é insuficiente para esta compra, por favor selecione outro método de pagamento. Considere também o valor da tarifa ({valorTarifa:C2}) ao calcular o total necessário.");
                else
                    mensagens.Add("Seu saldo é insuficiente para esta compra, por favor selecione outro método de pagamento");

            }

            return Ok(new
            {
                saldo,
                mensagens,
                saldoSuficiente,
                valorMinimo = valorMinimo.ToString("C2"),
                valorTarifa = valorTarifa.ToString("C2")
            });
        }

        [HttpGet]
        [Route("getGanhos")]
        public async Task<IActionResult> GetGanhos()
        {
            return Ok(
                new
                {
                    ganhos = _lancamentoNegocio.Get(l => l.Ativo && l.Valor > 0 && l.IdUsuario == IdUsuarioLogado && l.IdStatus != (int)StatusTransacaoEnum.Cancelada).Sum(l => l.Valor)
                });
        }

        [HttpGet]
        [Route("obterRedeUsuario")]
        public IActionResult ObterRedeUsuario()
        {
            return Ok(_proceduresRepositorio.spc_UsuarioDownLine(IdUsuarioLogado));
        }

        [HttpPost]
        [Route("obterRankUsuarioFiltrado")]
        public IActionResult ObterRankUsuarioFiltrado(FiltroViewModel.FiltroRank filtro)
        {
            return Ok(_negocio.ObterRankFiltrado(IdUsuarioLogado, filtro.Login, filtro.Ordenacao));
        }

        [HttpGet]
        [Route("obterPlanoAtivo")]
        public IActionResult ObterPlanoAtivo()
        {
            var result = _usuarioProdutoNegocio.BuscarProdutoAtivo(IdUsuarioLogado);
            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(json);
        }

        [HttpGet]
        [Route("alterarPerfil/{perfil}")]
        public IActionResult AlterarPerfil(char perfil)
        {
            var usuario = _negocio.FirstNoTracking(f => f.IdUsuario == IdUsuarioLogado);

            if (usuario != null)
            {
                usuario.Perfil = perfil;
                _negocio.Update(usuario);

                return Ok();
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("primeiraCompra")]
        public async Task<IActionResult> RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model)
        {
            try
            {
                //TODO: Colocar um meio de validação de autenticação CORS/Basic
                UsuarioCadastroFacilitadoViewModelValidator validator = new UsuarioCadastroFacilitadoViewModelValidator();
                var result = validator.Validate(model);
                if (!result.IsValid)
                {
                    throw new AggregateException(
                        result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
                }

                string comprovanteBlob = null;

                try
                {
                    if (!string.IsNullOrEmpty(model.ComprovanteCompra))
                    {
                        comprovanteBlob = SalvaImagemComprovante(model.ComprovanteCompra, model.CPF);
                        model.ComprovanteCompra = comprovanteBlob;
                    }

                    var usuario = _preCadastroNegocio.RegistrarFacilitado(model);

                    await EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email");

                    //var phone = "55" + usuario.Celular;
                    //var message = $"Olá, {usuario.Nome ?? usuario.Email}! \nBem-vindo ao Quanta Shop! 🙌 \nFicamos felizes que você tenha se juntado à nossa comunidade 💪 \nPara garantir o seu sucesso siga estes passos simples através da nossa plataforma 🖥 \n\n1️⃣ Confirme seu e-mail ✉ \n2️⃣ Complete seus dados 💯 \n3️⃣ Faça o tour pela plataforma e assista aos vídeos 🎞 \n4️⃣ Procure um parceiro online ou próximo de você, realize a primeira compra e receba seu primeiro cashback 🤑 \n5️⃣ Salve nosso contato na sua agenda \n\nAbraços,\nEquipe Quanta Shop";

                    //await _whatsAppService.SendMessageAsync(phone, message);

                    var phone = "+55" + usuario.Celular;
                    var username = usuario.Nome.Split(' ');
                    var firstName = username.Length > 0 ? username[0] : string.Empty;
                    var lastName = username.Length > 1 ? username[1] : string.Empty;

                    if (string.IsNullOrEmpty(firstName))
                        firstName = usuario.Email;

                    if (string.IsNullOrEmpty(lastName))
                        lastName = usuario.Celular;

                    var subscriber = await _botConversa.CreateSubscriberAsync(phone, firstName, lastName);
                    if (subscriber is not null)
                    {
                        await _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                        await _botConversa.SendFlowAsync(subscriber.Id, 7109853); // QS Boas Vindas
                        await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email); // Email

                        string documento = Regex.Replace(usuario.Documento, @"\D", ""); // Remove tudo que não é dígito

                        if (documento.Length == 11)
                            await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento); // CPF

                        if (documento.Length == 14)
                            await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento); // CNPJ

                        await _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}"); // Link_Indicação_QS
                        await _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191); // Tag PF
                    }

                    return Ok(usuario.IdUsuario);
                }
                catch
                {
                    if (comprovanteBlob != null)
                    {
                        ApagaImagemComprovante(comprovanteBlob);
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log exception, if needed
                // LogException(ex);
                throw;
            }
        }

        private string SalvaImagem(string imageBase64, string idUsuario)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var logoUrl = AzureStorage.CreateBlob(
                        imageBase64,
                        new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        "imagens-perfil",
                        idUsuario + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                        true
                    ).Result;

                return logoUrl;
            }

            return string.Empty;
        }

        private string SalvaImagemComprovante(string imageBase64, string cpf)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var logoUrl = AzureStorage.CreateBlob(
                        imageBase64,
                new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        "imagens-comprovante",
                        cpf + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                        true
                    ).Result;

                return logoUrl;
            }

            return string.Empty;
        }

        private void ApagaImagemComprovante(string blob)
        {
            if (!string.IsNullOrEmpty(blob))
            {
                string pattern = @"[^/]+$";

                Match match = Regex.Match(blob, pattern);

                if (match.Success)
                {
                    Console.WriteLine("Dados após a última barra: " + match.Value);
                    var logoUrlDelete = AzureStorage.Delete(
                            "imagens-comprovante",
                            match.Value,
                            _appSettings.StorageAccountConnectionString
                        ).Result;
                }
                else
                {
                    Console.WriteLine("Nenhuma correspondência encontrada.");
                }
            }
        }

        public static (DateTime primeiroDia, DateTime ultimoDia) ObterPrimeiroEUltimoDiaMesAnterior()
        {
            DateTime dataAtual = DateTime.Today;
            DateTime primeiroDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
            DateTime ultimoDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddDays(-1);
            return (primeiroDiaMesPassado, ultimoDiaMesPassado);
        }
    }
}