using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.Controllers.v1;
using MMN.Api.Helpers;
using MMN.Api.Models.Request;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Negocio.Negocio;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using UAParser;

namespace MMN.Api.Controllers.v2;

[Authorize, Route("api/v2/users"), ApiController]
public class UsersController : LoggedControllerBase
{
    private readonly IUsuarioNegocio _negocio;
    private readonly IUsersService _usersService;
    private readonly ITokenUtil _token;
    private readonly ICredenciamentoNegocio _credenciamentoNegocio;
    private readonly IEmailService _emailService;
    private readonly WhatsAppService _whatsAppService;

    public UsersController(IUsuarioNegocio negocio, ITokenUtil token, ICredenciamentoNegocio credenciamentoNegocio, IEmailService emailService, WhatsAppService whatsAppService, IUsersService usersServices)
    {
        _negocio = negocio;
        _token = token;
        _credenciamentoNegocio = credenciamentoNegocio;
        _emailService = emailService;
        _whatsAppService = whatsAppService;
        _usersService = usersServices;
    }

    [AllowAnonymous, HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token)
    {
        try
        {
            var retorno = _token.ValidarToken(token);

            _negocio.ValidarContaUsuario(Guid.Parse(retorno.IdUsuario));

            var credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == Guid.Parse(retorno.IdUsuario));

            if (credenciamento != null)
            {
                credenciamento.Status = StatusCredenciamento.Pendente;
                _credenciamentoNegocio.Update(credenciamento);
            }

            var indicated = _negocio.FirstNoTracking(x => x.IdUsuario == new Guid(retorno.IdUsuario));
            var indicator = _negocio.FirstNoTracking(x => x.IdUsuario == indicated.IdUsuarioPai);

            if (indicated.IndicadoPeloQS)
            {
                // Enviar email ao indicador
                await _emailService.SendEmailNewLeadToReferrer(indicated.IdUsuarioPai.Value, indicated.IdUsuario);

                if (string.IsNullOrEmpty(indicated.Nome))
                    indicated.Nome = indicated.Email;

                // Enviar uma mensagem de WhatsApp ao indicador
                var phone = "55" + indicator.Celular;
                var message = $"Olá, {indicator.Nome}! 👋\n\n🚀 O Quanta Shop quer te ver crescer e está ajudando a expandir sua rede de contatos. Olha só que incrível: indicamos *{indicated.Nome}*, e ele acabou de se cadastrar! Agora é a sua chance de fazer contato e apresentar para ele como o Quanta Shop funciona. 🤝\n\n💡 Mostre como vocês dois podem ganhar por minuto acumulando cashback em compras online e em parceiros locais. Quanto mais vocês colaboram, mais crescem juntos! 💰\n\nAproveite essa oportunidade para fortalecer sua rede e impulsionar seus ganhos! 🌟\n\nAbraços,\nEquipe Quanta Shop";
                var imageUrl = "https://img.freepik.com/premium-photo/horizontal-banner-website-that-emphasizes-personal-development-happiness_1269188-62798.jpg?w=826";

                await _whatsAppService.SendMessageWithImageAsync(phone, message, imageUrl);
            }
            else
            {
                // Enviar email ao indicador
                await _emailService.SendEmailToReferrer(indicated.IdUsuarioPai.Value, indicated.IdUsuario);

                if (string.IsNullOrEmpty(indicated.Nome))
                    indicated.Nome = indicated.Email;

                // Enviar uma mensagem de WhatsApp ao indicador
                var phone = "55" + indicator.Celular;
                var message = $"Olá, {indicator.Nome}! 👋\n\n🚀 Olha só que incrível: *{indicated.Nome}* acabou de se cadastrar na sua rede! Agora é a sua chance de fazer contato e apresentar para ele como o Quanta Shop funciona. 🤝\n\n💡 Mostre como vocês dois podem ganhar por minuto acumulando cashback em compras online e em parceiros locais. Quanto mais vocês colaboram, mais crescem juntos! 💰\n\nAproveite essa oportunidade para fortalecer sua rede e impulsionar seus ganhos! 🌟\n\nAbraços,\nEquipe Quanta Shop";
                var imageUrl = "https://img.freepik.com/premium-photo/horizontal-banner-website-that-emphasizes-personal-development-happiness_1269188-62798.jpg?w=826";

                await _whatsAppService.SendMessageWithImageAsync(phone, message, imageUrl);
            }

            return Ok(new ApiResponse<object>().SuccessResponse(retorno, "Confirmação de e-mail realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao confirmar o e-mail: {ex.Message}"));
        }
    }

    [HttpPost("register-access")]
    [Authorize]
    public async Task<IActionResult> RegisterAccess()
    {
        try
        {
            var httpContext = HttpContext.Request.Cookies;
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Captura do IP do usuário
            var ipAddress = string.Empty;

            if (string.IsNullOrEmpty(ipAddress))
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://api.ipify.org");
                    ipAddress = response;
                }
            }

            // Captura do User-Agent
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();
            var clientInfo = parser.Parse(userAgent);
            var browserFamily = clientInfo.UA.Family; // Exemplo: "Chrome Mobile"
            var browserVersion = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}"; // Exemplo: "131.0"
            var osFamily = clientInfo.OS.Family; // Exemplo: "Android"
            var osVersion = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}"; // Exemplo: "10"
            var deviceFamily = clientInfo.Device.Family != "Other" ? clientInfo.Device.Family : "Dispositivo Desconhecido";
            var formattedUserAgent = $"{browserFamily} {browserVersion} no {osFamily} {osVersion} usando {deviceFamily}";

            await _usersService.UpdateLastAccessDateAsync(Guid.Parse(userId), ipAddress, formattedUserAgent);

            return Ok(new ApiResponse<object>().SuccessResponse(null, "Data de último acesso registrada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao registrar o acesso: {ex.Message}"));
        }
    }

    [HttpGet("access-list")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAccessListAsync(string login, int page = 1, int limit = 25)
    {
        try
        {
            var accesses = await _usersService.GetAccessListAsync(login, page, limit);
            var totalItems = await _usersService.GetTotalAccessCountAsync(login);

            return Ok(new ApiResponse<object>().SuccessResponse(new { accesses, totalItems }, "Lista de acessos obtida com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao listar os acessos: {ex.Message}"));
        }
    }

    [HttpGet("accesses-by-user-id/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAccessesByUserIdAsync(Guid userId)
    {
        try
        {
            var accesses = await _usersService.GetAccessesByUserIdAsync(userId);

            return Ok(new ApiResponse<object>().SuccessResponse(accesses, "Lista de acessos do usuário obtida com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao registrar o acesso: {ex.Message}"));
        }
    }

    [HttpGet("access-indicators")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAccessIndicators()
    {
        try
        {
            var dailyAccessCount = await _usersService.GetDailyAccessCountAsync();
            var weeklyAccessCount = await _usersService.GetWeeklyAccessCountAsync();
            var monthlyAccessCount = await _usersService.GetMonthlyAccessCountAsync();
            var lastMonthAccessCount = await _usersService.GetLastMonthAccessCountAsync();

            return Ok(new ApiResponse<object>().SuccessResponse(new
            {
                dailyAccessCount,
                weeklyAccessCount,
                monthlyAccessCount,
                lastMonthAccessCount
            }, "Lista de acessos do usuário obtida com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao registrar o acesso: {ex.Message}"));
        }
    }

	[HttpGet("find-by-login/{login}")]
	[Authorize(Roles = "Admin")]
	public IActionResult FindUserByLogin(string login)
	{
		var user = _negocio.GetByLoginOrEmail(login);

		if (user == null)
			return NotFound(new ApiResponse<object>().ErrorResponse(null, 404, "Usuário não encontrado"));

		var result = new { id = user.IdUsuario, nome = user.Nome, login = user.Login };

		return Ok(new ApiResponse<object>().SuccessResponse(result));
	}
}
