using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Negocio.Services
{
    public sealed class CashbackDistribuicaoConfig
    {
        public decimal SustentabilidadePercentual { get; set; }
        public decimal SplitEmpresaPercentual { get; set; }
        public decimal SplitConsumidorPercentual { get; set; }
        public decimal SplitRedePercentual { get; set; }
        public decimal MultiplicadorPlus { get; set; } = 2.0m;
        public bool CompressaoDinamica { get; set; } = true;
        public IList<NivelResidualConfig> Niveis { get; set; } = new List<NivelResidualConfig>();
    }

    public sealed class NivelResidualConfig
    {
        public int Nivel { get; set; }
        public decimal Percentual { get; set; }
        public bool Ativo { get; set; }
    }

    public sealed class UplineMembro
    {
        public Guid IdUsuario { get; set; }
        public bool Ativo { get; set; }
        public bool IsPlus { get; set; }
    }

    public sealed class NivelDistribuicao
    {
        public int Nivel { get; set; }
        public Guid? IdUsuario { get; set; }
        public decimal Percentual { get; set; }
        public decimal Valor { get; set; }
        public bool PlusAplicado { get; set; }
        public bool Comprimido { get; set; }
        public int InativosPulados { get; set; }
    }

    public sealed class CashbackDistribuicaoResultado
    {
        public decimal ValorBruto { get; set; }
        public decimal Sustentabilidade { get; set; }
        public decimal LiquidoPosSustentabilidade { get; set; }
        public decimal Empresa { get; set; }
        public decimal Consumidor { get; set; }
        public decimal Rede { get; set; }
        public decimal RedeDistribuido { get; set; }
        public decimal RedeSobra { get; set; }
        public IList<NivelDistribuicao> Niveis { get; set; } = new List<NivelDistribuicao>();
    }

    public interface ICashbackDistribuicaoService
    {
        CashbackDistribuicaoResultado Calcular(decimal valorBruto, CashbackDistribuicaoConfig config, IList<UplineMembro> uplines);
    }

    /// <summary>
    /// Motor puro de distribuicao de cashback Wave 2.
    /// Sequencia:
    ///   1. Retem percentual de sustentabilidade sobre o valor bruto
    ///   2. Aplica split base (empresa / consumidor / rede) sobre o liquido
    ///   3. Distribui rede em ate 12 niveis com compressao dinamica e bonus Plus
    /// Sem dependencias de DB para permitir testes isolados.
    /// </summary>
    public sealed class CashbackDistribuicaoService : ICashbackDistribuicaoService
    {
        public CashbackDistribuicaoResultado Calcular(decimal valorBruto, CashbackDistribuicaoConfig config, IList<UplineMembro> uplines)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (valorBruto < 0m) throw new ArgumentOutOfRangeException(nameof(valorBruto), "valor_bruto_negativo");
            uplines = uplines ?? new List<UplineMembro>();

            var sustent = Round4(valorBruto * config.SustentabilidadePercentual / 100m);
            var liquido = Round4(valorBruto - sustent);
            var empresa = Round4(liquido * config.SplitEmpresaPercentual / 100m);
            var consumidor = Round4(liquido * config.SplitConsumidorPercentual / 100m);
            var rede = Round4(liquido * config.SplitRedePercentual / 100m);

            var niveisDist = new List<NivelDistribuicao>();
            var niveisOrdenados = (config.Niveis ?? new List<NivelResidualConfig>())
                .OrderBy(n => n.Nivel)
                .ToList();

            var uplineIdx = 0;
            foreach (var nivelCfg in niveisOrdenados)
            {
                if (!nivelCfg.Ativo) continue;
                if (uplineIdx >= uplines.Count) break;

                UplineMembro alvo = null;
                var pulados = 0;
                while (uplineIdx < uplines.Count)
                {
                    var u = uplines[uplineIdx];
                    if (u.Ativo || !config.CompressaoDinamica)
                    {
                        alvo = u;
                        uplineIdx++;
                        break;
                    }
                    pulados++;
                    uplineIdx++;
                }
                if (alvo == null) break;

                var basePct = nivelCfg.Percentual / 100m;
                var valor = Round4(rede * basePct);
                var plusApplied = false;
                if (alvo.IsPlus && config.MultiplicadorPlus > 1m)
                {
                    valor = Round4(valor * config.MultiplicadorPlus);
                    plusApplied = true;
                }

                niveisDist.Add(new NivelDistribuicao
                {
                    Nivel = nivelCfg.Nivel,
                    IdUsuario = alvo.IdUsuario,
                    Percentual = nivelCfg.Percentual,
                    Valor = valor,
                    PlusAplicado = plusApplied,
                    Comprimido = pulados > 0,
                    InativosPulados = pulados,
                });
            }

            var totalDist = niveisDist.Sum(n => n.Valor);
            return new CashbackDistribuicaoResultado
            {
                ValorBruto = valorBruto,
                Sustentabilidade = sustent,
                LiquidoPosSustentabilidade = liquido,
                Empresa = empresa,
                Consumidor = consumidor,
                Rede = rede,
                RedeDistribuido = Round4(totalDist),
                RedeSobra = Round4(rede - totalDist),
                Niveis = niveisDist,
            };
        }

        private static decimal Round4(decimal v) => Math.Round(v, 4, MidpointRounding.AwayFromZero);
    }
}
