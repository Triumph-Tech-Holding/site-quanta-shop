using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Data;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioProdutoRepositorio : BaseRepositorio<UsuarioProduto>, IUsuarioProdutoRepositorio
    {
        public UsuarioProdutoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public UsuarioProduto BuscarPorPedido(long idPedido)
        {
            return _ctx.UsuarioProduto.Include("Produto").FirstOrDefault(p => p.IdPedido == idPedido);
        }

        public UsuarioProduto BuscarProdutoAtivo(Guid idUsuario)
        {
            return GetNoTracking(p => p.IdUsuario == idUsuario && p.Ativo, "Pedido", "Produto").OrderByDescending(p => p.Produto.Pontos).FirstOrDefault();
            //return _ctx.UsuarioProduto.Include("Pedido").Include("Produto").OrderByDescending(p => p.Produto.Pontos).FirstOrDefault(p => p.IdUsuario == idUsuario && p.Ativo && p.Pedido.DataPagamento.HasValue);
        }

        public UsuarioProduto BuscarAssinaturaAtiva(Guid idUsuario)
        {
            return GetNoTracking(p => p.IdUsuario == idUsuario &&  p.Ativo && p.AssinaturaHabilitada == true, "Pedido", "Produto").OrderByDescending(p => p.Produto.Pontos).FirstOrDefault();
            //return _ctx.UsuarioProduto.Include("Pedido").Include("Produto").OrderByDescending(p => p.Produto.Pontos).FirstOrDefault(p => p.IdUsuario == idUsuario && p.Ativo && p.Pedido.DataPagamento.HasValue);
        }

        public bool InserirPlanoManual(Usuario usuario, Produto plano, int planoAtivoId)
        {
            using (_ctx.Database.BeginTransaction())
            {
                try
                {

                    var tipo = _ctx.Tipo.First(t => t.Chave == "PLCD" && t.Ativo);
                    var transacao = new Transacao
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdTipo = tipo.IdTipo,
                        ValorPrincipal = plano.Valor * -1,
                        DataTransacao = DateTime.UtcNow.HorarioBrasilia(),
                        Descricao = "Ativação manual",
                        IdStatus = (int)StatusTransacaoEnum.Finalizada,
                        Ativo = true
                    };

                    _ctx.Transacao.Add(transacao);
                    _ctx.SaveChanges();

                    _ctx.Lancamento.Add(new Lancamento
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdTransacao = transacao.IdTransacao,
                        IdTipo = tipo.IdTipo,
                        Valor = plano.Valor * -1,
                        DataLancamento = DateTime.UtcNow.HorarioBrasilia(),
                        Ativo = true,
                        Descricao = "Ativação manual",
                        IdStatus = (int)StatusTransacaoEnum.Finalizada
                    });

                    var pedido = new Pedido
                    {
                        IdTransacao = transacao.IdTransacao,
                        IdUsuario = usuario.IdUsuario,
                        DataPedido = DateTime.UtcNow.HorarioBrasilia(),
                        ValorPedido = plano.Valor,
                        ValorPago = plano.Valor,
                        DataPagamento = DateTime.UtcNow.HorarioBrasilia(),
                        Pago = true,
                        Ativo = true,
                        Quantidade = 1
                    };

                    _ctx.Pedido.Add(pedido);
                    _ctx.SaveChanges();

                    var planoAtivo = _ctx.UsuarioProduto.First(up => up.IdUsuarioProduto == planoAtivoId);
                    planoAtivo.Ativo = false;
                    _ctx.UsuarioProduto.Update(planoAtivo);
                    _ctx.SaveChanges();

                    _ctx.UsuarioProduto.Add(new UsuarioProduto
                    {
                        IdProduto = plano.IdProduto,
                        IdUsuario = usuario.IdUsuario,
                        Ativo = true,
                        IdPedido = pedido.IdPedido,
                        DataVinculo = DateTime.UtcNow.HorarioBrasilia()
                    });
                    _ctx.SaveChanges();

                    _ctx.Database.CommitTransaction();
                    return true;
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
        }

        public bool InserirPlanoPresente(Usuario usuario, Produto plano, int planoAtivoId)
        {
            using (_ctx.Database.BeginTransaction())
            {
                try
                {
                    var tipo = _ctx.Tipo.First(t => t.Chave == "LM" && t.Ativo);
                    var transacao = new Transacao
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdTipo = tipo.IdTipo,
                        ValorPrincipal = 0,
                        DataTransacao = DateTime.UtcNow.HorarioBrasilia(),
                        Descricao = "Ativação presente",
                        IdStatus = (int)StatusTransacaoEnum.Finalizada,
                        Ativo = true
                    };

                    _ctx.Transacao.Add(transacao);
                    _ctx.SaveChanges();

                    _ctx.Lancamento.Add(new Lancamento
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdTransacao = transacao.IdTransacao,
                        IdTipo = tipo.IdTipo,
                        Valor = 0,
                        DataLancamento = DateTime.UtcNow.HorarioBrasilia(),
                        Ativo = true,
                        Descricao = "Ativação presente",
                        IdStatus = (int)StatusTransacaoEnum.Finalizada
                    });

                    var pedido = new Pedido
                    {
                        IdTransacao = transacao.IdTransacao,
                        IdUsuario = usuario.IdUsuario,
                        DataPedido = DateTime.UtcNow.HorarioBrasilia(),
                        ValorPedido = 0,
                        ValorPago = 0,
                        DataPagamento = DateTime.UtcNow.HorarioBrasilia(),
                        Pago = true,
                        Ativo = true,
                        Quantidade = 1
                    };

                    _ctx.Pedido.Add(pedido);
                    _ctx.SaveChanges();

                    var planoAtivo = _ctx.UsuarioProduto.First(up => up.IdUsuarioProduto == planoAtivoId);
                    planoAtivo.Ativo = false;
                    _ctx.UsuarioProduto.Update(planoAtivo);
                    _ctx.SaveChanges();

                    _ctx.UsuarioProduto.Add(new UsuarioProduto
                    {
                        IdProduto = plano.IdProduto,
                        IdUsuario = usuario.IdUsuario,
                        Ativo = true,
                        IdPedido = pedido.IdPedido,
                        DataVinculo = DateTime.UtcNow.HorarioBrasilia()
                    });
                    _ctx.SaveChanges();

                    _ctx.Database.CommitTransaction();
                    return true;
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
