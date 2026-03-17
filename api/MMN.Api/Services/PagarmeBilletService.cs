using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public class PagarmeBilletService : IPagarmeBilletService
    {
        private const string ApiKey = "sk_25148da8278b455bad878daf5e5ea652";
        private const string BaseUrl = "https://api.pagar.me/core/v5";
        private readonly HttpClient _httpClient;

        public PagarmeBilletService(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public async Task<PagarmeBilletResult> CreateBilletAsync(PagarmeBilletRequest request)
        {
            var payload = new
            {
                amount = (int)(request.Valor * 100),
                payment_method = "boleto",
                boleto = new
                {
                    instructions = request.Descricao ?? "Pagamento via boleto"
                },
                customer = new
                {
                    name = request.Nome,
                    email = request.Email,
                    type = request.Documento.Length == 11 ? "individual" : "company",
                    document = request.Documento,
                    address = new
                    {
                        line_1 = request.Logradouro + ", " + request.Numero,
                        zip_code = request.Cep,
                        city = request.Cidade,
                        state = request.Uf,
                        country = "BR"
                    },
                    phones = new
                    {
                        mobile_phone = new
                        {
                            country_code = "55",
                            area_code = request.Telefone.Substring(0, 2),
                            number = request.Telefone.Substring(2)
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/orders", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new PagarmeBilletResult { Success = false, MensagemErro = responseBody };
                }

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                var boletoUrl = root.GetProperty("charges")[0].GetProperty("last_transaction").GetProperty("pdf").GetString();
                var linhaDigitavel = root.GetProperty("charges")[0].GetProperty("last_transaction").GetProperty("line").GetString();
                var idTransacao = root.GetProperty("charges")[0].GetProperty("last_transaction").GetProperty("id").GetString();

                return new PagarmeBilletResult
                {
                    Success = true,
                    BoletoUrl = boletoUrl,
                    LinhaDigitavel = linhaDigitavel,
                    IdTransacao = idTransacao
                };
            }
            catch (Exception ex)
            {
                return new PagarmeBilletResult { Success = false, MensagemErro = ex.Message };
            }
        }
    }
}
