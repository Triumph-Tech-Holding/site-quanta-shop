using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class Item
    {
        public List<ProgramItem> programItem { get; set; }
    }

    public class ProgramItem
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

    //USAR QUANDO A QUISER TRAZER AS CATEGORIAS JUNTO COM O PROGRAMA
    public class ItemComCategoria
    {
        public List<ProgramItemComCategoria> programItem { get; set; }
    }

    public class ProgramItemComCategoria
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<Categories> categories { get; set; }
    }

    public class ItemComCategoria1
    {
        public List<ProgramItemComCategoria1> programItem { get; set; }
    }

    public class ProgramItemComCategoria1
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<CategoryElement> categories { get; set; }
    }
}
