using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MMN.Integracoes.Oauth2.Models;
using Newtonsoft.Json;

namespace MMN.Integracoes.Oauth2
{
    public static class Oauth2
    {
        public static async Task<string> GetAccessTokenAsync(Oauth2Authenticate model)
        {
            var body = new Dictionary<string, string>{
                { "client_id", model.ClientId },
                { "client_secret", model.ClientSecret },
                { "code" ,model.Code },
                { "redirect_uri", model.RedirectUri },
            };

            foreach (var parameter in model.OptionalParameters)
            {
                body.Add(parameter.Key, parameter.Value);
            }

            var client = new HttpClient();

            var uri = $"{model.ApiUrl}?{string.Join('&', body.Select(s => $"{s.Key}={HttpUtility.UrlEncode(s.Value)}"))}";

            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            var response = await client.SendAsync(request);
            var responseString = await response?.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Invalid Oauth2 response:\n{response.StatusCode}: {responseString}");
            }

            return responseString;
        }

        public static async Task<T> GetAccessTokenAsync<T>(Oauth2Authenticate model)
        {
            var responseString = await GetAccessTokenAsync(model);

            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}
