using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class HistoricoGraduacaoRepositorio : BaseRepositorio<HistoricoGraduacao>, IHistoricoGraduacaoRepositorio
    {
        public HistoricoGraduacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
