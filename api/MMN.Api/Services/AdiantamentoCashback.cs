using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Dominio.Enum;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public class AdiantamentoCashback : IHostedService, IDisposable
    {
        private readonly bool _isDevelopment;
        private Timer _timer;
        private Task _lastTask;
        private CancellationToken _cancellationToken;
        private readonly IServiceProvider _serviceProvider;

        public AdiantamentoCashback(
            IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            _isDevelopment = env.IsDevelopment();
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            StopAsync(new CancellationToken(false)).Wait();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var comecarEm = TimeSpan.FromMinutes((60 - DateTime.Now.Minute) % 60);
                var repetirEm = TimeSpan.FromHours(1);

                _cancellationToken = cancellationToken;

                if (_isDevelopment)
                {
                    comecarEm = TimeSpan.Zero;
                }
                else if (comecarEm < repetirEm)
                {
                    comecarEm += repetirEm;
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    _timer = new Timer(TimerTick, cancellationToken, comecarEm, repetirEm);
                }
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var serviceToken = _cancellationToken;
            _cancellationToken = new CancellationToken(true);

            cancellationToken.Register(() => _cancellationToken = serviceToken);


            if (_lastTask != null)
            {
                await _lastTask;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _timer.Dispose();
            }
            else
            {
                _cancellationToken = serviceToken;
            }
        }

        public void TimerTick(object state)
        {
            var scope = _serviceProvider.CreateScope();
            var scopedService = scope.ServiceProvider;
            var pedidoRepositorio = scopedService.GetRequiredService<IPedidoRepositorio>();
            var cashbackNegocio = scopedService.GetRequiredService<ICashbackNegocio>();
            var configuracaoNegocio = scopedService.GetRequiredService<IConfiguracaoNegocio>();

            var cancellationToken = (CancellationToken)state;

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var configuracoes = configuracaoNegocio.GetFromCache();
            //var maximoAfilio = decimal.Parse(
            //    configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_AFILIO")?.Valor ?? "0",
            //    CultureInfo.InvariantCulture);
            var maximoAwin = decimal.Parse(
                 configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_AWIN")?.Valor ?? "0",
                CultureInfo.InvariantCulture);
            var maximoZanox = decimal.Parse(
                 configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_ZANOX")?.Valor ?? "0",
                CultureInfo.InvariantCulture);
            var maximoCredenciado = decimal.Parse(
                 configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_CREDENCIADO")?.Valor ?? "0",
                CultureInfo.InvariantCulture);

            //var afilioAtivo = configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_AFILIO")?.Ativo == true;
            var awinAivo = configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_AWIN")?.Ativo == true;
            var zanoxAtivo = configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_ZANOX")?.Ativo == true;
            var credenciadoAtivo = configuracoes.FirstOrDefault(w => w.Chave == "PRE_APROVADO_CREDENCIADO")?.Ativo == true;

            var pedidosCashback = pedidoRepositorio
                .Get(w =>
                    w.Ativo == true &&
                    (w.Tipo == (int)TipoPedido.CashbackExterno ||
                        w.Tipo == (int)TipoPedido.CashbackCredenciado) &&
                    (w.Status == (int)StatusPedido.AguardandoCashback ||
                        w.Status == (int)StatusPedido.AguardandoFaturaCredenciado ||
                        w.Status == (int)StatusPedido.AguardandoPagamento) &&
                    (
                        //(afilioAtivo && maximoAfilio > 0 && w.IdVendaAfilio != null && w.Cashback <= maximoAfilio) ||
                        (awinAivo && maximoAwin > 0 && w.IdAwinTransaction != null && w.Cashback <= maximoAwin) ||
                        (zanoxAtivo && maximoZanox > 0 && w.IdVendaZanox != null && w.Cashback <= maximoZanox) ||
                        (credenciadoAtivo && maximoCredenciado > 0 && w.IdUsuarioComerciante != null && w.Cashback <= maximoCredenciado)
                    ))
                .AsNoTracking()
                .Select(s => s.IdPedido)
                .ToArray();

            foreach (var idPedido in pedidosCashback)
            {
                try
                {
                    _lastTask = Task.Run(async () =>
                    {
                        await cashbackNegocio.LancarCashback(idPedido, finalizado: false);
                    });

                    _lastTask.Wait();
                }
                catch
                {

                }
            }

            scope.Dispose();
        }

    }
}
