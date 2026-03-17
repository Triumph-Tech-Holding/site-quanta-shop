using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class EstadoViewModel
    {
        public short IdEstado { get; set; }
        public string Nome { get; set; }
        public short IdPais { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Cidade> Cidade { get; set; }
    }
}
