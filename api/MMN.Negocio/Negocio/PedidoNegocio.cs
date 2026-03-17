using AutoMapper;
using MMN.Dominio.Enum;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MMN.Dominio.Excecao;
using MMN.Util.Model;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MMN.Repositorio.Contexto;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MMN.Integracoes.Afilio;
using MMN.Util.Model.LionBit;
using Microsoft.VisualBasic;
using MundiAPI.PCL.Models;
using Mapster;
using MMN.Util.Util;
using Newtonsoft.Json.Linq;

namespace MMN.Negocio.Negocio
{
    public class PedidoNegocio : BaseNegocio<PedidoViewModel, Pedido>, IPedidoNegocio
    {
        private readonly AppSettings _appSettings;
        private readonly ICache _cache;
        private readonly ICashbackNegocio _cashbackNegocio;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly DatabaseContext _context;
        private readonly ICuponCashbackPedidoRepositorio _cuponCashbackPedidoRepositorio;
        private readonly ICuponCashbackRepositorio _cuponCashbackRepositorio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly ILocation _location;
        private readonly IMapper _mapper;
        private readonly IPagamentoNegocio _pagamentoNegocio;
        private readonly IPagamentoPedidoRepositorio _pagamentoPedidoRepositorio;
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IPedidoDetalheRepositorio _pedidoDetalheRepositorio;
        private readonly IProceduresRepositorio _procedures;
        private readonly IProceduresRepositorio _proceduresRepository;
        private readonly IProdutoNegocio _produtoNegocio;
        private readonly IPedidoRepositorio _repositorio;
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStatusNegocio _statusNegocio;
        private readonly ITransacaoNegocio _transacaoNegocio;
        private readonly ITransacaoRepositorio _transacaoRepositorio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IUsuarioProdutoRepositorio _usuarioProdutoRepositorio;
        private readonly IUsuarioEnderecoNegocio _usuarioEnderecoNegocio;
        public PedidoNegocio(
            IOptions<AppSettings> appSettings,
            IPedidoRepositorio repositorio,
            IPagamentoRepositorio pagamentoRepositorio,
            IProceduresRepositorio proceduresRepository,
            IPagamentoPedidoRepositorio pagamentoPedidoRepositorio,
            ITransacaoRepositorio transacaoRepositorio,
            IUsuarioProdutoRepositorio usuarioProdutoRepositorio,
            IPedidoDetalheRepositorio pedidoDetalheRepositorio,
            ICuponCashbackRepositorio cuponCashbackRepositorio,
            ICuponCashbackPedidoRepositorio cuponCashbackPedidoRepositorio,
            IMapper mapper,
            IUsuarioNegocio usuarioNegocio,
            ILancamentoNegocio lancamentoNegocio,
            IPagamentoNegocio pagamentoNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IProdutoNegocio produtoNegocio,
            IConfiguracaoNegocio configNegocio,
            ILocation location,
            ICache cache,
            IStatusNegocio statusNegocio,
            IProceduresRepositorio procedures,
            ITransacaoNegocio transacaoNegocio,
            ICashbackNegocio cashbackNegocio,
            IServiceProvider serviceProvider,
            IUsuarioEnderecoNegocio usuarioEnderecoNegocio) : base(repositorio, mapper)
        {
            _appSettings = appSettings.Value;
            _repositorio = repositorio;
            _pagamentoRepositorio = pagamentoRepositorio;
            _proceduresRepository = proceduresRepository;
            _pagamentoPedidoRepositorio = pagamentoPedidoRepositorio;
            _transacaoRepositorio = transacaoRepositorio;
            _usuarioProdutoRepositorio = usuarioProdutoRepositorio;
            _pedidoDetalheRepositorio = pedidoDetalheRepositorio;
            _cuponCashbackRepositorio = cuponCashbackRepositorio;
            _cuponCashbackPedidoRepositorio = cuponCashbackPedidoRepositorio;
            _mapper = mapper;
            _usuarioNegocio = usuarioNegocio;
            //_suporteNegocio = suporteNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _lancamentoNegocio = lancamentoNegocio;
            _produtoNegocio = produtoNegocio;
            _location = location;
            _cache = cache;
            _procedures = procedures;
            _configNegocio = configNegocio;
            _statusNegocio = statusNegocio;
            _transacaoNegocio = transacaoNegocio;
            _pagamentoNegocio = pagamentoNegocio;
            _cashbackNegocio = cashbackNegocio;
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            _usuarioEnderecoNegocio = usuarioEnderecoNegocio;
        }

        public Pedido AdicionarParcela(long idPedido)
        {
            var pedidoBanco = _repositorio.FirstNoTracking(p => p.IdPedido == idPedido, "Pagamentos");

            // Verifica se o pedido existe
            if (pedidoBanco != null)
            {
                throw new NotFoundException("pedido_nao_encontrado");
            }

            // Calcula o novo valor de cada parcela baseado no valor restante do pedido
            var novoRestante = pedidoBanco.ValorPago.Value - pedidoBanco.ValorPedido;

            if (novoRestante <= 0)
            {
                throw new PadraoException("pedido_pago");
            }

            pedidoBanco.NumeroParcelas += 1;

            var novoValor = novoRestante / pedidoBanco.NumeroParcelas;

            // Caso o pedido seja a vista, faz a alteração para ser parcelado
            if (pedidoBanco.NumeroParcelas <= 1)
            {
                pedidoBanco.NumeroParcelas = 1;

                _pagamentoNegocio.Insert(new PagamentoViewModel
                {
                    IdPedido = idPedido,
                    DataValidade = DateTime.UtcNow.HorarioBrasilia().AddMonths(1),
                    NumeroParcela = 1,
                    Valor = novoValor,
                    Pago = false,
                });
                _pagamentoNegocio.SaveChanges();
                _repositorio.Update(pedidoBanco);
                _repositorio.SaveChanges();

                pedidoBanco = _repositorio.FirstNoTracking(p => p.IdPedido == idPedido, "Pagamentos");
            }

            // Atualiza as parcelas restantes
            foreach (var parcela in pedidoBanco.Pagamentos)
            {
                var parcelaBanco = _pagamentoNegocio.FirstNoTracking(p => p.IdPagamento == parcela.IdPagamento);
                parcelaBanco.Valor = novoValor;
                _pagamentoNegocio.Update(parcelaBanco);
            }

            // Adiciona a nova parcela
            var ultimaParcela = pedidoBanco.Pagamentos.OrderBy(p => p.NumeroParcela).Last();
            _pagamentoNegocio.Insert(new PagamentoViewModel
            {
                IdPedido = idPedido,
                DataValidade = ultimaParcela.DataValidade.AddMonths(1),
                NumeroParcela = ultimaParcela.NumeroParcela + 1,
                Valor = novoValor,
                Pago = false
            });
            _pagamentoNegocio.SaveChanges();


            // Atualiza o pedido no banco
            pedidoBanco.NumeroParcelas += 1;
            _repositorio.Update(pedidoBanco);
            _repositorio.SaveChanges();

            return pedidoBanco;
        }

        public Pedido AdquirirAssinaturaAnual(AdquirirAssinaturaAnualViewModel model, Guid idUsuarioLogado)
        {
            var usuario = _usuarioNegocio.GetById(idUsuarioLogado);

            if (usuario == null)
            {
                throw new PadraoException("usuario_nao_encontrado");
            }

            var produtoSelecionado = _produtoNegocio.GetById(model.IdProduto);

            var compra = new CompraViewModel()
            {
                IdProduto = model.IdProduto,
                IdUsuario = usuario.IdUsuario,
                MetodoDePagamento = model.FormaPagamento,
                SenhaEletronica = "",
                NumParcelas = produtoSelecionado.Parcelas,
                TipoTransacao = EnumTipoTransacao.Assinatura,
                Card = model.Card,
            };

            var pedido = GerarAssinaturaAnual(compra, TipoPedido.Assinatura, idUsuarioLogado);

            return pedido;
        }

        public Pedido AdquirirPlano(AdquirirPlanoViewModel model, Guid idUsuarioLogado)
        {
            var usuario = _usuarioNegocio.GetById(idUsuarioLogado);

            if (usuario == null)
            {
                throw new PadraoException("usuario_nao_encontrado");
            }

            var compra = new CompraViewModel()
            {
                IdProduto = model.IdProduto,
                IdUsuario = usuario.IdUsuario,
                MetodoDePagamento = model.FormaPagamento,
                SenhaEletronica = "",
                NumParcelas = model.NumParcelas,
                TipoTransacao = EnumTipoTransacao.CompraProduto
            };

            var pedido = GerarPedido(compra, TipoPedido.Baf, usuario, idUsuarioLogado);

            return pedido;
        }

        public Pedido AdquirirPlanoMensal(AdquirirPlanoViewModel model, Guid idUsuarioLogado)
        {
            var usuario = _usuarioNegocio.GetById(idUsuarioLogado);

            if (usuario == null)
            {
                throw new PadraoException("usuario_nao_encontrado");
            }


            var compra = new CompraViewModel()
            {
                IdProduto = model.IdProduto,
                IdUsuario = usuario.IdUsuario,
                MetodoDePagamento = model.FormaPagamento,
                SenhaEletronica = "",
                NumParcelas = 0,
                TipoTransacao = EnumTipoTransacao.Assinatura,
                Card = model.Card,
            };

            var pedido = GerarAssinatura(compra, TipoPedido.Assinatura, idUsuarioLogado);

            return pedido;
        }

