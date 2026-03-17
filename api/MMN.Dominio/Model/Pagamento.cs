using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public int Status { get; set; }
        public bool Ativo { get; set; }
        public long? IdPedido { get; set; }
        public bool Pago { get; set; }
        public decimal Valor { get; set; }
        public int NumeroParcela { get; set; }
        public DateTime DataValidade { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string CodigoReferenciaBoleto { get; set; }
        public string UrlBoleto { get; set; }
        public string LinhaDigitavelBoleto { get; set; }
        public DateTime? DataReferencia { get; set; }

        public virtual Pedido Pedido{ get; set; }
        public virtual ICollection<PagamentoPedido> PagamentoPedido { get; set; }
    }
}
