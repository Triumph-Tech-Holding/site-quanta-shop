using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.Controllers.v1;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2;

[Route("api/v2/subscriptions"), ApiController, Authorize]
public class SubscriptionsController : LoggedControllerBase
{
	private readonly IUsuarioNegocio _usuarioNegocio;
	private readonly IPedidoNegocio _pedidoNegocio;
	private readonly ISubscriptionsService _subscriptionsService;

	public SubscriptionsController(ISubscriptionsService subscriptionsService, IUsuarioNegocio usuarioNegocio, IPedidoNegocio pedidoNegocio)
	{
		_subscriptionsService = subscriptionsService;
		_usuarioNegocio = usuarioNegocio;
		_pedidoNegocio = pedidoNegocio;
	}

	[HttpGet("subscriptions-history")]
	public IActionResult GetSubscriptionsHistory()
	{
		try
		{
			var subscriptionHistory = _subscriptionsService.GetSubscriptionsHistory(IdUsuarioLogado);

			if (subscriptionHistory is null)
				return NotFound(new ApiResponse<object>().ErrorResponse("Histórico da assinatura não encontrado."));

			return Ok(new ApiResponse<object>().SuccessResponse(subscriptionHistory, "Histórico da assinatura obtido com sucesso."));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao obter o histórico da assinatura: {ex.Message}"));
		}
	}

	[AllowAnonymous, HttpGet("is-subscriber/{login}")]
	public IActionResult GetIsSubscriber(string login)
	{
		try
		{
			var user = _usuarioNegocio.GetByLoginOrEmail(login);

			if (user is null)
				return NotFound(new ApiResponse<object>().ErrorResponse(null, 404, "Usuário não encontrado"));

			var isSubscriber = _subscriptionsService.IsSubscriber(login);

			return Ok(new ApiResponse<object>().SuccessResponse(isSubscriber, (isSubscriber ? "Usuário é assinante" : "Usuário não é assinante")));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao consultar a assinatura: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("latest-summary")]
	public async Task<ActionResult<ApiResponse<IEnumerable<LatestSubscriptionDto>>>> GetLatestSubscriptionsSummary([FromQuery] string login, [FromQuery] string nome, [FromQuery] bool? ativa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
	{
		try
		{
			var summary = await _subscriptionsService.GetLatestSubscriptionsSummary(login, nome, ativa, pageNumber, pageSize);
			return Ok(new ApiResponse<IEnumerable<LatestSubscriptionDto>>().SuccessResponse(summary, "Relatório obtido"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao obter relatório: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPost("{userId}/cancel-manual")]
	public IActionResult CancelManualSubscription(Guid userId)
	{
		try
		{
			var user = _usuarioNegocio.GetById(IdUsuarioLogado);

			_pedidoNegocio.CancelarAssinatura(userId, user.Login, true);
			
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Assinatura cancelada com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao cancelar assinatura: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPost("create-manual")]
	public async Task<IActionResult> CreateManualSubscription([FromBody] CreateManualSubscriptionDto dto)
	{
		try
		{
			var user = _usuarioNegocio.GetByLoginOrEmail(dto.Login);

			if (user is null)
				return NotFound(new ApiResponse<object>().ErrorResponse(null, 404, "Usuário não encontrado"));

			var newSubscription = _subscriptionsService.CreateNewSubscription(dto.UserId, dto.StartDate, dto.EndDate, dto.PerformLaunch, true);
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Assinatura manual criada com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao criar assinatura: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPost("{userId}/launch-cycle")]
	public async Task<IActionResult> LaunchNewCycle([FromRoute] Guid userId, [FromBody] LaunchCycleDto dto)
	{
		try
		{
			await _subscriptionsService.LaunchNewCycleAsync(userId, dto);
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Novo ciclo lançado com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("{userId}/cycles")]
	public async Task<IActionResult> GetSubscriptionCycles(Guid userId)
	{
		var cycles = await _subscriptionsService.GetSubscriptionCyclesAsync(userId);
		return Ok(new ApiResponse<object>().SuccessResponse(cycles));
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("cycles/{cycleId}")]
	public async Task<IActionResult> DeleteSubscriptionCycle(long cycleId)
	{
		try
		{
			await _subscriptionsService.DeleteSubscriptionCycleAsync(cycleId);
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Ciclo removido com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro: {ex.Message}"));
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPatch("{userId}/virtual-assistant-link")]
	public async Task<IActionResult> UpdateVirtualAssistantLink([FromRoute] Guid userId,[FromBody] UpdateVirtualAssistantLinkDto dto)
	{
		try
		{
			await _subscriptionsService.UpdateVirtualAssistantLinkAsync(userId, dto.Link);
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Link do usuário atualizado com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao atualizar o link: {ex.Message}"));
		}
	}
}