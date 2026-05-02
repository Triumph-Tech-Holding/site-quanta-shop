using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IUsuarioNegocio : IBaseNegocio<UsuarioViewModel, Usuario>
    {
        UsuarioViewModel GetById(Guid idUsuario, params string[] entities);
        Guid RegistrarAdmin(UsuarioAdminViewModel model);
        Usuario CadastroPWA(UsuarioCadastroPWAViewModel model);
        Usuario Registrar(UsuarioCadastroViewModel model);
        Usuario RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model);
        Task<Usuario> RegistrarGoogleAsync(Oauth2CadastroViewModel model);
        UsuarioViewModel Autenticacao(string login, string senha, out Parceiro parceiro, bool verificarSenha = true);
        UsuarioViewModel AutenticacaoGoogle(string code, string redirectUri, out Parceiro parceiro);
        Task<(UsuarioViewModel usuario, Parceiro parceiro)> AutenticacaoGoogleCredentialAsync(string credential);
        Task<(UsuarioViewModel usuario, Parceiro parceiro)> AutenticacaoAppleCredentialAsync(string identityToken, string emailFallback, string fullNameFallback);
        UsuarioViewModel BuscarLoginOuEmail(string login);
        bool AlterarSenha(string idUsuario, string senha, string senhaConfirma);
        List<UsuarioViewModel> ListaUsuarioDiretos(Guid idUsuario);
        List<UsuarioViewModel> ListaUsuariosPatrocinadores(Guid idUsuario, int? nivel);
        List<Lancamento> ObterDistruibuicao(Guid idUsuario);
        LimitesGanhosViewModel BuscarLimitesGanhos(Guid idUsuarioLogado);
        void ValidarContaUsuario(Guid guid);
        bool AtualizarDados(UsuarioViewModel dados);
        object ObterDadosPessoais(Guid idUsuario);
        object ObterFotoPerfil(Guid idUsuario);
        UsuarioViewModel AssinaturaEletronicaAleatoria(Guid idUsuario, string assinaturaEletronica);
        bool VerificarSenha(Guid idUsuario, string senha);
        bool VerificarAssinaturaEletronica(Guid idUsuario, string assinatura);
        bool EditarDadosUsuario(UsuarioEditarViewModel viewModel);
        bool UpdateImage(Guid idUsuario, string image);
        bool UpdateAssinaturaEletronica(Guid idUsuario, string assinaturaEletronica);
        object FiltrarUsuarios(FiltroViewModel.FiltroUsuario filtroUsuario);
        bool AdminAtualizarDadosUsuario(AdminDadosUsuarioViewModel dados);
        bool VerificaLoginDisponivel(UsuarioViewModel usuario, string login);
        bool VerificaPatrocinador(string loginPatrocinador);
        bool BloquearUsuarioAdmin(Guid idUsuario);
        List<UsuarioViewModel> ObterTodosAdministrativos(UsuarioViewModel model);
        UsuarioViewModel GetByLoginOrEmail(string login);
        PontuacaoPorValorViewModel GetPontosFromCache(Guid idUsuario);
        IList<RankUsuarioViewModel> GetRankFromCache(Guid idUsuario);
        List<UsuarioViewModel> ObterUltimosDiretos(Guid idUsuario);
        IList<RankUsuarioViewModel> ObterRankFiltrado(Guid idUsuario, string login, string ordenacao);
        UsuarioViewModel CadastrarUsuarioComPlanoIntegracao(UsuarioViewModel usuario, int idPlano);
        decimal? GetPontosPremiacaoFromCache(Guid idUsuario, int porcentagem, int totalPontos, int idGraduacao);
        BarraDeStatusViewModel GetBarraStatusFromCache(Guid idUsuario);
        Task<RefreshTokenResponseViewModel> GenerateTokenAndRefreshToken(UsuarioViewModel usuario, bool comerciante);
        Task<RefreshTokenResponseViewModel> RefreshTokenAsync(string token, string refreshToken);
        bool ObterDadosPessoaisPorCpfCnpj(string email);
    }
}
