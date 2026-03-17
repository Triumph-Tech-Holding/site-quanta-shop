using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Valor { get; set; }
        public decimal TetoBinario { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Visivel { get; set; }
        public decimal ReaisPorPonto { get; set; }
        public int Pontos { get; set; }
        public int Parcelas { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAutalizacao { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<ProdutoNivel> ProdutoNivel { get; set; }
        public virtual ICollection<CacheResumoBinario> CacheResumoBinario { get; set; }
        public virtual ICollection<UsuarioProduto> UsuarioProduto { get; set; }
    }
}
