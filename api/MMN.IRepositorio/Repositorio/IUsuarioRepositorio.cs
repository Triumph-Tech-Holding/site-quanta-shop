using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Base;

namespace MMN.IRepositorio.Repositorio
{
    public interface IUsuarioRepositorio : IBaseRepositorio<Usuario>
    {
        Usuario GetById(Guid idUsuario, params string[] entities);

        IQueryable<Usuario> GetPerPage(Expression<Func<Usuario, bool>> predicate, params string[] entities);
        string GetByGuid(Guid idUsuario);
        Usuario GetByLoginOrEmail(string loginPatrocinador);
        void Create(Usuario usuarioModel, AutenticacaoExterna autenticacaoExterna);
        bool AlterarSenha(Guid idUsuario, string senha);
        List<Usuario> ListaUsuarioDiretos(Guid idUsuario);
        List<Usuario> ListaUsuariosPatrocinadores(Guid idUsuario, int? nivel);
        void AtivarConta(Guid idUsuario);
        bool AtualizarDados(UsuarioViewModel dados);
        bool AdminAtualizarDadosUsuario(AdminDadosUsuarioViewModel dados);
        bool VerificaLoginDisponivel(Usuario usuario, string login);
        bool VerificaPatrocinador(string loginPatrocinador);
        bool BloquearUsuarioAdmin(Guid idUsuario);
        List<Usuario> ObterTodosAdministrativos(UsuarioViewModel model);
        List<Usuario> ObterUltimosDiretos(Guid idUsuario);
        List<Lancamento> ObterUltimosLancamentosPendentes(Guid idUsuario);
        Usuario CadastrarUsuarioComPlanoIntegracao(Usuario usuario, int idPlano);
    }
}
