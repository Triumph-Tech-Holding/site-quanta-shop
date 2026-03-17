namespace MMN.Dominio.Model
{
    public class CategoriaAnunciante
    {
        public int IdCategoriaAnunciante { get; set; }
        public int IdCategoria { get; set; }
        public int IdAnunciante { get; set; }
        public bool Ativo { get; set; }

        public Categoria Categoria { get; set; }
        public Anunciante Anunciante { get; set; }
    }
}
