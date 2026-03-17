using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using System.Linq;
using System.Collections.Generic;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class GraduacaoRepositorio : BaseRepositorio<Graduacao>, IGraduacaoRepositorio
    {
        public GraduacaoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public Graduacao ObterPorNivel(int nivel)
        {
            return _ctx.Graduacao.FirstOrDefault(g => g.Nivel == nivel);
        }

        public Graduacao ObterMenorNivel()
        {
            return _ctx.Graduacao.OrderBy(g => g.Nivel).FirstOrDefault();
        }
    }
}
