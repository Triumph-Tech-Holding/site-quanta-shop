using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using MMN.Dominio.ViewModel;

namespace MMN.IRepositorio.Repositorio
{
    public interface ITipoRepositorio : IBaseRepositorio<Tipo>
    {
        IList<Tipo> ObterTipoPorChave(string chave);
    }
}
