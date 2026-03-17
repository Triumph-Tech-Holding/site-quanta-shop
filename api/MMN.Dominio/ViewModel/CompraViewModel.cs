using MMN.Util.Enum;
using System;

namespace MMN.Dominio.ViewModel
{
    public class CompraViewModel
    {
        public int? IdProduto { get; set; }
        public int NumParcelas { get; set; }
        public int NumParcelasPagas { get; set; }
        public Guid IdUsuario { get; set; }
        public string SenhaEletronica { get; set; }
        public EnumTipoPagamento MetodoDePagamento { get; set; }
        public decimal? ValorPedido { get; set; }
        public decimal? TaxaCompra { get; set; }
        public EnumTipoTransacao? TipoTransacao { get; set; }
        public decimal? ReaisPorPonto { get; set; }
        public decimal? ValorTaxa { get; set; }
        public DateTime? DataReferencia { get; set; }
        public string CardHash { get; set; }
        public string CardExpirationDate { get; set; }

        public CartaoViewModel Card { get; set; }
    }
}
