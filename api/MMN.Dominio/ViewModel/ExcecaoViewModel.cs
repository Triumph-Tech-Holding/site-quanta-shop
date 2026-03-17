using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMN.Dominio.ViewModel
{
    public class ExcecaoViewModel
    {
        [JsonPropertyName("erros")]
        public IEnumerable<MensagemErroViewModel> Erros { get; set; }
    }
}
