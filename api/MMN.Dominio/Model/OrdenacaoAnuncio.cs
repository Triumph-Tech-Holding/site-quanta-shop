namespace MMN.Dominio.Model
{
    public class OrdenacaoAnuncio
    {
        public long IdOrdenacaoAnuncio { get; set; }
        public long? IdCredenciamento { get; set; }
        public int? IdAnunciante { get; set; }
        public decimal? Ordenacao { get; set; }
        public bool ParceiroOnline { get; set; }

        public virtual Anunciante Anunciante { get; set; }
        public virtual Credenciamento Credenciamento { get; set; }
    }
}
