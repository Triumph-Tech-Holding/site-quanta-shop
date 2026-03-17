using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.ViewModel.Fatura;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    /// <summary>
    /// Metodos relativos à faturas de clientes credenciados
    /// </summary>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [Route("api/fatura")]
    [ApiController]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status500InternalServerError)]
    public class FaturaController : LoggedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFaturaNegocioNovo _faturaNegocio;
        private readonly IPedidoNegocio _pedidoNegocio;

        /// <summary>
        /// Contrutor, recebe parâmetros por injeção de dependencia
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="faturaNegocio"></param>
        /// <param name="pedidoNegocio"></param>
        public FaturaController(
            IMapper mapper,
            IFaturaNegocioNovo faturaNegocio,
            IPedidoNegocio pedidoNegocio)
        {
            _mapper = mapper;
            _faturaNegocio = faturaNegocio;
            _pedidoNegocio = pedidoNegocio;
        }

        /// <summary>
        /// Obter as faturas geradas para o credenciado logado.
        /// </summary>
        [HttpGet("obterFaturas")]
        [ProducesResponseType(typeof(IEnumerable<ListaFaturaViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListaFaturaViewModel>>> ObterFaturas()
        {
            var faturas = await _faturaNegocio.ObterFaturasAsync(IdUsuarioLogado);
            faturas = faturas.OrderByDescending(o => o.DataReferencia ?? o.DataPedido);

            foreach (var pedido in faturas)
            {
                if (pedido.Pagamentos.Count() > 1)
                {
                    pedido.Pagamentos = new List<Pagamento>
                    {
                        pedido.Pagamentos.OrderByDescending(o => o.DataValidade).First()
                    };
                }
            }

            return Ok(_mapper.Map<IEnumerable<ListaFaturaViewModel>>(faturas));
        }

        /// <summary>
        /// Obter os pedidos aguardando a criação de fatura.
        /// </summary>
        /// <param name="ateData">Pedido de venda mais recente a ser incluído.</param>
        [HttpGet("obterPedidosAguardandoFatura")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListaFaturaViewModel>>> ObterPedidosAguardandoFatura([FromQuery] DateTime? ateData)
        {
            if (ateData == null)
            {
                ateData = DateTime.UtcNow;
            }

            ateData = ateData.Value.HorarioBrasilia().AddDays(1).Date;

            IEnumerable<Pedido> pedidos = await _faturaNegocio.ObterPedidosAguardandoFaturaAsync(ateData.Value, IdUsuarioLogado);

            return Ok(_mapper.Map<IEnumerable<PedidoFaturaViewModel>>(pedidos));
        }

        /// <summary>
        /// Cria uma nova fatura com todos os pedidos sem fatura até o pedido indicado.
        /// </summary>
        /// <returns></returns>
        [HttpPost("criarFatura")]
        [ProducesResponseType(typeof(ListaFaturaViewModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<ListaFaturaViewModel>> CriarFatura(CriarFaturaViewModel criarFaturaViewModel)
        {
            var fatura = await _pedidoNegocio.CriarFaturaCashbackCredenciadoAsync(
                criarFaturaViewModel.AteData,
                criarFaturaViewModel.TipoPagamento,
                IdUsuarioLogado);

            return Ok(_mapper.Map<ListaFaturaViewModel>(fatura));
        }

        /// <summary>
        /// Cancela uma fatura não paga.
        /// </summary>
        /// <param name="IdFatura">Identificador da fatura.</param>
        [HttpPost("cancelarFatura")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListaFaturaViewModel>>> CancelarFatura([FromBody] long IdFatura)
        {
            _pedidoNegocio.CancelarPedido(IdFatura, IdUsuarioLogado);

            return Ok(_mapper.Map<ListaFaturaViewModel>($"Fatura cancelada com sucesso."));
        }
    }
}
