using System;

namespace MMN.Dominio.Model
{
    public class AnuncianteCashBackLog
    {
        public int IdAnuncianteCashBackLog { get; set; }
        public int IdAnuncianteCashBack { get; set; }
        public string Descricao { get; set; }
        public decimal? Percentual { get; set; }
        public decimal? ValorFixo { get; set; }
        public string IdProgramZanox { get; set; }
        public int IdAnunciante { get; set; }
        public bool Ativo { get; set; }
        public string IdTrackingCategorie { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
