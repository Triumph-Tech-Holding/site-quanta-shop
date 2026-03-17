using System;

namespace MMN.Dominio.ViewModel
{
    public class LancamentoRetidoViewModel
    {
        public long IdLancamentoRetido { get; set; }
        public long IdLancamento { get; set; }
        public long? IdPedido { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataDesbloqueio { get; set; }
        public bool Ativo { get; set; }
    }
}
