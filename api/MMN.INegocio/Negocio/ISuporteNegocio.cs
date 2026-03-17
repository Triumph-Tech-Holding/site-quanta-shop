using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.Util.Model;
using System;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface ISuporteNegocio : IBaseNegocio<SuporteViewModel, Suporte>
    {
        object FiltrarSolicitacoes(FiltroViewModel.FiltroSuporte viewModel);
        object ObterMinhasSolicitacoes(Guid idUsuario, int? idStatus);
        Task<bool> SolicitarCashbackNaoPago(SuporteViewModel viewModel, UsuarioViewModel usuario, ObjEmailUtilitis objectEmail);
        Task<bool> SolicitarCancelamentoParcela(SuporteViewModel viewModel, UsuarioViewModel usuario, ObjEmailUtilitis objectEmail);
    }
}
