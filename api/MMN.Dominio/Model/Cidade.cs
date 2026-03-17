using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Cidade
    {
        public int IdCidade { get; set; }
        public short IdEstado { get; set; }
        public string Nome { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<Credenciamento> Credenciamento { get; set; }
    }
}
