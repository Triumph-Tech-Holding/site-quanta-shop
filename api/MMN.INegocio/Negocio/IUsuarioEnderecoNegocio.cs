using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;

namespace MMN.INegocio.Negocio
{
    public interface IUsuarioEnderecoNegocio : IBaseNegocio<UsuarioEnderecoViewModel, UsuarioEndereco>
    {
        UsuarioEnderecoViewModel ObterEnderecoCompleto(Guid idUsuario);
        bool CadastrarEndereco(UsuarioEnderecoViewModel model);
        bool EditarEndereco(UsuarioEnderecoViewModel model);
    }
}
