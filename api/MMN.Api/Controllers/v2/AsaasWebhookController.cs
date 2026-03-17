using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MMN.Api.Models.Request.Asaas;
using MMN.Api.Models.Request.Asaas.Customer;
using MMN.Api.Models.Response.Asaas;
using MMN.Api.Services;
using MMN.INegocio.Negocio;
using MMN.Repositorio.Contexto;
using MMN.Util.Extensions;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2
{
    [ApiController]
    [Route("api/v2/asaas/webhook")]
    public class AsaasWebhookController : ControllerBase
    {
        private readonly IAsaasService _asaasService;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly IHostEnvironment _env;

        public AsaasWebhookController(IAsaasService asaasService, IPedidoNegocio pedidoNegocio, IHostEnvironment env)
        {
            _asaasService = asaasService;
            _pedidoNegocio = pedidoNegocio;
            _env = env;
        }

        [HttpPost("payment-confirmed")]
        public async Task<IActionResult> PaymentConfirmed([FromServices] DatabaseContext db)
        {
            // Ler corpo bruto, salvar em disco e tentar desserializar
            try
            {
                Request.EnableBuffering();
                string body;
                using (var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                    Request.Body.Position = 0;
                }

                var basePath = Path.Combine(_env.ContentRootPath, "App_Data", "Webhooks", "Asaas");
                Directory.CreateDirectory(basePath);
                var fileName = $"asaas-webhook-{DateTime.UtcNow.HorarioBrasilia():yyyyMMdd-HHmmss-fff}-{Guid.NewGuid():N}.json";
                var filePath = Path.Combine(basePath, fileName);
                await System.IO.File.WriteAllTextAsync(filePath, body);

                var request = JsonConvert.DeserializeObject<EventRequest>(body);

                if (request == null || string.IsNullOrEmpty(request.Payment.ExternalReference))
                    return BadRequest("ExternalReference obrigatório");

                var pedido = await db.Pedido.AsNoTracking().FirstOrDefaultAsync(p => p.Codigo == request.Payment.ExternalReference);

                if (pedido == null)
                    return NotFound("Pedido não encontrado");

                if (pedido.Pago)
                    return BadRequest("Pedido já está pago");

                decimal valorPago = (decimal)request.Payment.Value;
                decimal valorPedido = (decimal)Math.Round(pedido.ValorPedido, 2);

                if (valorPedido > valorPago)
                    return BadRequest("Valor pago é menor que o valor do pedido");

                var pagamento = db.Pagamento
                           .Include(i => i.Pedido)
                           .ThenInclude(p => p.UsuarioProduto)
                                  .ThenInclude(u => u.Produto)
                           .AsNoTracking()
                           .FirstOrDefault(p => p.CodigoReferenciaBoleto == request.Payment.Id && !p.Pago);

                if (pagamento != null)
                    _pedidoNegocio.PagarParcela(pagamento.IdPedido.Value, pagamento.NumeroParcela, pagamento.DataReferencia, distribuirNaRede: true);

                return Ok();
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }

        [HttpPost("withdrawal-validation")]
        public IActionResult WithdrawalValidation([FromBody] EventRequest request, [FromServices] DatabaseContext db)
        {
            return Ok(new { status = "APPROVED" });
        }

        [HttpGet("bill-issuance-test")]
        public async Task<IActionResult> BillIssuanceTest(string name, string document, string email, string mobilePhone)
        {
            try
            {
                var customerRequest = new CustomerRequest
                {
                    Name = name,
                    Document = document,
                    Email = email,
                    MobilePhone = mobilePhone,
                    PostalCode = "80330-310",
                    AddressNumber = "387",
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
                    Value = 100.00,
                    DueDate = DateTimeOffset.UtcNow.AddDays(5),
                    Description = "Teste de pagamento",
                    ExternalReference = "test-customer-001"
                };

                // Simulate creating a payment
                var paymentResponse = await _asaasService.CreatePaymentAsync(paymentRequest);

                if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.Id))
                    return BadRequest("Erro ao criar pagamento");

                // Endpoint de teste para verificar se o controller está funcionando
                return Ok(new { messagem = "Asaas Webhook Controller is working", customerResponse, paymentResponse });
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(new { error = message });
            }
        }
    }
}
