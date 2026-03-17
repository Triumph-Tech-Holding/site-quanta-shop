using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface IConfiguracaoNegocio : IBaseNegocio<ConfiguracaoViewModel, Configuracao>
    {
        ConfiguracaoViewModel BuscarPelaChave(string chave);
        ConfiguracaoViewModel BuscarRootSite();
        List<ConfiguracaoViewModel> GetFromCache();
        void EditarConfig(ConfiguracaoViewModel editar);
    }
}
