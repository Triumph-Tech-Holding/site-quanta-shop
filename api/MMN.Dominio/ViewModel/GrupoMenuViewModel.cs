using System;

namespace MMN.Dominio.ViewModel
{
    public class GrupoMenuViewModel
    {
        public int? IdGrupoMenu { get; set; }
        public int IdGrupo { get; set; }
        public int IdMenu { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdUsuarioAcao { get; set; }
        public bool Acesso { get; set; }
        public bool? SomenteLeitura { get; set; }

        //public virtual GrupoViewModel Grupo { get; set; }
        //public virtual Menu Menu { get; set; }
    }
}
