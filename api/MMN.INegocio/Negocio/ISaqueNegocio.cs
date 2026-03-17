using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface ISaqueNegocio : IBaseNegocio<SaqueViewModel, Saque>
    {
        SaqueViewModel InserirPedidoSaque(SaqueViewModel viewModel);
        object BuscarPagamentos(FiltroViewModel.FiltroSaque model);
        ResumoSaqueViewModel ObterResumoSaque(DateTime dataInicio, DateTime dataFim);
        void AlterarStatusSaque(List<SaqueViewModel> listaIdsSaque, StatusTransacaoEnum status);
        void CancelarSaque(List<SaqueViewModel> lista);
        void AprovarSolicitacaoSaque(List<SaqueViewModel> lista, Guid idUsuarioLogado);
        Task<decimal> ObterConsumoSaque(Guid IdUsuario, DateTime dataInicio, DateTime dataFim);
        Task<Dictionary<Guid, decimal>> ObterConsumoSaqueBatch(List<Guid> idsUsuarios, DateTime dataInicio, DateTime dataFim);
    }
}

