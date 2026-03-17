using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class TransacaoRepositorio : BaseRepositorio<Transacao>, ITransacaoRepositorio
    {
        public TransacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public void Delete(long key)
        {
            var entity = _ctx.Transacao.FirstOrDefault(ccp => ccp.IdTransacao == key);
            _ctx.Remove(entity);
        }
    }
}
