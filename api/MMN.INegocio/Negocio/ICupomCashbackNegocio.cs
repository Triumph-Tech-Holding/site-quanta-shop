using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using MMN.Integracoes.Afilio;

namespace MMN.INegocio.Negocio
{
    public interface ICupomCashbackNegocio : IBaseNegocioNovo<CupomCashback>
    {
        Task<CupomCashback> CriarCuponAsync(CupomCashback cupom, Guid idUsuarioLogado, bool resgatar = false);
        Task ResgatarCuponAsync(string token, Guid idUsuarioLogado);
        Task<CupomCashback> ObterCuponAsync(string token);
        Task<bool> AprovarReprovarCupomAsync(string token, Guid idUsuarioLogado, bool aprovado, int status, string justificativa, bool informarCliente);
        Task<bool> AprovarReprovarCupomAsync(CupomCashback cupom, bool aprovado, int status);
        Task<object> ObterCuponsCompraUsuarioAdmin(FiltroViewModel.FiltroVendasCredenciando viewModel);
        Task<object> ObterCuponsCompraUsuarioCredenciado(FiltroViewModel.FiltroVendasCredenciando viewModel);
        Task<List<ChaveUrlNFCeViewModel>> ObterChavesDeAcessoNF();
        Task<(bool status, string message)> CriarDadosNF(CupomCashbackDadosNF dadosNF);
        Task<CupomCashback> BuscarPelaChaveDeAcessoAsync(string chaveDeAcesso);
        Task<(bool status, string message)> Atualizar(CupomCashback cupom);
        Task<CupomCashback> CriarCuponCadastroFacilitadoAsync(decimal valorCompra, string comprovante, Usuario idComerciante, Usuario idUsuarioLogado);
        void DeletarRegistrosFalhaProcedure(CupomCashback cupom);
    }
}
