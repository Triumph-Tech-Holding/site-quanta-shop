using System;

namespace MMN.Dominio.Model
{
    public class CupomUso
    {
        public long IdCupomUso { get; set; }
        public int IdCupom { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdPedido { get; set; }
        public decimal ValorAplicado { get; set; }
        public DateTime DataUso { get; set; }

        public virtual Cupom Cupom { get; set; }
    }
}
