using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Banco
    {
        public int IdBanco { get; set; }
        public int? Febraban { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? Ordem { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<UsuarioBanco> UsuarioBanco { get; set; }
    }
}
