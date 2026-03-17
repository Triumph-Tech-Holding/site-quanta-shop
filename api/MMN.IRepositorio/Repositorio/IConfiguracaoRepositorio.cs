using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.IRepositorio.Repositorio
{
    public interface IConfiguracaoRepositorio : IBaseRepositorio<Configuracao>
    {
        Configuracao GetByKey(string chave);
    }
}
