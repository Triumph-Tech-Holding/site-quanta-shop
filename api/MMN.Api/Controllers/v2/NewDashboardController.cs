using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MMN.Api.Controllers.v1;
using MMN.Api.Models;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.INegocio.Negocio;
using RestSharp;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2;

[Route("api/v2/dashboard"), ApiController, Authorize]
public class NewDashboardController : LoggedControllerBase
{
    private readonly CacheManager _cache;
    private readonly IQuantaFriendshipService _quantaFriendshipService;
    private readonly ITotalizersUserService _totalizersUserService;
    private readonly IPartnersService _partnersService;
    private readonly IAwinFeedService _awinFeedService;
    private readonly IUsersService _usersService;
    private readonly IUsuarioNegocio _usuarioNegocio;
    private readonly IUsuarioEnderecoNegocio _usuarioEnderecoNegocio;

    public NewDashboardController(IQuantaFriendshipService quantaFriendshipService, ITotalizersUserService totalizersUserService, IAwinFeedService awinFeedService, IPartnersService partnersService, IUsuarioEnderecoNegocio usuarioEnderecoNegocio, IUsersService usersService, IUsuarioNegocio usuarioNegocio, IMemoryCache cache)
    {
        _quantaFriendshipService = quantaFriendshipService;
        _totalizersUserService = totalizersUserService;
        _awinFeedService = awinFeedService;
        _partnersService = partnersService;
        _usuarioEnderecoNegocio = usuarioEnderecoNegocio;
        _usersService = usersService;
        _usuarioNegocio = usuarioNegocio;
        _cache = new CacheManager(cache);
    }

    [HttpGet("get-friendship-info")]
    public IActionResult GetFriendshipInfo()
    {
        try
        {
            var friendships = _quantaFriendshipService.GetFriendshipInfo(IdUsuarioLogado);

            return Ok(new ApiResponse<object>().SuccessResponse(friendships, "Consulta do Quanta Amizade realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar Quanta Amizade: {ex.Message}"));
        }
    }

    [HttpGet("get-friendship-history/{loginUsuarioFilho}")]
    public IActionResult GetFriendshipHistory(string loginUsuarioFilho)
    {
        try
        {
            var friendships = _quantaFriendshipService.GetFriendshipHistory(IdUsuarioLogado, loginUsuarioFilho);

            return Ok(new ApiResponse<object>().SuccessResponse(friendships, "Consulta do Quanta Amizade realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar Quanta Amizade: {ex.Message}"));
        }
    }

