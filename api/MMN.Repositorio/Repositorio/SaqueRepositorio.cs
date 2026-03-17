using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class SaqueRepositorio : BaseRepositorio<Saque>, ISaqueRepositorio
    {
        private readonly ITipoRepositorio _tipoRepositorio;

        public SaqueRepositorio(
            ITipoRepositorio tipoRepositorio,
            DatabaseContext ctx) : base(ctx)
        {
            _tipoRepositorio = tipoRepositorio;
        }

        public Saque InserirSaque(Saque model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var tipo = _tipoRepositorio.First(t => t.Chave == "SQ");
                    var transacao = new Transacao
                    {
                        IdUsuario = model.IdUsuario,
                        IdTipo = tipo.IdTipo,
                        ValorPrincipal = model.Valor * -1,
                        DataTransacao = DateTime.UtcNow.HorarioBrasilia(),
                        Descricao = "Saque",
                        IdStatus = 4,
                        Ativo = true
                    };

                    _ctx.Add(transacao);
                    _ctx.SaveChanges();

                    model.IdTransacao = transacao.IdTransacao;
                    model.DataSolicitacao = transacao.DataTransacao;
                    model.IdStatus = 4;
                    model.IdTipo = tipo.IdTipo;

                    _ctx.Add(model);
                    _ctx.SaveChanges();

                    _ctx.Add(new Lancamento
                    {
                        IdTransacao = transacao.IdTransacao,
                        IdTipo = tipo.IdTipo,
                        IdUsuario = model.IdUsuario,
                        Valor = model.Valor * -1,
                        DataLancamento = transacao.DataTransacao,
                        Ativo = true,
                        Descricao = "Saque",
                        IdStatus = 4
                    });

                    _ctx.SaveChanges();

                    _ctx.Database.CommitTransaction();

                    return model;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public List<Saque> BuscarPagamentos(FiltroViewModel.FiltroSaque model, out int totalPages)
        {
            var saques = Get(s => (s.DataSolicitacao >= model.DataInicio || model.DataInicio.HasValue == false)
                             && (s.DataSolicitacao <= model.DataFim || model.DataFim.HasValue == false)
                             && s.Usuario.Login.Contains(model.Login)
                             && (s.IdStatus == model.IdStatus || model.IdStatus == 0), "Tipo", "Status", "Usuario");

            totalPages = (int)Math.Ceiling((double)saques.Count() / model.Quantidade);

            return saques.Skip(model.Quantidade * (model.Page - 1)).Take(model.Quantidade).ToList();
        }

        public void CancelarSaque(List<SaqueViewModel> lista)
        {
            using (_ctx.Database.BeginTransaction())
            {
                var data = DateTime.Now.HorarioBrasilia();
                foreach (var item in lista)
                {
                    var tipoEstorno = _ctx.Tipo.FirstOrDefault(t => t.Chave.Equals("EST"));

                    var saque = GetById(item.IdSaque);
                    saque.IdStatus = (int)StatusTransacaoEnum.Cancelada;
                    saque.Historico += $"Saque cancelado pelo sistema dia {data:dd/MM/yyyy HH:mm}. Valor foi devolvido ao cliente.<br/><br/>";
                    Update(saque);
                    _ctx.SaveChanges();
                    var lancamentosaque = _ctx.Lancamento.FirstOrDefault(l => l.IdTransacao == saque.IdTransacao);
                    var lancamentoEstorno = new Lancamento();
                    lancamentoEstorno.Ativo = true;
                    lancamentoEstorno.DataLancamento = data;
                    lancamentoEstorno.Descricao = "Estorno de saque. Motivo: Cancelamento";
                    lancamentoEstorno.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                    lancamentoEstorno.IdTipo = tipoEstorno.IdTipo;
                    lancamentoEstorno.IdTransacao = lancamentosaque.IdTransacao;
                    lancamentoEstorno.IdUsuario = lancamentosaque.IdUsuario;
                    lancamentoEstorno.Valor = lancamentosaque.Valor * -1;
                    _ctx.Lancamento.Add(lancamentoEstorno);
                    _ctx.SaveChanges();
                }
                _ctx.Database.CommitTransaction();
            }
        }

    }
}