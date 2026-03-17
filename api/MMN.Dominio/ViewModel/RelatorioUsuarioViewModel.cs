using System;

namespace MMN.Dominio.ViewModel
{
    public class RelatorioUsuarioViewModel
    {
        public int TotalCount { get; set; }
        public string Graduacao { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataCadastro { get; set; }
        public string QualificadoBinario { get; set; }
        public DateTime? DataQualificacao { get; set; }
        public DateTime? DataCompra { get; set; }
        public string Patrocinador { get; set; }
        public string ProdutoAtivo { get; set; }
        public DateTime? DataPagamentoPedido { get; set; }
        public decimal ValorPago { get; set; }
        public string MeioPagamento { get; set; }
    }
}
