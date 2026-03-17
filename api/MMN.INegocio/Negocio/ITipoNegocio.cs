using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;

namespace MMN.INegocio.Negocio
{
    public interface ITipoNegocio : IBaseNegocio<TipoViewModel, Tipo>
    {
        IList<TipoViewModel> ObterTipoPorChave(string chave);
        List<TipoViewModel> GetFromCache();
    }
}
