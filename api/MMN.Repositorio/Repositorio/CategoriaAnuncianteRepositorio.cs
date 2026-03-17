using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class CategoriaAnuncianteRepositorio : BaseRepositorio<CategoriaAnunciante>, ICategoriaAnuncianteRepositorio
    {
        public CategoriaAnuncianteRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
