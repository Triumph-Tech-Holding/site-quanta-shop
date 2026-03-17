using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class FaqRepositorio : BaseRepositorio<Faq>, IFaqRepositorio
    {
        public FaqRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Faq> BuscarFaq()
        {
            return _ctx.Faq.Include("FAQ").ToList();
        }
    }
}
