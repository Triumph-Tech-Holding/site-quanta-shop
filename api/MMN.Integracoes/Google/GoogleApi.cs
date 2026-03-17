using Newtonsoft.Json;
using SimplesmenteSou.Utils.Models.Google;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMN.Integracoes.Google
{
    public static class GoogleApi
    {
        public static async Task<GoogleUserIdModel> ObterIdUsuarioAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/userinfo/v2/me");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            var client = new HttpClient();
            var userDataResponse = await client.SendAsync(request);
            var responseString = await userDataResponse.Content.ReadAsStringAsync();
            var userData = JsonConvert.DeserializeObject<GoogleUserIdModel>(responseString);

            return userData;
        }
    }
}
