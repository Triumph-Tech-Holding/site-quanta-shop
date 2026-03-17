using Newtonsoft.Json;

namespace MMN.Dominio.ViewModel
{
    public class ResumoDashAdminViewModel
    {
        [JsonProperty(PropertyName = "Compras pagas Rexbit")]
        public decimal TotalRexbit { get; set; }
        //[JsonProperty(PropertyName = "Compras pagas CrystalPay")]
        //public decimal TotalCrystal { get; set; }
        [JsonProperty(PropertyName = "Valor total saques finalizados")]
        public decimal TotalSaquesFinalizados { get; set; }
        [JsonProperty(PropertyName = "Valor total saques pendentes")]
        public decimal TotalSaquesPendentes { get; set; }
        [JsonProperty(PropertyName = "Valor total saques em aprovação")]
        public decimal TotalSaquesEmAprovacao { get; set; }
        [JsonProperty(PropertyName = "Valor total pago de binário")]
        public decimal TotalPagoBinario { get; set; }
        [JsonProperty(PropertyName = "Valor total pago de cashback")]
        public decimal TotalPagoCashback { get; set; }
        [JsonProperty(PropertyName = "Valor total pago direto")]
        public decimal TotalPagoDireto { get; set; }
        public decimal EntradasTotais { get; set; }
        public decimal SaidasTotais { get; set; }
    }
}
