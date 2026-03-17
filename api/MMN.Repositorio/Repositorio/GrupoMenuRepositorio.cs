using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class GrupoMenuRepositorio : BaseRepositorio<GrupoMenu>, IGrupoMenuRepositorio
    {
        public GrupoMenuRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
