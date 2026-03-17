using System;

namespace MMN.Api.ViewModel.Fatura
{
    public class ListaFaturaViewModel
    {
        public long IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public int Status { get; set; }
        public decimal ValorPedido { get; set; }
        public string UrlBoleto { get; set; }
    }
}
