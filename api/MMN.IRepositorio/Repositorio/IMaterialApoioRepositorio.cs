using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;

namespace MMN.IRepositorio.Repositorio
{
    public interface IMaterialApoioRepositorio : IBaseRepositorio<MaterialApoio>
    {
        IList<MaterialApoio> BuscarMaterial();
        
    }
}
