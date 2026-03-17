using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IMenuRepositorio : IBaseRepositorio<Menu>
    {
        IList<Menu> ObterMenuPorGrupo(int idGrupo);
    }
}
