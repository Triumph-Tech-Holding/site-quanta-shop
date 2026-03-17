using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.ViewModel.Pagamento;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    /// <summary>
    /// Metodos relativos à faturas de clientes credenciados
    /// </summary>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [Route("api/pagamento")]
    [ApiController]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status500InternalServerError)]
    public class PagamentoController : LoggedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPagamentoNegocio _pagamentoNegocio;

        /// <summary>
        /// Contrutor, recebe parâmetros por injeção de dependencia
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="pagamentoNegocio"></param>
        public PagamentoController(
            IMapper mapper,
            IPagamentoNegocio pagamentoNegocio)
        {
            _mapper = mapper;
            _pagamentoNegocio = pagamentoNegocio;
        }

        /// <summary>
        /// Gera o boleto de um pedido.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("gerarBoleto")]
        [ProducesResponseType(typeof(PagamentoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagamentoViewModel>> GerarBoleto(GerarBoletoViewModel viewModel)
        {

            var pagamento = await _pagamentoNegocio.CriarBoletoAsync(
                viewModel.IdPedido,
                viewModel.NumeroParcela,
                IdUsuarioLogado);

            return Ok(pagamento);
        }
    }
}
