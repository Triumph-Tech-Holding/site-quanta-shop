using Microsoft.EntityFrameworkCore.Storage;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.IRepositorio.Repositorio
{
    public interface IPedidoRepositorio : IBaseRepositorio<Pedido>
    {
        Pedido ObterPedidoCompleto(string code);
        IList<Pedido> ObterPorUsuario(Guid idUsuario);
        Pedido Comprar(Pedido pedido, Int32? idProduto, EnumTipoTransacao? tipoTransacao, Guid idUsuarioLogado);
        Pedido Assinar(Pedido pedido, Int32? idProduto, EnumTipoTransacao? tipoTransacao, Guid idUsuarioLogado);
        bool ProcessarPagamento(FiltroViewModel.ConfirmarPagamento filtro);
        List<PedidosProcedureViewModel> BuscarPedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario);
        List<Pedido> VerificarPedidoBitCoin();
        Pedido BuscarPorCodigo(string codigo);
        public object TotalConsumoPlanos();
        Task<bool> CriarPedidoCredenciado(EfetuarCompraViewModel viewModel, Guid IdUsuario, CupomCashback token);
        Task<bool> EfetuarCompraComerciante(EfetuarCompraViewModel viewModel, Guid IdUsuario, CupomCashback token = null);
        public void DistribuirParcelaPlano(PagamentoViewModel pagamento, IDbContextTransaction dbTransaction = null);
        public void Delete(long key);        
    };
}
