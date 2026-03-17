using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioConfiguracaoRepositorio : BaseRepositorio<UsuarioConfiguracao>, IUsuarioConfiguracaoRepositorio
    {
        public UsuarioConfiguracaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
