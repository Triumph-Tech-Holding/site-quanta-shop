using System;

namespace MMN.Dominio.Model
{
    public class Faq
    {
        public int IdFaq { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
