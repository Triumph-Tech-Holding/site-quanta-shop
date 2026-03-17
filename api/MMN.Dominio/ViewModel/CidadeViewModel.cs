using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class CidadeViewModel
    {
        public int IdCidade { get; set; }
        public short IdEstado { get; set; }
        public string Nome { get; set; }

        public virtual Estado Estado { get; set; }
    }
}
