using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IFaturaNegocioNovo
    {
        Task<IEnumerable<Pedido>> ObterFaturasAsync(Guid idUsuarioLogado);
        Task<IEnumerable<Pedido>> ObterPedidosAguardandoFaturaAsync(DateTime ateData, Guid idUsuarioLogado);
    }
}
