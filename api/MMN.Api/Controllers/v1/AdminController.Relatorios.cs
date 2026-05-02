using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Enum;
using MMN.Dominio.ViewModel;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    public partial class AdminController
    {
        [HttpPost]
        [Route("RelatorioAnunciantes")]
        public async Task<IActionResult> RelatorioAnunciantes(FiltroRelatorioAnunciantesViewModel filtro)
        {
            var anunciantes = _anuncianteNegocio.Get(a =>
                a.IdAfilio != null || a.IdAwin != null
                && (!string.IsNullOrEmpty(a.IdAwin) || !string.IsNullOrEmpty(a.IdAfilio)),
                "AnuncianteCashBack",
                "CategoriaAnunciante");

            if (!string.IsNullOrEmpty(filtro.Nome))
                anunciantes = anunciantes.Where(a => a.Nome.Contains(filtro.Nome)).ToList();

            if (!string.IsNullOrEmpty(filtro.Moeda))
                anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(b => !string.IsNullOrEmpty(b.Moeda) && b.Moeda.Contains(filtro.Moeda))).ToList();

            if (filtro.Status.HasValue)
            {
                if (filtro.Status.Value == EnumStatusAnunciante.Ativo)
                    anunciantes = anunciantes.Where(a => a.Ativo).ToList();
                else if (filtro.Status.Value == EnumStatusAnunciante.Inativo)
                    anunciantes = anunciantes.Where(a => !a.Ativo).ToList();
            }

            if (filtro.TipoCashback.HasValue)
            {
                switch (filtro.TipoCashback)
                {
                    case EnumTipoCashbackAnunciante.Percentual:
                        anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(c => !string.IsNullOrEmpty(c.Tipo) && c.Tipo.Equals("percentage"))).ToList();
                        break;
                    case EnumTipoCashbackAnunciante.Valor:
                        anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(c => !string.IsNullOrEmpty(c.Tipo) && c.Tipo.Equals("amount"))).ToList();
                        break;
                }
            }

            if (filtro.Conexao.HasValue)
            {
                switch (filtro.Conexao)
                {
                    case EnumConexaoAnunciante.Awin:
                        anunciantes = anunciantes.Where(a => a.IdAwin != null).ToList();
                        break;
                    case EnumConexaoAnunciante.Afilio:
                        anunciantes = anunciantes.Where(a => a.IdAfilio != null).ToList();
                        break;
                }
            }

            if (filtro.Ordenacao.HasValue && filtro.Asc.HasValue)
            {
                switch (filtro.Ordenacao)
                {
                    case EnumOrdenacaoAnuncitantes.Nome:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.Nome).ToList() : anunciantes.OrderBy(a => a.Nome).ToList();
                        break;
                    case EnumOrdenacaoAnuncitantes.CashbackMax:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.CashbackMax).ToList() : anunciantes.OrderBy(a => a.CashbackMax).ToList();
                        break;
                    case EnumOrdenacaoAnuncitantes.CashbackMin:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.CashbackMin).ToList() : anunciantes.OrderBy(a => a.CashbackMin).ToList();
                        break;
                    case EnumOrdenacaoAnuncitantes.TipoCashback:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.TipoCashback).ToList() : anunciantes.OrderBy(a => a.TipoCashback).ToList();
                        break;
                    case EnumOrdenacaoAnuncitantes.Moeda:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.Moeda).ToList() : anunciantes.OrderBy(a => a.Moeda).ToList();
                        break;
                    case EnumOrdenacaoAnuncitantes.Status:
                        anunciantes = filtro.Asc.Value ? anunciantes.OrderByDescending(a => a.Ativo).ToList() : anunciantes.OrderBy(a => a.Ativo).ToList();
                        break;
                }
            }
            else
            {
                anunciantes = anunciantes.OrderByDescending(a => a.IdAwin).ThenBy(a => a.Nome).ToList();
            }

            var totalPages = (int)Math.Ceiling((double)anunciantes.Count() / filtro.QuantidadePorPagina);
            var anunciantesFiltrados = _anuncianteNegocio.BuscarCashback(anunciantes.ToList());
            return Ok(new { totalPages, filtro.QuantidadePorPagina, filtro.Pagina, anunciantesFiltrados, quantidadeTotal = anunciantes.Count() });
        }

        [HttpGet]
        [Route("RelatorioCadastrosMes")]
        public async Task<IActionResult> RelatorioCadastrosMes()
        {
            return Ok(_usuarioNegocio.Get(u => u.DataCadastro > DateTime.Now.AddMonths(-5))
                .GroupBy(g => g.DataCadastro.Value.ToString("yyyy/MM")).Select(u => new
                {
                    data = u.Key,
                    quantidade = u.Count()
                }).OrderBy(o => o.data));
        }

        [HttpGet]
        [Route("TotalUsuariosGraduacao")]
        public async Task<IActionResult> TotalUsuariosGraduacao()
        {
            return Ok(_graduacaoNegocio.Get(w => true, "Usuario")
                .Select(g => new { g.Nome, QuantidadeUsuarios = g.UsuarioViewModel.Count }));
        }

        [HttpGet]
        [Route("ResumoIntegracaoApi")]
        public IActionResult ResumoIntegracaoApi()
        {
            return Ok(
                _transacaoNegocio.Get(t =>
                    t.Ativo && t.IdAnunciante.HasValue && t.IdStatus == 2, "Anunciante")
                .GroupBy(g => new { g.IdAnunciante, g.AnuncianteViewModel })
                .Select(g => new
                {
                    loja = new { g.Key.AnuncianteViewModel.Nome, g.Key.AnuncianteViewModel.Ativo },
                    valorTotalComissao = g.Sum(s => s.ComissaoTotal),
                })
                .OrderByDescending(o => o.valorTotalComissao).Take(10)
            );
        }

        [HttpGet]
        [Route("DistribuicaoDetalhada")]
        public async Task<IActionResult> DistribuicaoDetalhada()
        {
            return Ok(new List<object>() {
                new {
                    nome = "Cashback",
                    valor = _lancamentoNegocio.Get(l =>
                        l.Ativo &&
                        l.IdStatus == (int)StatusTransacaoEnum.Finalizada &&
                        (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH"), "Tipo").Sum(s => s.Valor)
                },
                new {
                    nome = "Adesão paga ao patrocinador",
                    valor = _lancamentoNegocio.Get(l =>
                        l.Ativo &&
                        l.IdStatus == (int)StatusTransacaoEnum.Finalizada &&
                        (l.Tipo.Chave == "DTAD" || l.Tipo.Chave == "DTBAD"), "Tipo").Sum(s => s.Valor)
                }
            });
        }

        [HttpGet]
        [Route("ObterResumoValores")]
        public IActionResult ObterResumoValores()
        {
            try
            {
                var resumo = _cache.GetItem("ObterResumoValores");
                if (resumo != null) return Ok(resumo);

                var usuarios = _usuarioNegocio.GetAll();
                var totalConsumo = _transacaoNegocio.Get(t => t.Ativo && t.IdStatus == 2 && t.IdAnunciante.HasValue).Sum(s => s.ValorPrincipal);
                var totalCashback = _lancamentoNegocio.ObterCashbackDetalahdo();
                var totalConsumoPlanos = _pedidoNegocio.TotalConsumoPlanos();
                var totalSacado = _saqueNegocio.Get(s => s.IdStatus == 2).Sum(s => s.Valor);

                resumo = new
                {
                    totalConsumo,
                    totalCashback,
                    totalConsumoPlanos,
                    totalSacado,
                    usuariosAtivos = usuarios.Where(w => w.Ativo).Count(),
                    usuariosInativos = usuarios.Where(w => !w.Ativo).Count()
                };

                _cache.SetItem("ObterResumoValores", resumo, DateTime.Now.AddHours(2));
                return Ok(resumo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obter-faturamento-ecossistema/{ecossistemaId}")]
        public async Task<IActionResult> ObterFaturamentoEcossistema(int ecossistemaId)
        {
            try
            {
                var faturamento = await _adminService.GetEcosystemBilling(ecossistemaId);
                return Ok(faturamento);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-empresas-ecossistema/{ecossistemaId}")]
        public async Task<IActionResult> ObterEmpresasEcossistema(int ecossistemaId)
        {
            try
            {
                var empresas = await _adminService.GetCompaniesByEcosystem(ecossistemaId);
                return Ok(empresas);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("bi-financeiro")]
        public IActionResult GetBiFinanceiro([FromQuery] string periodo = "30d")
        {
            try
            {
                var hoje = DateTime.UtcNow.Date;
                var dias = (periodo ?? "30d").ToLowerInvariant() switch
                {
                    "7d" or "week" => 7,
                    "30d" or "month" => 30,
                    "90d" or "quarter" => 90,
                    "180d" or "semester" => 180,
                    "365d" or "year" => 365,
                    _ => 30
                };
                var de = hoje.AddDays(-dias);
                var dePrev = de.AddDays(-dias);

                var transacoes = _context.Transacao
                    .Where(t => t.Ativo && t.IdStatus == (int)StatusTransacaoEnum.Finalizada && t.DataTransacao >= de)
                    .Include(t => t.Anunciante).ThenInclude(a => a.CategoriaAnunciante).ThenInclude(c => c.Categoria)
                    .AsNoTracking().ToList();

                var transacoesPrev = _context.Transacao
                    .Where(t => t.Ativo && t.IdStatus == (int)StatusTransacaoEnum.Finalizada && t.DataTransacao >= dePrev && t.DataTransacao < de)
                    .AsNoTracking().Sum(t => (decimal?)t.ValorPrincipal) ?? 0m;

                var lancamentos = _context.Lancamento
                    .Where(l => l.Ativo && l.DataLancamento >= de && (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH" || l.Tipo.Chave == "CHBK"))
                    .Include(l => l.Tipo).Include(l => l.Status)
                    .AsNoTracking().ToList();

                var lancamentosPrev = _context.Lancamento
                    .Where(l => l.Ativo && l.DataLancamento >= dePrev && l.DataLancamento < de && (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH" || l.Tipo.Chave == "CHBK"))
                    .Include(l => l.Tipo).AsNoTracking().ToList();

                var totalTransacoes = transacoes.Count;
                var faturamentoTotal = transacoes.Sum(t => t.ValorPrincipal);
                var faturamentoDelta = transacoesPrev > 0
                    ? Math.Round(((faturamentoTotal - transacoesPrev) / transacoesPrev) * 100m, 1) : 0m;

                var lancamentosCredito = lancamentos.Where(l => l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH").ToList();
                var cashbackGerado = lancamentosCredito.Sum(l => l.Valor);
                var cashbackLiberado = lancamentosCredito.Where(l => !l.Bloqueado && l.IdStatus == (int)StatusTransacaoEnum.Finalizada).Sum(l => l.Valor);
                var cashbackAPagar = lancamentosCredito.Where(l => l.Bloqueado || (l.IdStatus.HasValue && l.IdStatus != (int)StatusTransacaoEnum.Finalizada)).Sum(l => l.Valor);
                var cashbackResidual = lancamentos.Where(l => l.Tipo.Chave == "DTCSH").Sum(l => l.Valor);

                var chbk = Math.Abs(lancamentos.Where(l => l.Tipo.Chave == "CHBK").Sum(l => l.Valor));
                var lancamentosCreditoPrev = lancamentosPrev.Where(l => l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH").ToList();
                var chbkPrev = Math.Abs(lancamentosPrev.Where(l => l.Tipo.Chave == "CHBK").Sum(l => l.Valor));
                var cashbackGeradoPrev = lancamentosCreditoPrev.Sum(l => l.Valor);
                var inadimplenciaPct = cashbackGerado > 0 ? Math.Round((chbk / cashbackGerado) * 100m, 2) : 0m;
                var inadimplenciaPrevPct = cashbackGeradoPrev > 0 ? Math.Round((chbkPrev / cashbackGeradoPrev) * 100m, 2) : 0m;
                var inadimplenciaDelta = Math.Round(inadimplenciaPct - inadimplenciaPrevPct, 2);
                var margem = faturamentoTotal > 0
                    ? Math.Round(((faturamentoTotal - cashbackGerado) / faturamentoTotal) * 100m, 1) : 0m;

                var palette = new[] { "#225F6B", "#2F7785", "#3A9AAD", "#98C73A", "#FFB342", "#7aad1f", "#dc2626", "#f59e0b", "#0ea5e9", "#a855f7" };

                var categoriasRaw = transacoes
                    .Where(t => t.Anunciante != null && t.Anunciante.CategoriaAnunciante != null)
                    .SelectMany(t => t.Anunciante.CategoriaAnunciante.Where(ca => ca.Ativo && ca.Categoria != null)
                        .Select(ca => new { Id = ca.Categoria.IdCategoria, Nome = ca.Categoria.Nome ?? "—", Valor = t.ValorPrincipal }))
                    .GroupBy(x => new { x.Id, x.Nome })
                    .Select(g => new { id = g.Key.Id, nome = g.Key.Nome, valor = g.Sum(x => x.Valor), qtd = g.Count() })
                    .OrderByDescending(x => x.valor).Take(10).ToList();

                var categorias = categoriasRaw
                    .Select((c, i) => new { id = c.id.ToString(), name = c.nome, valor = c.valor, qtd = c.qtd, color = palette[i % palette.Length] })
                    .ToList();

                var topParceirosRaw = transacoes
                    .Where(t => t.Anunciante != null)
                    .GroupBy(t => new { t.Anunciante.IdAnunciante, t.Anunciante.Nome, t.Anunciante.Cashback })
                    .Select(g => new { id = g.Key.IdAnunciante, nome = g.Key.Nome ?? "—", valor = g.Sum(t => t.ValorPrincipal), transacoes = g.Count(), cashbackPct = (double)g.Key.Cashback })
                    .OrderByDescending(x => x.valor).Take(10).ToList();

                var safras = lancamentos
                    .GroupBy(l => new { l.DataLancamento.Year, l.DataLancamento.Month })
                    .Select(g => new
                    {
                        mes = $"{new DateTime(g.Key.Year, g.Key.Month, 1):MMM/yy}",
                        ano = g.Key.Year,
                        mesNum = g.Key.Month,
                        qtd = g.Select(l => l.Transacao != null ? (int?)l.Transacao.IdTransacao : null).Where(id => id.HasValue).Distinct().Count(),
                        gerado = g.Where(l => l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH").Sum(l => l.Valor),
                        estornado = g.Where(l => l.Tipo.Chave == "CHBK").Sum(l => Math.Abs(l.Valor)),
                        liberado = g.Where(l => !l.Bloqueado && l.IdStatus == (int)StatusTransacaoEnum.Finalizada && (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH")).Sum(l => l.Valor),
                        aPagar = g.Where(l => l.Bloqueado && (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH")).Sum(l => l.Valor)
                    })
                    .OrderByDescending(s => s.ano).ThenByDescending(s => s.mesNum).Take(6)
                    .Select(s => new { s.mes, s.qtd, s.gerado, s.estornado, s.liberado, s.aPagar, diasParaLiberacao = quarentenaPlus(s.ano, s.mesNum, hoje) })
                    .ToList();

                var lancamentosBloqueadosRaw = _context.Lancamento
                    .Where(l => l.Ativo && (l.Bloqueado || l.Tipo.Chave == "CHBK") && l.DataLancamento < hoje)
                    .Include(l => l.Tipo).AsNoTracking()
                    .Select(l => new { l.Valor, l.DataLancamento }).ToList();

                var lancamentosBloqueados = lancamentosBloqueadosRaw
                    .Select(l => new { l.Valor, Idade = (int)(hoje - l.DataLancamento.Date).TotalDays }).ToList();

                var aging = new[]
                {
                    new { bucket = "1–15 dias",  valor = lancamentosBloqueados.Where(x => x.Idade >= 1  && x.Idade <= 15).Sum(x => Math.Abs(x.Valor)), qtd = lancamentosBloqueados.Count(x => x.Idade >= 1  && x.Idade <= 15), risk = "low"      },
                    new { bucket = "16–30 dias", valor = lancamentosBloqueados.Where(x => x.Idade > 15  && x.Idade <= 30).Sum(x => Math.Abs(x.Valor)), qtd = lancamentosBloqueados.Count(x => x.Idade > 15  && x.Idade <= 30), risk = "med"      },
                    new { bucket = "31–60 dias", valor = lancamentosBloqueados.Where(x => x.Idade > 30  && x.Idade <= 60).Sum(x => Math.Abs(x.Valor)), qtd = lancamentosBloqueados.Count(x => x.Idade > 30  && x.Idade <= 60), risk = "high"     },
                    new { bucket = "60+ dias",   valor = lancamentosBloqueados.Where(x => x.Idade > 60).Sum(x => Math.Abs(x.Valor)),                   qtd = lancamentosBloqueados.Count(x => x.Idade > 60),                   risk = "critical" }
                };

                return Ok(new
                {
                    periodo,
                    de = de.ToString("yyyy-MM-dd"),
                    ate = hoje.ToString("yyyy-MM-dd"),
                    totals = new { faturamento = faturamentoTotal, faturamentoDelta, cashbackReservado = cashbackAPagar, diasMedio = 30, inadimplencia = inadimplenciaPct, inadimplenciaDelta, inadimplenciaValor = aging.Sum(a => a.valor), margem },
                    distribuicaoModelo = new { empresa = 0.50m, consumidor = 0.25m, rede = 0.25m },
                    categorias, aging, safras,
                    topParceiros = topParceirosRaw,
                    fonte = "lancamento+transacao",
                    geradoEm = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_bi_financeiro", detail = ex.Message });
            }
        }
    }
}
