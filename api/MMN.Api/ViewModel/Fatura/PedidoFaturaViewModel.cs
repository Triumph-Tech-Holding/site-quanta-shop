using System;

namespace MMN.Api.ViewModel.Fatura
{
    public class PedidoFaturaViewModel
    {
        public long IdPedido { get; set; }
        public string Codigo { get; set; }
        public int Status { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataReferencia { get; set; }
        public decimal ValorPedido { get; set; }
        public int MeioPagamento { get; set; }
        public decimal? PercentualCashback { get; set; }
    }
}
