using MMN.Util.Enum;
using System.Text.Json.Serialization;

namespace MMN.Dominio.ViewModel
{
    public class CategoriaAnuncianteViewModel
    {
        public int IdCategoriaAnunciante { get; set; }
        public int IdCategoria { get; set; }
        public int IdAnunciante { get; set; }
        public bool Ativo { get; set; }
        public long? IdCredenciamento { get; set; }

        public int Status { get; set; }

        [JsonIgnore]
        public CategoriaViewModel Categoria { get; set; }
        [JsonIgnore]
        public AnuncianteViewModel Anunciante { get; set; }
    }

    public class CategoriaAnuncianteMapViewModel
    {
        public int IdCategoria { get; set; }
        public int IdAnunciante { get; set; }
    }
}
