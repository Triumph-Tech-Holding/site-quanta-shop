using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class AnuncianteViewModel
    {
        public int IdAnunciante { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public string Base64{ get; set; }
        public decimal Cashback { get; set; }
        public bool Ativo { get; set; }
        public string IdProgramZanox { get; set; }
        public string IdAfilio { get; set; }
        public string IdAwin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int Prioridade { get; set; }
        public bool EditadoUsuario { get; set; }

        public decimal? CashbackMin { get; set; }
        public decimal? CashbackMax { get; set; }
        public string TipoCashback { get; set; }
        public string Moeda { get; set; }

        public decimal? PercentualMenor { get; set; }
        public decimal? PercentualMaior { get; set; }
        public decimal? ValorFixoMenor { get; set; }
        public decimal? ValorFixoMaior { get; set; }
        public bool MostrarUmaPorcentagem { get; set; }
        public bool MostrarDuasPorcentagens { get; set; }
        public bool MostrarUmValorFixo { get; set; }
        public bool MostrarDoisValores { get; set; }
        public bool MostrarUmPercEUmValor { get; set; }
        public bool MostrarUmPercEDoisValores { get; set; }
        public bool MostrarDoisPercEUmValor { get; set; }
        public bool MostrarDoisPercEDoisValores { get; set; }
        public long? IdCredenciado { get; set; }
        public object AccountId { get; set; }
        public bool Ancora {  get; set; }
        public TipoAnuncianteEnum? Tipo { get; set; }
        public virtual ICollection<AnuncianteCashBackViewModel> AnuncianteCashBack { get; set; }
        public virtual ICollection<CategoriaAnuncianteViewModel> CategoriaAnunciante { get; set; }
    }
}
