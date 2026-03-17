using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class ProvedorAutenticacaoRepositorio : BaseRepositorio<ProvedorAutenticacao>, IProvedorAutenticacaoRepositorio
    {
        public ProvedorAutenticacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
