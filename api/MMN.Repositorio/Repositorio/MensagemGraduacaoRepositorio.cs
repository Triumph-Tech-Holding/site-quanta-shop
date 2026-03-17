using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class MensagemGraduacaoRepositorio : BaseRepositorio<MensagemGraduacao>, IMensagemGraduacaoRepositorio
    {
        public MensagemGraduacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
