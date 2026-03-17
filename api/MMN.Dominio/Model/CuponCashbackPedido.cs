using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.Model
{
    public class CuponCashbackPedido
    {
        public string IdCuponCashback { get; set; }
        public long IdPedido { get; set; }

        public CupomCashback CuponCashback { get; set; }
        public Pedido Pedido { get; set; }
    }
}
