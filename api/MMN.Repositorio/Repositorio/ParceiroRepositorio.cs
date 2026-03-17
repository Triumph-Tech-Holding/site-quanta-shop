using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class ParceiroRepositorio : BaseRepositorio<Parceiro>, IParceiroRepositorio
    {
        public ParceiroRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
