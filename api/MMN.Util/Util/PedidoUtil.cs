using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Util.Util
{
    public class PedidoDetalheProcedure
    {
        public Guid IdUsuario { get; set; }
        public Guid? IdVendaZanox { get; set; }
        public decimal ValorCashback { get; set; }
        public string NomeCampanha { get; set; }
        public int IdAnunciante { get; set; }
        public long IdPedido { get; set; }
        public int IdStatus { get; set; }
        public decimal ValorTotalCompra { get; set; }
        public long? IdVendaAfilio { get; set; }
        public long? IdVendaAwin { get; set; }
    }
}
