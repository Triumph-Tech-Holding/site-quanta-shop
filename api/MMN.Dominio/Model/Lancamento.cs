using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Lancamento
    {
        public long IdLancamento { get; set; }
        public Guid IdUsuario { get; set; }
        public long IdTransacao { get; set; }
        public int IdTipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public int? IdStatus { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataReferencia { get; set; }
        public int OrdemExibicao { get; set; }
        public virtual Status Status { get; set; }
        public virtual Tipo Tipo { get; set; }
        public virtual Transacao Transacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<LancamentoRetido> LancamentoRetido { get; set; }
    }
}
