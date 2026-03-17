using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MMN.Api.Controllers.v1;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2
{
	[Route("api/v2/invoices"), ApiController, Authorize]
	public class InvoicesController : LoggedControllerBase
	{
		readonly IPedidoRepositorio _pedidoRepositorio;
		readonly WhatsAppService _whatsAppService;
		private readonly IServiceProvider _serviceProvider;

		public InvoicesController(IPedidoRepositorio pedidoRepositorio, WhatsAppService whatsAppService, IServiceProvider serviceProvider)
		{
			_pedidoRepositorio = pedidoRepositorio;
			_whatsAppService = whatsAppService;
			_serviceProvider = serviceProvider;
		}

		[HttpGet("send-invoice/{id}")]
		public async Task<IActionResult> SendInvoice(int id)
		{
			Pedido pedido = _pedidoRepositorio.FirstNoTracking(x => x.IdPedido == id);

			if (pedido == null)
				return NotFound($"Pedido {id} não encontrado");

			var scope = _serviceProvider.CreateScope();

			try
			{
				var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
				var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				var usuarioComerciante = await context.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.IdUsuario == pedido.IdUsuario);
				var comerciante = await context.Credenciamento.FirstOrDefaultAsync(x => x.IdUsuario == usuarioComerciante.IdUsuario);
				var boleto = await context.Pagamento.FirstOrDefaultAsync(x => x.IdPedido == pedido.IdPedido);
				var telefone = "55" + usuarioComerciante.Celular;
				var valorFatura = pedido.ValorPedido;
				var dataVencimento = boleto.DataValidade;
				var mensagem = $"Olá, {comerciante.Estabelecimento}! \n\n🧾 Sua fatura foi fechada no valor de {valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))}. \n\n📅 Vencimento: {dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR"))}. \n\nCaso tenha dúvidas, entre em contato! \n\nAbraços, \nEquipe Quanta Shop";

				// Enviar Email
				await _emailService.SendEmailClosedInvoice(comerciante.Estabelecimento, usuarioComerciante.Email, valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR")), dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR")), boleto.LinhaDigitavelBoleto, boleto.UrlBoleto);

				// Enviar WhatsApp
				await _whatsAppService.SendMessageWithAttachmentAsync(telefone, mensagem, boleto.UrlBoleto);
				await _whatsAppService.SendMessageAsync(telefone, boleto.LinhaDigitavelBoleto);
			}
			catch(Exception ex)
			{ 
				return StatusCode(500, $"Erro ao enviar fatura: {ex.Message}");
			}
			finally
			{
				scope.Dispose();
			}

			return Ok($"Invoice {id} sent successfully");
		}

		[HttpPost("send-invoices")]
		public async Task<IActionResult> SendInvoices([FromBody] List<int> ids)
		{
			if (ids == null || ids.Count == 0)
			{
				return BadRequest("A lista de IDs não pode estar vazia.");
			}

			var scope = _serviceProvider.CreateScope();
			var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
			var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

			var resultados = new List<object>();

			foreach (var id in ids)
			{
				try
				{
					Pedido pedido = _pedidoRepositorio.FirstNoTracking(x => x.IdPedido == id);

					if (pedido == null)
					{
						resultados.Add(new { Id = id, Status = "Pedido não encontrado" });
						continue;
					}

					var usuarioComerciante = await context.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.IdUsuario == pedido.IdUsuario);
					var comerciante = await context.Credenciamento.FirstOrDefaultAsync(x => x.IdUsuario == usuarioComerciante.IdUsuario);
					var boleto = await context.Pagamento.FirstOrDefaultAsync(x => x.IdPedido == pedido.IdPedido);

					if (usuarioComerciante == null || comerciante == null || boleto == null)
					{
						resultados.Add(new { Id = id, Status = "Dados incompletos para envio" });
						continue;
					}

					var telefone = "55" + usuarioComerciante.Celular;
					var valorFatura = pedido.ValorPedido;
					var dataVencimento = boleto.DataValidade;
					var mensagem = $"Olá, {comerciante.Estabelecimento}! \n\n🧾 Sua fatura foi fechada no valor de {valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))}. \n\n📅 Vencimento: {dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR"))}. \n\nCaso tenha dúvidas, entre em contato! \n\nAbraços, \nEquipe Quanta Shop";

					// Enviar Email
					await _emailService.SendEmailClosedInvoice(
						comerciante.Estabelecimento,
						usuarioComerciante.Email,
						valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR")),
						dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR")),
						boleto.LinhaDigitavelBoleto,
						boleto.UrlBoleto
					);

					// Enviar WhatsApp
					await _whatsAppService.SendMessageWithAttachmentAsync(telefone, mensagem, boleto.UrlBoleto);
					await _whatsAppService.SendMessageAsync(telefone, boleto.LinhaDigitavelBoleto);

					resultados.Add(new { Id = id, Status = "Enviado com sucesso" });
				}
				catch (Exception ex)
				{
					resultados.Add(new { Id = id, Status = "Erro ao enviar", Erro = ex.Message });
				}
			}

			return Ok(resultados);
		}
	}
}
