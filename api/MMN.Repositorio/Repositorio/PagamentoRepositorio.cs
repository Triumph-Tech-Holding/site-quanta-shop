using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class PagamentoRepositorio : BaseRepositorio<Pagamento>, IPagamentoRepositorio
    {
        private readonly IProceduresRepositorio _procedures;
        public PagamentoRepositorio(
            IProceduresRepositorio procedures,
            DatabaseContext db) : base(db)
        {
            _procedures = procedures;
        }

        public IList<Pagamento> GetPagamentosUsuario(Guid idUsuario)
        {
            var pedidos = _ctx.Pedido
                .Where(p => 
                    p.IdUsuario == idUsuario 
                    && !string.IsNullOrEmpty(p.Codigo))
                .Include(p => p.Pagamentos);

            throw new NotImplementedException();
        }
    }
}
