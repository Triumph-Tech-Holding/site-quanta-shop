using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IProdutoRepositorio : IBaseRepositorio<Produto>
    {
        IList<Produto> BuscarAtivos();
        IList<Produto> BuscarTodos();
        IList<LogProduto> ObterHistorico(long id);
        public void SalvarLog(LogProduto log);
    }
}
