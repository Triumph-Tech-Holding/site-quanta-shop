using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Transacao
    {
        public long IdTransacao { get; set; }
        public Guid IdUsuario { get; set; }
        public int IdTipo { get; set; }
        public decimal ValorPrincipal { get; set; }
        public DateTime DataTransacao { get; set; }
        public string Descricao { get; set; }
        public int IdStatus { get; set; }
        public bool Ativo { get; set; }
        public Guid? IdVendaZanox { get; set; }
        public int? IdAnunciante { get; set; }
        public decimal? ComissaoTotal { get; set; }
        public long? IdVendaAfilio { get; set; }
        public long? IdVendaAwin { get; set; }
        public DateTime? DataReferencia { get; set; }
        public virtual Status Status { get; set; }
        public virtual Tipo Tipo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Anunciante Anunciante { get; set; }
        public virtual ICollection<Lancamento> Lancamento { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Saque> Saque { get; set; }
    }
}
