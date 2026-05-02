using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Translation;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public partial class UsuarioController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _negocio;
        private readonly IUsuarioEnderecoNegocio _negocioEndereco;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly ILocation _location;
        private readonly AppSettings _appSettings;
        private readonly ITokenUtil _token;
        private readonly ICache _cache;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IPreCadastroNegocio _preCadastroNegocio;
        private readonly WhatsAppService _whatsAppService;
        private readonly IBotConversaService _botConversa;
        private readonly IUsersService _usersService;
        private readonly ISaqueNegocio _saqueNegocio;

        public UsuarioController(
            IUsuarioNegocio negocio,
            ILocation location,
            ITokenUtil token,
            ICache cache,
            IConfiguracaoNegocio configNegocio,
            IUsuarioEnderecoNegocio negocioEndereco,
            ILancamentoNegocio lancamentoNegocio,
            IOptions<AppSettings> appSettings,
            IProceduresRepositorio proceduresRepositorio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IPreCadastroNegocio preCadastroNegocio,
            WhatsAppService whatsAppService,
            IUsersService usersService,
            ISaqueNegocio saqueNegocio,
            IBotConversaService botConversa)
        {
            _negocio = negocio;
            _location = location;
            _token = token;
            _cache = cache;
            _configNegocio = configNegocio;
            _negocioEndereco = negocioEndereco;
            _appSettings = appSettings.Value;
            _lancamentoNegocio = lancamentoNegocio;
            _proceduresRepositorio = proceduresRepositorio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _preCadastroNegocio = preCadastroNegocio;
            _whatsAppService = whatsAppService;
            _usersService = usersService;
            _saqueNegocio = saqueNegocio;
            _botConversa = botConversa;
        }

        // ── Private helpers (shared across partials) ─────────────────────────────────

        private async Task EnviarEmailConfirmacao(Guid idUsuario, string titulo)
        {
            var usuario = _negocio.GetById(idUsuario);
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = titulo,
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };
            var webToken = _token.ConstruirToken(usuario);
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
                { "{{ confirmation_link }}", link }
            };
            await new EmailUtilitis().EnviarEmail(body, _appSettings.ConfirmarEmail, null, objectEmail);
        }

        private void EnviarEmailAssinaturaEletronica(UsuarioViewModel usuario, string assinaturaEletronica)
        {
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                DestinationName = usuario.Nome,
                Subject = _appSettings.FromName + " - Nova Assinatura Eletrônica",
                To = usuario.Email,
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };
            var body = new Dictionary<string, string>
            {
                { "#NOMECLIENTE#", objectEmail.DestinationName },
                { "#ASINATURAELETRONICA", assinaturaEletronica }
            };
            new EmailUtilitis().EnviarEmail(body, _appSettings.NovaAssinaturaEletronica, _appSettings.TemplatePai, objectEmail);
        }

        private string SalvaImagem(string imageBase64, string idUsuario)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var logoUrl = AzureStorage.CreateBlob(
                    imageBase64,
                    new Guid(),
                    _appSettings.StorageAccountConnectionString,
                    "imagens-perfil",
                    idUsuario + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                    true).Result;
                return logoUrl;
            }
            return string.Empty;
        }

        private string SalvaImagemComprovante(string imageBase64, string cpf)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var logoUrl = AzureStorage.CreateBlob(
                    imageBase64,
                    new Guid(),
                    _appSettings.StorageAccountConnectionString,
                    "imagens-comprovante",
                    cpf + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                    true).Result;
                return logoUrl;
            }
            return string.Empty;
        }

        private void ApagaImagemComprovante(string blob)
        {
            if (!string.IsNullOrEmpty(blob))
            {
                string pattern = @"[^/]+$";
                Match match = Regex.Match(blob, pattern);
                if (match.Success)
                {
                    Console.WriteLine("Dados após a última barra: " + match.Value);
                    AzureStorage.Delete("imagens-comprovante", match.Value, _appSettings.StorageAccountConnectionString).Wait();
                }
                else
                {
                    Console.WriteLine("Nenhuma correspondência encontrada.");
                }
            }
        }

        public static (DateTime primeiroDia, DateTime ultimoDia) ObterPrimeiroEUltimoDiaMesAnterior()
        {
            DateTime dataAtual = DateTime.Today;
            DateTime primeiroDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
            DateTime ultimoDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddDays(-1);
            return (primeiroDiaMesPassado, ultimoDiaMesPassado);
        }
    }
}
