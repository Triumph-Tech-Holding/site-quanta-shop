using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class ConfiguracaoRepositorio : BaseRepositorio<Configuracao>, IConfiguracaoRepositorio
    {
        public ConfiguracaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public Configuracao GetByKey(string chave)
        {
            return _ctx.Configuracao.FirstOrDefault(c => c.Chave.Equals(chave));
        }
    }
}
