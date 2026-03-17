using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class LojasCredenciadoViewModel
    {
        public long IdCredenciamento { get; set; }
        public string Estabelecimento { get; set; }
        public string LogoUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string ImageBase64 { get; set; }
        public decimal? PercentualCashback { get; set; }
    }
}
