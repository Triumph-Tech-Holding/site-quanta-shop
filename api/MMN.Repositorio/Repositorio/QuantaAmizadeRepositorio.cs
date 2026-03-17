using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class QuantaAmizadeRepositorio : BaseRepositorio<QuantaAmizade>, IQuantaAmizadeRepositorio
    {
        public QuantaAmizadeRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
