using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model
{
    public class QuantaAmizadeHistorico
    {
        [Key]
        public int IdHistoricoCashbackUsuario { get; set; }
        public int IdQuantaAmizade { get; set; }
        public Guid IdUsuario { get; set; }
        public int? IdPedido { get; set; }
        public int? IdTransacao { get; set; }
        public DateTime DataCompra { get; set; }
        public decimal ValorCashback { get; set; }
        public bool? Finalizado { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("IdQuantaAmizade")]
        public virtual QuantaAmizade QuantaAmizade { get; set; }
    }
}
