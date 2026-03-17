using System;

namespace MMN.Dominio.ViewModel
{
    public class MensagemGraduacaoViewModel
    {
        public long IdMensagemGraduacao { get; set; }
        public int IdMensagem { get; set; }
        public int IdGraduacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public MensagemViewModel MensagemViewModel { get; set; }
        public GraduacaoViewModel GraduacaoViewModel { get; set; }
    }
}
