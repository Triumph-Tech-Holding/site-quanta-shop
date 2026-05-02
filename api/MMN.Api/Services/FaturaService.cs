using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMN.Api.Service;
using MMN.Api.ViewModel.Fatura;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Awin;
using MMN.Negocio.Negocio;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMN.Api.Services
{
    public class FaturaService : BaseService, IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FaturaService> _logger;
        private bool _isRunning;
        private bool _isDev;
        private readonly WhatsAppService _whatsAppService;
        private readonly HttpClient _httpClient;

        public FaturaService(IServiceProvider serviceProvider, ILogger<FaturaService> logger, IHostEnvironment env, WhatsAppService whatsAppService, HttpClient httpClient)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _isRunning = false;
            _isDev = env.IsDevelopment();

            _whatsAppService = whatsAppService;
            _httpClient = httpClient;
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
            return Task.CompletedTask;
        }

        public override void TimerTick(object info)
        {
            if ((!_isRunning && DateTime.Now.Day == 2 && DateTime.UtcNow.HorarioBrasilia().Hour == 02 && DateTime.UtcNow.HorarioBrasilia().Minute == 00 && DateTime.UtcNow.HorarioBrasilia().Second == 0))
            {
                _isRunning = true;

                if (!_isDev)
                    CriarFaturasAsync().Wait();
            }
        }

        private async Task CriarFaturasAsync()
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                var _pedidoNegocio = scope.ServiceProvider.GetRequiredService<IPedidoNegocio>();
                var _cashbackNegocio = scope.ServiceProvider.GetRequiredService<ICupomCashbackNegocio>();
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var hoje = DateTime.Now;
                var ultimoDiaDoMesAnterior = new DateTime(hoje.Year, hoje.Month, 1).AddDays(-1);

                // Aprovar todas as compras pendentes (status = 1)
                var comprasAguardandoAprovacao = await _cashbackNegocio.GetAsync(x => x.Status == 1);

                foreach (var compra in comprasAguardandoAprovacao)
                {
                    var cupom = await _cashbackNegocio.AprovarReprovarCupomAsync(
                        compra.Token,
                        compra.IdComerciante.Value,
                        true,
                        6,
                        $"Aprovado automaticamente na data {DateTime.Now} para geração de fatura",
                        false
                    );
                }

                // Percorrer todos os comerciantes que tenham pelo menos uma compra registrada
                var comerciantes = context.Credenciamento
                    .Where(cr => cr.Status == StatusCredenciamento.Aprovado)
                    .Join(context.CuponCashback, cr => cr.IdUsuario, cc => cc.IdComerciante, (cr, cc) => new { cr.IdCredenciamento, cr.IdUsuario })
                    .Distinct()
                    .ToList();

                foreach (var comerciante in comerciantes)
                {
                    try
                    {
                        var fatura = await _pedidoNegocio.CriarFaturaCashbackCredenciadoAsync(ultimoDiaDoMesAnterior, EnumTipoPagamento.PGASAAS, comerciante.IdUsuario.Value);

                        if (fatura is not null)
                            await EnviarFaturaAsync(fatura);
                    }
                    catch (Exception exFatura)
                    {
                        var msg = exFatura.InnerException?.Message ?? exFatura.Message;
                        _logger.LogError(message: $"[FaturaService] Falha ao gerar fatura para credenciado {comerciante.IdUsuario}: {msg}");
                    }
                }

            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;

                _logger.LogError(message: msg);
            }

            _isRunning = false;

            scope.Dispose();
        }

        public async Task EnviarFaturaAsync(Pedido pedido)
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var usuarioComerciante = await context.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.IdUsuario == pedido.IdUsuario);
                var comerciante = await context.Credenciamento.FirstOrDefaultAsync(x => x.IdUsuario == usuarioComerciante.IdUsuario);
                var boleto = await context.Pagamento.FirstOrDefaultAsync(x => x.IdPedido == pedido.IdPedido);
                var telefone = "55" + usuarioComerciante.Celular;
                var valorFatura = pedido.ValorPedido;
                var dataVencimento = boleto.DataValidade;
                var mensagem = $"Olá, {comerciante.Estabelecimento}! \n\n🧾 Sua fatura foi fechada no valor de {valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))}. \n\n📅 Vencimento: {dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR"))}. \n\nCaso tenha dúvidas, entre em contato! \n\nAbraços, \nEquipe Quanta Shop";

                // Enviar Email
                await _emailService.SendEmailClosedInvoice(comerciante.Estabelecimento, usuarioComerciante.Email, valorFatura.ToString("C2", new System.Globalization.CultureInfo("pt-BR")), dataVencimento.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR")), boleto.LinhaDigitavelBoleto, boleto.UrlBoleto);

                // Enviar WhatsApp
                await _whatsAppService.SendMessageWithAttachmentAsync(telefone, mensagem, boleto.UrlBoleto);
                await _whatsAppService.SendMessageAsync(telefone, boleto.LinhaDigitavelBoleto);
            }
            catch
            { }
        }
    }
}