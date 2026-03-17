using System.Collections.Generic;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class PromocaoRepositorio : BaseRepositorio<Promocao>, IPromocaoRepositorio
    {
        public PromocaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
