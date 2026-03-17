using MMN.Util.Cache;
using MMN.Util.Model;
using MMN.Util.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MMN.INegocio.Negocio;
using MMN.Api.Helpers;
using MMN.Util.Extensions;
using MMN.Dominio.Model;
using Microsoft.Extensions.Options;

namespace MMN.Api.Services;

public interface IEmailService
{
    Task SendConfirmationEmail(Guid userId, string title = "Quanta Shop - Confirmação de email");
    Task SendEmailToReferrer(Guid indicatorUserId, Guid indicatedUserId, string title = "Quanta Shop - Você tem um novo cadastro na sua rede 🙌");
    Task SendEmailNewLeadToReferrer(Guid indicatorUserId, Guid indicatedUserId, string title = "Quanta Shop - Você tem um novo cadastro na sua rede 🙌");
    Task SendEmailPurchasePending(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmountAmount, DateTime transactionTransactionDate);
    Task SendEmailPurchaseCompleted(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmountAmount, DateTime transactionTransactionDate);
    Task SendEmailPurchaseRejected(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmount, DateTime transactionTransactionDate, string transactionDeclineReason);
    Task SendEmailPurchaseApproved(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmountAmount, DateTime transactionTransactionDate);
    Task SendEmailClosedInvoice(string comercianteNome, string comercianteEmail, string valor, string dataVencimento, string linhaDigitavel, string UrlPDF);
}

public class EmailService : IEmailService
{
    private readonly IUsuarioNegocio _negocio;
    private readonly ITokenUtil _token;
    private readonly ICache _cache;
    private readonly IConfiguracaoNegocio _configNegocio;
    private readonly AppSettings _appSettings;

    public EmailService(IUsuarioNegocio negocio, ITokenUtil token, ICache cache, IConfiguracaoNegocio configNegocio, IOptions<AppSettings> appSettings)
    {
        _negocio = negocio;
        _token = token;
        _cache = cache;
        _configNegocio = configNegocio;
        _appSettings = appSettings.Value;
    }

    public async Task SendConfirmationEmail(Guid userId, string title = "Quanta Shop - Confirmação de email")
    {
        var user = _negocio.GetById(userId);
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = user.Nome,
            Subject = title,
            To = user.Email,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var webToken = _token.ConstruirToken(user);
        var rootSite = _configNegocio.BuscarRootSite().Valor;

        if (string.IsNullOrEmpty(rootSite))
        {
            _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
            rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
        }

        var link = rootSite + _appSettings.RootSiteConfirmEmail + webToken;
        var body = new Dictionary<string, string>
            {
                { "{{ name }}", objectEmail.DestinationName },
                { "{{ confirmation_link }}", link}
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.ConfirmarEmail, null, objectEmail);
    }

    public async Task SendEmailToReferrer(Guid indicatorUserId, Guid indicatedUserId, string title = "Quanta Shop - Você tem um novo cadastro na sua rede 🙌")
    {
        var userIndicator = _negocio.GetById(indicatorUserId);
        var userIndicated = _negocio.GetById(indicatedUserId);
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = userIndicator.Nome,
            Subject = title,
            To = userIndicator.Email,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };
        
