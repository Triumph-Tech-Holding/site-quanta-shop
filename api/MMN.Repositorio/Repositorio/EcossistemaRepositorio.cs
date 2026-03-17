using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class EcossistemaRepositorio : BaseRepositorio<Ecossistema>, IEcossistemaRepositorio
    {
        public EcossistemaRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
