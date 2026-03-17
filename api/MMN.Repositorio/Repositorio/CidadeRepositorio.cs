using System.Collections.Generic;
using System.Linq;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class CidadeRepositorio : BaseRepositorio<Cidade>, ICidadeRepositorio
    {
        public CidadeRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Cidade> BuscarCidadeEstado(int idEstado)
        {
            return _ctx.Set<Cidade>().Where(c => c.IdEstado == idEstado).ToList();
        }
    }
}
