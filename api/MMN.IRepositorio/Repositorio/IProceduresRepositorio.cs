using Microsoft.EntityFrameworkCore.Storage;
using MMN.Dominio.ViewModel;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IProceduresRepositorio
    {
        BarraDeStatusViewModel spc_obterBarraDeStatus(Guid idUsuario);
        PerfilPainelViewModel spc_obterPerfilPainel(Guid idUsuario);
        List<PerformanceRedeViewModel> spc_PerformanceRede(Guid idUsuario, string nome = "", string login = "");
        List<LancamentoRedeUsuarioViewModel> spc_LancamentoRedeUsuario(Guid idUsuario);
        List<PedidosProcedureViewModel> spc_Pedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario);
        LimitesGanhosViewModel spc_LimitesGanhos(object idUsuario);
        ResumoSaqueViewModel spc_ResumoSaque(DateTime dataInicio, DateTime dataFim);
        IList<UsuarioDownLineViewModel> spc_UsuarioDownLine(Guid idUsuario);
        IList<RankUsuarioViewModel> spc_ObterRankUsuario(Guid idUsuario);
        IList<RankUsuarioViewModel> spc_ObterRankUsuarioFiltrado(Guid idUsuario, string login, string ordenacao);
        void spc_LancarCashback(long idPedido, decimal totalCashback, IDbContextTransaction transaction = null);
        void spc_AtualizaGraduacao();
        decimal spc_ObterPontuacaoUsuarioPremiacao(Guid idUsuario, int porcentagem, int totalPontos, int idGraduacao);
        public void spc_DistribuicaoParcela(long idPedido, long idPagamento, IDbContextTransaction transaction = null);
        public void spc_DistribuicaoPlanoAdquirido(long idPedido);
        public PontuacaoPorValorViewModel spc_GetPontuacaoUsuarioPorValor(Guid idUsuario);
        public List<RelatorioMensalCashbackViewModel> spc_RelatorioMensalCashback(DateTime? dataInicial = null, DateTime? datafinal = null);
        void spc_PagamentoComSaldo(Guid idCupomCashback);
        void sp_InsertQuantaAmizade(Guid idUsuario, IDbContextTransaction transaction = null);
        void sp_ObjetivoIndicacao(Guid idUsuario, IDbContextTransaction transaction = null);
        decimal fnc_ObterResumoConsumoMensal(Guid idCupomCashback, DateTime dataInicial, DateTime dataFinal);
        void sp_LancarCashback_Assinatura(int usuarioProdutoId, IDbContextTransaction transaction = null);
    }
}
