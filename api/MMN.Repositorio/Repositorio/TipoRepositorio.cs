using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class TipoRepositorio : BaseRepositorio<Tipo>, ITipoRepositorio
    {
        public TipoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Tipo> ObterTipoPorChave(string chave)
        {
            return _ctx.Tipo.Where(t => t.IdTipoPai.Equals(_ctx.Tipo.FirstOrDefault(x => x.Chave.Equals(chave)).IdTipo) && t.Ativo )
                .ToList();
        }
    }
}
