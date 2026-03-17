using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class ProdutoNivelRepositorio : BaseRepositorio<ProdutoNivel>, IProdutoNivelRepositorio
    {
        public ProdutoNivelRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
