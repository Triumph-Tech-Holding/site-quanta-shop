using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Util.Model.Pedido
{
    public class UpdateParcela
    {
        public AcaoUpdateParcela Acao { get; set; }
        public int IdPedido { get; set; }
        public int NumeroParcela { get; set; }
        public bool? DistribuirNaRede { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataReferencia { get; set; }
    }
}
