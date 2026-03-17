using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.IRepositorio.Repositorio
{
    public interface IFaqRepositorio : IBaseRepositorio<Faq>
    {
        IList<Faq> BuscarFaq();
    }
}
