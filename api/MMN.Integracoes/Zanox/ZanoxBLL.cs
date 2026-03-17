using MMN.Integracoes.Zanox;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace MMN.Integracoes
{
    public class ZanoxBLL
    {
        public string ConnectId { get { return "60FCF3F4B33B72C89C73"; } }
        public string SecretKey { get { return "9bcc6874346d4a+cadf793fEF77F91/fA9b8Af4C"; } }
        public HttpClient Client { get { return new HttpClient(); } }
        public string UrlZanox { get { return "https://api.zanox.com/json/2011-03-01"; } }
        public string Data { get { return DateTime.Now.ToString("yyyy-MM-dd"); } }

        private string GetRestTimestamp()
        {
            return GetTimestamp("ddd, dd MMM yyyy HH:mm:ss") + " GMT";
        }

        private string GetTimestamp(string dateFormat)
        {
            var usCulture = new CultureInfo("en-US");
            var utcDate = DateTime.UtcNow.ToLocalTime().AddHours(-1);
            TimeZoneInfo gmtZone;
            try { gmtZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"); }
            catch { try { gmtZone = TimeZoneInfo.FindSystemTimeZoneById("Etc/GMT"); } catch { gmtZone = TimeZoneInfo.Utc; } }
            var gmt1Date = TimeZoneInfo.ConvertTime(utcDate, gmtZone);

            return gmt1Date.ToString(dateFormat, usCulture);
        }

        private string GetRestSignature(string httpVerb, string uri, string timestamp, string nonce, string secretKey)
        {
            string stringToSign = httpVerb + uri + timestamp + nonce;
            return GetSignature(stringToSign, secretKey);
        }

        private string GetSignature(string stringToSign, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(key: enc.GetBytes(secretKey), true);
            hmac.Initialize();

            byte[] rawHmac = hmac.ComputeHash(enc.GetBytes(stringToSign), 0, enc.GetBytes(stringToSign).Length);
            var encoded = Convert.ToBase64String(rawHmac, Base64FormattingOptions.InsertLineBreaks);

            return encoded;
        }

        private string GenerateNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public async Task<string> GetAdspaces()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/adspaces", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/adspaces" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetAdmedia(string programId, Guid idUsuarioLogado)
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/admedia", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/admedia" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&adspace=2331385" +
                $"&program={programId}" +
                $"&purpose=startpage" +
                $"&partnership=direct";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var adMedium = JsonSerializer.Deserialize<AdMedia>(responseBody);

            var ppc = adMedium.admediumItems.admediumItem.trackingLinks.trackingLink.FirstOrDefault().ppc.Split("&zpar9")[0];

            //var anuncioUsuario = programId + "|" + idUsuarioLogado;

            ppc = string.Format("{0}&zpar0=[[{1}]]", ppc, idUsuarioLogado.ToString());

            return ppc;
        }

        public async Task<string> GetPrograms()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/programs", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/programs" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&items=10" +
                $"&isexclusive=true" +
                $"&partnership=DIRECT" +
                $"&adspace=2331385";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<Item> GetProgramsById(string id)
        {
            var url = UrlZanox + $"/programs/program/{id}" +
                $"?connectid={ConnectId}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var programItem = JsonSerializer.Deserialize<Item>(responseBody);

            return programItem;
        }

        public async Task<ProgramApp> GetProgramAppArrayId(int page)
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/programapplications", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/programapplications" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&isexclusive=true" +
                $"&status=confirmed" +
                $"&adspace=2331385" +
                $"&items=50" +
                $"&page={page}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var programApp = JsonSerializer.Deserialize<ProgramApp>(responseBody);

            //var ids = programApp.programApplicationItems.programApplicationItem
            //    .Select(s => new { s.program.id, s.program.name })
            //    .ToDictionary(d => d.id, n => n.name);

            return programApp;
        }

        public async Task<string> GetProgramApp()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/programapplications", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/programapplications" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&isexclusive=true" +
                $"&status=confirmed" +
                $"&adspace=2331385";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var teste = JsonSerializer.Deserialize<ProgramApp>(responseBody);

            //var ids = teste.programApplicationItems.programApplicationItem.Select(s => s.program.id).ToList();

            return responseBody;
        }

        public async Task<string> GetIncentives()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/incentives", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/incentives" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&adspace=2331385";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetIncentivesExclusive()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/incentives/exclusive", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/incentives/exclusive" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&adspace=2331385";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetProducts()
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", "/products", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + "/products" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&adspace=2331385";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<IList<trackingCategoryItem>> GetTrackingCategories(string programId = "19520", string adspaceId = "2331385")
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", $"/programapplications/program/{programId}/adspace/{adspaceId}/trackingcategories", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + $"/programapplications/program/{programId}/adspace/{adspaceId}/trackingcategories" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                var trackingCategories = JsonSerializer.Deserialize<TrackingCategories>(responseBody);
                var porcentagens = trackingCategories.trackingCategoryItem.trackingCategoryItem.ToList();

                return porcentagens;
            }
            catch
            {
                var trackingCategories = JsonSerializer.Deserialize<TrackingCategoriesObject>(responseBody);
                var porcentagens = trackingCategories.trackingCategoryItem.trackingCategoryItem;

                var list = new List<trackingCategoryItem>
                {
                    porcentagens
                };

                return list;
            }
        }

        public async Task<Sales> GetSalesOpen(int page, string dataFormatada = "")
        {
            var data = DateTime.UtcNow.HorarioBrasilia().AddDays(-1).ToString("yyyy-MM-dd");
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", $"/reports/sales/date/{(string.IsNullOrEmpty(dataFormatada) ? data : dataFormatada)}", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + $"/reports/sales/date/{(string.IsNullOrEmpty(dataFormatada) ? data : dataFormatada)}" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}" +
                $"&state=open" +
                $"&items=50" +
                $"&page={page}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var serializerOptions = new JsonSerializerOptions
            {
                Converters = { new GppJsonConverter() }
            };

            try
            {
                var sales = JsonSerializer.Deserialize<Sales>(responseBody, serializerOptions);
                return sales;
            }
            catch
            {
                var sales = JsonSerializer.Deserialize<Sales>(responseBody);
                return sales;
            }
        }

        public async Task<Categorias> GetCategories()
        {
            var url = UrlZanox + $"/programs/categories" +
                $"?connectid={ConnectId}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<Categorias>(responseBody);

            return categories;
        }

        public async Task<string> GetProgramsByIdReturnsString(string id)
        {
            var url = UrlZanox + $"/programs/program/{id}" +
                $"?connectid={ConnectId}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<SaleItems> GetSaleById(Guid idVenda)
        {
            var restTs = GetRestTimestamp();
            var restNonce = GenerateNonce();
            var restSignature = GetRestSignature("GET", $"/reports/sales/sale/{idVenda}", restTs, restNonce, SecretKey);
            restSignature = HttpUtility.UrlEncode(restSignature);

            var url = UrlZanox + $"/reports/sales/sale/{idVenda}" +
                $"?connectid={ConnectId}" +
                $"&date={restTs}" +
                $"&nonce={restNonce}" +
                $"&signature={restSignature}";

            HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var serializerOptions = new JsonSerializerOptions
            {
                Converters = { new GppJsonConverter() }
            };

            try
            {
                var sales = JsonSerializer.Deserialize<SaleItems>(responseBody, serializerOptions);

                return sales;
            }
            catch
            {
                var sales = JsonSerializer.Deserialize<SaleItems>(responseBody, serializerOptions);

                return sales;
            }
        }
    }


    public class GppJsonConverter : JsonConverter<Gpp>
    {
        public override Gpp Read(ref Utf8JsonReader reader,
                                      Type typeToConvert,
                                      JsonSerializerOptions options)
        {

            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                try
                {
                    jsonDoc.RootElement.TryGetProperty("$", out JsonElement teste);
                    return new Gpp(teste.GetString(), true);
                }
                catch
                {
                    return new Gpp("Não processa", false);
                }
            }
        }

        public override void Write(Utf8JsonWriter writer,
                                   Gpp value,
                                   JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.IdUsuario);
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            var dateString = reader.GetString().Split("+")[0];
            return DateTime.Parse(dateString);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
