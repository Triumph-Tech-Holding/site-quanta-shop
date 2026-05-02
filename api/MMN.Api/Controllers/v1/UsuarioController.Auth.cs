using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.Util.Extensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    public partial class UsuarioController
    {
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
                    return Unauthorized(new { status = 401, message = "Unauthorized", errors = Array.Empty<string>() });

                UsuarioCadastroPWAViewModelValidator validator = new UsuarioCadastroPWAViewModelValidator();
                var result = validator.Validate(model);

                model.Login = model.Documento;

                if (model.Celular.StartsWith("55"))
                    model.Celular = model.Celular.Replace("55", "");

                if (!result.IsValid)
                {
                    #region Tayon
                    if (result.Errors.Any(x => x.ErrorMessage == "email_requerido"))
                        return BadRequest(new { status = 400, code = " email_requerido", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "email_invalido"))
                        return BadRequest(new { status = 400, code = " email_invalido", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "email_nao_permitido"))
                        return BadRequest(new { status = 400, code = " patrocinador_nao_encontrado", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "email_requisito"))
                        return BadRequest(new { status = 400, code = " email_requisito", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "senha_requisitos"))
                        return BadRequest(new { status = 400, code = " senha_requisitos", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "patrocinador_requerido"))
                        return BadRequest(new { status = 400, code = " patrocinador_nao_encontrado", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    if (result.Errors.Any(x => x.ErrorMessage == "cpf_cnpj_invalido"))
                        return BadRequest(new { status = 400, code = " patrocinador_nao_encontrado", message = "Bad request", errors = result.Errors.Select(s => new PadraoException(s.ErrorMessage).Message) });
                    #endregion Tayon
                }

                var usuarioPai = _negocio.GetByLoginOrEmail(model.LoginPatrocinador);
                if (usuarioPai == null || usuarioPai.Ativo == false || usuarioPai.Master)
                    return BadRequest(new { status = 400, code = " patrocinador_nao_encontrado", message = "Bad request", errors = new[] { "Patrocinador não encontrado" } });

                if (model.Login.TemCaracterEspecial())
                    return BadRequest(new { status = 400, code = " login_contem_caracteres_especiais", message = "Bad request", errors = new[] { "O login deve conter apenas letras" } });

                var usuarioLogin = _negocio.GetByLoginOrEmail(model.Login);
                if (usuarioLogin != null)
                    return BadRequest(new { status = 400, code = " login_em_uso", message = "Bad request", errors = new[] { "Login indisponível" } });

                usuarioLogin = _negocio.GetByLoginOrEmail(model.Email);
                if (usuarioLogin != null)
                    return BadRequest(new { status = 400, code = " email_em_uso", message = "Bad request", errors = new[] { "Já existe uma conta cadastrada com este endereço de email" } });

                if (_negocio.Get(u => u.Celular == model.Celular).Any())
                    return BadRequest(new { status = 400, code = " telefone_em_uso", message = "Bad request", errors = new[] { "Já existe uma conta cadastrada com este número de telefone" } });

                if (_negocio.Get(u => u.Documento == model.Documento).Any())
                    return BadRequest(new { status = 400, code = " cpf_cnpj_em_uso", message = "Bad request", errors = new[] { "Já existe uma conta cadastrada com este número de documento" } });

                var usuario = _negocio.CadastroPWA(model);
                EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email").Wait();

                var phone = "+55" + usuario.Celular;
                var username = usuario.Nome.Split(' ');
                var firstName = username.Length > 0 ? username[0] : string.Empty;
                var lastName  = username.Length > 1 ? username[1] : string.Empty;
                if (string.IsNullOrEmpty(firstName)) firstName = usuario.Email;
                if (string.IsNullOrEmpty(lastName))  lastName  = usuario.Celular;

                var subscriber = _botConversa.CreateSubscriberAsync(phone, firstName, lastName).Result;
                _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                _botConversa.SendFlowAsync(subscriber.Id, 7109853);
                _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email);
                string documento = Regex.Replace(usuario.Documento, @"\D", "");
                if (documento.Length == 11) _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento);
                if (documento.Length == 14) _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento);
                _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}");
                _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191);

                return Ok(new { status = 200, code = usuario.Login, userId = usuario.IdUsuario, message = "Cadastro realizado com sucesso", errors = Array.Empty<string>() });
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
        [EnableRateLimiting("auth-limit")]
        public IActionResult Registrar(UsuarioCadastroViewModel model)
        {
            UsuarioCadastroViewModelValidator validator = new UsuarioCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));

            var usuario = _negocio.Registrar(model);
            EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email").Wait();

            var phone = "+55" + usuario.Celular;
            var username = usuario.Nome.Split(' ');
            var firstName = username.Length > 0 ? username[0] : string.Empty;
            var lastName  = username.Length > 1 ? username[1] : string.Empty;
            if (string.IsNullOrEmpty(firstName)) firstName = usuario.Email;
            if (string.IsNullOrEmpty(lastName))  lastName  = usuario.Celular;

            var subscriber = _botConversa.CreateSubscriberAsync(phone, firstName, lastName).Result;
            if (subscriber is not null)
            {
                _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                _botConversa.SendFlowAsync(subscriber.Id, 7109853);
                _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email);
                string documento = Regex.Replace(usuario.Documento, @"\D", "");
                if (documento.Length == 11) _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento);
                if (documento.Length == 14) _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento);
                _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}");
                _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191);
            }
            return Ok(usuario.IdUsuario);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrarGoogle")]
        [EnableRateLimiting("auth-limit")]
        public async Task<IActionResult> RegistrarGoogleAsync(Oauth2CadastroViewModel model)
        {
            var validator = new OauthCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            var usuario = await _negocio.RegistrarGoogleAsync(model);
            return Ok(usuario.IdUsuario);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registrarGoogleCredential")]
        [EnableRateLimiting("auth-limit")]
        public async Task<IActionResult> RegistrarGoogleCredentialAsync([FromBody] Oauth2CredentialCadastroViewModel model)
        {
            var validator = new OauthCredentialCadastroViewModelValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
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
                return Ok(new { patrocinador = true });
            throw new PadraoException("patrocinador_nao_encontrado");
        }

        [HttpGet]
        [Route("reenviarativacao/{idUsuario}")]
        public IActionResult ReenviarAtivacao(string idUsuario)
        {
            _ = EnviarEmailConfirmacao(new Guid(idUsuario), "Quanta Shop - Confirmação de email");
            return Ok();
        }
    }
}
