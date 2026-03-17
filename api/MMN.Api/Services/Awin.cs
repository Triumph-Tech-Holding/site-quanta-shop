using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMN.Api.Service;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Awin;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMN.Api.Services
{
    public class AwinService : BaseService, IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AwinService> _logger;
        private readonly Awin _awin;
        private bool _isRunning;
        private bool _isDev;
        private readonly ICurrencyService _currencyService;
        private readonly WhatsAppService _whatsAppService;

        public AwinService(IServiceProvider serviceProvider, ILogger<AwinService> logger, IHostEnvironment env, WhatsAppService whatsAppService,ICurrencyService currencyService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _awin = new Awin();
            _isRunning = false;
            _isDev = env.IsDevelopment();
            _whatsAppService = whatsAppService;
            _currencyService = currencyService;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _awin.AddAccountsAsync(new string[] { Environment.GetEnvironmentVariable("AWIN_TOKEN_1") });

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
            if (
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 00 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 04 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 08 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 12 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 16 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (!_isRunning && DateTime.UtcNow.HorarioBrasilia().Hour == 20 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0) ||
                (_isDev && !_isRunning)
                )
            {
                _isRunning = true;

                ListarTransacoesAsync(689359).Wait();
            }
        }

        private async Task ListarTransacoesAsync(long accountId)
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var cashbackNegocio = scope.ServiceProvider.GetRequiredService<ICashbackNegocio>();

                for (var dataInicial = DateTime.Now.ToUniversalTime().AddDays(1).AddMonths(-12); dataInicial < DateTime.Now.AddDays(1).ToUniversalTime(); dataInicial = dataInicial.AddMonths(1))
                {
                    var dataFinal = dataInicial.AddMonths(1);

                    var transactionsByTransactionDate = await _awin.TransactionsAsync(
                        accountId,
                        validationDate: false,
                        startDate: dataInicial,
                        endDate: dataFinal);

                    var transactionsByValidationDate = await _awin.TransactionsAsync(
                        accountId,
                        validationDate: true,
                        startDate: dataInicial,
                        endDate: dataFinal);

                    IEnumerable<Transaction> transactions = new List<Transaction>();

                    if (transactionsByTransactionDate != null && transactionsByValidationDate != null)
                        transactions = transactionsByTransactionDate.Union(transactionsByValidationDate, new TransactionComparer());

                    var tipoCashback = context.Tipo.First(f => f.Chave == "CHBK");

                    foreach (var transaction in transactions)
                    {
                        using var dbTransaction = context.Database.BeginTransaction();
                        try
                        {
                            if (context.Pedido.Any(p => p.IdAwinTransaction == transaction.id))
                            {
                                var pedido = context.Pedido
                                    .Include(i => i.Transacao)
                                    .FirstOrDefault(p => p.IdAwinTransaction == transaction.id);

                                if (pedido != null)
                                {
                                    if (pedido.Status == 2 || pedido.Status == 6 || pedido.Status == 8)
                                        continue;

                                    double usdPrice = 1;

                                    if (transaction.commissionAmount.currency == "USD" || transaction.saleAmount.currency == "USD")
                                    { 
                                        var response = await _currencyService.GetCurrencyValueAsync();

                                        usdPrice = response.Value;
                                    }

                                    if (transaction.commissionAmount.currency == "USD")
                                    {
                                        transaction.commissionAmount.amount = transaction.commissionAmount.amount * usdPrice;
                                    }

                                    if (transaction.saleAmount.currency == "USD")
                                    {
                                        transaction.saleAmount.amount = transaction.saleAmount.amount * usdPrice;
                                    }

                                    pedido.Cashback = (decimal)transaction.commissionAmount.amount;
                                    pedido.ValorPedido = (decimal)transaction.saleAmount.amount;
                                }

                                context.SaveChanges();

                                //1 - pending, 6 - approved, 5 - declined, 3 - deleted
                                if (!context.PedidoDetalhe.Any(p => p.IdPedido == pedido.IdPedido && p.IdStatus == GetAwinTransactionStatus(transaction.commissionStatus)))
                                {
                                    await EnviarNotificacoesAsync(pedido, transaction);

                                    var anunciante = context.Anunciante.AsNoTracking().FirstOrDefault(a => a.IdAwin == transaction.advertiserId.ToString());

                                    var pedidoDetalhe = new PedidoDetalhe
                                    {
                                        Descricao = $"Compra em: {anunciante.Nome}",
                                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                        Ativo = true,
                                        IdPedido = pedido.IdPedido,
                                        IdStatus = GetAwinTransactionStatus(transaction.commissionStatus)
                                    };

                                    context.Add(pedidoDetalhe);
                                    context.SaveChanges();

                                    if (pedido.Transacao == null)
                                    {
                                        var transacao = cashbackNegocio.CriarTransacaoCashbackAsync(pedido, anunciante).Result;
                                        context.Add(transacao);

                                        pedido.IdTransacao = transacao.IdTransacao;
                                        pedido.Transacao = transacao;

                                        context.Update(pedido);
                                    }

                                    pedido.IdAnunciante = anunciante.IdAnunciante;
                                    pedido.Transacao.IdAnunciante = anunciante.IdAnunciante;
                                    context.Update(pedido);

                                    if (pedidoDetalhe.IdStatus == (int)StatusTransacaoEnum.Recusado ||
                                        pedidoDetalhe.IdStatus == (int)StatusTransacaoEnum.Cancelada)
                                    {
                                        pedido.Transacao.IdStatus = pedidoDetalhe.IdStatus;
                                        context.Update(pedido.Transacao);

                                        pedido.Status = (int)StatusPedido.Cancelado;
                                        pedido.Ativo = false;
                                        context.Update(pedido);

                                        context.SaveChanges();
                                    }
                                    else if (pedidoDetalhe.IdStatus == (int)StatusTransacaoEnum.Aprovado)
                                    {
                                        context.SaveChanges();

                                        cashbackNegocio.LancarCashback(
                                            pedido.IdPedido,
                                            anunciante: anunciante,
                                            finalizado: true,
                                            dbTransaction: dbTransaction)
                                            .Wait();
                                    }
                                }
                            }
                            else
                            {
                                Usuario usuario = null;

                                if (transaction.clickRefs != null)
                                {
                                    Guid id;

                                    if (!string.IsNullOrEmpty(transaction.clickRefs.clickRef) && Guid.TryParse(transaction.clickRefs.clickRef, out id))
                                    {
                                        usuario = context.Usuario.FirstOrDefault(u => u.IdUsuario == new Guid(transaction.clickRefs.clickRef));
                                    }
                                    else if (!string.IsNullOrEmpty(transaction.clickRefs.clickRef2) && Guid.TryParse(transaction.clickRefs.clickRef2, out id))
                                    {
                                        usuario = context.Usuario.FirstOrDefault(u => u.IdUsuario == new Guid(transaction.clickRefs.clickRef2));
                                    }
                                    else
                                    {
                                        // User cannot be found
                                        context.Database.CommitTransaction();
                                        continue;
                                    }
                                }
                                else
                                {
                                    // User cannot be found
                                    context.Database.CommitTransaction();
                                    continue;
                                }

                                if (usuario != null)
                                {
                                    var anunciante = context.Anunciante
                                        .AsNoTracking()
                                        .FirstOrDefault(a => a.IdAwin == transaction.advertiserId.ToString());

                                    double usdPrice = 1;

                                    if (transaction.commissionAmount.currency == "USD" || transaction.saleAmount.currency == "USD")
                                    {
                                        var response = await _currencyService.GetCurrencyValueAsync();

                                        usdPrice = response.Value;
                                    }

                                    if (transaction.commissionAmount.currency == "USD")
                                    {
                                        transaction.commissionAmount.amount = transaction.commissionAmount.amount * usdPrice;
                                    }

                                    if (transaction.saleAmount.currency == "USD")
                                    {
                                        transaction.saleAmount.amount = transaction.saleAmount.amount * usdPrice;
                                    }

                                    var pedido = new Pedido
                                    {
                                        IdUsuario = usuario.IdUsuario,
                                        IdTransacao = null,
                                        DataPedido = DateTime.Now.HorarioBrasilia(),
                                        Codigo = null,
                                        ValorTaxa = 0,
                                        ValorPedido = (decimal)transaction.saleAmount.amount,
                                        ValorPago = 0,
                                        DataPagamento = null,
                                        Pago = false,
                                        Ativo = true,
                                        EnderecoDeposito = null,
                                        MeioPagamento = 0,
                                        Quantidade = 1,
                                        UrlPagamento = null,
                                        Cotacao = 0,
                                        CodigoReferenciaBoleto = null,
                                        LinhaDigitavelBoleto = null,
                                        UrlBoleto = null,
                                        IdVendaZanox = null,
                                        IdVendaAfilio = null,
                                        IdAwinTransaction = transaction.id,
                                        Cashback = (decimal)transaction.commissionAmount.amount,
                                        Tipo = (int)TipoPedido.CashbackExterno,
                                        Status = (int)StatusPedido.AguardandoCashback
                                    };

                                    var transacao = cashbackNegocio.CriarTransacaoCashbackAsync(pedido, anunciante).Result;

                                    context.Add(transacao);

                                    pedido.IdAnunciante = anunciante.IdAnunciante;
                                    pedido.IdTransacao = transacao.IdTransacao;
                                    pedido.Transacao = transacao;
                                    context.Add(pedido);

                                    var pedidoDetalhe = new PedidoDetalhe
                                    {
                                        Descricao = $"Compra em: {anunciante.Nome}",
                                        DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                        Ativo = true,
                                        Pedido = pedido,
                                        IdStatus = GetAwinTransactionStatus(transaction.commissionStatus)
                                    };

                                    context.Add(pedidoDetalhe);
                                    context.SaveChanges();

                                    await EnviarNotificacoesAsync(pedido, transaction);
                                }
                            }

                            context.Database.CommitTransaction();

                        }
                        catch (Exception ex)
                        {
                            var msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

                            _logger.LogInformation($"Exception: {msg}");

                            context.Database.RollbackTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                //throw;
            }

            _isRunning = false;

            scope.Dispose();
        }

        private async Task EnviarNotificacoesAsync(Pedido pedido, Transaction transaction)
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var status = GetAwinTransactionStatus(transaction.commissionStatus);
                var consumidor = context.Usuario.AsNoTracking().FirstOrDefault(x => x.IdUsuario == pedido.IdUsuario); 
                var anunciante = context.Anunciante.AsNoTracking().FirstOrDefault(a => a.IdAwin == transaction.advertiserId.ToString());
                var telefone = "55" + consumidor.Celular;
                //consumidor.Email = "eric@souquantabank.com.br";
                //var telefone = "5541996407728";

                var valorCashback = transaction.commissionAmount.amount * 0.25;

                // Pendente
                if (status == 1)
                {
                    var mensagem = $"Olá, {consumidor.Nome ?? consumidor.Email}! \n\n🕒 Sua compra está em análise! Recebemos sua compra realizada com o anunciante {anunciante.Nome}. \n\n🔹 Valor da compra: {pedido.ValorPedido.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Valor do cashback: {valorCashback.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Data da compra: {transaction.transactionDate.Value.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("pt-BR"))} \n\nAssim que tivermos uma atualização, entraremos em contato. Obrigado por fazer parte do Quanta Shop! 🙌 \n\nAbraços, Equipe Quanta Shop";

                    // Enviar Email
                    await _emailService.SendEmailPurchasePending(consumidor.Nome, consumidor.Email, anunciante.Nome, pedido.ValorPedido, valorCashback, transaction.transactionDate.Value);

                    // Enviar WhatsApp
                    await _whatsAppService.SendMessageAsync(telefone, mensagem);
                }

                // Finalizado e pago
                if (status == 2)
                {
                    var mensagem = $"Olá, {consumidor.Nome ?? consumidor.Email}! \n\n🎊 Sua compra foi finalizada com sucesso! \nEstamos felizes em informar que a sua compra realizada com o anunciante {anunciante.Nome} foi concluída \n\n🔹 Valor da compra: {pedido.ValorPedido.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Valor do cashback: R$ {valorCashback.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Data da compra: {transaction.transactionDate.Value.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("pt-BR"))} \n\n💰 Seu cashback foi pago e já está disponível! \nObrigado por fazer parte do Quanta Shop! Agradecemos pela sua confiança e parceria. 🙌 \n\nAbraços,\nEquipe Quanta Shop";

                    // Enviar Email
                    await _emailService.SendEmailPurchaseCompleted(consumidor.Nome, consumidor.Email, anunciante.Nome, pedido.ValorPedido, valorCashback, transaction.transactionDate.Value);

                    // Enviar WhatsApp
                    await _whatsAppService.SendMessageAsync(telefone, mensagem);
                }

                // Cancelado ou rejeitado
                if (status == 3 || status == 5)
                {
                    var mensagem = $"Olá, {consumidor.Nome ?? consumidor.Email}! \n\n🚫 Sua compra foi recusada. \nInfelizmente, a compra realizada com o anunciante {anunciante.Nome} não foi aprovada. \n\n🔹 Valor da compra: {pedido.ValorPedido.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Valor do cashback: {valorCashback.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Data da compra: {transaction.transactionDate.Value.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("pt-BR"))} \n\n🔹 Motivo: {transaction.declineReason}  \n\nSe precisar de ajuda, estamos aqui para ajudar! \nObrigado por fazer parte do Quanta Shop! 🙌 \n\nAbraços,\nEquipe Quanta Shop";

                    // Enviar Email
                    await _emailService.SendEmailPurchaseRejected(consumidor.Nome, consumidor.Email, anunciante.Nome, pedido.ValorPedido, valorCashback, transaction.transactionDate.Value, transaction.declineReason?.ToString());

                    // Enviar WhatsApp
                    await _whatsAppService.SendMessageAsync(telefone, mensagem);
                }

                // Aprovado
                if (status == 6)
                {
                    var mensagem = $"Olá, {consumidor.Nome ?? consumidor.Email}! \n\n🎉 Sua compra foi aprovada! \nParabéns! A sua compra realizada com o anunciante {anunciante.Nome} foi confirmada. \n🔹 Valor da compra: {pedido.ValorPedido.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Valor do cashback: {valorCashback.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))} \n🔹 Data da compra: {transaction.transactionDate.Value.ToString("dd/MM/yyyy HH:mm", new System.Globalization.CultureInfo("pt-BR"))} \n\nFique atento ao seu cashback! 🤑 \nObrigado por fazer parte do Quanta Shop! 🙌 \n\nAbraços,\nEquipe Quanta Shop";

                    // Enviar Email
                    await _emailService.SendEmailPurchaseApproved(consumidor.Nome, consumidor.Email, anunciante.Nome, pedido.ValorPedido, valorCashback, transaction.transactionDate.Value);

                    // Enviar WhatsApp
                    await _whatsAppService.SendMessageAsync(telefone, mensagem);
                }
            }
            catch
            { }
        }

        private int GetAwinTransactionStatus(string status)
        {
            switch (status)
            {
                case "pending": return 1;
                case "approved": return 6;
                case "declined": return 5;
                case "deleted": return 3;
                default: return 1;
            }
        }
    }
}
