using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace MMN.Api.Services;

public interface IBotConversaService
{
    Task<SubscriberResponse> CreateSubscriberAsync(string phone, string firstName, string lastName);
    Task SubscribeCampaignAsync(long subscriberId, int campaignId);
    Task SendFlowAsync(long subscriberId, int flowId);
    Task SetCustomFieldAsync(long subscriberId, int customFieldId, string value);
    Task AddTagToSubscriberAsync(long subscriberId, int tagId);
    Task<SubscriberResponse> GetSubscriberByPhoneAsync(string phone);
}

public class BotConversaService : IBotConversaService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "5fd2c16d-b307-421e-ab82-2a757581acdb";

    public BotConversaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://backend.botconversa.com.br/api/v1/webhook/");
        _httpClient.DefaultRequestHeaders.Add("API-KEY", ApiKey);
    }

    public async Task<SubscriberResponse> CreateSubscriberAsync(string phone, string firstName, string lastName)
  {
        try
        {
            var payload = new
            {
                phone,
                first_name = firstName,
                last_name = lastName
            };

            var response = await _httpClient.PostAsJsonAsync("subscriber/", payload);
            response.EnsureSuccessStatusCode();
            
            var jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var subscriberResponse = JsonConvert.DeserializeObject<SubscriberResponse>(jsonString);
                    return subscriberResponse;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Erro de deserialização: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Erro: {response.StatusCode} - {jsonString}");
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }        
    }

    public async Task SubscribeCampaignAsync(long subscriberId, int campaignId)
    {
        var url = $"subscriber/{subscriberId}/campaigns/{campaignId}/";
        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendFlowAsync(long subscriberId, int flowId)
    {
        var url = $"subscriber/{subscriberId}/send_flow/";
        var payload = new { flow = flowId };
        var response = await _httpClient.PostAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task SetCustomFieldAsync(long subscriberId, int customFieldId, string value)
    {
        var url = $"subscriber/{subscriberId}/custom_fields/{customFieldId}/";
        var payload = new { value = value };
        var response = await _httpClient.PostAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddTagToSubscriberAsync(long subscriberId, int tagId)
    {
        var url = $"subscriber/{subscriberId}/tags/{tagId}/";
        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<SubscriberResponse> GetSubscriberByPhoneAsync(string phone)
    {
        var encodedPhone = HttpUtility.UrlEncode(phone); // Evita problemas com sinais
        var url = $"subscriber/get_by_phone/{encodedPhone}/";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var subscriberResponse = JsonConvert.DeserializeObject<SubscriberResponse>(jsonString);
                return subscriberResponse;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro de deserialização: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Erro: {response.StatusCode} - {jsonString}");
        }

        return null;
    }
}

public class SubscriberResponse
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Ddd { get; set; }
    public DateTime CreatedAt { get; set; }
    public string LiveChat { get; set; }
    public object Referrer { get; set; }
    public int ReferralCount { get; set; }
    public List<object> Campaigns { get; set; }
    public List<object> Tags { get; set; }
    public Dictionary<string, object> Variables { get; set; }
    public List<object> Sequences { get; set; }
    public bool Created { get; set; }
}
