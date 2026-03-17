using System.Linq;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class GrupoRepositorio : BaseRepositorio<Grupo>, IGrupoRepositorio
    {
        public GrupoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public Grupo GetByName(string nome)
        {
            return _ctx.Grupo.FirstOrDefault(g => g.Descricao.Equals(nome));
        }
    }
}
