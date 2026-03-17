using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class AnuncianteRepositorio : BaseRepositorio<Anunciante>, IAnuncianteRepositorio
    {
        public AnuncianteRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
