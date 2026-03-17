using System.Text.Json.Serialization;

namespace SimplesmenteSou.Utils.Models.Google
{
    public class GoogleUserIdModel
    {
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
        [JsonPropertyName("verified_email")]
        public bool VerifiedEmail { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("hd")]
        public string Hd { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

}
