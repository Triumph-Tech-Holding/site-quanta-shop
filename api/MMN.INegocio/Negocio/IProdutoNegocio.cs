using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.INegocio.Negocio
{
    public interface IProdutoNegocio : IBaseNegocio<ProdutoViewModel, Produto>
    {
        IList<ProdutoViewModel> BuscarAtivos();
        public void EditarPlano(ProdutoViewModel viewModel, Guid IdUsuarioLogado);
        IList<ProdutoViewModel> BuscarTodos();
        IList<LogProduto> ObterHistorico(long id);
        //public void AtivarProduto(int idProduto, Guid idUsuario, decimal valorPedido, bool distribuiNaRede, bool GerarPontos);
        void CriarPlano(ProdutoViewModel viewModel, Guid idUsuarioLogado);
        bool IsPlano(int idProduto);
    }
}
