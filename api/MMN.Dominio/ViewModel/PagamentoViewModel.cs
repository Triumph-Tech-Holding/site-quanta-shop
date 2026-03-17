using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class PagamentoViewModel
    {
        public int IdPagamento { get; set; }
        public int Status { get; set; }
        public long IdPedido { get; set; }
        public bool Pago { get; set; }
        public bool Ativo { get; set; }
        public decimal Valor { get; set; }
        public int NumeroParcela { get; set; }
        public string CodigoReferenciaBoleto { get; set; }
        public string UrlBoleto { get; set; }
        public string LinhaDigitavelBoleto { get; set; }
        public DateTime DataValidade { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataReferencia { get; set; }
        public string CodigoReferenciaAssinatura { get; set; }
    }
}
