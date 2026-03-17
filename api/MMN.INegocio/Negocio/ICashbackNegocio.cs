using Microsoft.EntityFrameworkCore.Storage;
using MMN.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface ICashbackNegocio
    {
        Task LancarCashback(
            long idPedido,
            Anunciante anunciante = null,
            Guid? IdUsuario = null,
            bool finalizado = true,
            IDbContextTransaction dbTransaction = null);
        Task<Transacao> CriarTransacaoCashbackAsync(Pedido pedido, Anunciante anunciante = null);
        Task InserirCupomFiscal( string chaveAcesso, string urlChaveDeAcessoNF, bool chaveManual, Guid idUsuario);
    }
}
