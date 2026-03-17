using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;

namespace MMN.Repositorio.Repositorio
{
    public class LancamentoRepositorio : BaseRepositorio<Lancamento>, ILancamentoRepositorio
    {
        public LancamentoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Lancamento> BuscarPorIdUsuario(FiltroViewModel.BuscarExtrato model, Guid idUsuario)
        {
            DateTime? dtInicio = null;
            DateTime? dtFim = null;

            if (!string.IsNullOrEmpty(model.DataInicio))
                dtInicio = Convert.ToDateTime(model.DataInicio);

            if (!string.IsNullOrEmpty(model.DataFim))
                dtFim = Convert.ToDateTime(model.DataFim.Split("T")[0] + "T23:59:59.000");

            var lanc = Get(l => l.IdUsuario == idUsuario
                            && l.Ativo
                            && !l.Bloqueado
                            && (!dtInicio.HasValue || l.DataLancamento >= dtInicio)
                            && (!dtFim.HasValue || l.DataLancamento <= dtFim)
                            && (string.IsNullOrEmpty(model.Chave) || l.Tipo.Chave == model.Chave), "Tipo", "Transacao", "LancamentoRetido").OrderByDescending(l => l.DataLancamento).ToList();

            if (lanc.Sum(s => s.Valor) < 0)
                lanc = lanc.Where(w => !w.Tipo.Chave.Equals("PLCD")).ToList();

            return lanc;
        }

        public void GerarLancamentoManual(UsuarioViewModel usuario, LancamentoManualViewModel viewModel, UsuarioViewModel usuarioLogado, List<UsuarioViewModel> listUsuariosDiretos)
        {
            using (_ctx.Database.BeginTransaction())
            {
                try
                {
                    var data = DateTime.UtcNow.HorarioBrasilia();
                    var tipoLancamento = _ctx.Tipo.FirstOrDefault(t => t.Chave.Equals("LM"));
                    var userBigCash = _ctx.Usuario.FirstOrDefault(t => t.Login.Equals("triumph"));

                    #region Transação
                    Transacao transacao = new Transacao
                    {
                        Ativo = true,
                        DataTransacao = data,
                        Descricao = $"Lançamento manual de {usuarioLogado.Login} - {viewModel.Descricao}",
                        IdTipo = tipoLancamento.IdTipo,
                        IdUsuario = usuario.IdUsuario,
                        IdStatus = (int)StatusTransacaoEnum.Finalizada,
                        ValorPrincipal = viewModel.Valor
                    };
                    _ctx.Transacao.Add(transacao);
                    _ctx.SaveChanges();
                    #endregion Transação

                    var valorTotalDistribuido = viewModel.MaximoDistribuido > 0 ? (viewModel.MaximoDistribuido / 100) * viewModel.Valor : viewModel.MaximoDistribuido;

                    if (viewModel.DistribuirRede)
                    {
                        int i = 0;
                        var totalValorNiveis = 0m;
                        foreach (var item in listUsuariosDiretos)
                        {
                            i++;
                            var valorPorNivel = 0m;
                            switch (i)
                            {
                                case 1:
                                    valorPorNivel = 5m / 100m * valorTotalDistribuido;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                case 10:
                                    valorPorNivel = 0.5m / 100m * valorTotalDistribuido;
                                    break;
                                case 11:
                                case 12:
                                    valorPorNivel = 0.25m / 100m * valorTotalDistribuido;
                                    break;
                                default:
                                    break;
                            }

                            totalValorNiveis += valorPorNivel;

                            Lancamento lancamentoDistribuido = new Lancamento
                            {
                                Ativo = true,
                                DataLancamento = data,
                                Descricao = $"Distribuição para a rede. (Lançamento manual). Usuário: {usuario.Login} - {viewModel.Descricao}",
                                IdStatus = (int)StatusTransacaoEnum.Finalizada,
                                IdTipo = tipoLancamento.IdTipo,
                                IdUsuario = userBigCash.IdUsuario,
                                IdTransacao = transacao.IdTransacao,
                                Valor = valorPorNivel
                            };

                            _ctx.Lancamento.Add(lancamentoDistribuido);

                        }

                        if (viewModel.DistribuirRede && viewModel.MaximoDistribuido > 0)
                        {
                            Lancamento lancamentoResidual = new Lancamento
                            {
                                Ativo = true,
                                DataLancamento = data,
                                Descricao = $"Valor residual do lançamento manual. Usuário: {usuario.Login} - {viewModel.Descricao}",
                                IdStatus = (int)StatusTransacaoEnum.Finalizada,
                                IdTipo = tipoLancamento.IdTipo,
                                IdUsuario = userBigCash.IdUsuario,
                                IdTransacao = transacao.IdTransacao,
                                Valor = valorTotalDistribuido - totalValorNiveis
                            };
                            _ctx.Lancamento.Add(lancamentoResidual);
                        }
                    }

                    var valorLancamentoManual = viewModel.DistribuirRede && viewModel.MaximoDistribuido > 0 ? viewModel.Valor - valorTotalDistribuido : viewModel.Valor;
                    Lancamento lancamento = new Lancamento
                    {
                        Ativo = true,
                        DataLancamento = data,
                        Descricao = $"Lançamento manual de {usuarioLogado.Login} - {viewModel.Descricao}",
                        IdStatus = (int)StatusTransacaoEnum.Finalizada,
                        IdTipo = tipoLancamento.IdTipo,
                        IdUsuario = usuario.IdUsuario,
                        IdTransacao = transacao.IdTransacao,
                        Valor = valorLancamentoManual
                    };
                    _ctx.Lancamento.Add(lancamento);


                    _ctx.SaveChanges();
                    _ctx.Database.CommitTransaction();
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
        }

        public decimal ValorCashback(Guid idUsuario, int[] tipoCashback)
        {
            return _ctx.Lancamento.Where(w => tipoCashback.Contains(w.IdTipo) && w.IdUsuario == idUsuario).Sum(s => s.Valor);
        }

        public decimal ValorAdesao(Guid idUsuario, int tipoAdesao)
        {
            return _ctx.Lancamento.Where(w => w.IdTipo == tipoAdesao && w.IdUsuario == idUsuario).Sum(s => s.Valor);
        }

        public decimal TotalEntradas(Guid idUsuario)
        {
            return _ctx.Lancamento.Where(w => w.Ativo && w.Valor > 0 && w.IdUsuario == idUsuario).Sum(s => s.Valor);
        }

        public decimal TotalSaidas(Guid idUsuario)
        {
            return _ctx.Lancamento.Where(w => w.Ativo && w.IdUsuario == idUsuario && w.Valor < 0).Sum(s => s.Valor);
        }
    }
}
