using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class GerarPedidoManualViewModel
    {
        public int IdProduto { get; set; }
        public string Login { get; set; }
        public decimal ValorPedido { get; set; }
        public decimal ReaisPorPonto { get; set; }
        public bool GerarPontos { get; set; }
        public bool DistribuirNaRede { get; set; }
        public int NumeroParcelas { get; set; }
        public int? NumeroParcelasPagas { get; set; }
        public DateTime? DataReferencia { get; set; }
    }
}
