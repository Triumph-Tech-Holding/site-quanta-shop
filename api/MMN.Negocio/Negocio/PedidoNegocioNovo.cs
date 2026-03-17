using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Base;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class PedidoNegocioNovo : BaseNegocioNovo<Pedido>, IPedidoNegocioNovo
    {
        public PedidoNegocioNovo(IBaseRepositorio<Pedido> baseRepositorio) : base(baseRepositorio)
        {

        }
    }
}
