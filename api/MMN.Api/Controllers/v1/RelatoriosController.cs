using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Api.Service;
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
    /// <summary>
    /// Métodos para consumo externo
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IRelatorios _relatorios;

        public RelatoriosController(IUsuarioNegocio usuarioNegocio, IRelatorios relatorios)
        {
            _usuarioNegocio = usuarioNegocio;
            _relatorios = relatorios;
        }

        [HttpGet("awin-transactions")]
        public IActionResult AwinTransactions()
        {
            var headerValue = Request.Headers["headerKey"];

            return !string.IsNullOrEmpty(headerValue) && headerValue == "bigcash-awin-header-auth" ? Ok(_relatorios.RelatorioVendasAwin()) : BadRequest();
        }


        [HttpGet("users")]
        public IActionResult Users()
        {
            var headerValue = Request.Headers["headerKey"];

            return !string.IsNullOrEmpty(headerValue) && headerValue == "bigcash-awin-header-auth" ? Ok(_usuarioNegocio.GetAll().Select(x => new { x.IdUsuario, x.Email, x.Login })) : BadRequest();
        }
    }
}