        public Pedido AdquirirSaldo(AdquirirSaldoViewModel model, Guid idUsuarioLogado)
        {
            {
                var usuario = _usuarioNegocio.GetById(idUsuarioLogado);
                var configuracao = _configNegocio.BuscarPelaChave("TAXA_COMPRA_SALDO");

                if (usuario == null)
                {
                    throw new PadraoException("usuario_nao_encontrado");
                }

                var taxa = decimal.Parse(configuracao.Valor, CultureInfo.InvariantCulture);
                var compra = new CompraViewModel()
                {
                    IdUsuario = usuario.IdUsuario,
                    MetodoDePagamento = model.FormaPagamento,
                    SenhaEletronica = "",
                    NumParcelas = 1,
                    IdProduto = int.Parse(_appSettings.IdProdutoSaldo),
                    ValorTaxa = taxa,
                    TipoTransacao = EnumTipoTransacao.CompraVoucher,
                    ValorPedido = model.Valor + taxa
                };

                if (model.Valor - taxa < model.SaldoReceber.Truncate(2))
                {
                    throw new PadraoException("taxa_incorreta");
                }

                var pedido = GerarPedido(compra, TipoPedido.Saldo, usuario, idUsuarioLogado);

                return pedido;
            }
        }

        public bool AlterarValorParcela(int idPedido, int numeroParcela, decimal valor)
        {
            var parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == idPedido && p.NumeroParcela == numeroParcela);
            var pedido = _repositorio.GetById(parcela.IdPedido);

            if (parcela.Pago)
            {
                throw new PadraoException("parcela_paga");
            }
            if (!parcela.Ativo)
            {
                throw new PadraoException("parcela_inativa");
            }
            if (parcela.NumeroParcela == pedido.NumeroParcelas)
            {
                throw new PadraoException("parcela_nao_editavel");
            }

            var valorOriginal = parcela.Valor;

            // Atribui novo valor da parcela
            parcela.Valor = valor;

            // Recalcula o novo valor das parelas restantes
            var diff = valorOriginal - valor;

            var valorRestante = pedido.ValorPago.HasValue ? pedido.ValorPedido - pedido.ValorPago.Value : pedido.ValorPedido;

            var parcelasPedido = _pagamentoNegocio.GetNoTracking(p => p.IdPedido == pedido.IdPedido).OrderBy(p => p.NumeroParcela).ToList();
            var parcelasNaoPagas = parcelasPedido.Where(p => !p.Pago && p.IdPagamento != parcela.IdPagamento);

            var novoValor = (valorRestante - valor) / parcelasNaoPagas.Count();

            foreach (var pgto in parcelasNaoPagas)
            {
                pgto.Valor = novoValor;

                var aux = _pagamentoNegocio.CriarBoletoAsync(pgto, pedido).Result;

                pgto.CodigoReferenciaBoleto = aux.CodigoReferenciaBoleto;
                pgto.LinhaDigitavelBoleto = aux.LinhaDigitavelBoleto;
                pgto.UrlBoleto = aux.UrlBoleto;

                _pagamentoNegocio.Update(pgto);
            }

            _pagamentoNegocio.Update(parcela);
            _repositorio.SaveChanges();
            return true;
        }

        public bool ApagarParcela(int idPedido, int numeroParcela)
        {
            var parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == idPedido && p.NumeroParcela == numeroParcela);
            var pedido = _repositorio.GetById(parcela.IdPedido);

            if (parcela.Pago)
            {
                throw new PadraoException("parcela_paga");
            }
            if (!parcela.Ativo)
            {
                throw new PadraoException("parcela_inativa");
            }
            if (pedido.NumeroParcelas <= 1)
            {
                throw new PadraoException("pedido_unica_parcela_nao_paga");
            }

            _pagamentoNegocio.Delete(parcela.IdPagamento);
            _pagamentoNegocio.SaveChanges();

            var valorRestante = pedido.ValorPago.HasValue ? pedido.ValorPedido - pedido.ValorPago.Value : pedido.ValorPedido;

            var parcelasPedido = _pagamentoNegocio.GetNoTracking(p => p.IdPedido == pedido.IdPedido).OrderBy(p => p.NumeroParcela).ToList();
            var parcelasNaoPagas = parcelasPedido.Where(p => !p.Pago);

            var novoValor = valorRestante / parcelasNaoPagas.Count();

            foreach (var pgto in parcelasNaoPagas)
            {
                pgto.Valor = novoValor;
                pgto.DataValidade = DateTime.UtcNow.HorarioBrasilia().AddMonths(pgto.NumeroParcela - 1).AddDays(8);

                var aux = _pagamentoNegocio.CriarBoletoAsync(pgto, pedido).Result;

                pgto.CodigoReferenciaBoleto = aux.CodigoReferenciaBoleto;
                pgto.LinhaDigitavelBoleto = aux.LinhaDigitavelBoleto;
                pgto.UrlBoleto = aux.UrlBoleto;

                _pagamentoNegocio.Update(pgto);
            }
            _repositorio.SaveChanges();

            pedido.NumeroParcelas -= 1;
            _repositorio.Update(pedido);
            _pagamentoNegocio.SaveChanges();

