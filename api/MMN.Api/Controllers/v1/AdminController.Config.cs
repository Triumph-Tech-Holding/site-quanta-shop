using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Api.Controllers.v1
{
    public partial class AdminController
    {
        [HttpGet]
        [Route("configuracoes-rede")]
        public IActionResult GetConfiguracoesRede()
        {
            try
            {
                var resLevels = new List<object>();
                for (int n = 1; n <= 12; n++)
                {
                    resLevels.Add(new { level = n, percentual = GetCfgDec($"Rede.ResLevel.{n}.Percentual"), active = GetCfgBool($"Rede.ResLevel.{n}.Active") });
                }
                var credLevels = new List<object>();
                for (int n = 1; n <= 3; n++)
                {
                    credLevels.Add(new { level = n, percentual = GetCfgDec($"Rede.CredLevel.{n}.Percentual"), active = GetCfgBool($"Rede.CredLevel.{n}.Active") });
                }
                return Ok(new
                {
                    sustentabilidadePerc = GetCfgDec("Rede.SustentabilidadePerc"),
                    splitBase = new
                    {
                        empresa    = GetCfgDec("Rede.SplitEmpresaPerc"),
                        consumidor = GetCfgDec("Rede.SplitConsumidorPerc"),
                        rede       = GetCfgDec("Rede.SplitRedePerc"),
                    },
                    compressaoDinamica  = GetCfgBool("Rede.CompressaoDinamica"),
                    quantaPontoValor    = GetCfgDec("Rede.QuantaPontoValor"),
                    plusMultiplicador   = GetCfgDec("Rede.PlusMultiplicador"),
                    quarentenaDias      = GetCfgInt("Rede.QuarentenaDias"),
                    profundidadeMax     = GetCfgInt("Rede.ProfundidadeMax"),
                    residualLevels      = resLevels,
                    credenciamentoLevels = credLevels,
                    fonte = "configuracao_db",
                    atualizadoEm = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_carregar_configuracoes", detail = ex.Message });
            }
        }

        public class ConfiguracoesRedeUpdateViewModel
        {
            public decimal? SustentabilidadePerc { get; set; }
            public SplitBaseConfig SplitBase { get; set; }
            public bool? CompressaoDinamica { get; set; }
            public decimal? QuantaPontoValor { get; set; }
            public decimal? PlusMultiplicador { get; set; }
            public int? QuarentenaDias { get; set; }
            public int? ProfundidadeMax { get; set; }
            public List<NivelConfig> ResidualLevels { get; set; }
            public List<NivelConfig> CredenciamentoLevels { get; set; }

            public class NivelConfig
            {
                public int Level { get; set; }
                public decimal Percentual { get; set; }
                public bool Active { get; set; }
            }

            public class SplitBaseConfig
            {
                public decimal Empresa { get; set; }
                public decimal Consumidor { get; set; }
                public decimal Rede { get; set; }
            }
        }

        [HttpPost]
        [Route("configuracoes-rede")]
        public IActionResult SaveConfiguracoesRede([FromBody] ConfiguracoesRedeUpdateViewModel view)
        {
            var allowWrites = string.Equals(Environment.GetEnvironmentVariable("ALLOW_ADMIN_WRITES"), "true", StringComparison.OrdinalIgnoreCase);
            if (!allowWrites)
            {
                return StatusCode(503, new
                {
                    message = "gravacao_admin_desabilitada",
                    detail = "Em ambiente de desenvolvimento, gravacoes administrativas estao desabilitadas para proteger a base de producao. Defina ALLOW_ADMIN_WRITES=true para habilitar."
                });
            }

            if (view == null) return BadRequest(new { message = "payload_invalido" });

            if (view.SplitBase != null)
            {
                var soma = view.SplitBase.Empresa + view.SplitBase.Consumidor + view.SplitBase.Rede;
                if (Math.Abs(soma - 100m) > 0.01m)
                    return BadRequest(new { message = "split_invalido", detail = $"Soma do split base deve ser 100% (atual: {soma}%)." });
            }
            if (view.SustentabilidadePerc.HasValue && (view.SustentabilidadePerc < 0m || view.SustentabilidadePerc > 50m))
                return BadRequest(new { message = "sustentabilidade_invalida", detail = "Percentual de sustentabilidade deve estar entre 0 e 50." });

            if (view.ResidualLevels != null)
            {
                var somaRes = view.ResidualLevels.Where(l => l.Active).Sum(l => l.Percentual);
                if (somaRes > 100m)
                    return BadRequest(new { message = "soma_residual_excede_teto", detail = $"Soma dos niveis residuais ativos excede 100% do pool de rede (atual: {somaRes}%)." });
            }

            try
            {
                if (view.SustentabilidadePerc.HasValue) UpsertCfg("Rede.SustentabilidadePerc", view.SustentabilidadePerc.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (view.SplitBase != null)
                {
                    UpsertCfg("Rede.SplitEmpresaPerc",    view.SplitBase.Empresa.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    UpsertCfg("Rede.SplitConsumidorPerc", view.SplitBase.Consumidor.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    UpsertCfg("Rede.SplitRedePerc",       view.SplitBase.Rede.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (view.CompressaoDinamica.HasValue) UpsertCfg("Rede.CompressaoDinamica", view.CompressaoDinamica.Value ? "true" : "false");
                if (view.QuantaPontoValor.HasValue)   UpsertCfg("Rede.QuantaPontoValor",   view.QuantaPontoValor.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (view.PlusMultiplicador.HasValue)  UpsertCfg("Rede.PlusMultiplicador",  view.PlusMultiplicador.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (view.QuarentenaDias.HasValue)     UpsertCfg("Rede.QuarentenaDias",     view.QuarentenaDias.Value.ToString());
                if (view.ProfundidadeMax.HasValue)    UpsertCfg("Rede.ProfundidadeMax",    view.ProfundidadeMax.Value.ToString());

                if (view.ResidualLevels != null)
                {
                    foreach (var lvl in view.ResidualLevels.Where(l => l.Level >= 1 && l.Level <= 12))
                    {
                        UpsertCfg($"Rede.ResLevel.{lvl.Level}.Percentual", lvl.Percentual.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        UpsertCfg($"Rede.ResLevel.{lvl.Level}.Active", lvl.Active ? "true" : "false");
                    }
                }
                if (view.CredenciamentoLevels != null)
                {
                    foreach (var lvl in view.CredenciamentoLevels.Where(l => l.Level >= 1 && l.Level <= 3))
                    {
                        UpsertCfg($"Rede.CredLevel.{lvl.Level}.Percentual", lvl.Percentual.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        UpsertCfg($"Rede.CredLevel.{lvl.Level}.Active", lvl.Active ? "true" : "false");
                    }
                }
                return Ok(new { message = "configuracoes_salvas", atualizadoEm = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_salvar_configuracoes", detail = ex.Message });
            }
        }

        // ── LGPD — Reveal de dado sensivel (gated por Usuario.Master) ────────────────

        public class RevelarDadoSensivelRequest
        {
            public Guid IdUsuarioAlvo { get; set; }
            public string Campo { get; set; }
            public string Motivo { get; set; }
        }

        [HttpPost]
        [Route("revelar-dado-sensivel")]
        public IActionResult RevelarDadoSensivel([FromBody] RevelarDadoSensivelRequest req)
        {
            if (req == null || req.IdUsuarioAlvo == Guid.Empty || string.IsNullOrWhiteSpace(req.Campo))
                return BadRequest(new { message = "payload_invalido" });

            var solicitante = _usuarioNegocio.GetById(IdUsuarioLogado);
            if (solicitante == null || !solicitante.Master)
                return StatusCode(403, new { message = "acesso_negado_lgpd", detail = "Apenas Admin Master pode revelar dados sensiveis." });

            var alvo = _usuarioNegocio.GetById(req.IdUsuarioAlvo);
            if (alvo == null)
                return NotFound(new { message = "usuario_alvo_nao_encontrado" });

            string valorRevelado = null;
            var campo = (req.Campo ?? string.Empty).Trim().ToUpperInvariant();
            try
            {
                switch (campo)
                {
                    case "CPF": case "CPFCNPJ": case "DOCUMENTO":
                        valorRevelado = alvo.Documento; break;
                    case "EMAIL":
                        valorRevelado = alvo.Email; break;
                    case "TELEFONE": case "CELULAR":
                        valorRevelado = alvo.Celular; break;
                    case "CONTA": case "CONTA_BANCARIA":
                    {
                        var ub = _context.UsuarioBanco.AsNoTracking().FirstOrDefault(u => u.IdUsuario == req.IdUsuarioAlvo);
                        valorRevelado = ub?.Conta; break;
                    }
                    case "AGENCIA":
                    {
                        var ub = _context.UsuarioBanco.AsNoTracking().FirstOrDefault(u => u.IdUsuario == req.IdUsuarioAlvo);
                        valorRevelado = ub?.Agencia; break;
                    }
                    default:
                        return BadRequest(new { message = "campo_nao_suportado", detail = "Campos validos: CPF, EMAIL, TELEFONE, CONTA, AGENCIA." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_revelar", detail = ex.Message });
            }

            try
            {
                var ip = HttpContext?.Connection?.RemoteIpAddress?.ToString();
                var ua = Request?.Headers["User-Agent"].ToString();
                _context.AuditoriaLgpd.Add(new Dominio.Model.AuditoriaLgpd
                {
                    IdUsuarioMaster = IdUsuarioLogado,
                    IdUsuarioAlvo   = req.IdUsuarioAlvo,
                    Campo           = campo,
                    Motivo          = req.Motivo,
                    IpOrigem        = ip,
                    UserAgent       = ua,
                    DataAcesso      = DateTime.UtcNow,
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    message = "auditoria_lgpd_indisponivel",
                    detail  = "Reveal bloqueado: trilha de auditoria obrigatoria nao pode ser gravada. Aplique a migration AuditoriaLgpd em producao.",
                    inner   = ex.Message,
                });
            }

            return Ok(new { campo, valor = valorRevelado, idUsuarioAlvo = req.IdUsuarioAlvo, reveladoEm = DateTime.UtcNow });
        }
    }
}
