using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class OrdenacaoAnuncioRepositorio : BaseRepositorio<OrdenacaoAnuncio>, IOrdenacaoAnuncioRepositorio
    {
        public OrdenacaoAnuncioRepositorio(DatabaseContext ctx) : base(ctx)
        {

        }
    }
}
