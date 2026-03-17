using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Status
    {
        public Status()
        {
            Lancamento = new HashSet<Lancamento>();
            Transacao = new HashSet<Transacao>();
        }

        public int IdStatus { get; set; }
        public string Nome { get; set; }
        public string ChaveTraducao { get; set; }
        public bool? Ativo { get; set; }

        public virtual ICollection<Lancamento> Lancamento { get; set; }
        public virtual ICollection<Transacao> Transacao { get; set; }
        public virtual ICollection<Saque> Saque { get; set; }
        public virtual ICollection<PedidoDetalhe> PedidoDetalhe { get; set; }
        public virtual ICollection<Suporte> Suporte { get; set; }
    }
}
