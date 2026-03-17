using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;

namespace MMN.INegocio.Negocio
{
    public interface IEstadoNegocio : IBaseNegocio<EstadoViewModel, Estado>
    {
        IList<EstadoViewModel> BuscarEstado();
    }
}
