using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioPremiacaoRepositorio : BaseRepositorio<UsuarioPremiacao>, IUsuarioPremiacaoRepositorio
    {
        public UsuarioPremiacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
