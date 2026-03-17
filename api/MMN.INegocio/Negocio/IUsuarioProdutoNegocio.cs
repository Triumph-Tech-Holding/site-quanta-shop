using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;

namespace MMN.INegocio.Negocio
{
    public interface IUsuarioProdutoNegocio : IBaseNegocio<UsuarioProdutoViewModel, UsuarioProduto>
    {
        UsuarioProdutoViewModel BuscarProdutoAtivo(Guid idUsuario);
        UsuarioProdutoViewModel BuscarAssinaturaAtiva(Guid idUsuario);
        UsuarioProdutoViewModel BuscarPorPedido(long IdPedido);
        bool InserirPlanoManual(UsuarioViewModel usuairo, ProdutoViewModel plano, int planoAtivoId);
        bool InserirPlanoPresente(UsuarioViewModel usuairo, ProdutoViewModel plano, int planoAtivoId);
    }
}
