using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IPagamentoRepositorio : IBaseRepositorio<Pagamento>
    {
        IList<Pagamento> GetPagamentosUsuario(Guid idUsuario);
    }
}
