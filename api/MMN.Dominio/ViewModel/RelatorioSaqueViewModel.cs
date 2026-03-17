using System;

namespace MMN.Dominio.ViewModel
{
    public class RelatorioSaqueViewModel
    {
        public int TotalCount { get; set; }
        public string Graduacao { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal ValorSaque { get; set; }
        public decimal PercentualTaxa { get; set; }
        public decimal ValorTaxa { get; set; }
        public decimal ValorLiquido { get; set; }
        public string Status { get; set; }
        public DateTime? DataPagamento { get; set; }
    }
}
