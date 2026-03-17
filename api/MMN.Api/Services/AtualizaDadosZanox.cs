using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Integracoes;
using MMN.Integracoes.Zanox;
using MMN.Repositorio.Contexto;
using MMN.Repositorio.Repositorio;
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
    public class AtualizaDadosZanox : BaseService, IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private ZanoxBLL _zanox = new ZanoxBLL();
        private readonly IServiceScope _scope;
        private readonly DatabaseContext _context;
        private readonly ProceduresRepositorio _proceduresRepository;
        private readonly ICashbackNegocio _cashbackNegocio;

        private CultureInfo cultures;

        public AtualizaDadosZanox(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _proceduresRepository = new ProceduresRepositorio(_context);
            cultures = new CultureInfo("en-US");
            _cashbackNegocio = _scope.ServiceProvider.GetRequiredService<ICashbackNegocio>();
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

        public async override void TimerTick(object info)
        {
            try
            {
                if (DateTime.UtcNow.HorarioBrasilia().Hour == 01)
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    await BuscarComprasAbertas();
                    await ProcessaPedidos();
                    //await VerificaAtualizacaoAnunciante();
                }
            }
            finally
            {
                Timer.Change(TickInterval, TickInterval);
            }
        }

        private async Task<bool> InserirProgramasRecursivo(List<ProgramApplicationItem> list, int total, IEnumerable<Anunciante> anunciantesLocal, int index = 0)
        {
            if (index == total)
                return true;
            else
            {
                var programLocal = anunciantesLocal.FirstOrDefault(f => f.IdProgramZanox == list[index].program.id);

                if (programLocal == null)
                {
                    var programZanox = await _zanox.GetProgramsById(list[index].program.id);

                    if (programZanox.programItem != null)
                    {
                        var anunciante = new Anunciante
                        {
                            Nome = programZanox.programItem.FirstOrDefault().name,
                            ImagemUrl = programZanox.programItem.FirstOrDefault().image,
                            Cashback = 0,
                            Ativo = true,
                            IdProgramZanox = list[index].program.id,
                            DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                            DataAtualizacao = DateTime.UtcNow.HorarioBrasilia()
                        };
                        _context.Anunciante.Add(anunciante);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var anunciante = new Anunciante
                        {
                            Nome = list[index].program.name,
                            ImagemUrl = string.Empty,
                            Cashback = 0,
                            Ativo = true,
                            IdProgramZanox = list[index].program.id,
                            DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                            DataAtualizacao = DateTime.UtcNow.HorarioBrasilia()
                        };
                        _context.Anunciante.Add(anunciante);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (!programLocal.Ativo)
                {
                    programLocal.Ativo = true;
                    programLocal.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                    _context.Update(programLocal);
                    await _context.SaveChangesAsync();
                }

                return await InserirProgramasRecursivo(list, list.Count, anunciantesLocal, index + 1);
            }
        }

        private async Task<bool> BuscarComprasAbertas()
        {
            var sales = await _zanox.GetSalesOpen(0);
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
                                Cashback = (item.commission * percentualCashback),
                                Tipo = (int)TipoPedido.CashbackExterno,
                                Status = (int)StatusPedido.AguardandoCashback,
                            };

                            var anunciante = _context.Anunciante.FirstOrDefault(f => f.IdProgramZanox == item.program.id);

                            var transacao = _cashbackNegocio.CriarTransacaoCashbackAsync(pedido, anunciante).Result;
                            _context.Add(transacao);

                            pedido.IdTransacao = transacao.IdTransacao;
                            pedido.Transacao = transacao;

                            _context.Add(pedido);
                            _context.SaveChanges();
                        }
                    }
                }
            }

            return true;
        }

        private async Task<bool> ProcessaPedidos()
        {
            var pedidos = await _context.Pedido
                .Include(i => i.PedidoDetalhe)
                .Include(i => i.Transacao)
                .Where(w => !w.Pago && w.IdVendaZanox.HasValue && w.Ativo.Value)
                .ToListAsync();

            return await ProcessaCompraZanoxRecursive(pedidos);
        }

        private async Task<bool> ProcessaCompraZanoxRecursive(IList<Pedido> pedidos, int index = 0)
        {
            if (index == pedidos.Count)
                return true;
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var venda = await _zanox.GetSaleById(pedidos[index].IdVendaZanox.Value);

                        if (venda.saleItem != null && venda.saleItem.Count > 0)
                        {
                            var anunciante = _context.Anunciante.FirstOrDefault(f => f.IdProgramZanox == venda.saleItem.FirstOrDefault().program.id);

                            if (pedidos[index].Transacao == null)
                            {

                                var transacao = _cashbackNegocio.CriarTransacaoCashbackAsync(pedidos[index], anunciante).Result;
                                _context.Add(transacao);

                                pedidos[index].IdTransacao = transacao.IdTransacao;
                                pedidos[index].Transacao = transacao;
                                _context.Update(pedidos[index]);
                            }

                            pedidos[index].Transacao.IdAnunciante = anunciante.IdAnunciante;
                            pedidos[index].Transacao.Anunciante = anunciante;
                            _context.Update(pedidos[index]);

                            if (venda.saleItem.FirstOrDefault().reviewState.Equals("approved"))
                            {
                                if (!pedidos[index].PedidoDetalhe.Any(a => a.IdStatus == (int)StatusTransacaoEnum.Aprovado))
                                {
                                    var pedidoDetalhe = new PedidoDetalhe()
                                    {
                                        IdPedido = pedidos[index].IdPedido,
                                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                        Ativo = true,
                                        Descricao = $"Compra em: {venda.saleItem.FirstOrDefault().program.name} teve uma atualização!",
                                        IdStatus = (int)StatusTransacaoEnum.Aprovado
                                    };

                                    _context.PedidoDetalhe.Add(pedidoDetalhe);

                                    pedidos[index].Transacao.IdStatus = (int)StatusTransacaoEnum.Aprovado;
                                    _context.Update(pedidos[index].Transacao);

                                    _context.SaveChanges();
                                }
                            }
                            else if (venda.saleItem.FirstOrDefault().reviewState.Equals("rejected"))
                            {
                                if (!pedidos[index].PedidoDetalhe.Any(a => a.IdStatus == (int)StatusTransacaoEnum.Recusado))
                                {
                                    var pedidoDetalhe = new PedidoDetalhe()
                                    {
                                        IdPedido = pedidos[index].IdPedido,
                                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                        Ativo = true,
                                        Descricao = $"Compra em: {venda.saleItem.FirstOrDefault().program.name} teve uma atualização!",
                                        IdStatus = (int)StatusTransacaoEnum.Recusado
                                    };

                                    _context.PedidoDetalhe.Add(pedidoDetalhe);
                                    _context.SaveChanges();

                                    pedidos[index].Transacao.IdStatus = (int)StatusTransacaoEnum.Recusado;
                                    pedidos[index].Transacao.Ativo = false;
                                    _context.Update(pedidos[index].Transacao);

                                    pedidos[index].Pago = false;
                                    pedidos[index].Ativo = false;
                                    pedidos[index].ValorPago = 0;
                                    pedidos[index].Status = (int)StatusPedido.Cancelado;

                                    _context.Update(pedidos[index]);
                                    _context.SaveChanges();
                                }
                            }
                            else if (venda.saleItem.FirstOrDefault().reviewState.Equals("confirmed"))
                            {
                                if (!pedidos[index].PedidoDetalhe.Any(a => a.IdStatus == (int)StatusTransacaoEnum.Finalizada))
                                {
                                    _context.SaveChanges();

                                    await _cashbackNegocio.LancarCashback(
                                       pedidos[index].IdPedido,
                                       anunciante: anunciante,
                                       finalizado: true,
                                       dbTransaction: transaction);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }

                return await ProcessaCompraZanoxRecursive(pedidos, index + 1);
            }
        }
    }
}
