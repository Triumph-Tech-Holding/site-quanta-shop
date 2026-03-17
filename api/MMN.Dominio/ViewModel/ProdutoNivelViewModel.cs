using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class ProdutoNivelViewModel
    {
        public int IdProdutoNivel { get; set; }
        public int Nivel { get; set; }
        public int IdProduto { get; set; }
        public decimal PorcentagemCashback { get; set; }
        public decimal PorcetagemAdesao { get; set; }

    }
}
