using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Estado
    {
        public short IdEstado { get; set; }
        public string Nome { get; set; }
        public short IdPais { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Cidade> Cidade { get; set; }
    }
}
