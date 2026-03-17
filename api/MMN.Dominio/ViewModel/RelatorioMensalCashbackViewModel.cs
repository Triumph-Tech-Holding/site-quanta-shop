namespace MMN.Dominio.ViewModel
{
    public class RelatorioMensalCashbackViewModel
    {
        public decimal CashbackAPagar { get; set; }
        public decimal CashbackPago { get; set; }
        public int TipoPedido { get; set; }
        public string DescricaoTipoPedido { get; set; }
        public int IdStatusPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public int IdTipoLancamento { get; set; }
        public string TipoLancamento { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
    }
}
