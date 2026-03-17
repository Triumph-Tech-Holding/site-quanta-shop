using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface ICurrencyService
{
    Task<double?> GetCurrencyValueAsync();
    Task<decimal?> GetCurrencyValueAsync(decimal amount);
}

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://api.invertexto.com/v1/currency/USD_BRL?token=17322|CcH3336wTnujztjBLHFIDxZSlBz1lfNI";

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double?> GetCurrencyValueAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return (double)result?.USD_BRL?.price;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Console.WriteLine($"Erro 404: {error?.Message}");
                return null;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Console.WriteLine($"Erro 401: {error?.Message}");
                return null;
            }
            else
            {
                Console.WriteLine($"Erro inesperado: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção: {ex.Message}");
            return null;
        }
    }

    public async Task<decimal?> GetCurrencyValueAsync(decimal amount)
    {
        try
        {
            var response = await _httpClient.GetAsync(ApiUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result?.USD_BRL?.price * amount;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Console.WriteLine($"Erro 404: {error?.Message}");
                return null;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Console.WriteLine($"Erro 401: {error?.Message}");
                return null;
            }
            else
            {
                Console.WriteLine($"Erro inesperado: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção: {ex.Message}");
            return null;
        }
    }

    private class ApiResponse
    {
        public CurrencyInfo USD_BRL { get; set; }
    }

    private class CurrencyInfo
    {
        public decimal price { get; set; }
        public long timestamp { get; set; }
    }

    private class ErrorResponse
    {
        public string Message { get; set; }
    }
}
