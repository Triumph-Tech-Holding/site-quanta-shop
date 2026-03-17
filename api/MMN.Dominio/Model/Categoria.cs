using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public int? IdCategoriaPai { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Nome { get; set; }
        public string NomeExibicao { get; set; }
        public string Slug { get; set; }
        public bool Ativo { get; set; }
        public string Icone { get; set; }
        public string ChaveTraducao { get; set; }
        public string Chave { get; set; }
        public bool Destaque { get; set; }
        public string IdCategoriaZanox { get; set; }
        public virtual Categoria CategoriaPai { get; set; }
        public virtual ICollection<Categoria> Subcategorias { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<Faq> Faq { get; set; }
        public virtual ICollection<CategoriaAnunciante> CategoriaAnunciante { get; set; }
        public virtual ICollection<Credenciamento> Credenciamentos { get; set; }
    }
}
