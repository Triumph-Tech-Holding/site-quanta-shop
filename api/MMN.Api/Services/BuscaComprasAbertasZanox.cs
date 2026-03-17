using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Dominio.Model;
using MMN.Integracoes;
using MMN.Integracoes.Zanox;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Service
{

    public class BuscaComprasAbertasZanox : IHostedService
    {
        public virtual int TickInterval { get; set; } = 900000;
        public virtual Timer Timer { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private ZanoxBLL _zanox = new ZanoxBLL();
        private readonly IServiceScope _scope;
        private readonly DatabaseContext _context;
        private CultureInfo cultures;
        public bool IsDev { get; set; }

        public BuscaComprasAbertasZanox(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            IsDev = env.IsDevelopment();
            _scope = _serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            Timer = new Timer(TimerTick, null, 0, TickInterval);
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            cultures = new CultureInfo("en-US");
        }

        public void Dispose()
        {
            Timer?.Dispose();
            _context.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("start");
                Timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(TickInterval));
                Timer.Change(TickInterval, TickInterval);
            });
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Stopped");
            });
            return task;
        }

        public async void TimerTick(object info)
        {
            try
            {
                if (!IsDev)
                    await BuscarComprasAbertas();
            }
            finally
            {
                Timer.Change(TickInterval, TickInterval);
            }
        }

        private async Task<bool> BuscarComprasAbertas()
        {
            var sales = await _zanox.GetSalesOpen(0, DateTime.UtcNow.ToString("yyyy-MM-dd"));

            var totalPaginas = (int)Math.Ceiling((double)sales.total / sales.items);

            for (int i = 1; i < totalPaginas; i++)
            {
                var sales1 = await _zanox.GetSalesOpen(i);

                sales.saleItems.saleItem.AddRange(sales1.saleItems.saleItem);
                sales.items += sales1.items;
            }

            var percentualCashback = Convert.ToDecimal(_context.Configuracao.FirstOrDefault(w => w.Chave.Equals("PERCENTUAL_CASHBACK")).Valor, cultures);

            if (sales.saleItems != null)
            {
                foreach (var item in sales.saleItems.saleItem)
                {
                    if (item.gpps.gpp.Processar)
                    {
                        if (!_context.Pedido.Any(a => a.IdVendaZanox == item.idSale))
                        {
                            var listPedidoDetalhe = new List<PedidoDetalhe>()
                            {
                                new PedidoDetalhe()
                                {
                                    Descricao = $"Compra em: {item.program.name}",
                                    DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                    Ativo = true,
                                    IdStatus = (int)StatusTransacaoEnum.EmProcessamento
                                }
                            };

                            var pedido = new Pedido
                            {
                                IdUsuario = Guid.Parse(item.gpps.gpp.IdUsuario),
                                DataPedido = item.trackingDate,
                                ValorTaxa = 0,
                                ValorPedido = item.amount,
                                Pago = false,
                                Ativo = true,
                                MeioPagamento = 0,
                                Quantidade = 1,
                                Cotacao = 0,
                                IdVendaZanox = item.idSale,
                                PedidoDetalhe = listPedidoDetalhe,
                                Cashback = (item.commission * percentualCashback)
                            };

                            _context.Add(pedido);
                            _context.SaveChanges();
                        }
                    }
                }
            }

            return true;
        }
    }
}
