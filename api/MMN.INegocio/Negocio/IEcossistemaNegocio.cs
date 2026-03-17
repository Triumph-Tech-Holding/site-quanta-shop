using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


namespace MMN.INegocio.Negocio
{
    public interface IEcossistemaNegocio : IBaseNegocio<EcossistemaViewModel, Ecossistema>
    {
        Task<IEnumerable<Ecossistema>> BuscarEcossistemas(FiltroViewModel.FiltroEcossistemas filtro);
        Task<Ecossistema> BuscarEcossistemaPorId(int id);
        Task CriarEcossistema(Ecossistema ecossistema);
        Task AtualizarEcossistema(Ecossistema ecossistema);
        Task DeletarEcossistema(int id);
    }
}
