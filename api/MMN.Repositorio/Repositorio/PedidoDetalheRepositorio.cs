using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class PedidoDetalheRepositorio : BaseRepositorio<PedidoDetalhe>, IPedidoDetalheRepositorio
    {
        public PedidoDetalheRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
