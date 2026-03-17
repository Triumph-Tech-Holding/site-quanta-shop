using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class CategoriaViewModel
    {
        public int IdCategoria { get; set; }
        public int? IdCategoriaPai { get; set; }
        public string Nome { get; set; }
        public string NomeExibicao { get; set; }
        public string Slug { get; set; }
        public bool Ativo { get; set; }
        public string Icone { get; set; }
        public string ChaveTraducao { get; set; }
        public string Chave { get; set; }
        public string IdCategoriaZanox { get; set; }
        public int TotalCadastros { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Destaque { get; set; }
        public CategoriaViewModel CategoriaPai { get; set; }
        public List<ProdutoViewModel> Produtos { get; set; }
        public virtual ICollection<CategoriaAnuncianteViewModel> CategoriaAnunciante { get; set; }
    }

    public class CriarCategoriaViewModel
    {
        public int? IdCategoria { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int? IdCategoriaPai { get; set; }
    }

    public class FiltroHomeCategoriaViewModel
    {
        public string Nome { get; set; }
    }
}
