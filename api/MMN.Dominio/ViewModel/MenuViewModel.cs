using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            SubMenus = new List<Menu>();
            SubMenus = new List<Menu>();
        }
        public int IdMenu { get; set; }
        public string Texto { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }
        public int Posicao { get; set; }
        public bool MenuPrincipal { get; set; }
        public string ChaveTraducao { get; set; }
        public int? IdMenuPai { get; set; }
        public List<Menu> SubMenus { get; set; }
        public Menu MenuPai { get; set; }
        public List<GrupoMenu> GrupoMenu { get; set; }
    }
}
