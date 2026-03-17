using System;

namespace MMN.Dominio.ViewModel
{
    public class ProdutoViewModel
    {
        public int IdProduto { get; set; }
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Valor { get; set; }
        public decimal TetoBinario { get; set; }
        public decimal ReaisPorPonto { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Visivel { get; set; }
        public int Pontos { get; set; }
        public int Parcelas { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAutalizacao { get; set; }
        public CategoriaViewModel Categoria { get; set; }

    }
}
