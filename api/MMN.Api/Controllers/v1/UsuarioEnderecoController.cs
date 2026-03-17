using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Model;
using MMN.Util.Translation;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioEnderecoController : LoggedControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUsuarioEnderecoNegocio _negocio;
        private readonly ILocation _location;
        public UsuarioEnderecoController(IOptions<AppSettings> appSettings, IUsuarioEnderecoNegocio negocio, ILocation location)
        {
            _negocio = negocio;
            _location = location;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("obterUsuarioEndereco")]
        public IActionResult ObterUsuarioEndereco()
        {
            UsuarioEnderecoViewModel usuarioEndereco = _negocio.First(f => f.IdUsuario == IdUsuarioLogado, "Cidade.Estado");

            if (usuarioEndereco != null)
                usuarioEndereco.Cidade.Estado.Cidade = null;

            return Ok(usuarioEndereco);
        }

        [HttpPut]
        [Route("editarUsuarioEndereco")]
        public IActionResult EditarUsuarioEndereco([FromBody] UsuarioEnderecoViewModel model)
        {
            bool retorno;
            model.IdUsuario = IdUsuarioLogado;
            if (model.IdEndereco != 0)
            {
                retorno = _negocio.EditarEndereco(model);
            }
            else
            {
                retorno = _negocio.CadastrarEndereco(model);
            }

            if (retorno)
                return Ok(new { message = _location.GetTranslation("EditarEnderecoSalvo") });
            else
                throw new Exception();
        }
    }
}