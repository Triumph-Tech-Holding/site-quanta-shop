using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Afilio;
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

    public class AtualizaDadosAfilio : BaseService, IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private AfilioBLL _afilio = new AfilioBLL();
        private readonly IServiceScope _scope;
        private readonly DatabaseContext _context;
        private CultureInfo cultures;
        private readonly ICashbackNegocio _cashbackNegocio;

        public AtualizaDadosAfilio(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            cultures = new CultureInfo("en-US");
            _cashbackNegocio = (ICashbackNegocio)_scope.ServiceProvider.GetRequiredService(typeof(ICashbackNegocio));
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
                if (DateTime.UtcNow.HorarioBrasilia().Hour == 02 && _context != null)
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                   // await BuscarComprasAbertas();
                    await ProcessaPedidos();
                    await VerificaAtualizacaoAnunciante();
                }
            }
            finally
            {
                Timer.Change(TickInterval, TickInterval);
            }
        }

        private async Task<List<Campanha>> GetCampanhas()
        {
            return await _afilio.GetCampanhas();
        }

        private string GetImagemCampanha(Anunciante anunciante)
        {
            var imgUrl = Helpers.ImagemHelper.GetImagemPeloNome(anunciante.Nome);
            return imgUrl;
        }

        private void AtualizaImagensProgramas(List<Anunciante> anunciantes)
        {
            foreach (var anunciante in anunciantes)
            {
                anunciante.ImagemUrl = GetImagemCampanha(anunciante);
                _context.Update(anunciante);
            }

            _context.SaveChanges();
        }

        private async Task<bool> VerificaAtualizacaoAnunciante()
        {
            try
            {
                var campanhas = await GetCampanhas();
                var anunciantesLocal = _context.Anunciante.Where(w => !string.IsNullOrEmpty(w.IdAfilio)).ToList();


                //AtualizaImagensProgramas(anunciantesLocal);

                await InserirProgramasRecursivo(campanhas, campanhas.Count, anunciantesLocal);
                await InativarProgramas(campanhas, anunciantesLocal);

                await VerificaAtualizacaoAnuncianteCashback(_context.Anunciante.Where(w => w.Ativo && !string.IsNullOrEmpty(w.IdAfilio)).ToList(), campanhas);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task<bool> InserirProgramasRecursivo(IList<Campanha> list, int total, IEnumerable<Anunciante> anunciantesLocal, int index = 0)
        {
            if (index == total)
                return true;
            else
            {
                var programLocal = anunciantesLocal.FirstOrDefault(f => f.IdAfilio == list[index].id);

                if (programLocal == null)
                {
                    var anunciante = new Anunciante
                    {
                        Nome = list[index].nom,
                        ImagemUrl = list[index].url_image != null ? list[index].url_image.ToString() : string.Empty,
                        Cashback = 0,
                        Ativo = true,
                        IdProgramZanox = null,
                        DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                        IdAfilio = list[index].id,
                        OrdenacaoAnuncio = new OrdenacaoAnuncio()
                    };

                    _context.Anunciante.Add(anunciante);
                    await _context.SaveChangesAsync();
                }
                else if (!programLocal.Ativo && programLocal.EditadoUsuario == false)
                {

                    programLocal.Ativo = true;

                    //var imagem = GetImagemCampanha(programLocal);
                    //var apiImagem = list[index].url_image != null ? list[index].url_image.ToString() : string.Empty;

                    //programLocal.ImagemUrl = !string.IsNullOrEmpty(imagem) ? imagem : apiImagem;
                    programLocal.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                    _context.Anunciante.Update(programLocal);
                    await _context.SaveChangesAsync();
                }

                return await InserirProgramasRecursivo(list, list.Count, anunciantesLocal, index + 1);
            }
        }

        private async Task<bool> InativarProgramas(IList<Campanha> list, IEnumerable<Anunciante> anunciantesLocal)
        {
            foreach (var anunciante in anunciantesLocal)
            {
                var item = list.FirstOrDefault(f => f.id == anunciante.IdAfilio);

                if (item == null)
                {
                    if (anunciante.Ativo)
                    {
                        anunciante.Ativo = false;
                        anunciante.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                        _context.Anunciante.Update(anunciante);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return true;
        }

        private async Task<bool> VerificaAtualizacaoAnuncianteCashback(IList<Anunciante> anunciantes, IList<Campanha> campanhas)
        {
            try
            {
                foreach (var anunciante in anunciantes)
                {
                    var anuncianteCashback = _context.AnuncianteCashBack.FirstOrDefault(f => f.IdAnunciante == anunciante.IdAnunciante && f.Descricao.Contains("Afilio"));
                    var campanha = campanhas.FirstOrDefault(f => f.id == anunciante.IdAfilio);

                    if (campanha != null)
                    {
                        var valor = Convert.ToDecimal(campanha.saleprice, cultures);

                        string tipo;
                        switch (campanha?.salecurrency)
                        {
                            case "R$":
                                tipo = "amount";
                                break;
                            case "%":
                                tipo = "percentage";
                                break;
                            default:
                                tipo = null;
                                break;
                        }

                        if (anuncianteCashback != null)
                        {
                            if (anuncianteCashback.Percentual != valor ||
                                anuncianteCashback.Tipo != tipo)
                            {
                                anuncianteCashback.Percentual = valor;
                                anuncianteCashback.Tipo = tipo;
                                anuncianteCashback.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();

                                _context.Update(anuncianteCashback);
                            }
                        }
                        else
                        {
                            var anuncianteCashBack = new AnuncianteCashBack
                            {
                                Descricao = "Afilio",
                                Tipo = tipo,
                                Percentual = valor,
                                ValorFixo = null,
                                IdProgramZanox = null,
                                IdAnunciante = anunciante.IdAnunciante,
                                IdTrackingCategorie = null,
                                Ativo = true,
                                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                DataCadastro = DateTime.UtcNow.HorarioBrasilia()
                            };

                            _context.Update(anunciante);
                            _context.AnuncianteCashBack.Add(anuncianteCashBack);
                        }
                    }
                    else
                    {
                        anunciante.Ativo = false;
                    }

                    anunciante.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
                    _context.Update(anunciante);
                    await _context.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        private async Task<bool> BuscarComprasAbertas()
        {
            var vendas = await _afilio.GetCompras();

            if (vendas.Count > 0)
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
                            Cashback = Convert.ToDecimal(item.comission, cultures),
                            PercentualCashback =
                                (Convert.ToDecimal(item.comission, cultures)
                                * 100
                                / Convert.ToDecimal(item.order_price, cultures))
                                .Truncate(2),
                            Tipo = (int)TipoPedido.CashbackExterno,
                            Status = (int)StatusPedido.AguardandoCashback,
                        };

                        var transacao = _cashbackNegocio.CriarTransacaoCashbackAsync(pedido, anunciante).Result;

                        _context.Add(transacao);

                        pedido.IdTransacao = transacao.IdTransacao;
                        pedido.Transacao = transacao;

                        _context.Add(pedido);
                        _context.SaveChanges();
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
                .Where(w => !w.Pago && w.IdVendaAfilio.HasValue && w.Ativo.Value)
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
                        var vendas = await _afilio
                            .GetCompras(pedidos[index].DataPedido.AddDays(-1).ToString("yyyy-MM-dd"), pedidos[index].DataPedido.ToString("yyyy-MM-dd"));

                        var venda = vendas.FirstOrDefault(f => f.saleid == pedidos[index].IdVendaAfilio.ToString());

                        if (venda != null)
                        {
                            var anunciante = _context.Anunciante.FirstOrDefault(f => f.IdAfilio == venda.progid);


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

                            if (venda.status.ToLower().Equals("rejected"))
                            {
                                if (!pedidos[index].PedidoDetalhe.Any(a => a.IdStatus == (int)StatusTransacaoEnum.Recusado))
                                {
                                    var pedidoDetalhe = new PedidoDetalhe()
                                    {
                                        IdPedido = pedidos[index].IdPedido,
                                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                        Ativo = true,
                                        Descricao = $"Compra em: {anunciante.Nome} teve uma atualização!",
                                        IdStatus = (int)StatusTransacaoEnum.Recusado
                                    };

                                    _context.PedidoDetalhe.Add(pedidoDetalhe);
                                    _context.SaveChanges();

                                    pedidos[index].Transacao.IdStatus = (int)StatusTransacaoEnum.Recusado;
                                    _context.Update(pedidos[index].Transacao);

                                    pedidos[index].Pago = true;
                                    pedidos[index].Ativo = false;
                                    pedidos[index].ValorPago = 0;
                                    pedidos[index].Status = (int)StatusPedido.Cancelado;

                                    _context.Update(pedidos[index]);
                                    _context.SaveChanges();
                                }
                            }
                            else if (venda.status.ToLower().Equals("accepted"))
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
