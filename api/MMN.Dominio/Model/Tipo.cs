using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Tipo
    {
        public Tipo()
        {
            Lancamento = new HashSet<Lancamento>();
            Transacao = new HashSet<Transacao>();
        }

        public int IdTipo { get; set; }
        public int? IdTipoPai { get; set; }
        public string Descricao { get; set; }
        public string Chave { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Lancamento> Lancamento { get; set; }

        public virtual ICollection<Transacao> Transacao { get; set; }

        public virtual ICollection<Saque> Saque { get; set; }
    }
}
