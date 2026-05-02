using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Api.ViewModel.Pedido;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Jwt;
using MMN.Util.Model;
using MMN.Util.Translation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public partial class AdminController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly ITokenUtil _token;
        private readonly IGraduacaoNegocio _graduacaoNegocio;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly ILocation _location;
        private readonly ITransacaoNegocio _transacaoNegocio;
        private readonly IAnuncianteNegocio _anuncianteNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly ISaqueNegocio _saqueNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IProdutoNegocio _produtoNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly ISuporteNegocio _suporteNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly ICupomCashbackNegocio _cupomCashbackNegocio;
        private readonly ICache _cache;
        private readonly AppSettings _appSettings;
        private readonly TokenManagement _tokenManagement;
        private readonly DatabaseContext _context;
        private readonly IUsersService _userService;
        private readonly IAdminService _adminService;

        public AdminController(
            IUsuarioNegocio usuarioNegocio,
            ILocation location,
            IGraduacaoNegocio graduacaoNegocio,
            IConfiguracaoNegocio configNegocio,
            IOptions<AppSettings> appSettings,
            ITokenUtil token,
            ITransacaoNegocio transacaoNegocio,
            IAnuncianteNegocio anuncianteNegocio,
            ILancamentoNegocio lancamentoNegocio,
            IPedidoNegocio pedidoNegocio,
            ICategoriaNegocio categoriaNegocio,
            ISaqueNegocio saqueNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IProdutoNegocio produtoNegocio,
            IProceduresRepositorio proceduresRepositorio,
            IOptions<TokenManagement> tokenManagement,
            ISuporteNegocio suporteNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            ICupomCashbackNegocio cupomCashbackNegocio,
            DatabaseContext context,
            ICache cache,
            IUsersService userService,
            IAdminService adminService)
        {
            _usuarioNegocio = usuarioNegocio;
            _location = location;
            _graduacaoNegocio = graduacaoNegocio;
            _configNegocio = configNegocio;
            _appSettings = appSettings.Value;
            _token = token;
            _categoriaNegocio = categoriaNegocio;
            _transacaoNegocio = transacaoNegocio;
            _anuncianteNegocio = anuncianteNegocio;
            _lancamentoNegocio = lancamentoNegocio;
            _pedidoNegocio = pedidoNegocio;
            _saqueNegocio = saqueNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _produtoNegocio = produtoNegocio;
            _proceduresRepositorio = proceduresRepositorio;
            _tokenManagement = tokenManagement.Value;
            _suporteNegocio = suporteNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _cupomCashbackNegocio = cupomCashbackNegocio;
            _context = context;
            _cache = cache;
            _userService = userService;
            _adminService = adminService;
        }

        // ── Private config helpers (shared across partials) ──────────────────────────

        private static readonly Dictionary<string, (string Valor, string Tipo, string Descricao)> _redeDefaults =
            new Dictionary<string, (string, string, string)>
            {
                { "Rede.CompressaoDinamica",    ("true",  "bool",    "Compressao dinamica habilitada") },
                { "Rede.QuantaPontoValor",      ("1.00",  "decimal", "Valor em R$ de 1 Quanta Ponto") },
                { "Rede.PlusMultiplicador",     ("2.00",  "decimal", "Multiplicador Plus para residual") },
                { "Rede.QuarentenaDias",        ("30",    "int",     "Dias de quarentena (aging) para liberar cashback") },
                { "Rede.ProfundidadeMax",       ("12",    "int",     "Profundidade maxima da rede para residual") },
                { "Rede.SustentabilidadePerc",  ("10.00", "decimal", "Percentual de retencao de sustentabilidade sobre valor bruto") },
                { "Rede.SplitEmpresaPerc",      ("50.00", "decimal", "Split base: percentual da empresa") },
                { "Rede.SplitConsumidorPerc",   ("25.00", "decimal", "Split base: percentual do consumidor (cashback principal)") },
                { "Rede.SplitRedePerc",         ("25.00", "decimal", "Split base: percentual destinado a rede MLM") },
                { "Rede.ResLevel.1.Percentual", ("20.00", "decimal", "Residual nivel 1 (% do pool de rede)") },
                { "Rede.ResLevel.1.Active",     ("true",  "bool",    "Residual nivel 1 ativo") },
                { "Rede.ResLevel.2.Percentual", ("15.00", "decimal", "Residual nivel 2 (% do pool de rede)") },
                { "Rede.ResLevel.2.Active",     ("true",  "bool",    "Residual nivel 2 ativo") },
                { "Rede.ResLevel.3.Percentual", ("12.00", "decimal", "Residual nivel 3 (% do pool de rede)") },
                { "Rede.ResLevel.3.Active",     ("true",  "bool",    "Residual nivel 3 ativo") },
                { "Rede.ResLevel.4.Percentual", ("10.00", "decimal", "Residual nivel 4 (% do pool de rede)") },
                { "Rede.ResLevel.4.Active",     ("true",  "bool",    "Residual nivel 4 ativo") },
                { "Rede.ResLevel.5.Percentual", ("8.00",  "decimal", "Residual nivel 5 (% do pool de rede)") },
                { "Rede.ResLevel.5.Active",     ("true",  "bool",    "Residual nivel 5 ativo") },
                { "Rede.ResLevel.6.Percentual", ("7.00",  "decimal", "Residual nivel 6 (% do pool de rede)") },
                { "Rede.ResLevel.6.Active",     ("true",  "bool",    "Residual nivel 6 ativo") },
                { "Rede.ResLevel.7.Percentual", ("6.00",  "decimal", "Residual nivel 7 (% do pool de rede)") },
                { "Rede.ResLevel.7.Active",     ("true",  "bool",    "Residual nivel 7 ativo") },
                { "Rede.ResLevel.8.Percentual", ("5.00",  "decimal", "Residual nivel 8 (% do pool de rede)") },
                { "Rede.ResLevel.8.Active",     ("true",  "bool",    "Residual nivel 8 ativo") },
                { "Rede.ResLevel.9.Percentual", ("5.00",  "decimal", "Residual nivel 9 (% do pool de rede)") },
                { "Rede.ResLevel.9.Active",     ("true",  "bool",    "Residual nivel 9 ativo") },
                { "Rede.ResLevel.10.Percentual",("4.00",  "decimal", "Residual nivel 10 (% do pool de rede)") },
                { "Rede.ResLevel.10.Active",    ("true",  "bool",    "Residual nivel 10 ativo") },
                { "Rede.ResLevel.11.Percentual",("4.00",  "decimal", "Residual nivel 11 (% do pool de rede)") },
                { "Rede.ResLevel.11.Active",    ("true",  "bool",    "Residual nivel 11 ativo") },
                { "Rede.ResLevel.12.Percentual",("4.00",  "decimal", "Residual nivel 12 (% do pool de rede)") },
                { "Rede.ResLevel.12.Active",    ("true",  "bool",    "Residual nivel 12 ativo") },
                { "Rede.CredLevel.1.Percentual",("8.00",  "decimal", "Credenciamento nivel 1 (%)") },
                { "Rede.CredLevel.1.Active",    ("true",  "bool",    "Credenciamento nivel 1 ativo") },
                { "Rede.CredLevel.2.Percentual",("4.00",  "decimal", "Credenciamento nivel 2 (%)") },
                { "Rede.CredLevel.2.Active",    ("true",  "bool",    "Credenciamento nivel 2 ativo") },
                { "Rede.CredLevel.3.Percentual",("2.00",  "decimal", "Credenciamento nivel 3 (%)") },
                { "Rede.CredLevel.3.Active",    ("false", "bool",    "Credenciamento nivel 3 ativo") },
            };

        private string GetCfg(string chave)
        {
            var item = _configNegocio.BuscarPelaChave(chave);
            if (item != null && !string.IsNullOrEmpty(item.Valor)) return item.Valor;
            return _redeDefaults.TryGetValue(chave, out var def) ? def.Valor : null;
        }

        private decimal GetCfgDec(string chave)
        {
            var v = GetCfg(chave);
            return decimal.TryParse(v, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d) ? d : 0m;
        }

        private int GetCfgInt(string chave) { var v = GetCfg(chave); return int.TryParse(v, out var i) ? i : 0; }
        private bool GetCfgBool(string chave) { var v = GetCfg(chave); return string.Equals(v, "true", StringComparison.OrdinalIgnoreCase); }

        private int quarentenaPlus(int ano, int mes, DateTime hoje)
        {
            var quarentenaDias = GetCfgInt("Rede.QuarentenaDias");
            if (quarentenaDias <= 0) quarentenaDias = 30;
            var fimMes = new DateTime(ano, mes, 1).AddMonths(1).AddDays(-1);
            var liberacao = fimMes.AddDays(quarentenaDias);
            var dias = (liberacao - hoje).Days;
            return dias > 0 ? dias : 0;
        }

        private void UpsertCfg(string chave, string valor)
        {
            var existente = _configNegocio.BuscarPelaChave(chave);
            var defs = _redeDefaults.TryGetValue(chave, out var def) ? def : ("", "string", "");
            if (existente != null)
            {
                existente.Valor = valor;
                _configNegocio.EditarConfig(existente);
            }
            else
            {
                _configNegocio.Insert(new ConfiguracaoViewModel
                {
                    Chave = chave,
                    Valor = valor,
                    Descricao = defs.Item3,
                    Ativo = true
                });
                _configNegocio.SaveChanges();
            }
        }
    }
}
