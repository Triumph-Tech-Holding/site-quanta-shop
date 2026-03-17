using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class FaturaNegocioNovo : IFaturaNegocioNovo
    {
        private IPedidoRepositorio _pedidoRepositorio;

        public FaturaNegocioNovo(
            IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task<IEnumerable<Pedido>> ObterFaturasAsync(Guid idUsuarioLogado)
        {
            var pedidos = _pedidoRepositorio.GetNoTracking(
                g =>
                    g.IdUsuario == idUsuarioLogado &&
                    g.Tipo == (int)TipoPedido.FaturaCashbackCredenciado,
                "Pagamentos.PagamentoPedido.Pedido");


            return pedidos.ToArray();
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosAguardandoFaturaAsync(DateTime ateData, Guid idUsuarioLogado)
        {
            var pedidos = _pedidoRepositorio.GetNoTracking(g =>
              g.IdUsuarioComerciante == idUsuarioLogado &&
              g.Ativo == true &&
              g.Status == (int)StatusPedido.AguardandoFaturaCredenciado &&
              g.Tipo == (int)TipoPedido.CashbackCredenciado &&
              g.DataPedido < ateData.HorarioBrasilia().AddDays(1).Date);

            return pedidos.ToArray();
        }
    }
}
