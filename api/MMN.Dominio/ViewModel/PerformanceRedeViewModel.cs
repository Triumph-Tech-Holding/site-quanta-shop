using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class PerformanceRedeViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public int Direto { get; set; }
        public decimal ValorGerado { get; set; }
        public int Pontos { get; set; }
        public string NomeProduto { get; set; }
        public string  URLIMG {get;set;}

    }
}
