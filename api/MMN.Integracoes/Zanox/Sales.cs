using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class Sales
    {
        public int page { get; set; }
        public int items { get; set; }
        public int total { get; set; }
        public SaleItems saleItems { get; set; }
    }
    public class SaleItems
    {
        public List<SaleItem> saleItem { get; set; }
    }

    public class SaleItem
    {
        [JsonPropertyName("@id")]
        public Guid idSale { get; set; }
        public string reviewState { get; set; }
        public decimal amount { get; set; }
        public decimal commission { get; set; }
        public string currency { get; set; }
        //[JsonConverter(typeof(DateTimeConverter))]
        public DateTime trackingDate { get; set; }
        public Program program { get; set; }
        public Gpps gpps { get; set; }
    }

    public class Gpps
    {
        [JsonConverter(typeof(GppJsonConverter))]
        public Gpp gpp { get; set; }
    }

    public class Gpp
    {
        [JsonPropertyName("$")]
        public string IdUsuario { get; set; }
        public bool Processar { get; set; }
        public Gpp(string idusuario, bool processar)
        {
            IdUsuario = idusuario;
            Processar = processar;
        }
    }
}
