using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface ILancamentoNegocio : IBaseNegocio<LancamentoViewModel, Lancamento>
    {
        IList<LancamentoViewModel> BuscarPorIdUsuario(FiltroViewModel.BuscarExtrato model, Guid idUsuario);
        object GerarLancamentoManual(LancamentoManualViewModel viewModel, Guid idUsuario);
        object ObterSaldoPorTipo(Guid idUsuario);
        IList<LancamentoViewModel> ObterLancamentos(Guid idUsuario, int tipoLancamento);
        object ObterCashbackDetalahdo();
        object ObterLancamentosAdmin(FiltroViewModel.FiltroLancamento filtros, Guid idUsuarioLogado);
        LancamentoViewModel GerarLancamentoTaxaSaldo(Pedido pedido);
    }
}
