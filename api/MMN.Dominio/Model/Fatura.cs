using System;

namespace MMN.Dominio.Model
{
    public class Fatura
    {
        public long IdFatura { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public string UrlBoleto { get; set; }
        public string CodigoBoleto { get; set; }
        public Guid IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}