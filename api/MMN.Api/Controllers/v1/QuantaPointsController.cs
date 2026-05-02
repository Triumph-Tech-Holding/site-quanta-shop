using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMN.INegocio.Negocio;
using MMN.Repositorio.Contexto;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/usuario")]
    [ApiController]
    public class QuantaPointsController : LoggedControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguracaoNegocio _configNegocio;

        public QuantaPointsController(DatabaseContext context, IConfiguracaoNegocio configNegocio)
        {
            _context = context;
            _configNegocio = configNegocio;
        }

        [HttpGet]
        [Route("quanta-points")]
        public IActionResult ObterSaldo()
        {
            try
            {
                var saldo = 0;
                try
                {
                    saldo = _context.Set<Dominio.Model.QuantaPontoLancamento>()
                        .Where(q => q.Ativo && q.IdUsuario == IdUsuarioLogado)
                        .Sum(q => (int?)q.Pontos) ?? 0;
                }
                catch (Exception exSaldo)
                {
                    // QuantaPontos table may not exist yet in environments without Wave 2 migration; default 0 + log
                    Console.Error.WriteLine($"[QuantaPoints] Falha ao consultar saldo (usuario {IdUsuarioLogado}): {exSaldo.Message}");
                    saldo = 0;
                }
                if (saldo < 0) saldo = 0;

                var valorPonto = 1.0m;
                try
                {
                    var cfg = _configNegocio.BuscarPelaChave("Rede.QuantaPontoValor");
                    if (cfg != null && decimal.TryParse(cfg.Valor, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v))
                    {
                        valorPonto = v;
                    }
                }
                catch (Exception exCfg)
                {
                    Console.Error.WriteLine($"[QuantaPoints] Configuracao 'Rede.QuantaPontoValor' indisponivel, usando default 1.0: {exCfg.Message}");
                }

                return Ok(new
                {
                    saldo,
                    valorPonto,
                    valorMonetario = saldo * valorPonto,
                    moeda = "BRL",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_quanta_points", detail = ex.Message });
            }
        }

        public class ResgatarRequest
        {
            public int Pontos { get; set; }
            public string Origem { get; set; }
            public Guid? IdReferencia { get; set; }
        }

        [HttpPost]
        [Route("quanta-points/resgatar")]
        public IActionResult Resgatar([FromBody] ResgatarRequest req)
        {
            var allowWrites = string.Equals(Environment.GetEnvironmentVariable("ALLOW_ADMIN_WRITES"), "true", StringComparison.OrdinalIgnoreCase);
            if (!allowWrites)
            {
                return StatusCode(503, new { message = "gravacao_desabilitada", detail = "Defina ALLOW_ADMIN_WRITES=true para habilitar." });
            }
            if (req == null || req.Pontos <= 0)
            {
                return BadRequest(new { message = "pontos_invalidos" });
            }

            // Isolamento SERIALIZABLE para impedir double-spend sob concorrencia:
            // duas requests simultaneas nunca aprovarao o mesmo saldo.
            using var tx = _context.Database.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                var saldoInicial = _context.Set<Dominio.Model.QuantaPontoLancamento>()
                    .Where(q => q.Ativo && q.IdUsuario == IdUsuarioLogado)
                    .Sum(q => (int?)q.Pontos) ?? 0;

                if (saldoInicial < req.Pontos)
                {
                    tx.Rollback();
                    return BadRequest(new { message = "saldo_insuficiente", saldo = saldoInicial });
                }

                _context.Add(new Dominio.Model.QuantaPontoLancamento
                {
                    IdUsuario = IdUsuarioLogado,
                    Tipo = "RESGATE",
                    Pontos = -req.Pontos,
                    Origem = req.Origem ?? "checkout",
                    IdReferencia = req.IdReferencia,
                    DataLancamento = DateTime.UtcNow,
                    Ativo = true,
                });
                _context.SaveChanges();

                // Re-checagem pos-gravacao: garante invariante saldo>=0 (defesa em profundidade).
                var saldoFinal = _context.Set<Dominio.Model.QuantaPontoLancamento>()
                    .Where(q => q.Ativo && q.IdUsuario == IdUsuarioLogado)
                    .Sum(q => (int?)q.Pontos) ?? 0;

                if (saldoFinal < 0)
                {
                    tx.Rollback();
                    return Conflict(new { message = "conflito_concorrencia", detail = "Outro resgate ocorreu em paralelo. Tente novamente." });
                }

                tx.Commit();
                return Ok(new { sucesso = true, pontosResgatados = req.Pontos, saldoFinal });
            }
            catch (Exception ex)
            {
                try { tx.Rollback(); } catch { }
                return StatusCode(500, new { message = "erro_resgate", detail = ex.Message });
            }
        }
    }
}
