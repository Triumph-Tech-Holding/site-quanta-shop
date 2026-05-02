using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.Integracoes.Afilio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IAnuncianteNegocio : IBaseNegocio<AnuncianteViewModel, Anunciante>
    {
        Task<object> BuscarAnunciantes(FiltroViewModel.AnunciantePaginado filtro);
        IEnumerable<AnuncianteViewModel> BuscarAnunciantesAleatorio();
        Task<IList<MMN.Integracoes.Afilio.Cupom>> GetCuponsFromCache();
        List<AnuncianteViewModel> BuscarCashback(List<AnuncianteViewModel> anunciantes);
        AnuncianteViewModel BuscarCashbackAnunciante(AnuncianteViewModel anunciante);
        void EditarAnunciante(AnuncianteViewModel viewModel, Guid IdUsuarioLogado);
        Task<IEnumerable<OrdenacaoAnuncioViewModel>> ObterOrdenacaoAnunciosFromCashAsync();
        Task<string> GerarUrlAsync(long? anuncianteId);
    }
}
