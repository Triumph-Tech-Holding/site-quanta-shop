using MMN.Dominio.ViewModel;
using System.Collections.Generic;

namespace MMN.INegocio.Negocio
{
    public interface ITipoPagamentoNegocio
    {
        IEnumerable<TipoPagamentoViewModel> GetTipoPagamentoVendaOffline();
    }
}
