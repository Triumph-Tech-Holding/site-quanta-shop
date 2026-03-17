using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioBancoRepositorio : BaseRepositorio<UsuarioBanco>, IUsuarioBancoRepositorio
    {
        public UsuarioBancoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
