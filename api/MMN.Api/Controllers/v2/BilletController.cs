using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MMN.Api.Models.Request;
using MMN.Api.Models.Request.Asaas;
using MMN.Api.Models.Request.Asaas.Customer;
using MMN.Api.Models.Response.Asaas;
using MMN.Api.Services;
using MMN.Dominio.WebHook;
using MMN.INegocio.Negocio;
using MMN.Util.Util;
using NuGet.Versioning;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2
{
	[ApiController]
	[Route("api/v2/billet")]
	public class BilletController : ControllerBase
	{
		readonly IUsuarioNegocio _usuarioNegocio;
		readonly IUsuarioEnderecoNegocio _usuarioEnderecoNegocio;
		readonly ICredenciamentoNegocio _credenciamentoNegocio;
		readonly IPedidoNegocio _pedidoNegocio;
		readonly IPagamentoNegocio _pagamentoNegocio;
		readonly IAsaasService _asaasService;

        public BilletController(IUsuarioNegocio usuarioNegocio, IPedidoNegocio pedidoNegocio, IPagamentoNegocio pagamentoNegocio, IAsaasService asaasService, IUsuarioEnderecoNegocio usuarioEnderecoNegocio, ICredenciamentoNegocio credenciamentoNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
            _pedidoNegocio = pedidoNegocio;
            _pagamentoNegocio = pagamentoNegocio;
            _asaasService = asaasService;
            _usuarioEnderecoNegocio = usuarioEnderecoNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
        }

        [HttpPost("create-asaas-billet")]
		public async Task<IActionResult> CreateAsaasBillet([FromBody] CreateAsaasBilletRequest request)
		{
			try
			{
				var pedido = _pedidoNegocio.GetNoTracking(x => x.IdPedido == request.PedidoId).FirstOrDefault();

				if (pedido is null)
					return NotFound("Pedido não encontrado");

				if (pedido.Pago)
					return BadRequest("Pedido já está pago");

				var usuario = _usuarioNegocio.GetById(pedido.IdUsuario);

				if (usuario is null)
					return NotFound("Usuário não encontrado");

				var credenciamento = _credenciamentoNegocio.GetNoTracking(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault();

                if (credenciamento is null)
                    return NotFound("Credenciamento não encontrado");

                var name = credenciamento.Estabelecimento;
				var document = credenciamento.Cnpj;
				var email = credenciamento.Email;
				var mobilePhone = UtilBase.FiltrarDigitos(credenciamento.Telefone);

				var usuarioEndereco = _usuarioEnderecoNegocio.Get(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault();

                if (usuarioEndereco is null)
                    return NotFound("Endereço do usuário não encontrado");

                var customerRequest = new CustomerRequest
				{
					Name = name,
					Document = document,
					Email = email,
					MobilePhone = mobilePhone,
					PostalCode = usuarioEndereco.Cep,
					AddressNumber = usuarioEndereco.Numero,
				};

				var customers = await _asaasService.GetCustomerByQueryAsync(customerRequest.Email, customerRequest.Document);

				CustomerResponse customerResponse = customers.Data.FirstOrDefault() ?? new();

				if (customerResponse == null || string.IsNullOrEmpty(customerResponse.Id))
					customerResponse = await _asaasService.CreateCustomerAsync(customerRequest);
				else
					customerResponse = await _asaasService.UpdateCustomerAsync(customerResponse.Id, customerRequest);

				if (customerResponse == null || string.IsNullOrEmpty(customerResponse.Id))
					return BadRequest("Erro ao criar cliente");

				var paymentRequest = new NewPaymentRequest(PaymentType.BOLETO)
				{
					Customer = customerResponse.Id,
					Value = Convert.ToDouble(pedido.ValorPedido + pedido.ValorTaxa),
					DueDate = DateTimeOffset.UtcNow.AddDays(5),
					Description = "Pagamento de cashback",
					ExternalReference = pedido.Codigo
				};

				var paymentResponse = await _asaasService.CreatePaymentAsync(paymentRequest);

				if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.Id))
					return BadRequest("Erro ao criar pagamento");

				pedido.CodigoReferenciaBoleto = paymentResponse.Id;
				pedido.UrlBoleto = paymentResponse.InvoiceUrl.AbsoluteUri;

				_pedidoNegocio.Update(pedido);
				_pedidoNegocio.SaveChanges();

				var pagamento = _pagamentoNegocio.GetNoTracking(x => x.IdPedido == request.PedidoId).FirstOrDefault();

				pagamento.DataValidade = paymentResponse.DueDate.Date;
				pagamento.CodigoReferenciaBoleto = paymentResponse.InvoiceNumber;
				pagamento.UrlBoleto = paymentResponse.InvoiceUrl.AbsoluteUri;
				pagamento.LinhaDigitavelBoleto = string.Empty;

				_pagamentoNegocio.Update(pagamento);
				_pagamentoNegocio.SaveChanges();

				return Ok(new
				{
					Message = "Boleto criado com sucesso",
					InvoiceUrl = paymentResponse.InvoiceUrl,
					DueDate = paymentResponse.DueDate,
					PedidoId = request.PedidoId
                });
			}
			catch (Exception ex)
			{
				string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

				// Log the exception (ex)
				return StatusCode(500, new { error = message });
			}
		}

	}
}
