using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class TokenViewModel
    {
        public string IdUsuario { get; set; }
        public string Jti { get; set; }
        public DateTime DataExpira { get; set; }
    }
}
