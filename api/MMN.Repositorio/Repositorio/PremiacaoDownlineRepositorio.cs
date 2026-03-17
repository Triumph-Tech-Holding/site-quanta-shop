using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class PremiacaoDownlineRepositorio : BaseRepositorio<PremiacaoDownline>, IPremiacaoDownlineRepositorio
    {
        public PremiacaoDownlineRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
