using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MMN.Api.Services;

public class WhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrlSendMessage;
    private readonly string _apiUrlSendMessageWithImage;
    private readonly string _apiUrlSendMessageWithAttachment;
    private readonly string _apiKey;

    public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrlSendMessage = "https://whatsapp-bot.quantashop.com.br/send-message";
        _apiUrlSendMessageWithImage = "https://whatsapp-bot.quantashop.com.br/send-message-with-image";
        _apiUrlSendMessageWithAttachment = "https://whatsapp-bot.quantashop.com.br/send-message-with-attachment";
        _apiKey = configuration["WhatsAppBot:ApiKey"];
    }

    public async Task<HttpResponseMessage> SendMessageAsync(string number, string message)
    {
        var request = new
        {
            number,
            message
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiUrlSendMessage);
        requestMessage.Content = content;
        requestMessage.Headers.Add("x-api-key", _apiKey);

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> SendMessageWithImageAsync(string number, string message, string imageUrl)
    {
        var request = new
        {
            number,
            message,
            imageUrl
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiUrlSendMessageWithImage);
        requestMessage.Content = content;
        requestMessage.Headers.Add("x-api-key", _apiKey);

        return await _httpClient.SendAsync(requestMessage);
    }

    public async Task<HttpResponseMessage> SendMessageWithAttachmentAsync(string number, string message, string attachmentUrl)
    {
        var request = new
        {
            number,
            message,
            attachmentUrl
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _apiUrlSendMessageWithAttachment);
        requestMessage.Content = content;
        requestMessage.Headers.Add("x-api-key", _apiKey);

        return await _httpClient.SendAsync(requestMessage);
    }
}

