using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class AlteracaoPerfilRepositorio : BaseRepositorio<AlteracaoPerfil>, IAlteracaoPerfilRepositorio
    {
        public AlteracaoPerfilRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
