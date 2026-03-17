using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class PagamentoPedidoRepositorio : BaseRepositorio<PagamentoPedido>, IPagamentoPedidoRepositorio
    {
        public PagamentoPedidoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