        var body = new Dictionary<string, string>
            {
                { "{{ indicator }}", userIndicator.Nome },
                { "{{ indicated }}", userIndicated.Nome}
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.NovoMembroParaIndicador, null, objectEmail);
    }

    public async Task SendEmailNewLeadToReferrer(Guid indicatorUserId, Guid indicatedUserId, string title = "Quanta Shop - Você tem um novo cadastro na sua rede 🙌")
    {
        var userIndicator = _negocio.GetById(indicatorUserId);
        var userIndicated = _negocio.GetById(indicatedUserId);
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = userIndicator.Nome,
            Subject = title,
            To = userIndicator.Email,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ indicator }}", userIndicator.Nome },
                { "{{ indicated }}", userIndicated.Nome}
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.NovoLeadParaIndicador, null, objectEmail);
    }

    public async Task SendEmailPurchasePending(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmount, DateTime transactionTransactionDate)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = consumidorNome,
            Subject = $"{anuncianteNome} nos avisou da sua compra",
            To = consumidorEmail,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ consumidor.Nome }}", consumidorNome },
                { "{{ anunciante.Nome }}", anuncianteNome },
                { "{{ pedido.ValorPedido }}", pedidoValorPedido.ToString("C2") },
                { "{{ transaction.commissionAmount.amount }}", transactionCommissionAmount.ToString("C2") },
                { "{{ transaction.transactionDate }}", transactionTransactionDate.HorarioBrasilia().ToString("dd/MM/yyyy HH:mm") },
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.CompraPendente, null, objectEmail);
    }

    public async Task SendEmailPurchaseCompleted(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmount, DateTime transactionTransactionDate)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = consumidorNome,
            Subject = $"Compra com o anunciante {anuncianteNome} finalizada",
            To = consumidorEmail,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ consumidor.Nome }}", consumidorNome },
                { "{{ anunciante.Nome }}", anuncianteNome },
                { "{{ pedido.ValorPedido }}", pedidoValorPedido.ToString("C2") },
                { "{{ transaction.commissionAmount.amount }}", transactionCommissionAmount.ToString("C2") },
                { "{{ transaction.transactionDate }}", transactionTransactionDate.HorarioBrasilia().ToString("dd/MM/yyyy HH:mm") },
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.CompraFinalizada, null, objectEmail);
    }

    public async Task SendEmailPurchaseRejected(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmount, DateTime transactionTransactionDate, string transactionDeclineReason)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = consumidorNome,
            Subject = $"Sua compra foi recusada pela loja {anuncianteNome}",
            To = consumidorEmail,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ consumidor.Nome }}", consumidorNome },
                { "{{ anunciante.Nome }}", anuncianteNome },
                { "{{ pedido.ValorPedido }}", pedidoValorPedido.ToString("C2") },
                { "{{ transaction.commissionAmount.amount }}", transactionCommissionAmount.ToString("C2") },
                { "{{ transaction.transactionDate }}", transactionTransactionDate.HorarioBrasilia().ToString("dd/MM/yyyy HH:mm") },
                { "{{ transaction.declineReason }}", transactionDeclineReason ?? "Nenhum motivo informado" },
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.CompraRejeitada, null, objectEmail);
    }

    public async Task SendEmailPurchaseApproved(string consumidorNome, string consumidorEmail, string anuncianteNome, decimal pedidoValorPedido, double transactionCommissionAmount, DateTime transactionTransactionDate)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = consumidorNome,
            Subject = $"Sua compra confirmada pela loja { anuncianteNome}",
            To = consumidorEmail,
            EmailSuporte = _appSettings.EmailSuporte,
            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ consumidor.Nome }}", consumidorNome },
                { "{{ anunciante.Nome }}", anuncianteNome },
                { "{{ pedido.ValorPedido }}", pedidoValorPedido.ToString("C2") },
                { "{{ transaction.commissionAmount.amount }}", transactionCommissionAmount.ToString("C2") },
                { "{{ transaction.transactionDate }}", transactionTransactionDate.HorarioBrasilia().ToString("dd/MM/yyyy HH:mm") },
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.CompraAprovada, null, objectEmail);
    }

    public async Task SendEmailClosedInvoice(string comercianteNome, string comercianteEmail, string valor, string dataVencimento, string linhaDigitavel, string UrlPDF)
    {
        var objectEmail = new ObjEmailUtilitis
        {
            Data = DateTime.UtcNow.HorarioBrasilia(),
            From = _appSettings.EmailToSmtp,
            FromName = _appSettings.FromName,
            DestinationName = comercianteNome,
            Subject = $"Sua fatura fechou",
            To = comercianteEmail,
            EmailSuporte = _appSettings.EmailSuporte,
            BrevoApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY")
        };

        var body = new Dictionary<string, string>
            {
                { "{{ comerciante.Nome }}", comercianteNome },
                { "{{ fatura.Valor }}", valor },
                { "{{ fatura.DataVencimento }}", dataVencimento },
                { "{{ fatura.LinhaDigitavel }}", linhaDigitavel },
                { "{{ fatura.UrlPDF }}", UrlPDF},
            };

        var emailUtil = new EmailUtilitis();
        await emailUtil.EnviarEmail(body, _appSettings.FaturaFechada, null, objectEmail);
    }
}
