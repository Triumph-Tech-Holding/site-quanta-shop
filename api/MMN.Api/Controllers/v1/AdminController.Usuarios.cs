using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.Util.Enum;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    public partial class AdminController
    {
        [HttpPost]
        [Route("filtrarUsuarios")]
        public IActionResult FiltrarUsuarios(FiltroUsuario filtroUsuario)
        {
            var usuarios = _usuarioNegocio.FiltrarUsuarios(filtroUsuario);
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("editarDadosUsuario")]
        public IActionResult EditarDadosUsuario(AdminDadosUsuarioViewModel view)
        {
            var validacao = new AdminDadosUsuarioViewModelValidator();
            var result = validacao.Validate(view);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _usuarioNegocio.GetById(view.IdUsuario);

            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            if (!_usuarioNegocio.VerificaLoginDisponivel(usuario, view.Login))
                throw new PadraoException("login_em_uso");

            if (!_usuarioNegocio.VerificaPatrocinador(view.LoginPatrocinador))
                throw new NotFoundException("patrocinador_nao_encontrado");

            if (!_usuarioNegocio.AdminAtualizarDadosUsuario(view))
                throw new Exception();

            return Ok(new { message = _location.GetTranslation("DadosUsuarioAtualizado") });
        }

        [HttpGet]
        [Route("dadosUsuario/{idUsuario}")]
        public IActionResult DadosUsuario(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.ObterDadosPessoais(idUsuario);
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");
            return Ok(usuario);
        }

        [HttpGet]
        [Route("listaGraduacoes")]
        public IActionResult listaGraduacoes()
        {
            return Ok(_graduacaoNegocio.GetAll());
        }

        [HttpGet]
        [Route("redefinirSenhaUsuairo/{idUsuario}")]
        public IActionResult RedefinirSenhaUsuairo(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            var webToken = _token.ConstruirToken(usuario);
            var rootSite = _configNegocio.BuscarRootSite().Valor;
            var link = rootSite + _appSettings.RootSiteResetPassword + webToken;
            link = link.Replace("quantashop.com.br", "escritorio.quantashop.com.br");
            return Ok(link);
        }

        [HttpGet]
        [Route("bloquearUsuairoAdmin/{idUsuario}")]
        public IActionResult BloquearUsuairoAdmin(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");
            return Ok(_usuarioNegocio.BloquearUsuarioAdmin(idUsuario));
        }

        [HttpPost]
        [Route("AtivarDesativarUsuario")]
        public IActionResult AtivarDesativarUsuario(UsuarioViewModel viewModel)
        {
            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario != null)
            {
                usuario.Ativo = !usuario.Ativo;
                _usuarioNegocio.Update(usuario);
                return Ok(usuario.Ativo);
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("confirmar-email")]
        public IActionResult ConfirmarEmail(UsuarioViewModel viewModel)
        {
            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario != null)
            {
                usuario.EmailConfirmado = true;
                _usuarioNegocio.Update(usuario);
                return Ok(usuario.EmailConfirmado);
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpGet]
        [Route("efetuarAcessoRemoto/{idUsuario}")]
        public IActionResult EfetuarAcessoRemoto(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == idUsuario, "Grupo", "UsuarioEndereco");
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            if (!usuario.EmailConfirmado)
                throw new PadraoException("usuario_remoto_email_confirmado");

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.Grupo.Descricao),
                new Claim(ClaimTypes.PrimarySid, usuario.Grupo.IdGrupo.ToString()),
                new Claim(ClaimTypes.PrimaryGroupSid, usuario.IdGraduacao.HasValue ? usuario.IdGraduacao.ToString() : "0"),
                new Claim("Cultura", usuario.Cultura),
                new Claim("IdUsurioAdminRemoto", IdUsuarioLogado.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            var comerciante = false;
            var credenciamento = new CredenciamentoViewModel();
            if (usuario.Grupo.Descricao == "Comerciante")
            {
                comerciante = true;
                credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == usuario.IdUsuario);
            }

            if (usuario.Empreendedor)
                usuario.Perfil = 'E';

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new
            {
                Id = usuario.IdUsuario,
                Username = usuario.Nome,
                usuario.Login,
                usuario.Email,
                token,
                usuario.Grupo.Descricao,
                usuario.Cultura,
                usuario.UrlImg,
                logoUrl = comerciante && credenciamento != null && !string.IsNullOrEmpty(credenciamento.LogoUrl) ? credenciamento.LogoUrl : string.Empty,
                admin = usuario.IdGrupo == 1,
                usuario.Perfil,
                dadosCompletos = usuario.UsuarioEndereco.Count > 0,
                usuario.Empreendedor,
                comerciante
            });
        }

        [HttpGet]
        [Route("obterRedeUsuarioEspecifico/{login}")]
        public IActionResult ObterRedeUsuarioEspecifico(string login)
        {
            var usuario = _usuarioNegocio.First(f => f.Login == login);
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            var diretos = _usuarioNegocio.ListaUsuarioDiretos(usuario.IdUsuario);
            var result = diretos
                .Select(async u => new
                {
                    u.IdUsuario,
                    u.Nome,
                    u.UrlImg,
                    u.Login,
                    u.Email,
                    u.DataCadastro,
                    u.Celular,
                    Graduacao = new { u.Graduacao.Nome },
                    TemFilhos = u.Filhos.Count > 0,
                    Pontuacao = _usuarioNegocio.GetPontosFromCache(u.IdUsuario).TotalPontosUsuario
                })
                .Select(s => s.Result);
            return Ok(result);
        }

        [HttpGet]
        [Route("obterRedeUsuarioEspecificoExport/{login}")]
        public IActionResult ObterRedeUsuarioEspecificoExport(string login)
        {
            var usuario = _usuarioNegocio.First(f => f.Login == login);
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            var lstUsuario = _usuarioNegocio.ListaUsuarioDiretos(usuario.IdUsuario);
            var listaRedeExport = new System.Collections.Generic.List<UsuarioExportViewModel>();

            foreach (var u in lstUsuario)
            {
                var user = new UsuarioExportViewModel
                {
                    Nivel = 0,
                    Login = u.Login,
                    Nome = u.Nome,
                    Email = u.Email,
                    Celular = u.Celular,
                    Graduacao = u.Graduacao.Nome,
                    DataCadastro = u.DataCadastro.Value.ToString("dd/mm/yyyy hh:mm")
                };
                listaRedeExport.Add(user);
                if (u.Filhos.Count > 0)
                {
                    var filhos = _proceduresRepositorio.spc_UsuarioDownLine(u.IdUsuario);
                    listaRedeExport.AddRange(filhos.Select(f => new UsuarioExportViewModel
                    {
                        Nivel = f.Nivel,
                        LoginFilho = f.Login,
                        Nome = f.Nome,
                        Email = f.Email,
                        Celular = f.Celular,
                        Graduacao = ((TipoGraduacao)f.IdGraduacao).GetDescription(),
                        DataCadastro = u.DataCadastro.Value.ToString("dd/mm/yyyy hh:mm"),
                    }));
                }
            }
            return Ok(listaRedeExport);
        }

        [HttpGet("obter-aniversariantes")]
        public async Task<IActionResult> ObterAniversariantes(int mes)
        {
            try
            {
                var aniversarios = await _userService.GetBirthdays(mes);
                return Ok(aniversarios);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-total-cadastros-por-dia")]
        public async Task<IActionResult> ObterTotalCadastrosPorDia(int mes, int ano)
        {
            try
            {
                var cadastros = await _userService.GetRegistrationsPerDay(mes, ano);
                return Ok(cadastros);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-cadastros-por-dia")]
        public async Task<IActionResult> ObterCadastrosPorDia(DateTime dia)
        {
            try
            {
                var cadastros = await _userService.GetRegistrationsPerDay(dia);
                return Ok(cadastros);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
