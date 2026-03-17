using System.Text.Json.Serialization;

namespace MMN.Dominio.ViewModel
{
    public class MensagemErroViewModel
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }
        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }
    }
}
