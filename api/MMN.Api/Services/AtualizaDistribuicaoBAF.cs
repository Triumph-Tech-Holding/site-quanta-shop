using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMN.Api.Service;
using MMN.Repositorio.Contexto;
using MMN.Repositorio.Repositorio;
using MMN.Util.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public class AtualizaDistribuicaoBAF : BaseService, IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _scope;
        private DateTime _ultimaExecucao { get; set; }
        private DatabaseContext _context;
        public bool _isRunning { get; set; }
        private readonly ILogger<AtualizaDistribuicaoBAF> _logger;
        private readonly ProceduresRepositorio _proceduresRepository;

        public AtualizaDistribuicaoBAF(IServiceProvider serviceProvider, IHostingEnvironment env, ILogger<AtualizaDistribuicaoBAF> logger)
        {
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _proceduresRepository = new ProceduresRepositorio(_context);
            _logger = logger;
            _isRunning = false;
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

        public override void TimerTick(object info)
        {
            try
            {
                if (!_isRunning && PassouUmaHora())
                {
                    if (_context == null)
                    {
                        _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    }

                    _isRunning = true;
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);

                    VerificaPagamentos();

                    AtualizarDistribuicao();
                }
            }
            finally
            {
                Timer.Change(TickInterval, TickInterval);
            }
        }

        private void VerificaPagamentos()
        {
            try
            {
                var pedidos = _context.Pedido.Include(p => p.Pagamentos).Where(p => p.DataPedido < DateTime.Now.AddDays(-90) && !p.Pago);

                foreach (var pedido in pedidos)
                {
                    // Pedido tem ao menos uma parcela paga
                    if (pedido.Pagamentos != null && pedido.Pagamentos.Count() > 0 && pedido.Pagamentos.Any(p => p.Pago))
                    {
                        // Inativa o usuário
                        var usuario = _context.Usuario.FirstOrDefault(u => u.IdUsuario == pedido.IdUsuario);
                        usuario.Ativo = false;
                        _context.Update(usuario);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                _logger.LogInformation($"Exception: {msg}");
            }
        }

        private bool AtualizarDistribuicao()
        {
            try
            {
                _context.Database.BeginTransaction();

                var lancamentos = _context.Lancamento
                    .Where(l => (l.IdTipo == 48 || l.IdTipo == 55) && l.Bloqueado)
                    .Include(l => l.Transacao)
                        .ThenInclude(t => t.Pedido)
                    .ToList();

                foreach (var lancamento in lancamentos)
                {
                    // Lançamento dentro do prazo máximo
                    if ((DateTime.UtcNow.HorarioBrasilia() - lancamento.DataLancamento).TotalDays < 62)
                    {
                        // Verifica plano do patrocinador
                        var plano = _context.UsuarioProduto.AsNoTracking().FirstOrDefault(p => p.Ativo && p.IdUsuario == lancamento.IdUsuario);

                        // Patrocinador com plano superior ao Vision
                        if (plano.IdProduto > 1)
                        {
                            lancamento.Valor = CalculaValor(plano.IdProduto, lancamento.Valor, lancamento.Transacao.Pedido.First().IdPedido);
                            lancamento.Bloqueado = false;

                            var retido = _context.LancamentoRetido.FirstOrDefault(r => r.IdLancamento == lancamento.IdLancamento);
                            retido.Valor = lancamento.Valor * (decimal)0.10;
                            _context.LancamentoRetido.Update(retido);
                        }
                    }
                    else // Distribui para a Big Cash
                    {
                        lancamento.Bloqueado = false;
                        lancamento.IdUsuario = new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B");
                    }
                    _context.Lancamento.Update(lancamento);
                }

                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                _logger.LogInformation($"Exception: {msg}");

                _context.Database.RollbackTransaction();
            }
            finally
            {
                _isRunning = false;
            }

            return true;
        }

        private decimal CalculaValor(int idPlano, decimal valorLanc, long idPedido)
        {
            var plano = _context.Produto.FirstOrDefault(p => p.IdProduto == idPlano);
            var pedido = _context.Pedido.FirstOrDefault(p => p.IdPedido == idPedido);
            var valor = (plano.Valor / pedido.NumeroParcelas) * (decimal)0.15;

            if (valor > valorLanc)
            {
                return valorLanc;
            }

            return valor;
        }

        private bool PassouUmaHora()
        {
            if ((DateTime.Now - _ultimaExecucao).TotalHours >= 1)
            {
                _ultimaExecucao = DateTime.Now;
                return true;
            }

            return false;
        }

    }
}
