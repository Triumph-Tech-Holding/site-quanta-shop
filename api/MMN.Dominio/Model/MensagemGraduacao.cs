using System;

namespace MMN.Dominio.Model
{
    public class MensagemGraduacao
    {
        public long IdMensagemGraduacao { get; set; }
        public int IdMensagem { get; set; }
        public int IdGraduacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public virtual Mensagem Mensagem { get; set; }
        public virtual Graduacao Graduacao { get; set; }
    }
}