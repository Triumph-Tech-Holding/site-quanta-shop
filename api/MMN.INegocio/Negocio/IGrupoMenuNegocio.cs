using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface IGrupoMenuNegocio : IBaseNegocio<GrupoMenuViewModel, GrupoMenu>
    {
        List<object> SelecionarMenus(List<MenuViewModel> menus, List<GrupoMenuViewModel> grupoMenus);
    }
}
