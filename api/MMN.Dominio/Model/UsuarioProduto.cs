using System;

namespace MMN.Dominio.Model
{
    public class UsuarioProduto
    {
        public int IdUsuarioProduto { get; set; }
        public int IdProduto { get; set; }
        public Guid IdUsuario { get; set; }
        public long IdPedido { get; set; }
        public DateTime DataVinculo { get; set; }
        public bool Ativo { get; set; }
        public bool AssinaturaHabilitada { get; set; }
        public DateTime? DataAssinatura {  get; set; }
        public DateTime? AssinaturaDe {  get; set; }
        public DateTime? AssinaturaAte { get; set; }
        public DateTime? AssinaturaProximaCobranca { get; set; }
        public bool AssinaturaManual { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
