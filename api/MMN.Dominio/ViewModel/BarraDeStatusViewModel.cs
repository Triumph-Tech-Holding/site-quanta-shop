using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class BarraDeStatusViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Graduacao { get; set; }
        public int MesesAtivo { get; set; }
        public int? DiasVencimento { get; set; }
        public decimal? ValorDistribuicao { get; set; }
        public string MeuLink { get; set; }
        public int PosicaoBinario { get; set; }
        public string PlanoAtivo { get; set; }
        public int TotalEsquerda { get; set; }
        public int TotalDireita { get; set; }
        public int TotalDireitos { get; set; }
        public int Total { get; set; }
        public string UrlImg { get; set; }
    }
}
