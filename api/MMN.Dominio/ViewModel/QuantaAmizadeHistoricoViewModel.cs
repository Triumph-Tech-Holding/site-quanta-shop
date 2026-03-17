using System;

namespace MMN.Dominio.ViewModel
{
    public class QuantaAmizadeHistoricoViewModel
    {
        public int IdHistoricoCashbackUsuario { get; set; }
        public int IdQuantaAmizade { get; set; }
        public Guid IdUsuario { get; set; }
        public int? IdPedido { get; set; }
        public int? IdTransacao { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal ValorCashback { get; set; }
        public bool? Finalizado { get; set; }
    }
}
