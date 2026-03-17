using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Extensions;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IQueryable<Usuario> GetPerPage(Expression<Func<Usuario, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<Usuario>().Where(predicate);
            foreach (var include in entities)
                query = query.Include(include);
            return query;
        }

        public Usuario GetById(Guid idUsuario, params string[] entities)
        {
            var ct = _ctx.Set<Usuario>().AsQueryable().AsNoTracking();
            foreach (var item in entities)
            {
                ct = ct.Include(item);
            }
            return ct.FirstOrDefault(u => u.IdUsuario == idUsuario);
        }
        public string GetByGuid(Guid idUsuario)
        {
            return _ctx.Set<Usuario>().FirstOrDefault(u => u.IdUsuario == idUsuario)?.Nome;
        }

        public Usuario GetByLoginOrEmail(string login)
        {
            var user = _ctx.Set<Usuario>().FirstOrDefault(u => (u.Login.Equals(login) || u.Email.Equals(login))); //&& u.Master == false);
            return user;
        }

        public void Create(Usuario usuarioModel, AutenticacaoExterna autenticacaoExterna)
        {
            _ctx.Usuario.Add(usuarioModel);

            var data = DateTime.UtcNow.HorarioBrasilia();

            var produto = _ctx.Produto.FirstOrDefault(w => w.IdProduto == 1);
            var pedido = new Pedido
            {
                DataPedido = data,
                ValorTaxa = 0,
                ValorPedido = produto.Valor,
                ValorPago = produto.Valor,
                DataPagamento = data,
                Pago = true,
                Ativo = true,
                Quantidade = 1,
                Cotacao = 0,
                MeioPagamento = 0,
                Usuario = usuarioModel,
                Tipo = (int)TipoPedido.Cadastro,
                Status = (int)StatusPedido.Processado
            };
            _ctx.Pedido.Add(pedido);

            var usuarioProduto = new UsuarioProduto
            {
                IdProduto = produto.IdProduto,
                Pedido = pedido,
                DataVinculo = data,
                Ativo = true,
                Usuario = usuarioModel
            };
            _ctx.UsuarioProduto.Add(usuarioProduto);

            if (autenticacaoExterna != null)
            {
                autenticacaoExterna.Usuario = usuarioModel;
                _ctx.AutenticacaoExterna.Add(autenticacaoExterna);
            }
        }

        public bool AlterarSenha(Guid idUsuario, string senha)
        {
            var usuario = _ctx.Set<Usuario>().FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null) return false;

            usuario.Bloqueado = false;
            usuario.TentativasIncorretas = 0;
            usuario.Senha = Hash.Get_HASH_SHA512(senha, usuario.Email, usuario.SaltKey);
            _ctx.SaveChanges();

            return true;
        }

        public List<Usuario> ListaUsuarioDiretos(Guid idUsuario)
        {
            var usuarios = _ctx.Usuario
                .Where(u => u.IdUsuarioPai == idUsuario)
                .Include(u => u.Graduacao)
                .Include(u => u.Filhos)
                .Include(u => u.UsuarioProduto)
                    .ThenInclude(p => p.Produto)
                .ToList();
            return usuarios;
        }

        public List<Usuario> ListaUsuariosPatrocinadores(Guid idUsuario, int? nivel = 0)
        {
            var listPais = new List<Usuario>();
            Guid idUsuarioDesconsiderar = idUsuario;
            for (int i = 0; i <= nivel; i++)
            {
                var usuario = _ctx.Usuario.Include(u => u.UsuarioPai).Include(u => u.Graduacao).FirstOrDefault(u => u.IdUsuario == idUsuario);
                if (!usuario.IdUsuarioPai.HasValue)
                    break;
                if (idUsuarioDesconsiderar != usuario.IdUsuario)
                    listPais.Add(usuario);
                idUsuario = usuario.IdUsuarioPai.Value;
            }
            return listPais;
        }

        public void AtivarConta(Guid idUsuario)
        {
            var objUser = GetById(idUsuario);
            objUser.Ativo = true;
            objUser.EmailConfirmado = true;
            _ctx.Update(objUser);
            _ctx.SaveChanges();
        }

        public bool AtualizarDados(UsuarioViewModel dados)
        {
            try
            {
                var usuario = GetById(dados.IdUsuario);

                if (!string.IsNullOrEmpty(dados.Senha))
                    usuario.Senha = Hash.Get_HASH_SHA512(dados.Senha, usuario.Email, usuario.SaltKey);

                if (!string.IsNullOrEmpty(dados.UrlImg))
                    usuario.UrlImg = dados.UrlImg;

                usuario.Nome = dados.Nome;
                usuario.Celular = dados.Celular;

                if (!string.IsNullOrEmpty(dados.AssinaturaEletronica))
                    usuario.AssinaturaEletronica = Hash.Get_HASH_SHA512(dados.AssinaturaEletronica, usuario.Email, usuario.SaltKey);

                _ctx.Update(usuario);
                _ctx.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException(" AtualizarDados(UsuarioCadastroCompletoViewModel dados)", e, "UsuarioRepositorio");
                return false;
            }

        }

        public bool AdminAtualizarDadosUsuario(AdminDadosUsuarioViewModel dados)
        {
            try
            {
                var usuario = _ctx.Set<Usuario>().FirstOrDefault(u => u.IdUsuario == dados.IdUsuario);

                if (usuario == null) return false;

                usuario.Login = dados.Login;
                usuario.Email = dados.Email;

                if (!string.IsNullOrEmpty(dados.Senha))
                    usuario.Senha = Hash.Get_HASH_SHA512(dados.Senha, dados.Email, usuario.SaltKey);

                usuario.IdUsuarioPai = _ctx.Usuario.FirstOrDefault(u => u.Login == dados.LoginPatrocinador)?.IdUsuario;
                usuario.Celular = dados.Celular;
                usuario.Nome = dados.Nome;

                _ctx.SaveChanges();
                _ctx.Dispose();

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException("AdminAtualizarDadosUsuario(AdminDadosUsuarioViewModel dados)", e, "UsuarioRepositorio");
                return false;
            }

        }

        public bool VerificaLoginDisponivel(Usuario usuario, string login)
        {
            try
            {
                var ret = !_ctx.Usuario.Any(u => u.Login == login && usuario.Login != login);
                return ret;
            }
            catch (Exception e)
            {
                LogHelper.LogException("VerificaLoginDisponivel(Usuario usuario, string login)", e, "UsuarioRepositorio");
                return false;
            }
        }

        public bool VerificaPatrocinador(string loginPatrocinador)
        {
            try
            {
                var ret = _ctx.Usuario.Any(u => u.Login == loginPatrocinador);
                return ret;
            }
            catch (Exception e)
            {
                LogHelper.LogException("VerificaPatrocinador(string loginPatrocinador)", e, "UsuarioRepositorio");
                return false;
            }
        }

        public bool BloquearUsuarioAdmin(Guid idUsuario)
        {
            try
            {
                var user = _ctx.Usuario.FirstOrDefault(u => u.IdUsuario == idUsuario);

                if (user.Bloqueado)
                    user.Bloqueado = false;
                else
                    user.Bloqueado = true;

                user.TentativasIncorretas = 0;

                _ctx.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Usuario> ObterTodosAdministrativos(UsuarioViewModel model)
        {
            return _ctx.Usuario.Where(u => u.Master == false
                                        && (u.Login.Contains(model.Login) || string.IsNullOrEmpty(model.Login))
                                        && (u.Email.Contains(model.Email) || string.IsNullOrEmpty(model.Email))
                                        && u.Ativo == model.Ativo
                                        ).Include(u => u.Grupo).ToList();
        }

        public List<Lancamento> ObterUltimosLancamentosPendentes(Guid idUsuario)
        {
            var lancamentos = _ctx.Lancamento
                .Where(l =>
                    l.IdUsuario == idUsuario &&
                    l.Bloqueado == true &&
                    l.Ativo == true &&
                    l.DataLancamento.AddDays(60) > DateTime.Now).Include(x => x.Usuario);

            return lancamentos.ToList();
        }

        public List<Usuario> ObterUltimosDiretos(Guid idUsuario)
        {
            return _ctx.Usuario.Where(u => u.IdUsuarioPai == idUsuario).Include(u => u.Graduacao).OrderByDescending(o => o.DataCadastro).Take(10).ToList();
        }

        public Usuario CadastrarUsuarioComPlanoIntegracao(Usuario usuario, int idPlano)
        {

            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Usuario.Add(usuario);
                    _ctx.SaveChanges();

                    var data = DateTime.UtcNow.HorarioBrasilia();

                    var produto = _ctx.Produto.FirstOrDefault(w => w.IdProduto == idPlano);
                    var pedido = new Pedido
                    {
                        IdUsuario = usuario.IdUsuario,
                        DataPedido = data,
                        ValorTaxa = 0,
                        ValorPedido = produto.Valor,
                        ValorPago = produto.Valor,
                        DataPagamento = data,
                        Pago = true,
                        Ativo = true,
                        Quantidade = 1,
                        Cotacao = 0,
                        MeioPagamento = 0
                    };
                    _ctx.Pedido.Add(pedido);
                    _ctx.SaveChanges();

                    var usuarioProduto = new UsuarioProduto
                    {
                        IdProduto = produto.IdProduto,
                        IdUsuario = usuario.IdUsuario,
                        IdPedido = pedido.IdPedido,
                        DataVinculo = data,
                        Ativo = true
                    };
                    _ctx.UsuarioProduto.Add(usuarioProduto);
                    _ctx.SaveChanges();

                    _ctx.Database.CommitTransaction();

                    return usuario;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
