using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using MMN.Dominio.ViewModel;

namespace MMN.IRepositorio.Repositorio
{
    public interface ILancamentoRepositorio : IBaseRepositorio<Lancamento>
    {
        IList<Lancamento> BuscarPorIdUsuario(FiltroViewModel.BuscarExtrato model, Guid idUsuario);
        void GerarLancamentoManual(UsuarioViewModel usuario, LancamentoManualViewModel viewModel, UsuarioViewModel usuarioLogado, List<UsuarioViewModel> listUsuarioDireto);
        decimal ValorCashback(Guid idUsuario, int[] tipoCashback);
        decimal ValorAdesao(Guid idUsuario, int tipoAdesao);
        decimal TotalEntradas(Guid idUsuario);
        decimal TotalSaidas(Guid idUsuario);
    }
}