            return true;
        }

        public void AtivarProduto(int idProduto, string login, decimal valorPedido, bool distribuiNaRede, bool GerarPontos)
        {
            var produto = _produtoNegocio.FirstNoTracking(p => p.IdProduto == idProduto);
            var usuario = _usuarioNegocio.FirstNoTracking(u => u.Login == login, "UsuarioProduto", "UsuarioProduto.Nome");

            // Verifica se o usuário tem produto ativo
            if (usuario.UsuarioProduto.Any(p => p.Ativo))
            {
                // Caso tenha inativa o produto
                var usuarioProduto = usuario.UsuarioProduto.FirstOrDefault(p => p.Ativo);
                usuarioProduto.Ativo = false;
                _usuarioProdutoNegocio.Update(_mapper.Map<UsuarioProdutoViewModel>(usuarioProduto));
                _usuarioProdutoNegocio.SaveChanges();
            }

            // Verifica se o usuário tem pedidos de BAF ativos
            var pedidosUsuario = _repositorio
                .GetNoTracking(p => p.IdUsuario == usuario.IdUsuario
                && !p.Pago
                && !string.IsNullOrEmpty(p.Codigo)
                && (p.Ativo.HasValue && p.Ativo.Value || !p.Cancelado.HasValue && !p.Cancelado.Value),
                "Pagamentos");

            // Caso tenha os pedidos são cancelados
            if (pedidosUsuario != null)
            {
                foreach (var pedido in pedidosUsuario)
                {
                    pedido.Cancelado = true;
                    _repositorio.Update(pedido);
                }
                _repositorio.SaveChanges();
            }

            // Adiciona o novo pedido no banco
            var novoPedido = GerarPedido(new CompraViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NumParcelas = 1,
                IdProduto = idProduto,
                MetodoDePagamento = EnumTipoPagamento.Saldo
            },
            TipoPedido.Baf,
            usuario,
            usuario.IdUsuario);

            // Caso o pedido seja subsidiada, faz o débito do saldo do cliente

            // Adiciona o novo produto ao usuário


            // Caso a opção de distribuição estiver marcada. Chama a procedure de distribuição de cashback
        }

        public object BuscarProdutoPorCodigo(string codigo, Guid idUsuario)
        {
            var pedido = First(f => f.Codigo == codigo && f.IdUsuario == idUsuario, "Transacao");
            if (pedido != null)
            {
                var usuarioProduto = _usuarioProdutoNegocio.First(f => f.IdPedido == pedido.IdPedido, "Produto");

                return new
                {
                    pedido.ValorPedido,
                    pedido.DataPedido,
                    pedido.EnderecoDeposito,
                    pedido.UrlPagamento,
                    CodidoPedido = pedido.Codigo,
                    StatusTransacao = _statusNegocio.GetFromCache().First(w => w.IdStatus == pedido.Transacao.IdStatus).Nome,
                    NomeProduto = usuarioProduto.Produto.Nome,
                    pedido.Cotacao
                };
            }
            return false;
        }

        public DadosRetorno<string> CancelarAssinatura(Guid idUsuarioLogado, string tipo, bool manual = false)
        {
            DadosRetorno<string> retorno = new DadosRetorno<string>();

            try
            {
                var produtoAtivo = _usuarioProdutoNegocio.Get(x => x.IdUsuario == idUsuarioLogado && x.Ativo && x.AssinaturaHabilitada).FirstOrDefault();

                if (produtoAtivo == null)
                {
                    retorno.Success = false;
                    retorno.Message = "Assinatura não encontrada";
                    return retorno;
                }

                var pedido = _repositorio.Get(x => x.IdUsuario == idUsuarioLogado && x.Tipo == (int)TipoPedido.Assinatura && !x.Cancelado.Value && x.Pago && !string.IsNullOrEmpty(x.CodigoReferenciaAssinatura)).FirstOrDefault();
                var result = _pagamentoNegocio.CancelarAssinatura(pedido);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (result.Result.Success || manual)
                        {
                            pedido.Status = (int)StatusPedido.Cancelado;
                            pedido.Cancelado = true;
                            
                            produtoAtivo.AssinaturaHabilitada = false;
                            produtoAtivo.AssinaturaManual = manual;

                            var pedidoDetalhe = new PedidoDetalhe()
                            {
                                IdPedido = pedido.IdPedido,
                                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                                Ativo = true,
                                Descricao = $"Assinatura do plano cancelada por " + tipo,
                                IdStatus = (int)StatusTransacaoEnum.Aprovado,
                                IdUsuario = pedido.IdUsuario,
                            };

                            _context.UsuarioProduto.Update(_mapper.Map<UsuarioProduto>(produtoAtivo));
                            _context.PedidoDetalhe.Add(pedidoDetalhe);
                            _context.SaveChanges();

                            _repositorio.Update(pedido);
                            _repositorio.SaveChanges();

                            transaction.CommitAsync();

                            retorno.Success = true;

                            //TODO: Enviar e-mail de cancelamento realizado com sucesso
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackAsync();
                        retorno.Success = false;
                        retorno.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    }
                }


            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            }
            return retorno;

        }

        public bool CancelarPedido(long idPedido, Guid idUsuario)
        {
            var pedido = _repositorio.FirstNoTracking(
                p => p.IdPedido == idPedido,
                "Pagamentos",
                "Transacao");


            if (pedido == null)
            {
                throw new NotFoundException("pedido_nao_encontrado");
            }

            if (idUsuario != pedido.IdUsuario)
            {
                var usuario = _usuarioNegocio.GetById(idUsuario, "Grupo");
                if (usuario.Grupo.Descricao != "Admin" && usuario.Grupo.Descricao != "Suporte")
                {
                    throw new UnauthorizedException("pedido_nao_encontrado");
                }
            }

            if (pedido.Pago)
            {
                throw new PadraoException("pedido_pago");
            }

            // Inativa os boletos
            foreach (var pagamento in pedido.Pagamentos.Where(w => !w.Pago))
            {
                _pagamentoNegocio.InativarPagamento(pagamento);
            }

            pedido.Ativo = false;
            pedido.Cancelado = true;
            pedido.Status = (int)StatusPedido.Cancelado;
            _repositorio.Update(pedido);
            pedido.Transacao.IdStatus = (int)StatusTransacaoEnum.Cancelada;
            _transacaoRepositorio.Update(pedido.Transacao);

            switch (pedido.Tipo)
            {
                case (int)TipoPedido.Baf:
                    var planoAtual = _usuarioProdutoNegocio
                        .FirstNoTracking(p => p.IdPedido == pedido.IdPedido && p.Ativo);

                    if (planoAtual != null)
                    {
                        planoAtual.Ativo = false;
                        _usuarioProdutoNegocio.Update(planoAtual);

                        var planoAnterior = _usuarioProdutoNegocio
                            .GetNoTracking(
                                g =>
                                    g.IdUsuario == pedido.IdUsuario &&
                                    !g.Ativo &&
                                    g.IdProduto != planoAtual.IdProduto &&
                                    g.Pedido.Pago,
                                "Pedido")
                            .OrderByDescending(o => o.Pedido.DataPagamento)
                            .ThenByDescending(t => t.DataVinculo)
                            .FirstOrDefault();

                        if (planoAnterior == default)
                        {
                            planoAnterior = _usuarioProdutoNegocio
                                .GetNoTracking(g => g.IdUsuario == pedido.IdUsuario && g.IdPedido != pedido.IdPedido && !g.Ativo)
                                .OrderBy(o => o.DataVinculo)
                                .First();
                        }

                        planoAnterior.Ativo = true;
                        _usuarioProdutoNegocio.Update(planoAnterior);
                    }

                    break;
                case (int)TipoPedido.FaturaCashbackCredenciado:
                    var pagamentoPedidos = pedido.Pagamentos.SelectMany(s => _pagamentoPedidoRepositorio
                        .Get(
                            g =>
                                g.IdPagamento == s.IdPagamento &&
                                g.Pedido.Tipo == (int)TipoPedido.CashbackCredenciado,
                            "Pedido"))
                        .ToList();

                    foreach (var pagamentoPedido in pagamentoPedidos)
                    {
                        pagamentoPedido.Pedido.Status = (int)StatusPedido.AguardandoFaturaCredenciado;
                        _repositorio.Update(pagamentoPedido.Pedido);
                    }

                    _pagamentoPedidoRepositorio.DeleteRange(pagamentoPedidos);
                    break;
            }
            _repositorio.SaveChanges();

            return true;
        }

        public async Task<Pedido> CriarFaturaCashbackCredenciadoAsync(DateTime ateData, EnumTipoPagamento tipoPagamento, Guid idUsuarioLogado)
        {
            var faturaAberta = _repositorio.Get(
                g =>
                    g.IdUsuario == idUsuarioLogado &&
                    g.Tipo == (int)TipoPedido.FaturaCashbackCredenciado &&
                    (g.Status == (int)StatusPedido.AguardandoPagamento || g.Status == (int)StatusPedido.EmPagamento))
                .Any();

            //if (faturaAberta)
            //{
            //    throw new PadraoException("fatura_em_pagamento");
            //}

            var configuracao = _configNegocio.BuscarPelaChave("TAXA_COMPRA_SALDO");
            var taxa = decimal.Parse(configuracao.Valor, CultureInfo.InvariantCulture);
            var usuario = _usuarioNegocio.GetById(idUsuarioLogado);

            if (string.IsNullOrEmpty(usuario.Documento))
                throw new PadraoException("compra_cadastre_dados");

            if (usuario.PreCadastro)
                throw new PadraoException("compra_cadastre_dados");

            var usuarioEndereco = _usuarioEnderecoNegocio.Get(x => x.IdUsuario == idUsuarioLogado, ["Cidade", "Cidade.Estado"]).FirstOrDefault();

            if (usuarioEndereco is null)
                throw new PadraoException("empreendedor_sem_endereco");

            var pedidosCashback = _repositorio
                .Get(p =>
                    p.IdUsuarioComerciante == idUsuarioLogado &&
                    p.Ativo == true &&
                    p.Status == (int)StatusPedido.AguardandoFaturaCredenciado &&
                    p.Tipo == (int)TipoPedido.CashbackCredenciado &&
                    p.DataPedido < ateData.HorarioBrasilia().AddDays(1).Date);

            if (pedidosCashback.Sum(s => s.ValorPedido * (s.PercentualCashback.Value / 100)) <= 0)
            {
                throw new Exception();
            }

            var viewModel = new CompraViewModel
            {
                IdProduto = null,
                IdUsuario = idUsuarioLogado,
                MetodoDePagamento = tipoPagamento,
                NumParcelas = 1,
                NumParcelasPagas = 0,
                SenhaEletronica = "",
                TipoTransacao = EnumTipoTransacao.CashbackCredenciado,
                ValorPedido = taxa + pedidosCashback.Sum(s => s.ValorPedido * (s.PercentualCashback.Value / 100)),
                ValorTaxa = taxa
            };

            var pedido = GerarPedido(
                viewModel,
                TipoPedido.FaturaCashbackCredenciado,
                usuario,
                idUsuarioLogado,
                ativacaoManual: false,
                contabilizarPontacao: false,
                gerarBoleto: true);

            foreach (var pedidoCashback in pedidosCashback)
            {
                var pagamentoPedido = new PagamentoPedido
                {
                    IdPedido = pedidoCashback.IdPedido,
                    Pagamento = pedido.Pagamentos.First(),
                    Ordem = 1,
                    Status = (int)StatusPagamentoPedido.AguardandoPagamento,
                    Valor = pedidoCashback.ValorPedido * (pedidoCashback.PercentualCashback.Value / 100),
                    ValorPago = 0
                };

                _pagamentoPedidoRepositorio.Insert(pagamentoPedido);
                pedidoCashback.Status = (int)StatusPedido.AguardandoCashback;
                _repositorio.Update(pedido);
            }

            await _pagamentoPedidoRepositorio.SaveChangesAsync();

            return pedido;
        }

        public Pedido EditarPedido(PedidoViewModel pedido)
        {
            var pedidoBanco = _repositorio.FirstNoTracking(p => p.IdPedido == pedido.IdPedido, "Pagamentos");

            // Verifica se o pedido existe
            if (pedidoBanco == null)
            {
                throw new NotFoundException("pedido_nao_encontrado");
            }

            if (!pedidoBanco.Ativo.HasValue || !pedidoBanco.Ativo.Value)
            {
                throw new PadraoException("pedido_inativo");

            }

            var valorAlterado = false;

            // Verifica se o valor do pedido foi alterado
            if (pedidoBanco.ValorPedido != pedido.ValorPedido)
            {
                pedidoBanco.ValorPedido = pedido.ValorPedido;
                valorAlterado = true;
            }

            // Verifica se o valor pago do pedio foi alterado
            if (pedidoBanco.ValorPago != pedido.ValorPago)
            {
                pedidoBanco.ValorPago = pedido.ValorPago;
                valorAlterado = true;
            }

            // Verifica se as parcelas devem ser alteradas
            if (valorAlterado || pedido.NumeroParcelas != pedidoBanco.NumeroParcelas)
            {
                var numeroParcelasOld = pedidoBanco.NumeroParcelas;
                pedidoBanco.NumeroParcelas = pedido.NumeroParcelas;

                // Calcula o novo valor restante a ser pago
                var parcelasDb = _pagamentoNegocio.ObterPagamentos(pedidoBanco.IdPedido).OrderBy(p => p.NumeroParcela).ToList();
                var parcelasRestantes = pedidoBanco.NumeroParcelas - parcelasDb.Where(p => p.Pago).Count();

                var valorRestante = pedidoBanco.ValorPago.HasValue ? pedidoBanco.ValorPedido - pedidoBanco.ValorPago.Value : pedido.ValorPedido;
                var novoValorParcela = valorRestante / parcelasRestantes;

                var dataAtual = DateTime.UtcNow.HorarioBrasilia().AddDays(8);
                var novaParcela = false;
                for (int i = 1; i <= pedidoBanco.NumeroParcelas; i++)
                {
                    var parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == pedidoBanco.IdPedido && p.NumeroParcela == i);

                    if (parcela != null && parcela.Pago)
                    {
                        continue;
                    }

                    // Caso o pedido já tenha uma parcela criada, apenas atualiza. Caso contrário uma nova é criada
                    if (parcela != null)
                    {
                        parcela.Valor = novoValorParcela;
                        parcela.DataValidade = dataAtual.AddMonths(i - 1);
                    }
                    else
                    {
                        novaParcela = true;

                        // Gera uma nova parcela
                        parcela = new PagamentoViewModel
                        {
                            Ativo = true,
                            Pago = false,
                            DataValidade = dataAtual.AddMonths(i - 1),
                            DataPagamento = null,
                            NumeroParcela = i,
                            IdPedido = pedidoBanco.IdPedido,
                            Valor = novoValorParcela
                        };
                    }

                    // Gera o novo boleto
                    var novaParcelaAux = _pagamentoNegocio.CriarBoletoAsync(parcela, pedidoBanco).Result;

                    parcela.CodigoReferenciaBoleto = novaParcelaAux.CodigoReferenciaBoleto;
                    parcela.LinhaDigitavelBoleto = novaParcelaAux.LinhaDigitavelBoleto;
                    parcela.UrlBoleto = novaParcelaAux.UrlBoleto;

                    // Cria ou atualiza a parcela no banco
                    if (novaParcela)
                    {
                        _pagamentoNegocio.Insert(parcela);
                    }
                    else
                    {
                        _pagamentoNegocio.Update(parcela);
                    }
                    _pagamentoNegocio.SaveChanges();
                }

                var parcelasValorAtualizada = _pagamentoNegocio.GetNoTracking(p => p.IdPedido == pedido.IdPedido);

                if (parcelasValorAtualizada.Count > 0)
                {
                    for (int i = numeroParcelasOld; i > pedidoBanco.NumeroParcelas; i--)
                    {
                        _pagamentoNegocio.Delete(parcelasValorAtualizada.FirstOrDefault(p => p.NumeroParcela == i).IdPagamento);
                    }

                    _pagamentoNegocio.SaveChanges();
                }

            }

            pedidoBanco.ReaisPorPonto = pedido.ReaisPorPonto;

            _repositorio.Update(pedidoBanco);
            _repositorio.SaveChanges();

            return pedidoBanco;
        }

        public async Task<bool> EfetuarCompraComerciante(EfetuarCompraViewModel viewModel, Guid IdUsuario)
        {
            return await _repositorio.EfetuarCompraComerciante(viewModel, IdUsuario);
        }

        public PedidoViewModel GerarPedidoManual(GerarPedidoManualViewModel model, Guid idUsuarioLogado)
        {
            var usuario = _usuarioNegocio.BuscarLoginOuEmail(model.Login);
            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }
            var pedido = GerarPedido(
                new CompraViewModel()
                {
                    IdProduto = model.IdProduto,
                    IdUsuario = usuario.IdUsuario,
                    MetodoDePagamento = EnumTipoPagamento.PGPAGARME,
                    SenhaEletronica = "",
                    NumParcelas = model.NumeroParcelas,
                    NumParcelasPagas = model.NumeroParcelasPagas.HasValue ? model.NumeroParcelasPagas.Value : 0,
                    ValorPedido = model.ValorPedido,
                    ReaisPorPonto = model.ReaisPorPonto,
                    DataReferencia = model.DataReferencia
                },
                TipoPedido.Baf,
                usuario,
                idUsuarioLogado,
                true,
                model.GerarPontos,
                model.DistribuirNaRede);
            return _mapper.Map<PedidoViewModel>(pedido);
        }

        public bool GetAssinatura(Guid idUsuarioLogado)
        {
            var retorno = false;

            var produtoAtivo = _usuarioProdutoNegocio.Get(x => x.IdUsuario == idUsuarioLogado && x.Ativo && x.AssinaturaHabilitada).FirstOrDefault();

            if (produtoAtivo != null)
            {
                retorno = true;
            }

            return retorno;
        }

        public List<PedidosProcedureViewModel> ListarPedidos(FiltroViewModel.BuscarPedido filtro, string idUsuario)
        {
            return _repositorio.BuscarPedidos(filtro, idUsuario);
        }

        public IList<PedidoViewModel> ListarPedidosAfiliados(FiltroViewModel.PedidoAfiliados filtro, Guid idUsuario)
        {
            if (filtro.DataFim.HasValue && filtro.DataInicio.HasValue)
            {
                TimeSpan ti = new TimeSpan(0, 0, 0);
                TimeSpan tf = new TimeSpan(23, 59, 59);
                filtro.DataFim = filtro.DataFim.Value.Date + tf;
                filtro.DataInicio = filtro.DataInicio.Value.Date + ti;
            }

            var pedidos = _repositorio.Get(
                g =>
                    g.IdUsuario == idUsuario &&
                    (g.IdVendaZanox.HasValue || g.IdVendaAfilio.HasValue || g.IdAwinTransaction.HasValue || g.IdUsuarioComerciante.HasValue) &&
                    (filtro.DataInicio == null || g.DataPedido >= filtro.DataInicio.Value) &&
                    (filtro.DataFim == null || g.DataPedido <= filtro.DataFim.Value) &&
                    (string.IsNullOrEmpty(filtro.Descricao) || g.PedidoDetalhe.Last().Descricao.ToLower().Contains(filtro.Descricao.ToLower()))
                    )
                .Include(i => i.PedidoDetalhe)
                    .ThenInclude(t => t.Status)
                .Include(i => i.Transacao)
                    .ThenInclude(t => t.Status)
                .OrderByDescending(o => o.DataPedido).ToList();

            var configuracoes = _configNegocio.GetFromCache();
            var PERCENTUAL_CASHBACK = Convert.ToDecimal(configuracoes.FirstOrDefault(c => c.Chave.Equals("PERCENTUAL_CASHBACK")).Valor);
            var pedidosVM = _mapper.Map<List<PedidoViewModel>>(pedidos);

            foreach (var pedido in pedidosVM)
                pedido.CashbackReceber = pedido.Cashback * (PERCENTUAL_CASHBACK / 100);

            return pedidosVM;
        }

        public object ListarPedidosAfiliadosAdmin(FiltroViewModel.PedidoAfiliadosAdmin viewModel)
        {
            if (viewModel.DataFim.HasValue && viewModel.DataInicio.HasValue)
            {
                TimeSpan ti = new TimeSpan(0, 0, 0);
                TimeSpan tf = new TimeSpan(23, 59, 59);
                viewModel.DataFim = viewModel.DataFim.Value.Date + tf;
                viewModel.DataInicio = viewModel.DataInicio.Value.Date + ti;
                viewModel.DataFim = viewModel.DataFim.Value.AddDays(1);
            }

            var lista = Get(w => (string.IsNullOrEmpty(viewModel.Login) || w.Usuario.Login.Contains(viewModel.Login))
                && (string.IsNullOrEmpty(viewModel.LoginPatrocinador) || w.Usuario.UsuarioPai.Login.Contains(viewModel.LoginPatrocinador))
                && (string.IsNullOrEmpty(viewModel.LoginCredenciado) || w.UsuarioComerciante.Login.Contains(viewModel.LoginCredenciado))
                && (w.IdVendaZanox.HasValue || w.IdVendaAfilio.HasValue || w.IdAwinTransaction.HasValue || w.IdUsuarioComerciante.HasValue)
                && (!viewModel.IdStatus.HasValue ||
                    viewModel.IdStatus == w.Transacao.IdStatus ||
                    (w.Transacao == null && viewModel.IdStatus == w.PedidoDetalhe.OrderByDescending(o => o.DataAtualizacao).FirstOrDefault().IdStatus))
                && (!viewModel.DataInicio.HasValue || w.DataPedido >= viewModel.DataInicio.Value.Date)
                && (!viewModel.DataFim.HasValue || w.DataPedido <= viewModel.DataFim.Value.Date),
                "Usuario.UsuarioPai", "PedidoDetalhe.Status", "UsuarioComerciante.Credenciamento");

            switch (viewModel.Ordenacao)
            {
                case EnumOrdenacaoComprasAdmin.Login:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.Usuario.Login).ToList();
                    else lista = lista.OrderBy(l => l.Usuario.Login).ToList();
                    break;
                case EnumOrdenacaoComprasAdmin.Patrocinador:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.Usuario.UsuarioPai.Login).ToList();
                    else lista = lista.OrderBy(l => l.Usuario.UsuarioPai.Login).ToList();
                    break;
                case EnumOrdenacaoComprasAdmin.Produto:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.PedidoDetalhe.FirstOrDefault().Descricao).ToList();
                    else lista = lista.OrderBy(l => l.PedidoDetalhe.FirstOrDefault().Descricao).ToList();
                    break;
                case EnumOrdenacaoComprasAdmin.Status:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.PedidoDetalhe.LastOrDefault().Status.Nome).ToList();
                    else lista = lista.OrderBy(l => l.PedidoDetalhe.LastOrDefault().Status.Nome).ToList();
                    break;
                case EnumOrdenacaoComprasAdmin.Valor:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.ValorPedido).ToList();
                    else lista = lista.OrderBy(l => l.ValorPedido).ToList();
                    break;
                default:
                    if (viewModel.OrderDesc) lista = lista.OrderByDescending(l => l.DataPedido).ToList();
                    else lista = lista.OrderBy(l => l.DataPedido).ToList();
                    break;
            }

            var totalPages = (int)Math.Ceiling((double)lista.Count() / viewModel.PerPage);

            var retorno = lista.Skip(viewModel.PerPage * (viewModel.Page - 1)).Take(viewModel.PerPage).Select(s => new
            {
                s.IdPedido,
                s.Usuario.Login,
                LoginPatrocinador = s.Usuario.UsuarioPai != null ? s.Usuario.UsuarioPai.Login : "",
                s.DataPedido,
                s.ValorPedido,
                s.Cashback,
                Loja = s.PedidoDetalhe?.FirstOrDefault()?.Descricao?.Replace("Compra em: ", "") ??
                    s.UsuarioComerciante?.Credenciamento?.Estabelecimento,
                Status = s.PedidoDetalhe?.LastOrDefault()?.Status?.Nome ??
                    ((StatusPedido)s.Status).GetDescription(),
                Atualizacao = s.PedidoDetalhe?.LastOrDefault()?.DataAtualizacao ??
                    s.DataPedido,
                Afiliado = s.IdVendaAfilio.HasValue ? "Afilio" :
                    s.IdAwinTransaction.HasValue ? "Awin" :
                    s.IdVendaZanox.HasValue ? "Zanox" :
                    s.IdUsuarioComerciante.HasValue ? "Credenciado" : "",
                s.PedidoDetalhe
            });

            return new
            {
                totalPages,
                data = retorno,
                excel = JsonSerializer.Serialize(retorno)
            };
        }

        public Pedido ObterDetalhes(string codigo)
        {
            var pedido = _repositorio.ObterPedidoCompleto(codigo);
            return pedido;
        }

        public Pedido ObterPedidoVigente(Guid idUsuario)
        {
            var pedido = _repositorio.Get(p => p.IdUsuario == idUsuario && !p.Pago && p.Ativo.HasValue && p.Ativo.Value && (!p.Cancelado.HasValue || !p.Cancelado.Value), "UsuarioProduto");
            var vigente = pedido.OrderByDescending(p => p.DataPedido).First();
            return vigente;
        }

        public decimal ObterValorPagoBaf(Guid idUsuario)
        {
            decimal valor = 0;

            // Busca todos os pedidos de planos pagos
            var pedidosPagos = _repositorio.Get(p =>
                p.IdUsuario == idUsuario &&
                p.Pago &&
                p.Tipo == (int)TipoPedido.Baf);

            if (pedidosPagos != null && pedidosPagos.Any())
            {
                valor += pedidosPagos.Sum(p => p.ValorPedido);
            }

            // Busca pedidos em pagamento parcelado
            var pedidosParcelados = _repositorio.Get(
                p =>
                    p.IdUsuario == idUsuario &&
                    p.NumeroParcelas > 1 &&
                    !p.Pago &&
                    p.Cancelado != true &&
                    p.Tipo == (int)TipoPedido.Baf,
                "Pagamentos");

            foreach (var pedido in pedidosParcelados)
            {
                valor += pedido.Pagamentos
                    .Where(p => p.Pago)
                    .Sum(p => p.Valor);
            }

            return valor;
        }

        public bool PagarParcela(long idPedido, int numeroParcela, DateTime? dataReferencia, bool distribuirNaRede = false)
        {
            var dbTransaction = _repositorio.GetTransaction();

            try
            {
                var parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == idPedido && p.NumeroParcela == numeroParcela);

                if (parcela == null)
                {
                    // Busca pelo pedido caso este tenha uma unica parcela. Isso é feito dessa forma pois os pedidos a vista não tem parcelas
                    var buscarPedido = _repositorio.FirstNoTracking(p => p.IdPedido == idPedido, "Pagamentos", "UsuarioProduto", "UsuarioProduto.Produto");

                    if (buscarPedido != null && buscarPedido.NumeroParcelas <= 1 && (buscarPedido.Pagamentos == null || buscarPedido.Pagamentos.Count == 0))
                    {
                        // Adiciona a primeira parcela caso o pedido não tivesse
                        var novaParcela = new PagamentoViewModel
                        {
                            IdPedido = buscarPedido.IdPedido,
                            DataValidade = buscarPedido.DataPedido.AddDays(8),
                            Valor = buscarPedido.ValorPago.HasValue ? (buscarPedido.ValorPedido - buscarPedido.ValorPago.Value) : buscarPedido.ValorPedido / buscarPedido.NumeroParcelas,
                            Pago = false,
                            CodigoReferenciaBoleto = buscarPedido.CodigoReferenciaBoleto,
                            LinhaDigitavelBoleto = buscarPedido.LinhaDigitavelBoleto,
                            UrlBoleto = buscarPedido.UrlBoleto,
                            NumeroParcela = 1,
                            Ativo = true
                        };

                        _pagamentoNegocio.Insert(novaParcela);
                        _pagamentoNegocio.SaveChanges();

                        parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == idPedido && p.NumeroParcela == numeroParcela);
                    }
                    else
                    {
                        parcela = _mapper.Map<PagamentoViewModel>(buscarPedido.Pagamentos.FirstOrDefault(p => p.NumeroParcela == numeroParcela));
                    }
                }

                var pedido = _repositorio
                    .FirstNoTracking(
                        p => p.IdPedido == parcela.IdPedido
                        //"UsuarioProduto",
                        //"UsuarioProduto.Produto.Categoria",
                        //"Transacao",
                        //"Usuario",
                        //"Pagamentos"
                        );

                if (parcela.Pago)
                {
                    throw new PadraoException("parcela_paga");
                }
                if (!parcela.Ativo)
                {
                    throw new PadraoException("parcela_inativa");
                }

                parcela.Pago = true;
                parcela.Status = (int)StatusPagamento.PagoProcessado;
                parcela.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
                parcela.DataReferencia = dataReferencia;

                _pagamentoNegocio.Update(parcela);

                var usuarioProduto = _usuarioProdutoNegocio.GetNoTracking(x => x.IdPedido == pedido.IdPedido).FirstOrDefault().Adapt<UsuarioProduto>(); // pedido.UsuarioProduto.FirstOrDefault(p => p.IdPedido == pedido.IdPedido);

                switch (pedido.Tipo)
                {
                    case (int)TipoPedido.Baf:
                        InativarPedidosNaoPagos(pedido);
                        usuarioProduto.Ativo = true;
                        _usuarioProdutoNegocio.Update(new UsuarioProdutoViewModel
                        {
                            IdUsuarioProduto = usuarioProduto.IdUsuarioProduto,
                            Ativo = true,
                            IdPedido = usuarioProduto.IdPedido,
                            DataVinculo = DateTime.UtcNow.HorarioBrasilia(),
                            IdUsuario = usuarioProduto.IdUsuario,
                            IdProduto = usuarioProduto.IdProduto,
                        });
                        InativarOutrosProdutos(usuarioProduto);

                        if (!pedido.ValorPago.HasValue)
                            pedido.ValorPago = 0;

                        pedido.ValorPago += parcela.Valor;
                        _repositorio.Update(pedido);
                        _repositorio.SaveChanges();

                        if (distribuirNaRede)
                        {
                            _repositorio.DistribuirParcelaPlano(parcela, dbTransaction);
                        }
                        break;

                    case (int)TipoPedido.Saldo:
                        var lancamentoSaldo = LancamentoNegocio.GerarLancamentoSaldo(pedido);
                        _lancamentoNegocio.Insert(lancamentoSaldo);
                        var lancamentoTaxaSaldo = _lancamentoNegocio.GerarLancamentoTaxaSaldo(pedido);
                        _lancamentoNegocio.Insert(lancamentoTaxaSaldo);
                        _repositorio.SaveChanges();
                        break;

                    case (int)TipoPedido.FaturaCashbackCredenciado:
                        var pagamentoPedidosCashback = _pagamentoPedidoRepositorio
                            .Get(g =>
                                g.IdPagamento == parcela.IdPagamento &&
                                g.Pedido.Tipo == (int)TipoPedido.CashbackCredenciado,
                                "Pedido.Transacao")
                            .ToArray();
                        foreach (var pagamentoPedidoCash in pagamentoPedidosCashback)
                        {
                            _cashbackNegocio.LancarCashback(
                                pagamentoPedidoCash.Pedido.IdPedido,
                                finalizado: true,
                                dbTransaction: dbTransaction)
                                .Wait();

                            pagamentoPedidoCash.ValorPago = pagamentoPedidoCash.Valor;
                            pagamentoPedidoCash.Status = (int)StatusPagamentoPedido.PagoProcessado;
                            _pagamentoPedidoRepositorio.Update(pagamentoPedidoCash);
                        }
                        _pagamentoPedidoRepositorio.SaveChanges();

                        break;
                }

                _pagamentoNegocio.SaveChanges();

                var parcelasPagas = _pagamentoNegocio
                    .GetNoTracking(g =>
                        g.IdPedido == pedido.IdPedido &&
                        g.Pago);

                pedido.ValorPago = parcelasPagas.Sum(p => p.Valor);

                if (parcelasPagas.Count() == pedido.NumeroParcelas ||
                    pedido.ValorPago == pedido.ValorPedido)
                {
                    pedido.Pago = true;
                    pedido.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
                    pedido.Status = (int)StatusPedido.Processado;

                    var transacao = _transacaoNegocio
                        .FirstNoTracking(f => f.IdTransacao == pedido.IdTransacao);

                    transacao.IdStatus = (int)StatusTransacaoEnum.Finalizada;

                    _transacaoNegocio.Update(transacao);
                }

                _repositorio.Update(pedido);
                _repositorio.SaveChanges();

                dbTransaction.Commit();
                return true;
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public bool ReativarPedido(long idPedido, Guid idUsuario)
        {
            // Verifica se não existe outro pedido em pagamento
            var pedidos = _repositorio.Get(x => x.IdUsuario == idUsuario && !string.IsNullOrEmpty(x.Codigo) && x.IdPedido != idPedido);
            var pedido = _repositorio.FirstNoTracking(x => x.IdPedido == idPedido);

            if (pedido == null)
            {
                throw new NotFoundException("pedido_nao_encontrado");
            }

            if (pedido.Pago)
            {
                throw new PadraoException("pedido_pago");
            }

            // Verifica se não existe outro pedido em pagamento
            var pedidosEmAberto = pedidos.Where(x =>
            (x.Ativo.HasValue && x.Ativo.Value) &&
            (!x.Cancelado.HasValue || !x.Cancelado.Value) &&
            (!x.Pago && x.Pagamentos.Any(p => p.Pago)));

            if (pedidosEmAberto.Any())
            {
                throw new PadraoException("usuario_pedido_aberto");
            }

            // Reativa pedido
            pedido.Ativo = true;
            pedido.Cancelado = false;

            _repositorio.Update(pedido);

            // Gera novos boletos para as parcelas
            foreach (var pagamento in pedido.Pagamentos)
            {
                _pagamentoNegocio.ReativarPagamentoAsync(pagamento, pedido);
            }

            return true;
        }

        public async Task RemoverCompra(long idPedido)
        {
            var compra = _repositorio.First(g => g.IdPedido == idPedido,
                "Transacao",
                "UsuarioProduto",
                "PedidoDetalhe",
                "PagamentoPedido",
                "CuponCashbackPedido.CuponCashback");

            if (compra.Tipo != (int)TipoPedido.CashbackCredenciado
                && compra.Tipo != (int)TipoPedido.CashbackExterno)
            {
                throw new PadraoException("remover_compra_tipo");
            }

            if (compra.Status == (int)StatusPedido.Processado)
            {
                throw new PadraoException("remover_compra_status");
            }

            _transacaoRepositorio.DeleteRange(new List<Transacao> { compra.Transacao });
            _usuarioProdutoRepositorio.DeleteRange(compra.UsuarioProduto.ToList());
            _pedidoDetalheRepositorio.DeleteRange(compra.PedidoDetalhe.ToList());
            _pagamentoPedidoRepositorio.DeleteRange(compra.PagamentoPedido.ToList());
            if (compra.CuponCashbackPedido != null)
            {
                _cuponCashbackPedidoRepositorio.DeleteRange(new List<CuponCashbackPedido> { compra.CuponCashbackPedido });
            }
            if (compra.CuponCashbackPedido?.CuponCashback != null)
            {
                _cuponCashbackRepositorio.DeleteRange(new List<CupomCashback> { compra.CuponCashbackPedido.CuponCashback });
            }
            _repositorio.DeleteRange(new List<Pedido> { compra });

            await _repositorio.SaveChangesAsync();
        }

        public Pedido RemoverParcela(long idPedido)
        {
            var pedidoBanco = _repositorio.FirstNoTracking(p => p.IdPedido == idPedido, "Pagamentos");

            // Verifica se o pedido existe
            if (pedidoBanco != null)
            {
                throw new NotFoundException("pedido_nao_encontrado");
            }

            // Verifica se o pedido pode ter uma parcela removida
            if (pedidoBanco.Pagamentos.Where(p => !p.Pago).Count() < 2)
            {
                throw new PadraoException("pedido_unica_parcela_nao_paga_remover");
            }

            // Atualiza dados do pedido
            pedidoBanco.NumeroParcelas -= 1;
            _repositorio.Update(pedidoBanco);
            _repositorio.SaveChanges();

            // Recalcula o valor das parcelas restantes
            var novoRestante = pedidoBanco.ValorPago.Value - pedidoBanco.ValorPedido;
            var novoValor = novoRestante / pedidoBanco.NumeroParcelas;

            foreach (var parcela in pedidoBanco.Pagamentos)
            {
                var parcelaBanco = _pagamentoNegocio.FirstNoTracking(p => p.IdPagamento == parcela.IdPagamento);
                parcelaBanco.Valor = novoValor;
                _pagamentoNegocio.Update(parcelaBanco);
            }
            _pagamentoNegocio.SaveChanges();

            // Deleta a última parcela
            _pagamentoNegocio.Delete((int)pedidoBanco.Pagamentos.OrderBy(p => p.NumeroParcela).Last().IdPagamento);

            return _repositorio.FirstNoTracking(p => p.IdPedido == idPedido, "Pagamentos");
        }

        public bool RenovarBoleto(int idPedido, int numeroParcela)
        {
            var parcela = _pagamentoNegocio.FirstNoTracking(p => p.IdPedido == idPedido && p.NumeroParcela == numeroParcela);
            var pedido = _repositorio.GetById(parcela.IdPedido);

            if (parcela.Pago)
            {
                throw new PadraoException("parcela_paga");
            }
            if (!parcela.Ativo)
            {
                throw new PadraoException("parcela_inativa");
            }

            parcela = _pagamentoNegocio.CriarBoletoAsync(parcela, pedido).Result;

            parcela.DataValidade = DateTime.UtcNow.HorarioBrasilia().AddDays(8).AddMonths(parcela.NumeroParcela - 1);
            _pagamentoNegocio.Update(parcela);
            _pagamentoNegocio.SaveChanges();

            return true;
        }

        public object TotalConsumoPlanos()
        {
            return _repositorio.TotalConsumoPlanos();
        }

        private Pedido GerarAssinatura(CompraViewModel model, TipoPedido tipo, Guid idUsuarioLogado, bool ativacaoManual = false)
        {

            var produtoSelecionado = model.IdProduto != null ?
                _produtoNegocio.GetById(model.IdProduto.Value) :
                null;

            var pedido = new Pedido();
            var randomNumber = IntExtensions.GetRandom(1000);
            pedido.Codigo = $"PED{DateTime.UtcNow.HorarioBrasilia().ToString("yyMMddhhmmss")}_RN{randomNumber}";
            pedido.DataPedido = DateTime.UtcNow.HorarioBrasilia();

            pedido.Pago = false;
            pedido.Ativo = true;
            pedido.Cancelado = false;
            pedido.ValorProduto = produtoSelecionado?.Valor ?? 0;
            pedido.NumeroParcelas = model.NumParcelas;
            pedido.MeioPagamento = (int)model.MetodoDePagamento;
            pedido.DataReferencia = model.DataReferencia;
            pedido.Tipo = (int)tipo;

            pedido.GeradoManualmente = ativacaoManual;

            if (model.ValorPedido == null)
            {
                pedido.ValorPedido = produtoSelecionado.Valor;
            }
            else
            {
                pedido.ValorPedido = model.ValorPedido.Value;
            }

            if (ativacaoManual)
            {
                pedido.IdUsuario = model.IdUsuario;
            }
            else
            {
                pedido.IdUsuario = idUsuarioLogado;
            }

            pedido = _repositorio.Assinar(pedido, model.IdProduto, model.TipoTransacao, idUsuarioLogado);
            pedido.Status = (int)StatusPedido.AguardandoPagamento;

            var usuarioProduto = _usuarioProdutoNegocio.GetNoTracking(x => x.IdUsuario == idUsuarioLogado && x.Ativo).FirstOrDefault();

            var result = _pagamentoNegocio.CriarAssinaturaAsync(pedido, model.Card);

            if (result.Result.Success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        pedido.Tipo = 57;
                        pedido.CodigoReferenciaAssinatura = result.Result.Data;
                        pedido.DataPagamento = DateTime.Now;
                        pedido.Pago = result.Result.Response.Status == "active";
                        pedido.Status = result.Result.Response.Status == "active" ? 2 : (int)StatusPedido.AguardandoPagamento;

                        if (result.Result.Response is not null)
                        {
                            usuarioProduto.AssinaturaHabilitada = true;
                            usuarioProduto.DataAssinatura = result.Result.Response.CreatedAt;
                            usuarioProduto.AssinaturaDe = result.Result.Response.CurrentCycle.StartAt;
                            usuarioProduto.AssinaturaAte = result.Result.Response.CurrentCycle.EndAt;
                            usuarioProduto.AssinaturaProximaCobranca = result.Result.Response.NextBillingAt;
                        }

                        _context.Entry(_mapper.Map<UsuarioProduto>(usuarioProduto)).State = EntityState.Modified;
                        _context.SaveChanges();

                        var pedidoDetalhe = new PedidoDetalhe()
                        {
                            IdPedido = pedido.IdPedido,
                            DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                            Ativo = true,
                            Descricao = $"Assinatura do plano realizada",
                            IdStatus = (int)StatusTransacaoEnum.Aprovado,
                            IdUsuario = pedido.IdUsuario,
                            DataAssinatura = result.Result.Response.CreatedAt,
                            AssinaturaDe = result.Result.Response.CurrentCycle.StartAt,
                            AssinaturaAte = result.Result.Response.CurrentCycle.EndAt,
                            AssinaturaProximaCobranca = result.Result.Response.NextBillingAt,
                        };

                        long idTransacao = Convert.ToInt32(pedido.IdTransacao.Value);
                        var transacao = _transacaoNegocio.FirstNoTracking(x => x.IdTransacao == idTransacao);
                        transacao.IdStatus = 2;

                        _context.PedidoDetalhe.Add(pedidoDetalhe);
                        _context.SaveChanges();

                        _repositorio.Update(pedido);
                        _repositorio.SaveChanges();

                        //_transacaoNegocio.Update(transacao);
                        _context.Entry(_mapper.Map<Transacao>(transacao)).State = EntityState.Modified;
                        _transacaoNegocio.SaveChanges();

                        transaction.CommitAsync();

                        _procedures.sp_LancarCashback_Assinatura(usuarioProduto.IdUsuarioProduto);

                        var usuario = _usuarioNegocio.GetById(idUsuarioLogado);
                        var objectEmail = new ObjEmailUtilitis
                        {
                            Data = DateTime.UtcNow.HorarioBrasilia(),
                            From = _appSettings.EmailToSmtp,
                            FromName = _appSettings.FromName,
                            DestinationName = usuario.Nome,
                            Subject = "Assinatura Quanta Plus realizada com sucesso ✨",
                            To = usuario.Email,
                            EmailSuporte = _appSettings.EmailSuporte,
                            SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                        };
                        
                        var rootSite = _configNegocio.BuscarRootSite().Valor;
                        
                        if (string.IsNullOrEmpty(rootSite))
                        {
                            _cache.SetItem(CacheKeys.RootSite, _configNegocio.BuscarPelaChave("URL_BASE").Valor);
                            rootSite = Convert.ToString(_cache.GetItem(CacheKeys.RootSite));
                        }

                        var body = new Dictionary<string, string>
                        {
                            { "{{ name }}", objectEmail.DestinationName },
                        };

                        var emailUtil = new EmailUtilitis();
                        _ = emailUtil.EnviarEmail(body, _appSettings.AssinaturaConfirmada, null, objectEmail);

                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackAsync();

                        throw;

                        //TODO: Cancelar a assinatura via API

                        //TODO: Enviar email da recusa com a justificativa
                    }
                }
            }
            else
            {
                var pedidoDetalhe = new PedidoDetalhe()
                {
                    IdPedido = pedido.IdPedido,
                    DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                    Ativo = true,
                    Descricao = "Assinatura do plano retornou : " + result.Result.Message,
                    IdStatus = (int)StatusTransacaoEnum.Aprovado,
                    IdUsuario = pedido.IdUsuario,
                };

                //pedido.PedidoDetalhe.Add(pedidoDetalhe);
                _repositorio.Update(pedido);

                _context.PedidoDetalhe.Add(pedidoDetalhe);
                _context.SaveChanges();
            }

            return pedido;
        }

        private Pedido GerarAssinaturaAnual(CompraViewModel model, TipoPedido tipo, Guid idUsuarioLogado, bool ativacaoManual = false)
        {

            var produtoSelecionado = model.IdProduto != null ?
                _produtoNegocio.GetById(model.IdProduto.Value) :
                null;

            var pedido = new Pedido();
            var randomNumber = IntExtensions.GetRandom(1000);
            pedido.Codigo = $"PED{DateTime.UtcNow.HorarioBrasilia().ToString("yyMMddhhmmss")}_RN{randomNumber}";
            pedido.DataPedido = DateTime.UtcNow.HorarioBrasilia();

            pedido.Pago = false;
            pedido.Ativo = true;
            pedido.Cancelado = false;
            pedido.ValorProduto = produtoSelecionado?.Valor ?? 0;
            pedido.NumeroParcelas = model.NumParcelas;
            pedido.MeioPagamento = (int)model.MetodoDePagamento;
            pedido.DataReferencia = model.DataReferencia;
            pedido.Tipo = (int)tipo;

            pedido.GeradoManualmente = ativacaoManual;

            if (model.ValorPedido == null)
            {
                pedido.ValorPedido = produtoSelecionado.Valor;
            }
            else
            {
                pedido.ValorPedido = model.ValorPedido.Value;
            }

            if (ativacaoManual)
            {
                pedido.IdUsuario = model.IdUsuario;
            }
            else
            {
                pedido.IdUsuario = idUsuarioLogado;
            }

            pedido = _repositorio.Assinar(pedido, model.IdProduto, model.TipoTransacao, idUsuarioLogado);
            pedido.Status = (int)StatusPedido.AguardandoPagamento;

            var usuarioProduto = _usuarioProdutoNegocio.GetNoTracking(x => x.IdUsuario == idUsuarioLogado && x.Ativo).FirstOrDefault();

            var result = _pagamentoNegocio.CriarAssinaturaAsync(pedido, model.Card);

            if (result.Result.Success)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        pedido.CodigoReferenciaAssinatura = result.Result.Data;
                        pedido.DataPagamento = DateTime.Now;
                        pedido.Pago = result.Result.Response.Status == "active";
                        pedido.Status = result.Result.Response.Status == "active" ? (int)StatusPedido.Pago : (int)StatusPedido.AguardandoPagamento;

                        if (result.Result.Response is not null)
                        {
                            usuarioProduto.AssinaturaHabilitada = true;
                            usuarioProduto.DataAssinatura = result.Result.Response.CreatedAt;
                            usuarioProduto.AssinaturaDe = result.Result.Response.CurrentCycle.StartAt;
                            usuarioProduto.AssinaturaAte = result.Result.Response.CurrentCycle.EndAt;
                            usuarioProduto.AssinaturaProximaCobranca = result.Result.Response.NextBillingAt;
                        }

                        _context.Entry(_mapper.Map<UsuarioProduto>(usuarioProduto)).State = EntityState.Modified;
                        _context.SaveChanges();

                        var pedidoDetalhe = new PedidoDetalhe()
                        {
                            IdPedido = pedido.IdPedido,
                            DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                            Ativo = true,
                            Descricao = $"Assinatura do plano realizada",
                            IdStatus = (int)StatusTransacaoEnum.Aprovado,
                            IdUsuario = pedido.IdUsuario,
                        };

                        _context.PedidoDetalhe.Add(pedidoDetalhe);
                        _context.SaveChanges();

                        _repositorio.Update(pedido);
                        _repositorio.SaveChanges();

                        transaction.CommitAsync();

                        //TODO: Enviar e-mail de assinatura realizada com sucesso

                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackAsync();
                    }
                }
            }
            else
            {
                var pedidoDetalhe = new PedidoDetalhe()
                {
                    IdPedido = pedido.IdPedido,
                    DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                    Ativo = true,
                    Descricao = "Assinatura do plano retornou : " + result.Result.Message,
                    IdStatus = (int)StatusTransacaoEnum.Aprovado,
                    IdUsuario = pedido.IdUsuario,
                };

                pedido.PedidoDetalhe.Add(pedidoDetalhe);
                _repositorio.Update(pedido);

                _context.PedidoDetalhe.Add(pedidoDetalhe);
                _context.SaveChanges();
            }

            return pedido;
        }

        private Pedido GerarPedido(CompraViewModel model, TipoPedido tipo, UsuarioViewModel usuario, Guid idUsuarioLogado, bool ativacaoManual = false, bool contabilizarPontacao = true, bool distribuirParcelas = true, bool gerarBoleto = true)
        {
            decimal taxaCompra;

            var produtoSelecionado = model.IdProduto != null ?
                _produtoNegocio.GetById(model.IdProduto.Value) :
                null;

            if (model.NumParcelas < 1)
            {
                model.NumParcelas = 1;
            }

            var pedido = new Pedido();
            var randomNumber = IntExtensions.GetRandom(1000);
            pedido.Codigo = $"PED{DateTime.UtcNow.HorarioBrasilia().ToString("yyMMddhhmmss")}_RN{randomNumber}";
            pedido.DataPedido = DateTime.UtcNow.HorarioBrasilia();

            pedido.Pago = false;
            pedido.Ativo = true;
            pedido.Cancelado = false;
            pedido.ValorProduto = produtoSelecionado?.Valor ?? 0;
            pedido.NumeroParcelas = model.NumParcelas;
            pedido.MeioPagamento = (int)model.MetodoDePagamento;
            pedido.DataReferencia = model.DataReferencia;
            pedido.Tipo = (int)tipo;

            pedido.ContabilizarPontuacao = contabilizarPontacao;
            pedido.GeradoManualmente = ativacaoManual;

            if (model.ValorPedido == null)
            {
                pedido.ValorPedido = produtoSelecionado.Valor;
            }
            else
            {
                pedido.ValorPedido = model.ValorPedido.Value;
            }

            if (model.TaxaCompra == null)
            {
                var configuracoes = _configNegocio.GetFromCache();
                taxaCompra = Convert.ToDecimal(configuracoes.FirstOrDefault(c => c.Chave.Equals("TAXA_COMPRA")).Valor);
            }
            else
            {
                taxaCompra = model.TaxaCompra.Value;
            }
            if (model.ValorTaxa == null)
            {
                pedido.ValorTaxa = pedido.ValorPedido * taxaCompra;
            }
            else
            {
                pedido.ValorTaxa = model.ValorTaxa.Value;
            }

            if (contabilizarPontacao)
            {
                pedido.ReaisPorPonto = model.ReaisPorPonto.HasValue ?
                    model.ReaisPorPonto.Value :
                    produtoSelecionado?.ReaisPorPonto ?? 0;
            }

            if (ativacaoManual)
            {
                pedido.IdUsuario = model.IdUsuario;
            }
            else
            {
                pedido.IdUsuario = idUsuarioLogado;
            }

            //BUSCAR DA LIONBIT
            pedido.EnderecoDeposito = "";

            // Usuário tem pedidos não pagos
            var pedidosEmAberto = _repositorio
                .GetNoTracking(p =>
                    p.IdUsuario == pedido.IdUsuario &&
                    !p.Pago &&
                    !string.IsNullOrEmpty(p.Codigo) &&
                    (!p.Cancelado.HasValue || !p.Cancelado.Value) &&
                    p.Ativo.HasValue &&
                    p.Ativo.Value)
                .ToArray();

            if (pedidosEmAberto.Any())
            {
                foreach (var pedAberto in pedidosEmAberto)
                {
                    if (pedAberto.NumeroParcelas > 1)
                    {
                        var parcelasPedidoAberto = _pagamentoNegocio.ObterPagamentos(pedAberto.IdPedido);

                        foreach (var par in parcelasPedidoAberto.Where(p => !p.Pago))
                        {
                            par.Ativo = false;
                            par.Status = (int)StatusPagamento.Cancelado;
                            _pagamentoNegocio.InativarPagamento(par);
                        }
                    }

                    pedAberto.Ativo = false;
                    _repositorio.Update(pedAberto);
                }
            }

            if (tipo == TipoPedido.Baf)
            {
                var produtoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(idUsuarioLogado);
                /// Usuário tem produto ativo
                if (produtoAtivo != null)
                {
                    if (produtoAtivo.Produto.Pontos > produtoSelecionado.Pontos && !pedido.GeradoManualmente)
                    {
                        throw new PadraoException("compra_upgrade_produto_menor");
                    }
                    else
                    {
                        pedido.ValorPedido = pedido.ValorPedido - ObterValorPagoBaf(idUsuarioLogado);
                        pedido.ValorTaxa = pedido.ValorPedido * taxaCompra;
                    }
                }
            }

            pedido = _repositorio.Comprar(pedido, model.IdProduto, model.TipoTransacao, idUsuarioLogado);
            var parcelas = new List<Pagamento>();
            var valor = Math.Round(pedido.ValorPedido / model.NumParcelas, 2);

            // Cria as parcelas já pagas
            if (model.NumParcelasPagas > 0)
            {
                pedido.Status = (int)StatusPedido.EmPagamento;
                pedido.ValorPago = 0;
                for (int i = 1; i <= model.NumParcelasPagas; i++)
                {
                    parcelas.Add(new Pagamento
                    {
                        Status = (int)StatusPagamento.PagoProcessado,
                        Valor = valor,
                        DataPagamento = DateTime.UtcNow.HorarioBrasilia(),
                        DataReferencia = model.DataReferencia ?? DateTime.UtcNow.HorarioBrasilia(),
                        DataValidade = model.DataReferencia ?? DateTime.UtcNow.HorarioBrasilia(),
                        IdPedido = pedido.IdPedido,
                        Pago = true,
                        Ativo = true,
                        NumeroParcela = i,
                        CodigoReferenciaBoleto = "Parcela paga manualmente",
                        LinhaDigitavelBoleto = "Parcela paga manualmente",
                        UrlBoleto = "Parcela paga manualmente",
                    });


                    pedido.ValorPago += valor;
                }

                if (tipo == TipoPedido.Baf)
                {
                    // Inativa produtos anteriores
                    var produtosUsuario = _usuarioProdutoNegocio
                        .GetNoTracking(p => p.IdUsuario == pedido.IdUsuario && p.Ativo);
                    foreach (var prod in produtosUsuario)
                    {
                        prod.Ativo = false;
                    }
                    _usuarioProdutoNegocio.UpdateRange(produtosUsuario);

                    // Ativa o novo produto do usuário
                    var produtoAtivacao = pedido.UsuarioProduto
                        .First(p => p.IdProduto == model.IdProduto);
                    produtoAtivacao.Ativo = true;
                }
            }
            else
            {
                pedido.Status = (int)StatusPedido.AguardandoPagamento;
            }

            for (int i = model.NumParcelasPagas + 1, j = 0; i <= model.NumParcelas; i++, j++)
            {

                DateTime dataValidade;

                if (j == 0)
                {
                    dataValidade = DateTime.Now.AddDays(8);
                }
                else
                {
                    var proximoMes = DateTime.Now.AddMonths(j + 1);

                    if (proximoMes.Month == 2)
                    {
                        int ultimoDiaFevereiro = DateTime.DaysInMonth(proximoMes.Year, 2);
                        dataValidade = new DateTime(proximoMes.Year, 2, Math.Min(ultimoDiaFevereiro, 28));
                    }
                    else
                    {
                        dataValidade = new DateTime(proximoMes.Year, proximoMes.Month, 30);
                    }
                }

                parcelas.Add(new Pagamento
                {
                    IdPedido = pedido.IdPedido,
                    Pago = false,
                    Valor = valor,
                    NumeroParcela = i,
                    DataValidade = dataValidade,
                    Ativo = true
                });
            }


            parcelas = _pagamentoNegocio.CriarParcelasAsync(parcelas, pedido, gerarBoleto)
                .Result
                .OrderBy(p => p.DataValidade).ToList();

            if (distribuirParcelas)
            {
                foreach (var parcela in parcelas.Where(p => p.Pago))
                {
                    var par = _pagamentoNegocio.FirstNoTracking(p => p.IdPagamento == parcela.IdPagamento);
                    _repositorio.DistribuirParcelaPlano(par);
                }
            }

            pedido.Pagamentos = parcelas;
            pedido.UrlBoleto = parcelas.FirstOrDefault(p => !p.Pago)?.UrlBoleto ?? "Pedido pago manualmente";
            pedido.CodigoReferenciaBoleto = parcelas.FirstOrDefault(p => !p.Pago)?.CodigoReferenciaBoleto ?? "Pedido pago manualmente";
            pedido.LinhaDigitavelBoleto = parcelas.FirstOrDefault(p => !p.Pago)?.LinhaDigitavelBoleto ?? "Pedido pago manualmente";

            if (pedido.NumeroParcelas <= parcelas.Where(p => p.Pago).Count())
            {
                pedido.Pago = true;
                pedido.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
                pedido.Status = (int)StatusPedido.Pago;
                pedido.Transacao.IdStatus = 2;
            }


            _repositorio.Update(pedido);
            _repositorio.SaveChanges();
            return pedido;
        }

        private void InativarOutrosProdutos(UsuarioProduto usuarioProdutoAtivo)
        {
            // Inativa outros produtos ativos anteriores
            var usuarioProdutosAntigos = _usuarioProdutoNegocio
                .GetNoTracking(p =>
                    p.IdUsuario == usuarioProdutoAtivo.IdUsuario &&
                    p.IdUsuarioProduto != usuarioProdutoAtivo.IdUsuarioProduto &&
                    p.Ativo);

            foreach (var usuarioProdutoAntigo in usuarioProdutosAntigos)
            {
                usuarioProdutoAntigo.Ativo = false;
                _usuarioProdutoNegocio.Update(usuarioProdutoAntigo);
            }
        }

        private void InativarPedidosNaoPagos(Pedido pedido)
        {
            // Inativa pedidos não pagos
            var pedidosNaoPagos = _repositorio
                .GetNoTracking(p =>
                    p.IdPedido != pedido.IdPedido &&
                    p.IdUsuario == pedido.IdUsuario &&
                    !p.Pago &&
                    p.Tipo == (int)TipoPedido.Baf,
                    "Pagamentos");

            if (pedidosNaoPagos.Any(p => p.Ativo.HasValue && p.Ativo.Value))
            {
                foreach (var pedidoNaoPago in pedidosNaoPagos)
                {
                    pedidoNaoPago.Ativo = false;

                    if (pedidoNaoPago.Pagamentos.Any())
                    {
                        foreach (var pagamento in pedido.Pagamentos)
                        {
                            pagamento.Ativo = false;
                            _pagamentoRepositorio.Update(pagamento);
                        }
                    }

                    _repositorio.Update(pedidoNaoPago);
                }
            }
        }
    }
}
