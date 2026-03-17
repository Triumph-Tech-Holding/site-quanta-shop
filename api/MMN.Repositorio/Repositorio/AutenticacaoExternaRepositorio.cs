using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class AutenticacaoExternaRepositorio : BaseRepositorio<AutenticacaoExterna>, IAutenticacaoExternaRepositorio
    {
        public AutenticacaoExternaRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
