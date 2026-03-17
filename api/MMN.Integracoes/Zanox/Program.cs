using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class Program
    {
        [JsonPropertyName("@active")]
        public string active { get; set; }
        [JsonPropertyName("@id")]
        public string id { get; set; }
        [JsonPropertyName("$")]
        public string name { get; set; }
    }
}
