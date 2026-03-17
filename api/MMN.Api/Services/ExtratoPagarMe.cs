using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
using MMN.Util;
using MMN.Util.Extensions;
using MMN.Util.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Service
{
    public class ExtratoPagarMe : BaseService, IHostedService
    {
        private readonly IServiceScope _scope;
        private readonly DatabaseContext _ctx;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly IProceduresRepositorio _proceduresRepository;
        private readonly bool _isDev;
        private bool _isRunning;

        public ExtratoPagarMe(
            IServiceProvider serviceProvider,
            IHostingEnvironment env,
            IOptions<AppSettings> appSettings)
        {
            _isDev = env.IsDevelopment();
            _scope = serviceProvider.CreateScope();
            _ctx = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _pedidoNegocio = _scope.ServiceProvider.GetRequiredService<IPedidoNegocio>();
            _proceduresRepository = _scope.ServiceProvider.GetRequiredService<IProceduresRepositorio>();
        }

        public void Dispose()
        {
            Timer?.Dispose();
            _scope.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Stopped");
            });

            return task;
        }

        public override void TimerTick(object info)
        {
            if (!_isDev && (DateTime.UtcNow.HorarioBrasilia().Hour == 01 || DateTime.UtcNow.HorarioBrasilia().Hour == 13) && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0)
            {
                _isRunning = true;
                VerificarBoletosPagos();
            }
        }
        private void VerificarBoletosPagos()
        {
            var response = Pagarme.Extrato(PagarMe.TransactionStatus.Paid).Result;

            foreach (var item in response)
            {
                try
                {
                    var pagamento = _ctx.Pagamento
                        //.Include(i => i.Pedido)
                        //    .ThenInclude(p => p.UsuarioProduto)
                        //        .ThenInclude(u => u.Produto)
                        .AsNoTracking()
                        .FirstOrDefault(p => p.CodigoReferenciaBoleto == item.id && !p.Pago);

                    if (pagamento != null)
                    {
                        _pedidoNegocio.PagarParcela(pagamento.IdPedido.Value, pagamento.NumeroParcela, pagamento.DataReferencia, distribuirNaRede: true);
                    }
                }
                catch (Exception)
                {

                }
            }

            _isRunning = false;
        }
    }
}
