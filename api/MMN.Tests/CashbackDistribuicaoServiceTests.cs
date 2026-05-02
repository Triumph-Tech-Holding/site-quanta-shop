using System;
using System.Collections.Generic;
using System.Linq;
using MMN.Negocio.Services;
using Xunit;

namespace MMN.Tests
{
    public class CashbackDistribuicaoServiceTests
    {
        private static CashbackDistribuicaoConfig BaseConfig(bool compressao = true)
        {
            return new CashbackDistribuicaoConfig
            {
                SustentabilidadePercentual = 10m,
                SplitEmpresaPercentual = 50m,
                SplitConsumidorPercentual = 25m,
                SplitRedePercentual = 25m,
                MultiplicadorPlus = 2m,
                CompressaoDinamica = compressao,
                Niveis = Enumerable.Range(1, 12)
                    .Select(n => new NivelResidualConfig { Nivel = n, Percentual = 100m / 12m, Ativo = true })
                    .ToList(),
            };
        }

        private static List<UplineMembro> Uplines(int total, params int[] indicesInativos)
        {
            var inativos = new HashSet<int>(indicesInativos);
            return Enumerable.Range(0, total)
                .Select(i => new UplineMembro { IdUsuario = Guid.NewGuid(), Ativo = !inativos.Contains(i), IsPlus = false })
                .ToList();
        }

        [Fact]
        public void SplitBase_10Sustentabilidade_50_25_25_DistribuiCorreto()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            var res = svc.Calcular(100m, cfg, Uplines(12));

            Assert.Equal(10m, res.Sustentabilidade);
            Assert.Equal(90m, res.LiquidoPosSustentabilidade);
            Assert.Equal(45m, res.Empresa);
            Assert.Equal(22.5m, res.Consumidor);
            Assert.Equal(22.5m, res.Rede);
        }

        [Fact]
        public void DozeNiveisAtivos_ComRedeCheia_DistribuiTotalmente()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            var res = svc.Calcular(120m, cfg, Uplines(12));

