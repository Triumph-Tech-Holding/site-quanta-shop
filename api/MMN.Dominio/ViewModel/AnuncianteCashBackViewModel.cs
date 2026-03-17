using System;

namespace MMN.Dominio.ViewModel
{
    public class AnuncianteCashBackViewModel
    {
        public int IdAnuncianteCashBack { get; set; }
        public string Descricao { get; set; }
        public decimal? Percentual { get; set; }
        public decimal? ValorFixo { get; set; }
        public string IdProgramZanox { get; set; }
        public string Tipo { get; set; }
        public string Moeda { get; set; }
        public int IdAnunciante { get; set; }
        public bool Ativo { get; set; }
        public string IdTrackingCategorie { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
