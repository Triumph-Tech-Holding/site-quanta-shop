using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Afilio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class CashbackNegocio : ICashbackNegocio
    {
        private IPedidoRepositorio _pedidoRepositorio;
        private IPedidoDetalheRepositorio _pedidoDetalheRepositorio;
        private IAnuncianteRepositorio _anuncianteRepositorio;
        private ITipoRepositorio _tipoRepositorio;
        private ITransacaoRepositorio _transacaoRepositorio;
        private IProceduresRepositorio _proceduresRepositorio;
        private IUsuarioRepositorio _usuarioRepositorio;
        private ICuponCashbackRepositorio _cupomCashbackRepositorio;
        private readonly AppSettings _appSettings;

        public CashbackNegocio(
            IPedidoRepositorio pedidoRepositorio,
            IPedidoDetalheRepositorio pedidoDetalheRepositorio,
            IAnuncianteRepositorio anuncianteRepositorio,
            ITipoRepositorio tipoRepositorio,
            ITransacaoRepositorio transacaoRepositorio,
            IProceduresRepositorio proceduresRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ICuponCashbackRepositorio cuponCashbackRepositorio,
            IOptions<AppSettings> appSettings)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _pedidoDetalheRepositorio = pedidoDetalheRepositorio;
            _anuncianteRepositorio = anuncianteRepositorio;
            _tipoRepositorio = tipoRepositorio;
            _transacaoRepositorio = transacaoRepositorio;
            _proceduresRepositorio = proceduresRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _cupomCashbackRepositorio = cuponCashbackRepositorio;
            _appSettings = appSettings.Value;
        }

        public async Task<Transacao> CriarTransacaoCashbackAsync(Pedido pedido, Anunciante anunciante = null)
        {
            string estabelecimento = null;
            if (
                pedido.IdAwinTransaction.HasValue ||
                pedido.IdVendaZanox.HasValue)
            {
                estabelecimento = anunciante?.Nome;
            }
            else if (pedido.IdUsuarioComerciante.HasValue)
            {
                estabelecimento = pedido.UsuarioComerciante?.Credenciamento?.Estabelecimento;
            }

            int tipoTransacao;
            switch (pedido.Tipo)
            {
                case (int)TipoPedido.CashbackCredenciado:
                    tipoTransacao = await _tipoRepositorio
                        .Get(t => t.Chave == "CHBLF")
                        .Select(s => s.IdTipo)
                        .FirstAsync();
                    break;
                case (int)TipoPedido.CashbackExterno:
                    tipoTransacao = await _tipoRepositorio.
                        Get(t => t.Chave == "CHBK")
                        .Select(s => s.IdTipo)
                        .FirstAsync();
                    break;
                default:
                    throw new PadraoException("tipo_pedido");
            }

            var transacao = new Transacao
            {
                IdUsuario = pedido.IdUsuario,
                IdTipo = tipoTransacao,
                ValorPrincipal = pedido.Cashback.Value,
                DataTransacao = DateTime.UtcNow.HorarioBrasilia(),
                Descricao = $"Cashback referente à compra em: {estabelecimento}",
                IdStatus = (int)StatusTransacaoEnum.EmProcessamento,
                Ativo = true,
                ComissaoTotal = pedido.Cashback.Value,
                IdVendaAwin = pedido.IdAwinTransaction,
                //IdVendaAfilio = pedido.IdVendaAfilio,
                IdVendaZanox = pedido.IdVendaZanox,
                IdAnunciante = anunciante?.IdAnunciante
            };

            return transacao;
        }

        public async Task InserirCupomFiscal(string chaveAcesso, string urlChaveDeAcessoNF, bool chaveManual, Guid idUsuario)
        {
            var usuario = await _usuarioRepositorio.Get(u => u.IdUsuario == idUsuario).FirstOrDefaultAsync();

            var chaveExiste = _cupomCashbackRepositorio.Any(cc => cc.ChaveDeAcessoNF == chaveAcesso);

            if (chaveExiste)
            {
                throw new Exception("Nota já cadastrada no sistema, por favor tente outra nota");
            }
            else
            {
                var cupomGerado = new CupomCashback
                {
                    Token = Guid.NewGuid().ToString(),
                    Valor = 0,
                    PercentualCashback = 0,
                    Documento = usuario.Documento,
                    DataCompra = DateTime.Now,
                    Descricao = "",
                    MeioPagamento = 0,
                    IdComerciante = null,
                    CompraUsuario = true,
                    ComprovanteCompra = "",
                    CompraUsuarioAprovada = false,
                    ChaveDeAcessoNF = chaveAcesso,
                    Status = 9,
                    ChaveManual = chaveManual,
                    UrlChaveDeAcessoNF = urlChaveDeAcessoNF
                };

                _cupomCashbackRepositorio.Insert(cupomGerado);
                _cupomCashbackRepositorio.SaveChanges();

                var objectEmail = new ObjEmailUtilitis
                {
                    Data = DateTime.UtcNow.HorarioBrasilia(),
                    From = "contato@quantashop.com.br",
                    FromName = _appSettings.FromName,
                    DestinationName = usuario.Nome,
                    Subject = "Estamos processando seu cashback 🚀",
                    To = usuario.Email,
                    EmailSuporte = _appSettings.EmailSuporte,
                    SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                };

                var body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName } };

                var emailUtil = new EmailUtilitis();
                await emailUtil.EnviarEmail(body, _appSettings.EstamosProcessandoCashback, null, objectEmail);
            }
        }

        public async Task LancarCashback(long idPedido, Anunciante anunciante = null, Guid? IdUsuario = null, bool finalizado = true, IDbContextTransaction dbTransaction = null)
        {
            string descricao;
            StatusTransacaoEnum status;
            IDbContextTransaction managedDbTransaction = null;

            if (dbTransaction == null)
            {
                managedDbTransaction = dbTransaction = _pedidoRepositorio.GetTransaction();
            }

            try
            {
                var pedido = await _pedidoRepositorio
                    .Get(w =>
                        w.IdPedido == idPedido &&
                        (w.Tipo == (int)TipoPedido.CashbackExterno ||
                            w.Tipo == (int)TipoPedido.CashbackCredenciado))
                    .Include(i => i.PedidoDetalhe)
                    .Include(i => i.Transacao)
                        .ThenInclude(i => i.Anunciante)
                    .Include(i => i.UsuarioComerciante)
                        .ThenInclude(t => t.Credenciamento)
                    .FirstOrDefaultAsync();

                var statusAnterior = pedido.Transacao?.IdStatus;
                anunciante ??= pedido?.Transacao?.Anunciante;

                if (finalizado)
                {
                    var estabelecimento = anunciante?.Nome ?? pedido.UsuarioComerciante?.Credenciamento?.Estabelecimento;
                    descricao = $"Compra em: {estabelecimento} foi finalizado!";
                    status = StatusTransacaoEnum.Finalizada;
                }
                else
                {
                    descricao = "Cashback pago pela Quanta Shop";
                    status = StatusTransacaoEnum.PagoBigCash;
                }

                var pedidoDetalhe = new PedidoDetalhe
                {
                    Descricao = descricao,
                    DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                    Ativo = true,
                    IdPedido = pedido.IdPedido,
                    IdStatus = (int)status,
                    IdUsuario = IdUsuario
                };

                _pedidoDetalheRepositorio.Insert(pedidoDetalhe);

                if (pedido.Transacao == null)
                {
                    var transacao = await CriarTransacaoCashbackAsync(pedido, anunciante);

                    _transacaoRepositorio.Insert(transacao);
                    await _transacaoRepositorio.SaveChangesAsync();

                    pedido.IdTransacao = transacao.IdTransacao;
                    pedido.Transacao = transacao;
                }

                pedido.Transacao.IdStatus = (int)status;
                _transacaoRepositorio.Update(pedido.Transacao);

                _pedidoRepositorio.Update(pedido);
                await _pedidoRepositorio.SaveChangesAsync();


                if (statusAnterior != (int)StatusTransacaoEnum.PagoBigCash &&
                    statusAnterior != (int)StatusTransacaoEnum.Finalizada)
                    _proceduresRepositorio.spc_LancarCashback(
                        pedido.IdPedido,
                        pedido.Cashback.Value,
                        dbTransaction);

                if (finalizado)
                {
                    pedido.Status = (int)StatusPedido.Processado;
                    pedido.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
                    if (pedido.Tipo == (int)TipoPedido.CashbackExterno)
                    {
                        pedido.Pago = true;
                        pedido.ValorPago = pedido.Cashback;
                    }

                    _pedidoRepositorio.Update(pedido);
                    await _pedidoRepositorio.SaveChangesAsync();
                }
                else
                {
                    pedido.Status = (int)StatusPedido.PagoBigCash;

                    _pedidoRepositorio.Update(pedido);
                    await _pedidoRepositorio.SaveChangesAsync();
                }

                if (managedDbTransaction != null)
                {
                    await managedDbTransaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                managedDbTransaction?.Rollback();

                throw;
            }
        }
    }
}
