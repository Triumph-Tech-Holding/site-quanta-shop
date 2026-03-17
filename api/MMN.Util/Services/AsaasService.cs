using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MMN.Util.Models.Request.Asaas;
using MMN.Util.Models.Request.Asaas.Customer;
using MMN.Util.Models.Response.Asaas;
using MMN.Util.Models.Response.Asaas.Customer;
using MMN.Util.Models.Response.Asaas.Payment;
using Newtonsoft.Json;


namespace MMN.Util.Services
{
    public interface IAsaasService
    {
        #region Customer Methods
        Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest customerRequest);
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest customerRequest);
        Task<CustomerResponse> GetCustomerAsync(string customerId);
        Task<CustomerListResponse> GetCustomerByQueryAsync(string email = null, string cpfCnpj = null);
        #endregion Customer Methods

        #region Payment Methods
        Task<PaymentResponse> GetPaymentAsync(string paymentId);
        Task<PaymentResponse> CreatePaymentAsync(NewPaymentRequest paymentRequest);
        #endregion Payment Methods
    }

    public class AsaasService : IAsaasService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public AsaasService()
        {
            _httpClient = new HttpClient();
            _apiKey = Environment.GetEnvironmentVariable("ASAAS_API_KEY");
            _baseUrl = Environment.GetEnvironmentVariable("ASAAS_BASE_URL");
        }

        #region Customer Methods
        // Atualizar um customer v3/customers/{id}
        public async Task<CustomerResponse> UpdateCustomerAsync(string customerId, CustomerRequest customerRequest)
        {
            var url = $"{_baseUrl}v3/customers/{customerId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);

            var json = JsonConvert.SerializeObject(customerRequest);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CustomerResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao atualizar cliente: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }

        // Criar novo cliente v3/customers
        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest customerRequest)
        {
            var url = $"{_baseUrl}v3/customers";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);

            var json = JsonConvert.SerializeObject(customerRequest);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CustomerResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao criar cliente: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }

        // Recuperar um único cliente v3/customers/{id}
        public async Task<CustomerResponse> GetCustomerAsync(string customerId)
        {
            var url = $"{_baseUrl}v3/customers/{customerId}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CustomerResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao recuperar cliente: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }

        // Buscar customer por email ou cpfCnpj
        public async Task<CustomerListResponse> GetCustomerByQueryAsync(string email = null, string cpfCnpj = null)
        {
            var query = new List<string>();
            if (!string.IsNullOrEmpty(email))
                query.Add($"email={Uri.EscapeDataString(email)}");
            if (!string.IsNullOrEmpty(cpfCnpj))
                query.Add($"cpfCnpj={Uri.EscapeDataString(cpfCnpj)}");
            var url = $"{_baseUrl}v3/customers" + (query.Count > 0 ? $"?{string.Join("&", query)}" : "");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CustomerListResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao buscar cliente: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }
        #endregion Customer Methods

        #region Payment Methods
        // Recuperar uma única cobrança v3/payments/{id}
        public async Task<PaymentResponse> GetPaymentAsync(string paymentId)
        {
            var url = $"{_baseUrl}v3/payments/{paymentId}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<PaymentResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao recuperar cobrança: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }

        // Criar nova cobrança v3/payments
        public async Task<PaymentResponse> CreatePaymentAsync(NewPaymentRequest paymentRequest)
        {
            var url = $"{_baseUrl}v3/payments";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("User-Agent", "QuantaShop");
            request.Headers.Add("access_token", _apiKey);
            var json = JsonConvert.SerializeObject(paymentRequest);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<PaymentResponse>(responseContent);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<BadRequestResponse>(responseContent);
                throw new Exception($"Erro ao criar cobrança: {string.Join(", ", error.Errors.Select(e => e.Description))}");
            }
        }
        #endregion Payment Methods
    }
}