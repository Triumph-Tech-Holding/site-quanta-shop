using Newtonsoft.Json;

namespace MMN.Util.Models.Response.Asaas
{
    public partial class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}