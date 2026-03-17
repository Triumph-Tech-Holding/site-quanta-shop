using Newtonsoft.Json;

namespace MMN.Util.Jwt
{
    [JsonObject("tokenManagement")]
    public class TokenManagement
    {
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }
    }
}
