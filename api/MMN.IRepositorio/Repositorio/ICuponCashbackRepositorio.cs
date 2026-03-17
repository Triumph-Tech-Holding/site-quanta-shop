using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using MMN.Dominio.ViewModel;

namespace MMN.IRepositorio.Repositorio
{
    public interface ICuponCashbackRepositorio : IBaseRepositorio<CupomCashback>
    {
        Task<(bool status, string message)> CriarDadosNF(CupomCashbackDadosNF dadosNF);
        Task<CupomCashback> BuscarPelaChaveDeAcessoAsync(string chaveDeAcesso);
        Task<CupomCashback> BuscarPeloTokenAsync(string token);
        Task<object> ObterCuponsCompraUsuarioCredenciado(FiltroViewModel.FiltroVendasCredenciando viewModel);
        void Delete(string key);
    }
}
