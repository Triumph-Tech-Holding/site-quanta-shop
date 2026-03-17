using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Models.Produto
{
    public class AtivarProdutoRequest
    {
        public int IdProduto { get; set; }
        public string Login { get; set; }
        public decimal ValorPedido { get; set; }
        public decimal ReaisPorPonto { get; set; }
        public bool GerarPontos { get; set; }
        public bool DistribuirNaRede { get; set; }
        public bool AtivacaoPresente { get; set; }
    }
}
