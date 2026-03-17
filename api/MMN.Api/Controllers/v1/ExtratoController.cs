using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using System;
using System.Linq;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExtratoController : LoggedControllerBase
    {
        private readonly ILancamentoNegocio _negocio;

        public ExtratoController(ILancamentoNegocio negocio)
        {
            _negocio = negocio;
        }

        [HttpPost]
        [Route("buscarExtrato")]
        public IActionResult BuscarExtrato([FromBody] BuscarExtrato modelFiltro)
        {
            var lstExtrato = _negocio.BuscarPorIdUsuario(modelFiltro, IdUsuarioLogado);

            return Ok(lstExtrato);
        }

        [HttpGet]
        [Route("obterSaldoPorTipo")]
        public IActionResult ObterSaldoPorTipo()
        {
            var valores = _negocio.ObterSaldoPorTipo(IdUsuarioLogado);

            return Ok(valores);
        }

        [HttpGet]
        [Route("obterLancamentos/{tipoLancamento}")]
        public IActionResult ObterLancamentos(int tipoLancamento)
        {
            var valores = _negocio.ObterLancamentos(IdUsuarioLogado, tipoLancamento).OrderBy(x => x.Valor).OrderByDescending(o => o.DataLancamento);

            return Ok(valores);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("obterLancamentos")]
        public IActionResult ObterLancamentosAdmin(FiltroLancamento filtros)
        {
            var lancamentos = _negocio.ObterLancamentosAdmin(filtros, IdUsuarioLogado);

            return Ok(lancamentos);
        }
    }
}