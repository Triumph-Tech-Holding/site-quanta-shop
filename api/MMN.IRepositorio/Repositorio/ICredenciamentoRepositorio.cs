using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.IRepositorio.Repositorio
{
    public interface ICredenciamentoRepositorio : IBaseRepositorio<Credenciamento>
    {
        object BuscarPorEstabelecimento(FiltroCredenciamento filtroCredenciamento);

        Task<(bool status, string message)> Credenciar(NovoCredenciamentoViewModel cupom);
    }
}
