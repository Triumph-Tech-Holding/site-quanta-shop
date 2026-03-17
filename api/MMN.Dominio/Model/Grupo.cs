using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Grupo
    {
        public int IdGrupo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<GrupoMenu> GrupoMenu { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
