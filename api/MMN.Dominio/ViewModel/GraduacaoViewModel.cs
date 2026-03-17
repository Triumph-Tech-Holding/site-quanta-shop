using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class GraduacaoViewModel
    {
        public int IdGraduacao { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public int? Pontos { get; set; }
        public string Image { get; set; }
        public string Premio { get; set; }
        public int? PercentualPremiacao { get; set; }
        public int? QuantidadeDiretos { get; set; }
        //public ICollection<HistoricoGraduacaoViewModel> HistoricoGraduacao { get; set; }
        public ICollection<UsuarioViewModel> UsuarioViewModel { get; set; }
        //public ICollection<LogGraduacaoViewModel> LogGraduacao { get; set; }
        public ICollection<MensagemGraduacaoViewModel> MensagemGraduacaoViewModel { get; set; }
        public ICollection<PremiacaoDownlineViewModel> PremiacaoDownlineViewModel { get; set; }
        public virtual ICollection<GraduacaoRequisitosViewModel> GraduacaoRequisitos { get; set; }
    }
}