    [HttpGet("get-totalizers-user")]
    public IActionResult GetTotalizersUser()
    {
        try
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.Name)?.Value);
            var totalizers = _totalizersUserService.GetTotalizersUser(userId);

            return Ok(new ApiResponse<object>().SuccessResponse(totalizers, "Consulta dos totalizadores realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar os totalizadores: {ex.Message}"));
        }
    }

    [HttpGet("get-products")]
    public IActionResult GetProducts()
    {
        try
        {
            var cacheKey = "ev_2_products";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta dos produtos realizada com sucesso a partir do cache."));
            }

            var products = _awinFeedService.GetRandomProducts(quantity: 2);

            _cache.SetItem(cacheKey, products);


            return Ok(new ApiResponse<object>().SuccessResponse(products, "Consulta dos produtos realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar os produtos: {ex.Message}"));
        }
    }

    [HttpGet("get-user-info")]
    public IActionResult GetUserInfo()
    {
        var httpContext = HttpContext.Request.Cookies;
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { UserId = userId, UserRole = userRole });
    }

    [HttpGet("get-user-month-cashback/{dataInicio}/{dataFim}")]
    public IActionResult GetMonthCashback(DateTime dataInicio, DateTime dataFim)
    {
        try
        {
            var userCashback = _totalizersUserService.GetMonthCashback(IdUsuarioLogado, dataInicio, dataFim);

            return Ok(new ApiResponse<object>().SuccessResponse(userCashback, "Consulta do cashback do usuário realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar o cashback do usuário: {ex.Message}"));
        }
    }

    [HttpGet("get-top5-connections")]
    public IActionResult GetTop5Connections()
    {
        try
        {
            var top5 = _totalizersUserService.GetTop5Connections(IdUsuarioLogado);

            return Ok(new ApiResponse<object>().SuccessResponse(top5, "Ação realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse("Erro ao realizar esta ação: " + ex.Message));
        }
    }

    [HttpGet("get-shops-nearby")]
    public IActionResult GetShopsNearby(float latitude, float longitude, int raio)
    {
        try
        {
            var shops = _partnersService.GetShopsNearby(latitude, longitude, raio);

            return Ok(new ApiResponse<object>().SuccessResponse(shops, "Ação realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse("Erro ao realizar esta ação: " + ex.Message));
        }
    }

    [HttpGet("get-user-available-level/{level}")]
    public IActionResult GetUserAvailableLevel(decimal level)
    {
        try
        {
            var availableLevels = _totalizersUserService.GetUserAvaliableLevel(new Guid("bd420f6b-c725-4dec-ab38-7f2df70341aa"), level);

            return Ok(new ApiResponse<object>().SuccessResponse(availableLevels, "Consulta de níveis disponíveis para o usuário realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar níveis disponíveis para o usuário: {ex.Message}"));
        }
    }

    [HttpGet("get-objectives-by-level/{level}")]
    public IActionResult GetObjectivesByLevel(decimal level)
    {
        try
        {
            var objectives = _totalizersUserService.GetObjectivesByLevel(level);

            return Ok(new ApiResponse<object>().SuccessResponse(objectives, "Consulta de objetivos por nível realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar objetivos por nível: {ex.Message}"));
        }
    }

    [HttpGet("get-user-objectives-progress")]
    public IActionResult GetUserObjectivesProgress()
    {
        try
        {
            var userObjectivesProgress = _totalizersUserService.GetUserObjectivesProgess(IdUsuarioLogado);

            return Ok(new ApiResponse<object>().SuccessResponse(userObjectivesProgress, "Consulta de progresso de objetivos do usuário realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar o progresso dos objetivos do usuário: {ex.Message}"));
        }
    }

    [HttpGet("get-user-lat-lng")]
    public async Task<IActionResult> GetUserLatLngAsync()
    {
        try
        {
            var usuarioEndereco = _usuarioEnderecoNegocio.GetNoTracking(x => x.IdUsuario == IdUsuarioLogado).FirstOrDefault();

            if (usuarioEndereco is null)
                return Ok(new ApiResponse<object>().SuccessResponse(new { latitude = -22.9068f, longitude = -43.1729d }, "Ação realizada com sucesso."));

            if (usuarioEndereco.Latitude is null || usuarioEndereco.Longitude is null)
            {
                var endereco = $"{usuarioEndereco.Rua}, {usuarioEndereco.Numero}, {usuarioEndereco.Bairro}, {usuarioEndereco.Cep}";

                string apiKey = "5a882080157b49e59b925f96afcd6511";
                var formattedAddress = Uri.EscapeDataString(endereco);
                var url = $"https://api.opencagedata.com/geocode/v1/json?key={apiKey}&q={formattedAddress}&pretty=1&no_annotations=1";

                var client = new RestClient();
                var request = new RestRequest(url, Method.Get);
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                    throw new Exception("Erro na requisição: " + response.ErrorMessage);

                var content = response.Content;
                using var document = JsonDocument.Parse(content);

                var results = document.RootElement.GetProperty("results");

                if (results.GetArrayLength() > 0)
                {
                    var geometry = results[0].GetProperty("geometry");
                    var latitude = geometry.GetProperty("lat").GetDouble();
                    var longitude = geometry.GetProperty("lng").GetDouble();

                    return Ok(new ApiResponse<object>().SuccessResponse(new { latitude, longitude }, "Ação realizada com sucesso."));
                }
            }

            throw new Exception("Não foi possível encontrar as coordenadas para o endereço fornecido.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse("Erro ao realizar esta ação: " + ex.Message));
        }
    }

    [HttpGet("get-check-user-eligibility")]
    public IActionResult GetCheckUserEligibility()
    {
        try
        {
            var result = _quantaFriendshipService.GetCheckUserEligibility(IdUsuarioLogado);

            return Ok(new ApiResponse<object>().SuccessResponse(result, "Ação realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse("Erro ao realizar esta ação: " + ex.Message));
        }
    }

    [HttpPost("send-message-ceo")]
    public async Task<IActionResult> SendMessageToCEO([FromBody] MessageToCEO message)
    {
        try
        {
            if (string.IsNullOrEmpty(message.Message))
                throw new Exception("Você precisa preencher a mensagem");

            var user = _usuarioNegocio.GetById(IdUsuarioLogado) ?? throw new Exception("Usuário não encontrado");
            
            message.Sender = user.Nome;
            message.Login = user.Login;
            message.From = user.Email;
            message.Phone = user.Celular;

            var sent = await _usersService.SendMessageToCEO(message);

            if (!sent)
                throw new Exception();

            return Ok(new ApiResponse<object>().SuccessResponse(null, "Mensagem enviada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse("Erro ao enviar a mensagem: " + ex.Message));
        }
    }

}