using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class CarrosselRepositorio : BaseRepositorio<Carrossel>, ICarrosselRepositorio
    {
        public CarrosselRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