            Assert.Equal(12, res.Niveis.Count);
            Assert.True(res.RedeDistribuido > 0m);
            // soma das distribuicoes deve equiparar (com pequena tolerancia de arredondamento) o pool de rede
            Assert.True(Math.Abs(res.RedeDistribuido - res.Rede) < 0.01m,
                $"Esperava RedeDistribuido≈Rede, obteve {res.RedeDistribuido} vs {res.Rede}");
        }

        [Fact]
        public void CompressaoDinamica_3InativosConsecutivos_PulaEDistribuiAoProximo()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig(compressao: true);
            // 3 primeiros inativos consecutivos (indices 0,1,2)
            var uplines = Uplines(8, 0, 1, 2);

            var res = svc.Calcular(1000m, cfg, uplines);

            // O nivel 1 deve ser atribuido ao primeiro upline ativo (indice 3) com 3 inativos pulados
            var nivel1 = res.Niveis.First(n => n.Nivel == 1);
            Assert.Equal(uplines[3].IdUsuario, nivel1.IdUsuario);
            Assert.True(nivel1.Comprimido);
            Assert.Equal(3, nivel1.InativosPulados);
            // O nivel 2 deve ir para o seguinte (indice 4), sem compressao
            var nivel2 = res.Niveis.First(n => n.Nivel == 2);
            Assert.Equal(uplines[4].IdUsuario, nivel2.IdUsuario);
            Assert.False(nivel2.Comprimido);
        }

        [Fact]
        public void CompressaoDesativada_3InativosConsecutivos_DistribuiAoInativo()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig(compressao: false);
            var uplines = Uplines(8, 0, 1, 2);

            var res = svc.Calcular(1000m, cfg, uplines);

            var nivel1 = res.Niveis.First(n => n.Nivel == 1);
            Assert.Equal(uplines[0].IdUsuario, nivel1.IdUsuario);
            Assert.False(nivel1.Comprimido);
            Assert.Equal(0, nivel1.InativosPulados);
        }

        [Fact]
        public void ValorZero_RetornaTudoZeradoSemDistribuicao()
        {
            var svc = new CashbackDistribuicaoService();
            var res = svc.Calcular(0m, BaseConfig(), Uplines(12));

            Assert.Equal(0m, res.Sustentabilidade);
            Assert.Equal(0m, res.Empresa);
            Assert.Equal(0m, res.Consumidor);
            Assert.Equal(0m, res.Rede);
            Assert.All(res.Niveis, n => Assert.Equal(0m, n.Valor));
        }

        [Fact]
        public void RedeVazia_NenhumaDistribuicao_SobraIgualRede()
        {
            var svc = new CashbackDistribuicaoService();
            var res = svc.Calcular(100m, BaseConfig(), new List<UplineMembro>());

            Assert.Empty(res.Niveis);
            Assert.Equal(0m, res.RedeDistribuido);
            Assert.Equal(res.Rede, res.RedeSobra);
        }

        [Fact]
        public void MultiplicadorPlus_DobraValorDoNivelComUplinePlus()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            cfg.Niveis = new List<NivelResidualConfig>
            {
                new NivelResidualConfig { Nivel = 1, Percentual = 100m, Ativo = true },
            };
            var u = new List<UplineMembro>
            {
                new UplineMembro { IdUsuario = Guid.NewGuid(), Ativo = true, IsPlus = true }
            };

            var res = svc.Calcular(100m, cfg, u);

            // rede = 22.5 * 100% = 22.5; com plus x2 = 45.0
            Assert.Single(res.Niveis);
            Assert.True(res.Niveis[0].PlusAplicado);
            Assert.Equal(45m, res.Niveis[0].Valor);
        }

        [Fact]
        public void NiveisInativos_SaoIgnoradosNaDistribuicao()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            // Desativa niveis 2, 4, 6
            foreach (var n in cfg.Niveis.Where(x => x.Nivel == 2 || x.Nivel == 4 || x.Nivel == 6)) n.Ativo = false;

            var res = svc.Calcular(1200m, cfg, Uplines(12));

            Assert.DoesNotContain(res.Niveis, n => n.Nivel == 2);
            Assert.DoesNotContain(res.Niveis, n => n.Nivel == 4);
            Assert.DoesNotContain(res.Niveis, n => n.Nivel == 6);
            Assert.Equal(9, res.Niveis.Count);
        }

        [Fact]
        public void ReconfiguracaoRuntime_NovoCalculoUsaConfigNova()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg1 = BaseConfig();
            var res1 = svc.Calcular(100m, cfg1, Uplines(12));

            var cfg2 = BaseConfig();
            cfg2.SustentabilidadePercentual = 5m; // mudou em runtime
            cfg2.SplitEmpresaPercentual = 40m;
            cfg2.SplitConsumidorPercentual = 30m;
            cfg2.SplitRedePercentual = 30m;
            var res2 = svc.Calcular(100m, cfg2, Uplines(12));

            Assert.Equal(10m, res1.Sustentabilidade);
            Assert.Equal(5m, res2.Sustentabilidade);
            Assert.Equal(45m, res1.Empresa);
            Assert.Equal(38m, res2.Empresa);
            Assert.NotEqual(res1.Rede, res2.Rede);
        }

        [Fact]
        public void PercentuaisCustomizadosPorNivel_RespeitaConfig()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            cfg.Niveis = new List<NivelResidualConfig>
            {
                new NivelResidualConfig { Nivel = 1, Percentual = 50m, Ativo = true },
                new NivelResidualConfig { Nivel = 2, Percentual = 30m, Ativo = true },
                new NivelResidualConfig { Nivel = 3, Percentual = 20m, Ativo = true },
            };

            var res = svc.Calcular(400m, cfg, Uplines(3));

            // sustent=40, liquido=360, rede=90
            Assert.Equal(90m, res.Rede);
            Assert.Equal(45m, res.Niveis.First(n => n.Nivel == 1).Valor);
            Assert.Equal(27m, res.Niveis.First(n => n.Nivel == 2).Valor);
            Assert.Equal(18m, res.Niveis.First(n => n.Nivel == 3).Valor);
            Assert.Equal(0m, res.RedeSobra);
        }

        [Fact]
        public void CompressaoDinamica_TodosUplinesInativos_RedeFicaSemDistribuir()
        {
            var svc = new CashbackDistribuicaoService();
            var cfg = BaseConfig();
            var u = Enumerable.Range(0, 5)
                .Select(_ => new UplineMembro { IdUsuario = Guid.NewGuid(), Ativo = false, IsPlus = false })
                .ToList();

            var res = svc.Calcular(1000m, cfg, u);

            Assert.Empty(res.Niveis);
            Assert.Equal(0m, res.RedeDistribuido);
            Assert.Equal(res.Rede, res.RedeSobra);
        }

        [Fact]
        public void ValorBrutoNegativo_LancaArgumentOutOfRange()
        {
            var svc = new CashbackDistribuicaoService();
            Assert.Throws<ArgumentOutOfRangeException>(() => svc.Calcular(-1m, BaseConfig(), Uplines(1)));
        }

        [Fact]
        public void ConfigNula_LancaArgumentNullException()
        {
            var svc = new CashbackDistribuicaoService();
            Assert.Throws<ArgumentNullException>(() => svc.Calcular(100m, null, Uplines(1)));
        }
    }
}
