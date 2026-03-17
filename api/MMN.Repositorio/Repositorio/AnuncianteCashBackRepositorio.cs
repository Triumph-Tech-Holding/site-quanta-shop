using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class AnuncianteCashBackRepositorio : BaseRepositorio<AnuncianteCashBack>, IAnuncianteCashBackRepositorio
    {
        public AnuncianteCashBackRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
