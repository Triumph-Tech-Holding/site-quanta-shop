using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Translation;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BancoController : LoggedControllerBase
    {
        private readonly IBancoNegocio _bancoNegocio;
        public BancoController(IBancoNegocio bancoNegocio)
        {
            _bancoNegocio = bancoNegocio;
        }

        [HttpGet]
        [Route("listarBancos")]
        public IActionResult ListarBancos()
        {
            var bancos = _bancoNegocio.Get(w => w.Ativo).
                //var listBancos = bancos.
                Select(x => new BancoViewModel
                {
                    IdBanco = x.IdBanco,
                    Febraban = x.Febraban,
                    Nome = x.Nome.Trim(),
                    Descricao = x.Descricao,
                    Ordem = x.Ordem,
                    Ativo = x.Ativo
                }).OrderBy(x => x.Nome);//.ToList();

            return Ok(bancos);
        }
    }
}
