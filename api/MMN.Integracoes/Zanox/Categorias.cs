using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class Categorias
    {
        public ICollection<Categories> Categories { get; set; }
    }

    public class Categories
    {
        public ICollection<Category> Category { get; set; }
        //public category category { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("@id")]
        public string IdCategorie { get; set; }
        [JsonPropertyName("$")]
        public string Name { get; set; }
    }

    public partial class CategoryElement
    {
        public Category Category { get; set; }
    }
}
