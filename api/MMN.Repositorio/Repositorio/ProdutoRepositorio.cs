using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class ProdutoRepositorio : BaseRepositorio<Produto>, IProdutoRepositorio
    {
        public ProdutoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Produto> BuscarAtivos()
        {
            return _ctx.Produto.Include("Categoria").Where(p => p.Ativo).ToList();
        }

        public IList<Produto> BuscarTodos()
        {
            return _ctx.Produto.Include("Categoria").ToList();
        }

        public IList<LogProduto> ObterHistorico(long id)
        {
            var logs = _ctx.LogProduto.AsNoTracking().Where(l => l.IdProduto == id).OrderByDescending(l => l.DataAtualizacao).ToList();

            foreach (var log in logs)
            {
                log.Usuario = _ctx.Usuario.FirstOrDefault(u => u.IdUsuario == log.IdUsuario);
            }

            return logs;
        }

        public void SalvarLog(LogProduto log)
        {
            _ctx.LogProduto.Add(log);
            _ctx.SaveChanges();
        }
    }
}
