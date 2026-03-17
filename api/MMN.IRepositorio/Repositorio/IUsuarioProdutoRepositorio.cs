using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System;

namespace MMN.IRepositorio.Repositorio
{
    public interface IUsuarioProdutoRepositorio : IBaseRepositorio<UsuarioProduto>
    {
        UsuarioProduto BuscarProdutoAtivo(Guid idUsuario);
        UsuarioProduto BuscarAssinaturaAtiva(Guid idUsuario);
        UsuarioProduto BuscarPorPedido(long idPedido);
        bool InserirPlanoManual(Usuario usuario, Produto plano, int planoAtivoId);
        bool InserirPlanoPresente(Usuario usuario, Produto plano, int planoAtivoId);
    }
}
