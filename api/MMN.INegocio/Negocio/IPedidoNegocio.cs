using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.Util.Enum;
using MMN.Util.Model.LionBit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IPedidoNegocio : IBaseNegocio<PedidoViewModel, Pedido>
    {
        public Pedido AdicionarParcela(long idPedido);

        Pedido AdquirirPlano(AdquirirPlanoViewModel model, Guid idUsuarioLogado);

        Pedido AdquirirPlanoMensal(AdquirirPlanoViewModel model, Guid idUsuarioLogado);
        Pedido AdquirirAssinaturaAnual(AdquirirAssinaturaAnualViewModel model, Guid idUsuarioLogado);

        Pedido AdquirirSaldo(AdquirirSaldoViewModel model, Guid idUsuarioLogado);

        public bool AlterarValorParcela(int idPedido, int numeroParcela, decimal valor);

        public bool ApagarParcela(int idPedido, int numeroParcela);

        public void AtivarProduto(int idProduto, string login, decimal valorPedido, bool distribuiNaRede, bool GerarPontos);

        object BuscarProdutoPorCodigo(string codigo, Guid idUsuario);

        DadosRetorno<string> CancelarAssinatura(Guid idUsuarioLogado, string tipo, bool manual = false);

        bool CancelarPedido(long idPedido, Guid idUsuario);

        Task<Pedido> CriarFaturaCashbackCredenciadoAsync(DateTime ateData, EnumTipoPagamento tipoPagamento, Guid idUsuarioLogado);

        public Pedido EditarPedido(PedidoViewModel pedido);

        Task<bool> EfetuarCompraComerciante(EfetuarCompraViewModel viewModel, Guid IdUsuario);

        PedidoViewModel GerarPedidoManual(GerarPedidoManualViewModel model, Guid idUsuarioLogado);

        public bool GetAssinatura(Guid idUsuarioLogado);

        List<PedidosProcedureViewModel> ListarPedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario);

        IList<PedidoViewModel> ListarPedidosAfiliados(FiltroViewModel.PedidoAfiliados filtro, Guid idUsuario);

        public object ListarPedidosAfiliadosAdmin(FiltroViewModel.PedidoAfiliadosAdmin viewModel);

        Pedido ObterDetalhes(string codigo);
        public Pedido ObterPedidoVigente(Guid idUsuario);

        public decimal ObterValorPagoBaf(Guid idUsuario);

        public bool PagarParcela(long idPedido, int numeroParcela, DateTime? dataReferencia, bool distribuirNaRede = false);

        bool ReativarPedido(long idPedido, Guid idUsuario);
        Task RemoverCompra(long idPedido);

        public Pedido RemoverParcela(long idPedido);

        public bool RenovarBoleto(int idPedido, int numeroParcela);

        public object TotalConsumoPlanos();
    }
}