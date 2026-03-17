namespace MMN.Integracoes.Afilio
{
    public class RootVenda
    {
        public Venda Venda { get; set; }
    }
    public class Venda
    {
        public string saleid { get; set; }
        public string siteid { get; set; }
        public string status { get; set; }
        public string progid { get; set; }
        public string order_id { get; set; }
        public string order_price { get; set; }
        public string comission { get; set; }
        public string date { get; set; }
        public string payment { get; set; }
        public string aff_xtra { get; set; }
    }
}
