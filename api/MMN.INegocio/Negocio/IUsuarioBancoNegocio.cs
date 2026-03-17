using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;

namespace MMN.INegocio.Negocio
{
    public interface IUsuarioBancoNegocio : IBaseNegocio<UsuarioBancoViewModel, UsuarioBanco>
    {
        bool CadastrarUsuarioBanco(UsuarioBancoViewModel viewModel, Guid IdUsuario);
    }
}
