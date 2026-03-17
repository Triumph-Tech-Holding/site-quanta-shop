using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;

namespace MMN.INegocio.Negocio
{
    public interface IFaqNegocio : IBaseNegocio<FaqViewModel, Faq>
    {
        void CriarFaq(FaqViewModel viewModel, Guid idUsuarioLogado);
        void EditarFaq(FaqViewModel editar, Guid idUsuarioLogado);
        IList<FaqViewModel> BuscarFaq();
        List<Faq> FiltrarFAQ(FiltroFaqAdmin filtroFaqAdmin, Guid idUsuarioLogado);
    }
}
