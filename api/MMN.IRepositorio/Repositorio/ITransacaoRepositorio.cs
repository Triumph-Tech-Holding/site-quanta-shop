using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface ITransacaoRepositorio : IBaseRepositorio<Transacao>
    {
        void Delete(long key);
    }
}
