using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class Adspace
    {
        [JsonPropertyName("@id")]
        public string id { get; set; }
    }
}
