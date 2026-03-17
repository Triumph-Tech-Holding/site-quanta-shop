using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.INegocio.Negocio
{
    public interface IMenuNegocio : IBaseNegocio<MenuViewModel, Menu>
    {
        IList<MenuViewModel> ObterMenuPorGrupo(int idGrupo);
    }
}
