using System;

namespace MMN.Dominio.Model
{
    public class Saque
    {
        public int IdSaque { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal Valor { get; set; }
        public string EnderecoBTC { get; set; }
        public bool Processado { get; set; }
        public DateTime? DataProcessado { get; set; }
        public decimal TaxaSaque { get; set; }
        public decimal? Cotacao { get; set; }
        public Guid IdUsuario { get; set; }
        public int IdStatus { get; set; }
        public int IdTipo { get; set; }
        public long IdTransacao { get; set; }
        public string UrlTransacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public string Aprovador { get; set; }
        public string Historico { get; set; }
        public int IdUsuarioBanco { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Status Status { get; set; }
        public virtual Tipo Tipo { get; set; }
        public virtual Transacao Transacao { get; set; }
        public UsuarioBanco UsuarioBanco { get; set; }
    }
}
