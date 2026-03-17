using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Dominio.Model;
using MMN.Integracoes;
using MMN.Integracoes.Afilio;
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

    public class BuscaComprasAbertasAfilio : IHostedService
    {
        public virtual int TickInterval { get; set; } = 900000;
        public virtual Timer Timer { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private AfilioBLL _afilio = new AfilioBLL();
        private readonly IServiceScope _scope;
        private readonly DatabaseContext _context;
        private CultureInfo cultures;
        public bool IsDev { get; set; }

        public BuscaComprasAbertasAfilio(IServiceProvider serviceProvider, IHostingEnvironment env)
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
            _scope.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("start");
                Timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
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
            var vendas = await _afilio.GetCompras();

            var percentualCashback = Convert.ToDecimal(_context.Configuracao.FirstOrDefault(w => w.Chave.Equals("PERCENTUAL_CASHBACK")).Valor, cultures);

            if (vendas != null && vendas.Count > 0)
            {
                foreach (var item in vendas)
                {
                    if (!_context.Pedido.Any(a => a.IdVendaAfilio == Convert.ToInt64(item.saleid)))
                    {
                        var anunciante = _context.Anunciante.FirstOrDefault(w => w.IdAfilio == item.progid);

                        var listPedidoDetalhe = new List<PedidoDetalhe>()
                        {
                            new PedidoDetalhe()
                            {
                                Descricao = $"Compra em: {(anunciante != null ? anunciante.Nome : "----")}",
                                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                Ativo = true,
                                IdStatus = (int)StatusTransacaoEnum.EmProcessamento
                            }
                        };

                        var pedido = new Pedido
                        {
                            IdUsuario = Guid.Parse(item.aff_xtra),
                            DataPedido = DateTime.Parse(item.date),
                            ValorTaxa = 0,
                            ValorPedido = Convert.ToDecimal(item.order_price, cultures),
                            Pago = false,
                            Ativo = true,
                            MeioPagamento = 0,
                            Quantidade = 1,
                            Cotacao = 0,
                            IdVendaZanox = null,
                            IdVendaAfilio = Convert.ToInt64(item.saleid),
                            PedidoDetalhe = listPedidoDetalhe,
                            Cashback = (Convert.ToDecimal(item.comission, cultures) * percentualCashback)
                        };

                        _context.Add(pedido);
                        _context.SaveChanges();
                    }
                }
            }

            return true;
        }
    }
}
