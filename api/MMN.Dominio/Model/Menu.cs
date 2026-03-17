using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Menu
    {
        public int IdMenu { get; set; }
        public string Texto { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }
        public int Posicao { get; set; }
        public string ChaveTraducao { get; set; }
        public bool RotaPublica { get; set; }
        public bool MenuPrincipal { get; set; }        
        public int? IdMenuPai { get; set; }
        public virtual ICollection<Menu> SubMenus { get; set; }
        public virtual Menu MenuPai { get; set; }
        public virtual ICollection<GrupoMenu> GrupoMenu { get; set; }
    }
}
