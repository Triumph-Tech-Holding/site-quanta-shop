namespace MMN.Dominio.ViewModel
{
    public class OrdenacaoAnuncioViewModel
    {
        public long? IdCredenciamento { get; set; }
        public int? IdAnunciante { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public string TipoCashback { get; set; }
        public decimal? Cashback { get; set; }
        public decimal? CashbackMin { get; set; }
        public decimal? CashbackMax { get; set; }
        public string UrlAnuncio { get; set; }
        public decimal? Ordenacao { get; set; }
        public bool ParceiroOnline { get; set; }
        public int? IdCategoria { get; set; }
    }
}
