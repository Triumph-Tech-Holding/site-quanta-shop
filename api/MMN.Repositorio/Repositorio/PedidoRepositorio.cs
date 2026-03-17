using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Repositorio.Repositorio
{
    public class PedidoRepositorio : BaseRepositorio<Pedido>, IPedidoRepositorio
    {
        private readonly IProceduresRepositorio _procedures;
        public PedidoRepositorio(
            IProceduresRepositorio procedures,
            DatabaseContext db) : base(db)
        {
            _procedures = procedures;
        }
        public IList<Pedido> ObterPorUsuario(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public void DistribuirParcelaPlano(PagamentoViewModel pagamento, IDbContextTransaction dbTransaction = null)
        {
            _procedures.spc_DistribuicaoParcela(pagamento.IdPedido, pagamento.IdPagamento, dbTransaction);
        }

        public bool ProcessarPagamento(FiltroViewModel.ConfirmarPagamento model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    Pedido pedidoModel = _ctx.Pedido.FirstOrDefault(t => t.Codigo == model.CodigoPedido);

                    if ((pedidoModel.ValorPedido + pedidoModel.ValorTaxa) > model.ValorPago)
                    {
                        throw new PadraoException("pagamento_valor_insuficiente");
                    }

                    Transacao transacaoModel = _ctx.Transacao.FirstOrDefault(t => t.IdTransacao == pedidoModel.IdTransacao);
                    UsuarioProduto usuarioProdutoModel = _ctx.UsuarioProduto.FirstOrDefault(t => t.IdPedido == pedidoModel.IdPedido);

                    pedidoModel.DataPagamento = model.DataPagamento;
                    pedidoModel.ValorPago = model.ValorPago;
                    pedidoModel.Pago = true;
                    transacaoModel.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                    usuarioProdutoModel.Ativo = true;
                    usuarioProdutoModel.DataVinculo = model.DataPagamento;

                    _ctx.SaveChanges();

                    using (var command = _ctx.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "spc_processaPagamento";
                        command.Transaction = transaction.GetDbTransaction();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter() { ParameterName = "@idPedido", Value = pedidoModel.IdPedido });
                        var reader = command.ExecuteReader();
                        reader.Close();
                    }
                    _ctx.Database.CommitTransaction();
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
            return true;
        }

        public Pedido LancarManual(Pedido pedido, int idProduto, bool presente, Guid idUsuarioLogado, bool contabilizarPontuacao = false)
        {
            using (_ctx.Database.BeginTransaction())
            {

                Tipo tipoModel = _ctx.Tipo.FirstOrDefault(t => t.Chave.Equals("CP"));
                Tipo tipoManualModel = _ctx.Tipo.FirstOrDefault(t => t.Chave.Equals("AU"));

                var produtoModel = _ctx.Produto.FirstOrDefault(p => p.IdProduto == idProduto);

                pedido.GeradoManualmente = true;
                pedido.ContabilizarPontuacao = contabilizarPontuacao;
                pedido.ValorPago = pedido.ValorPedido;

                Transacao transacaoModel = new Transacao();
                transacaoModel.Ativo = true;
                transacaoModel.DataTransacao = pedido.DataPedido;
                transacaoModel.Descricao = $"Ativação manual do produto [{produtoModel.Nome}]";
                transacaoModel.IdTipo = tipoModel.IdTipo;
                transacaoModel.IdUsuario = pedido.IdUsuario;
                transacaoModel.ValorPrincipal = pedido.ValorPedido + pedido.ValorTaxa;
                transacaoModel.IdStatus = (int)StatusTransacaoEnum.Finalizada;

                _ctx.Transacao.Add(transacaoModel);
                _ctx.SaveChanges();

                pedido.IdTransacao = transacaoModel.IdTransacao;
                _ctx.Pedido.Add(pedido);
                _ctx.SaveChanges();

                // Caso não seja presente, debitar do saldo
                if (!presente)
                {
                    Lancamento lancamentoManual = new Lancamento();
                    lancamentoManual.Ativo = true;
                    lancamentoManual.DataLancamento = pedido.DataPagamento.Value;
                    lancamentoManual.Descricao = "Débito por ativação manual de pacote.";
                    lancamentoManual.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                    lancamentoManual.IdTipo = tipoManualModel.IdTipo;
                    lancamentoManual.IdUsuario = idUsuarioLogado;
                    lancamentoManual.IdTransacao = transacaoModel.IdTransacao;
                    lancamentoManual.Valor = (pedido.ValorPago.Value * -1);

                    _ctx.Lancamento.Add(lancamentoManual);
                    _ctx.SaveChanges();
                }

                _ctx.Database.CommitTransaction();
            }

            return pedido;
        }

        public Pedido Comprar(Pedido pedido, Int32? idProduto, EnumTipoTransacao? tipoTransacao, Guid idUsuarioLogado)
        {
            using (_ctx.Database.BeginTransaction())
            {
                try
                {
                    var produtoModel = _ctx.Produto.FirstOrDefault(p => p.IdProduto == idProduto);

                    if (pedido.MeioPagamento == (int)EnumTipoPagamento.PGPAGARME || pedido.MeioPagamento == (int)EnumTipoPagamento.PGASAAS)
                    {
                        pedido.Pago = false;

                        var usuario = _ctx.Usuario.FirstOrDefault(u => u.IdUsuario == pedido.IdUsuario);
                        var endereco = _ctx.UsuarioEndereco.FirstOrDefault(e => e.IdUsuario == pedido.IdUsuario);

                        if (endereco == null)
                        {
                            throw new PadraoException("usuario_sem_endereco");
                        }

                        var cidade = _ctx.Cidade.FirstOrDefault(c => c.IdCidade == endereco.IdCidade);

                        if (cidade == null)
                        {
                            throw new PadraoException("usuario_sem_endereco");
                        }

                        var estado = _ctx.Estado.FirstOrDefault(e => e.IdEstado == cidade.IdEstado);

                        if (estado == null)
                        {
                            throw new PadraoException("usuario_sem_endereco");
                        }
                    }

                    Tipo tipoModel = _ctx.Tipo.FirstOrDefault(t =>
                        t.Chave.Equals(tipoTransacao.HasValue ?
                            tipoTransacao.Value.GetDescription() :
                            EnumTipoTransacao.CompraProduto.GetDescription()));

                    var transacaoModel = new Transacao();

                    transacaoModel.Ativo = true;
                    transacaoModel.DataTransacao = pedido.DataPedido;
                    transacaoModel.IdTipo = tipoModel.IdTipo;
                    transacaoModel.IdUsuario = pedido.IdUsuario;
                    transacaoModel.IdStatus = (int)StatusTransacaoEnum.EmProcessamento;
                    transacaoModel.ValorPrincipal = pedido.ValorPedido + pedido.ValorTaxa;

                    switch (pedido.Tipo)
                    {
                        case (int)TipoPedido.Baf:
                            transacaoModel.Descricao = $"Compra do produto [{produtoModel.Nome}]";
                            break;
                        case (int)TipoPedido.Saldo:
                            transacaoModel.Descricao = "Compra de saldo";
                            break;
                        case (int)TipoPedido.FaturaCashbackCredenciado:
                            transacaoModel.Descricao = "Fatura de cashback";
                            break;
                        default:
                            transacaoModel.Descricao = "";
                            break;
                    }

                    if (pedido.Pago)
                    {
                        transacaoModel.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                    }

                    _ctx.Transacao.Add(transacaoModel);
                    _ctx.SaveChanges();

                    pedido.IdTransacao = transacaoModel.IdTransacao;
                    _ctx.Pedido.Add(pedido);
                    _ctx.SaveChanges();

                    if (idProduto != null)
                    {
                        UsuarioProduto usuarioProdutoModel = new UsuarioProduto();
                        usuarioProdutoModel.IdPedido = pedido.IdPedido;
                        usuarioProdutoModel.IdProduto = idProduto.Value;
                        usuarioProdutoModel.IdUsuario = pedido.IdUsuario;
                        usuarioProdutoModel.Ativo = false;
                        usuarioProdutoModel.DataVinculo = pedido.DataPedido;

                        if (pedido.Pago && pedido.Tipo == (int)TipoPedido.Baf)
                        {
                            usuarioProdutoModel.Ativo = true;
                            usuarioProdutoModel.DataVinculo = pedido.DataPagamento.Value;
                        }

                        _ctx.UsuarioProduto.Add(usuarioProdutoModel);
                        _ctx.SaveChanges();
                    }

                    if (pedido.Pago)
                    {
                        var tipoManualModel = _ctx.Tipo.FirstOrDefault(t => t.Chave.Equals("AU"));
                        var lancamentoManual = new Lancamento();

                        lancamentoManual.Ativo = true;
                        lancamentoManual.DataLancamento = pedido.DataPagamento.Value;
                        lancamentoManual.Descricao = "Débito por ativação manual de pacote.";
                        lancamentoManual.IdStatus = (int)StatusTransacaoEnum.Finalizada;
                        lancamentoManual.IdTipo = tipoManualModel.IdTipo;
                        lancamentoManual.IdUsuario = pedido.IdUsuario;
                        lancamentoManual.IdTransacao = transacaoModel.IdTransacao;
                        lancamentoManual.Valor = (pedido.ValorPago.Value * -1);

                        _ctx.Lancamento.Add(lancamentoManual);
                        _ctx.SaveChanges();
                    }

                    _ctx.Database.CommitTransaction();

                    if (pedido.Pago)
                    {
                        ProcessarPagamento(new FiltroViewModel.ConfirmarPagamento()
                        {
                            CodigoPedido = pedido.Codigo,
                            DataPagamento = pedido.DataPagamento.Value,
                            ValorPago = pedido.ValorPago.Value
                        });
                    }
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
            return pedido;
        }

        public Pedido Assinar(Pedido pedido, Int32? idProduto, EnumTipoTransacao? tipoTransacao, Guid idUsuarioLogado)
        {
            using (_ctx.Database.BeginTransaction())
            {
                try
                {
                    var produtoModel = _ctx.Produto.FirstOrDefault(p => p.IdProduto == idProduto);

                    Tipo tipoModel = _ctx.Tipo.FirstOrDefault(t =>
                        t.Chave.Equals(tipoTransacao.HasValue ?
                            tipoTransacao.Value.GetDescription() :
                            EnumTipoTransacao.Assinatura.GetDescription()));

                    var transacaoModel = new Transacao();

                    transacaoModel.Ativo = true;
                    transacaoModel.DataTransacao = pedido.DataPedido;
                    transacaoModel.IdTipo = tipoModel.IdTipo;
                    transacaoModel.IdUsuario = pedido.IdUsuario;
                    transacaoModel.IdStatus = (int)StatusTransacaoEnum.EmProcessamento;
                    transacaoModel.ValorPrincipal = pedido.ValorPedido + pedido.ValorTaxa;

                    switch (pedido.Tipo)
                    {
                        case (int)TipoPedido.Baf:
                            transacaoModel.Descricao = $"Compra do produto [{produtoModel.Nome}]";
                            break;
                        case (int)TipoPedido.Saldo:
                            transacaoModel.Descricao = "Compra de saldo";
                            break;
                        case (int)TipoPedido.FaturaCashbackCredenciado:
                            transacaoModel.Descricao = "Fatura de cashback";
                            break;
                        case (int)TipoPedido.Assinatura:
                            transacaoModel.Descricao = $"Assinatura mensal [{produtoModel.Nome}]";
                            break;
                        default:
                            transacaoModel.Descricao = "";
                            break;
                    }

                    if (pedido.Pago)
                    {
                        transacaoModel.IdStatus = (int)StatusTransacaoEnum.AssinaturaAtiva;
                    }

                    _ctx.Transacao.Add(transacaoModel);
                    _ctx.SaveChanges();

                    pedido.IdTransacao = transacaoModel.IdTransacao;
                    _ctx.Pedido.Add(pedido);
                    _ctx.SaveChanges();                   

                    _ctx.Database.CommitTransaction();
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }
            return pedido;
        }

        public List<PedidosProcedureViewModel> BuscarPedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario)
        {
            return _procedures.spc_Pedidos(filtro, idUsuario);
        }

        public List<Pedido> VerificarPedidoBitCoin()
        {
            return _ctx.Pedido.Where(p => !string.IsNullOrEmpty(p.EnderecoDeposito) && !p.Pago && p.Ativo == true).ToList();
        }

        public Pedido BuscarPorCodigo(string codigo)
        {
            return _ctx.Pedido.Where(p => p.Codigo.Equals(codigo) && p.Ativo == true).FirstOrDefault();
        }

        public object TotalConsumoPlanos()
        {
            var pedidos = _ctx.Pedido
                          .Include("Transacao")
                          .Where(p =>
                            p.Ativo == true &&
                            p.ValorPago != 0 &&
                            !p.IdAwinTransaction.HasValue &&
                            !p.IdVendaZanox.HasValue &&
                            !p.IdVendaAfilio.HasValue)
                          .AsEnumerable();

            var somaPedidos = new
            {
                planoSubsidiada = pedidos.Where(w => w.Transacao?.IdTipo == 49).Sum(s => s.ValorPago),
                planoPresente = pedidos.Where(w => w.Transacao?.IdTipo == 4).Sum(s => s.ValorPago),
                compraPlano = pedidos.Where(w => w.Transacao?.IdTipo == 10).Sum(s => s.ValorPago),
                total = pedidos.Where(w => w.Tipo == (int)TipoPedido.Baf).Sum(s => s.ValorPago)
            };

            //return _ctx.Pedido.Include("Transacao").Where(p => p.Pago && !p.IdVendaZanox.HasValue && !p.IdVendaAfilio.HasValue && p.Transacao.IdStatus == (int)StatusTransacaoEnum.Finalizada).Sum(s => s.ValorPago);
            return somaPedidos;
        }

        public async Task<bool> CriarPedidoCredenciado(EfetuarCompraViewModel viewModel, Guid IdUsuario, CupomCashback token)
        {
            Transacao transacao = null;
            Pedido pedido = null;
            Usuario usuarioComprador = null;

            using (await _ctx.Database.BeginTransactionAsync())
            {
                try
                {                   
                    var comerciante = _ctx.Usuario.Include("Grupo")
                        .FirstOrDefault(c => c.IdUsuario == viewModel.IdComerciante && c.Grupo.Descricao == "Comerciante");

                    if (comerciante == null)
                    {
                        throw new PadraoException("comerciante_nao_encontrado");
                    }

                    var credenciamentoComerciante = _ctx.Credenciamento
                        .FirstOrDefault(c => c.IdUsuario == comerciante.IdUsuario && c.Status == StatusCredenciamento.Aprovado);

                    if (credenciamentoComerciante == null)
                    {
                        throw new PadraoException("comerciante_nao_aprovado");
                    }

                    if (token.CompraUsuario)
                    {
                        usuarioComprador = _ctx.Usuario.FirstOrDefault(u => u.Documento == token.Documento);
                    }

                    var tipoParaTransacao = _ctx.Tipo.FirstOrDefault(t => t.Chave == "CHBLF");
                    var percentualCashback = token?.PercentualCashback * 100 ?? credenciamentoComerciante.PercentualCashback;

                    pedido = new Pedido
                    {
                        IdUsuario = token.CompraUsuario? usuarioComprador.IdUsuario : IdUsuario,
                        IdUsuarioComerciante = viewModel.IdComerciante,
                        Tipo = (int)TipoPedido.CashbackCredenciado,
                        Status = (int)StatusPedido.AguardandoFaturaCredenciado,
                        DataPedido = DateTime.UtcNow.HorarioBrasilia(),
                        ValorTaxa = 0,
                        ValorPedido = viewModel.Valor,
                        ValorPago = viewModel.Valor,
                        DataPagamento = DateTime.UtcNow.HorarioBrasilia(),
                        Pago = true,
                        Ativo = true,
                        MeioPagamento = (int)viewModel.TipoPagamento,
                        Quantidade = 1,
                        PercentualCashback = percentualCashback,
                        Cashback = decimal.Ceiling(viewModel.Valor * percentualCashback.Value) / 100
                    };

                    _ctx.Pedido.Add(pedido);

                    transacao = new Transacao
                    {
                        IdUsuario = token.CompraUsuario ? usuarioComprador.IdUsuario : IdUsuario,
                        IdTipo = tipoParaTransacao.IdTipo,
                        ValorPrincipal = viewModel.Valor * percentualCashback.Value / 100,
                        DataTransacao = DateTime.UtcNow.HorarioBrasilia(),
                        Descricao = viewModel.Descricao ?? string.Empty,
                        IdStatus = (int)StatusTransacaoEnum.AguardandoPagamentoFatura,
                        Ativo = true,
                        ComissaoTotal = viewModel.Valor * percentualCashback / 100
                    };

                    _ctx.Transacao.Add(transacao);
                    await _ctx.SaveChangesAsync();

                    pedido.IdTransacao = transacao.IdTransacao;

                    await _ctx.SaveChangesAsync();

                    var idPedido = pedido.IdPedido;

                    if (token != null)
                    {
                        _ctx.CuponCashbackPedido.Add(new CuponCashbackPedido()
                        {
                            IdPedido = pedido.IdPedido,
                            IdCuponCashback = token.IdCuponCashback,
                        });

                        await _ctx.SaveChangesAsync();
                    }

                    _ctx.Database.CommitTransaction();
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> EfetuarCompraComerciante(EfetuarCompraViewModel viewModel, Guid IdUsuario, CupomCashback token = null)
        {
            Transacao transacao = null;

            using (await _ctx.Database.BeginTransactionAsync())
            {
                try
                {
                    var comerciante = _ctx.Usuario.Include("Grupo")
                        .FirstOrDefault(c => c.IdUsuario == viewModel.IdComerciante && c.Grupo.Descricao == "Comerciante");

                    if (comerciante == null)
                    {
                        throw new PadraoException("comerciante_nao_encontrado");
                    }

                    var credenciamentoComerciante = _ctx.Credenciamento
                        .FirstOrDefault(c => c.IdUsuario == comerciante.IdUsuario && c.Status == StatusCredenciamento.Aprovado);

                    if (credenciamentoComerciante == null)
                    {
                        throw new PadraoException("comerciante_nao_aprovado");
                    }


                    var tipoParaTransacao = _ctx.Tipo.FirstOrDefault(t => t.Chave == "CHBLF");

                    transacao = new Transacao
                    {
                        IdUsuario = IdUsuario,
                        IdTipo = tipoParaTransacao.IdTipo,
                        ValorPrincipal = viewModel.Valor,
                        DataTransacao = DateTime.UtcNow,
                        Descricao = viewModel.Descricao,
                        IdStatus = (int)StatusTransacaoEnum.AguardandoPagamentoFatura,
                        Ativo = true
                    };

                    _ctx.Transacao.Add(transacao);
                    await _ctx.SaveChangesAsync();

                    Pedido pedido = new Pedido()
                    {
                        IdUsuario = IdUsuario,
                        IdUsuarioComerciante = viewModel.IdComerciante,
                        IdTransacao = transacao.IdTransacao,
                        DataPedido = DateTime.UtcNow,
                        ValorTaxa = 0,
                        ValorPedido = viewModel.Valor,
                        ValorPago = viewModel.Valor,
                        DataPagamento = DateTime.UtcNow,
                        Pago = true,
                        Ativo = true,
                        MeioPagamento = (int)viewModel.TipoPagamento,
                        Quantidade = 1,
                        PercentualCashback = credenciamentoComerciante.PercentualCashback,
                        Cashback = decimal.Ceiling(viewModel.Valor * credenciamentoComerciante.PercentualCashback.Value) / 100
                    };

                    var buscarPedido = _ctx.Pedido.FirstOrDefault(f => f.DataPedido >= pedido.DataPedido.Date && f.DataPedido < pedido.DataPedido.Date.AddDays(1) && f.ValorPedido == pedido.ValorPedido && f.IdUsuario == pedido.IdUsuario && f.IdUsuarioComerciante == pedido.IdUsuarioComerciante);

                    if (buscarPedido != null)
                    {
                        throw new PadraoException("pedido_duplicado");
                    }

                    _ctx.Pedido.Add(pedido);
                    await _ctx.SaveChangesAsync();


                    if (token != null)
                    {
                        _ctx.CuponCashbackPedido.Add(new CuponCashbackPedido()
                        {
                            IdPedido = pedido.IdPedido,
                            Pedido = pedido,
                            CuponCashback = token,
                            IdCuponCashback = token.IdCuponCashback,
                        });
                    }

                    _ctx.Database.CommitTransaction();
                }
                catch
                {
                    _ctx.Database.RollbackTransaction();
                    throw;
                }
            }

            return true;
        }

        public Pedido ObterPedidoCompleto(string code)
        {
            var pedido = _ctx.Pedido
                .Include(p => p.Pagamentos)
                .Include(p => p.PedidoDetalhe)
                .FirstOrDefault(p => p.Codigo == code);

            pedido.UsuarioProduto = _ctx.UsuarioProduto.Include(u => u.Produto).Where(p => p.IdPedido == pedido.IdPedido).ToList();
            return pedido;
        }

        public void Delete(long key)
        {
            var entity = _ctx.Pedido.FirstOrDefault(ccp => ccp.IdPedido== key);
            _ctx.Remove(entity);
        }
    }
}
