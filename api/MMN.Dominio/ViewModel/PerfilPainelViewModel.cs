using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class PerfilPainelViewModel
    {
        public Guid IdUsuario { get; set; }
        public string PlanoAtivo { get; set; }
        public int PessoasAtivas { get; set; }
        public decimal Ganho { get; set; }
        public decimal Saques { get; set; }
        public decimal Investimento { get; set; }
        public int Indireto { get; set; }
        public int Total { get; set; }
    }
}
