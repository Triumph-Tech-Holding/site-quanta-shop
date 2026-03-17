using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IUsuarioEnderecoRepositorio : IBaseRepositorio<UsuarioEndereco>
    {
        UsuarioEndereco ObterEnderecoCompleto(Guid idUsuario);
    }
}
