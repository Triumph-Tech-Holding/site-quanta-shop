using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class QuantaAmizadeHistoricoRepositorio : BaseRepositorio<QuantaAmizadeHistorico>, IQuantaAmizadeHistoricoRepositorio
    {
        public QuantaAmizadeHistoricoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
