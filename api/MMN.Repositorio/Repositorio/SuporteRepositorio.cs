using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class SuporteRepositorio : BaseRepositorio<Suporte>, ISuporteRepositorio
    {
        public SuporteRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
