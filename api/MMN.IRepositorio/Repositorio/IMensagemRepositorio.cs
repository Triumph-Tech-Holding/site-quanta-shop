using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IMensagemRepositorio : IBaseRepositorio<Mensagem>
    {
        IList<Mensagem> BuscarPorIdUsuario(Guid idUsuario);

        IList<Mensagem> BuscarPorIdUsuarioETipo(Guid idUsuario, long idTipo);
        IList<Mensagem> BuscarComunicados();
    }
}
