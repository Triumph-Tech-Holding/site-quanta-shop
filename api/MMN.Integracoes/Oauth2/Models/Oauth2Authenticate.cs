using System.Collections.Generic;

namespace MMN.Integracoes.Oauth2.Models
{
    public class Oauth2Authenticate
    {
        public Dictionary<string,string> OptionalParameters { get; set; }
        public string ApiUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Code { get; set; }
        public string RedirectUri { get; set; }
    }
}
