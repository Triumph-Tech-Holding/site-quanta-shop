using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface ICredenciamentoNegocio : IBaseNegocio<CredenciamentoViewModel, Credenciamento>
    {
        object FiltrarCredenciamento(FiltroViewModel.FiltroCredenciamento filtroCredenciamento);
        UsuarioViewModel CredenciarNovoUsuario(CredenciarNovoUsuarioViewModel viewModel);
        Credenciamento Credenciar(CredenciarViewModel viewModel, Guid IdUsuarioLogado);
        void Atualizar(CredenciamentoViewModel viewModel);
        void EnviarEmailCredenciamento(long IdCredenciamento);
        void EditarAnunciante(LojasCredenciadoViewModel viewModel, Guid IdUsuarioLogado, long IdCredenciamento);
        Task<(bool status, string message)> Credenciar(NovoCredenciamentoViewModel cupom);
    }
}