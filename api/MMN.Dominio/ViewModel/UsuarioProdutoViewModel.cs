using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioProdutoViewModel
    {
        public int IdUsuarioProduto { get; set; }
        public int IdProduto { get; set; }
        public Guid IdUsuario { get; set; }
        public long IdPedido { get; set; }
        public DateTime DataVinculo { get; set; }
        public bool Ativo { get; set; }
        public bool AssinaturaHabilitada { get; set; }
        public DateTime? DataAssinatura { get; set; }
        public DateTime? AssinaturaDe { get; set; }
        public DateTime? AssinaturaAte { get; set; }
        public DateTime? AssinaturaProximaCobranca { get; set; }
        public bool AssinaturaManual { get; set; }

        public PedidoViewModel Pedido { get; set; }
        public ProdutoViewModel Produto { get; set; }
        public UsuarioViewModel Usuario { get; set; }
    }
}
