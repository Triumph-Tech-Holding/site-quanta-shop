using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface ISaqueRepositorio : IBaseRepositorio<Saque>
    {
        Saque InserirSaque(Saque model);
        List<Saque> BuscarPagamentos(FiltroViewModel.FiltroSaque model, out int totalPages);
        void CancelarSaque(List<SaqueViewModel> lista);
    }
}
