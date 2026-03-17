using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System.Threading.Tasks;

namespace MMN.IRepositorio.Repositorio
{
    public interface ICuponCashbackPedidoRepositorio : IBaseRepositorio<CuponCashbackPedido>
    {
        void Delete(string key);
    }
}
