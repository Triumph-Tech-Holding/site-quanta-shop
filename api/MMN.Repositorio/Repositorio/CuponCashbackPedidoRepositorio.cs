using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class CuponCashbackPedidoRepositorio : BaseRepositorio<CuponCashbackPedido>, ICuponCashbackPedidoRepositorio
    {
        public CuponCashbackPedidoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public void Delete(string key)
        {
            var entity = _ctx.CuponCashbackPedido.FirstOrDefault(ccp => ccp.IdCuponCashback == key);
            _ctx.Remove(entity);
        }
    }
}
