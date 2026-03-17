using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Api.Service;
using MMN.Api.Services;
using MMN.Api.Models.Response.Asaas.Payment;
using MMN.INegocio.Negocio;
using MMN.Repositorio.Contexto;
using MMN.Util.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public class ExtratoAsaas : BaseService, IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _isRunning;
        private bool _isDev;

        public ExtratoAsaas(IServiceProvider serviceProvider, IHostEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _isDev = env.IsDevelopment();
            _isRunning = false;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("ExtratoAsaas stopped");
            });

            return task;
        }

        public override void TimerTick(object info)
        {
            var nowBr = DateTime.UtcNow.HorarioBrasilia();

            if (!_isDev && !_isRunning && nowBr.Hour == 05 && nowBr.Minute == 00 && nowBr.Second == 0)
            {
                _isRunning = true;
                try
                {
                    ProcessarBoletosPagosAsync().Wait();
                }
                finally
                {
                    _isRunning = false;
                }
            }
        }

        private async Task ProcessarBoletosPagosAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var asaasService = scope.ServiceProvider.GetRequiredService<IAsaasService>();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var pedidoNegocio = scope.ServiceProvider.GetRequiredService<IPedidoNegocio>();

            var list = await asaasService.GetBoletosLast30DaysAsync();
            if (list == null || list.Count == 0)
                return;

            foreach (var payment in list)
            {
                try
                {
                    var pedido = await db.Pedido.AsNoTracking().FirstOrDefaultAsync(p => p.Codigo == payment.ExternalReference);
                    if (pedido == null || pedido.Pago)
                        continue;

                    decimal valorPago = (decimal)payment.Value;
                    decimal valorPedido = (decimal)Math.Round(pedido.ValorPedido, 2);

                    if (valorPedido > valorPago)
                        continue;

                    var pagamento = db.Pagamento
                        .Include(i => i.Pedido)
                            .ThenInclude(p => p.UsuarioProduto)
                                .ThenInclude(u => u.Produto)
                        .AsNoTracking()
                        .FirstOrDefault(p => p.IdPedido == pedido.IdPedido && !p.Pago);

                    if (pagamento != null)
                        pedidoNegocio.PagarParcela(pagamento.IdPedido.Value, pagamento.NumeroParcela, pagamento.DataReferencia, distribuirNaRede: true);
                }
                catch (Exception)
                {
                    // Evita que um erro em um item interrompa o processamento diário
                }
            }
        }

        private static bool IsPaymentReceived(PaymentResponse payment)
        {
            if (payment?.Status == null) return false;

            var status = payment.Status.ToUpperInvariant();
            // Considera status de recebimento/confirmado
            return status == "RECEIVED" || status == "RECEIVED_IN_CASH" || status == "CONFIRMED";
        }
    }
}

