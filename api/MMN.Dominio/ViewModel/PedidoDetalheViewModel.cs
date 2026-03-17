using MMN.Dominio.Model;
using System;

namespace MMN.Dominio.ViewModel
{
    public class PedidoDetalheViewModel
    {
        public long IdPedidoDetalhe { get; set; }
        public string Descricao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
        public long IdPedido { get; set; }
        public int IdStatus { get; set; }
        public Guid? IdUsuario { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public DateTime? AssinaturaDe { get; set; }
        public DateTime? AssinaturaAte { get; set; }
        public DateTime? AssinaturaProximaCobranca { get; set; }
        public string CodigoReferenciaFatura { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual Status Status { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
