using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Graduacao
    {
        public int IdGraduacao { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public int? Pontos { get; set; }
        public string Image { get; set; }
        public string Premio { get; set; }
        public int? PercentualPremiacao { get; set; }
        public int? QuantidadeDiretos { get; set; }
        public virtual ICollection<HistoricoGraduacao> HistoricoGraduacao { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
        public virtual ICollection<LogGraduacao> LogGraduacao { get; set; }
        public virtual ICollection<MensagemGraduacao> MensagemGraduacao { get; set; }
        public virtual ICollection<PremiacaoDownline> PremiacaoDownline { get; set; }
        public virtual ICollection<GraduacaoRequisitos> GraduacaoRequisitos { get; set; }
        public virtual ICollection<GraduacaoRequisitos> GraduacaoRequisitosSecundario { get; set; }
        public virtual ICollection<UsuarioPremiacao> UsuarioPremiacao { get; set; }
        public virtual ICollection<AuditoriaPremiacao> AuditoriaPremiacao { get; set; }
    }
}
