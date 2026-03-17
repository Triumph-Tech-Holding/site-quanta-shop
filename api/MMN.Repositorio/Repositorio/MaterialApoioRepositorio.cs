using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class MaterialApoioRepositorio : BaseRepositorio<MaterialApoio>, IMaterialApoioRepositorio
    {
        public MaterialApoioRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<MaterialApoio> BuscarMaterial()
        {
            return _ctx.MaterialApoio.Include("MaterialApoio").ToList();
        }

    }
}
