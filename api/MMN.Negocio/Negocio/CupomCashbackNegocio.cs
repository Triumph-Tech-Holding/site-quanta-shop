using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Enum;
using System.Threading.Tasks;
using System;
using MMN.Util.Extensions;
using MMN.Integracoes.Afilio;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Http.HttpResults;
using MMN.Util.Model;
using MMN.Util.Util;
using Microsoft.Extensions.Options;

namespace MMN.Negocio.Negocio
{
    public class CupomCashbackNegocio : BaseNegocioNovo<CupomCashback>, ICupomCashbackNegocio
    {
        private readonly ICuponCashbackRepositorio _repositorio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly ICredenciamentoRepositorio _credenciamentoRepositorio;
        private readonly ICuponCashbackPedidoRepositorio _cuponCashbackPedidoRepositorio;
        private readonly ITransacaoRepositorio _transacaoRepositorio;
        private readonly AppSettings _appSettings;

        public CupomCashbackNegocio(
            ICuponCashbackRepositorio repositorio,
            IMapper mapper,
            IUsuarioNegocio usuarioNegocio,
            IPedidoRepositorio pedidoRepositorio,
            ICuponCashbackPedidoRepositorio cuponCashbackPedidoRepositorio,
            ICredenciamentoRepositorio credenciamentoRepositorio,
            ITransacaoRepositorio transacaoRepositorio,
            IOptions<AppSettings> appSettings
            ) : base(repositorio)
        {
            _repositorio = repositorio;
            _usuarioNegocio = usuarioNegocio;
            _pedidoRepositorio = pedidoRepositorio;
            _credenciamentoRepositorio = credenciamentoRepositorio;
            _cuponCashbackPedidoRepositorio = cuponCashbackPedidoRepositorio;
            _transacaoRepositorio = transacaoRepositorio;
            _appSettings = appSettings.Value;
        }

        public async Task<CupomCashback> CriarCuponAsync(CupomCashback cupom, Guid idUsuarioLogado, bool resgatar = false)
        {
            UsuarioViewModel usuarioCompra = null;

            if (cupom.CompraUsuario)
            {
                var compraValida = await CheckCompra(idUsuarioLogado, cupom);
                if (!compraValida)
                {
                    throw new Exception("Cupom já inserido, por favor, tente outro cupom");
                }
            }

            if (resgatar)
            {
                usuarioCompra = _usuarioNegocio.FirstNoTracking(g => g.Documento == cupom.Documento);

                if (usuarioCompra == null)
                {
                    throw new PadraoException("usuario_nao_encontrado");
                }
            }

            cupom = await CriarCuponAsync(cupom, idUsuarioLogado);


            if (resgatar)
            {
                await ResgatarCuponAsync(cupom.Token, usuarioCompra.IdUsuario);
            }

            return cupom;
        }

        private async Task<CupomCashback> CriarCuponAsync(CupomCashback cupom, Guid idUsuarioLogado)
        {
            var buscaCupom = await _repositorio
                .GetNoTracking(g => g.Token == cupom.Token)
                .SingleOrDefaultAsync();

            if (buscaCupom != null)
            {
                throw new PadraoException("token_venda_duplicado");
            }
            if (cupom.IdComerciante.HasValue)
            {
                var usuarioLogado = _usuarioNegocio.GetById(cupom.IdComerciante.Value, "Grupo", "Credenciamento");

                if (usuarioLogado.Grupo.Descricao != "Comerciante")
                {
                    throw new UnauthorizedException("token_venda_nao_comerciante");
                }

                if (usuarioLogado.Credenciamento.Status != StatusCredenciamento.Aprovado)
                {
                    throw new PadraoException("comerciante_nao_aprovado");
                }
            }


            var cupomGerado = new CupomCashback
            {
                Token = cupom.Token,
                Valor = cupom.Valor,
                PercentualCashback = cupom.PercentualCashback,
                Documento = cupom.Documento,
                DataCompra = cupom.DataCompra,
                Descricao = cupom.Descricao,
                MeioPagamento = cupom.MeioPagamento,
                IdComerciante = cupom.IdComerciante,
                CompraUsuario = cupom.CompraUsuario,
                ComprovanteCompra = cupom.ComprovanteCompra,
                CompraUsuarioAprovada = cupom.CompraUsuario == true ? false : true,
                Status = cupom.CompraUsuario == true ? 1 : 6,
                UrlChaveDeAcessoNF = cupom.UrlChaveDeAcessoNF,
                ChaveManual = cupom.ChaveManual,
            };

            _repositorio.Insert(cupomGerado);
            _repositorio.SaveChanges();


            return cupomGerado;
        }

        public async Task<CupomCashback> ObterCuponAsync(string token)
        {
            var cupon = await _repositorio
                .Get(g => g.Token == token)
                .Include(i => i.Comerciante)
                    .ThenInclude(t => t.Credenciamento)
                .Include(i => i.CuponCashbackPedido)
                    .ThenInclude(t => t.Pedido)
                .AsNoTracking()
                .SingleOrDefaultAsync();


            return cupon;
        }

        public async Task ResgatarCuponAsync(string token, Guid idUsuarioLogado)
        {
            try
            {
                var cupon = await _repositorio
                .Get(g => g.Token == token)
                .Include(i => i.CuponCashbackPedido)
                .SingleOrDefaultAsync();

                var usuario = _usuarioNegocio.GetById(idUsuarioLogado, "Grupo", "Credenciamento");

                if (cupon == null)
                {
                    throw new NotFoundException("token_venda_nao_encontrado");
                }
                else if (cupon.CuponCashbackPedido != null)
                {
                    throw new NotFoundException("token_venda_resgatado");
                }

                if (usuario.Grupo.Descricao != "Admin" && cupon.CompraUsuario && cupon.Status != 10)
                {
                    if (cupon.CompraUsuario)
                    {
                        if (usuario.IdUsuario != cupon.IdComerciante)
                        {
                            throw new UnauthorizedException("");
                        }
                    }
                    else
                    {
                        if (usuario.Documento != cupon.Documento)
                        {
                            throw new UnauthorizedException("");
                        }
                    }
                }


                var compraViewModel = new EfetuarCompraViewModel
                {
                    Descricao = cupon.Descricao,
                    IdComerciante = cupon.IdComerciante,
                    TipoPagamento = (EnumTipoPagamento)cupon.MeioPagamento,
                    Valor = cupon.Valor
                };

                //Conversar com o Eric sobre essa regra de criar o pedido dentro do criar venda

                if (!await _pedidoRepositorio.CriarPedidoCredenciado(compraViewModel, idUsuarioLogado, cupon))
                {
                    throw new Exception("Não foi possível efetuar a compra");
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                throw;
            }
        }

        public async Task<bool> AprovarReprovarCupomAsync(string token, Guid idUsuarioLogado, bool aprovado, int status, string justificativa = null, bool informarCliente = false)
        {
            try
            {
                var cupom = await _repositorio.BuscarPeloTokenAsync(token);

                var usuarioLogado = _usuarioNegocio.GetById(idUsuarioLogado, "Grupo", "Credenciamento");

                if (usuarioLogado.Grupo.Descricao != "Comerciante" && usuarioLogado.Grupo.Descricao != "Admin" && cupom.Status != 10)
                    throw new UnauthorizedException("token_venda_nao_comerciante");

                if (cupom == null)
                    throw new NotFoundException("token_venda_nao_encontrado");

                cupom.CompraUsuarioAprovada = aprovado;
                cupom.Status = status;
                cupom.Justificativa = justificativa;

                _repositorio.Update(cupom);
                await _repositorio.SaveChangesAsync();

                if (aprovado)
                    await ResgatarCuponAsync(token, idUsuarioLogado);         
                
                if(!aprovado && status != 4 && informarCliente)
                {
                    var emailUtil = new EmailUtilitis();
                    Dictionary<string, string> body = null;
                    ObjEmailUtilitis objectEmail = new()
                    {
                        Data = DateTime.UtcNow.HorarioBrasilia(),
                        From = "contato@quantashop.com.br",
                        FromName = _appSettings.FromName,
                        Subject = "Seu comprovante de compra foi recusado 😕",
                        EmailSuporte = _appSettings.EmailSuporte,
                        SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                    };

                    var usuario = _usuarioNegocio.FirstNoTracking(x => x.Documento == cupom.Documento);

                    objectEmail.DestinationName = usuario.Nome;                    
                    objectEmail.To = usuario.Email;

                    body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName }, { "{{ cause }}", justificativa } };
                    await emailUtil.EnviarEmail(body, _appSettings.NotaRecusada, null, objectEmail);
                }

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;

                return false;
            }
        }

        public async Task<bool> AprovarReprovarCupomAsync(CupomCashback cupom, bool aprovado, int status)
        {
            try
            {

                var comerciante = _usuarioNegocio.GetById(cupom.IdComerciante.Value, "Grupo", "Credenciamento");

                if (comerciante.Grupo.Descricao != "Comerciante" && comerciante.Grupo.Descricao != "Admin" && cupom.Status != 10)
                    throw new UnauthorizedException("token_venda_nao_comerciante");

                if (cupom == null)
                    throw new NotFoundException("token_venda_nao_encontrado");

                cupom.CompraUsuarioAprovada = aprovado;
                cupom.Status = status;

                _repositorio.Update(cupom);
                await _repositorio.SaveChangesAsync();

                if (aprovado)
                    await ResgatarCuponAsync(cupom.Token, comerciante.IdUsuario);

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException is null ? ex.Message : ex.InnerException.Message;

                return false;
            }
        }

        public async Task<object> ObterCuponsCompraUsuarioAdmin(FiltroViewModel.FiltroVendasCredenciando viewModel)
        {

            var cupons = _repositorio.Get(g => g.DataCompra >= viewModel.DataInicio && g.DataCompra <= viewModel.DataFim && g.Status != 9).
                Include(i => i.Comerciante).Where(g => g.CompraUsuario == true);

            var totalPages = (int)Math.Ceiling((double)cupons.Count() / viewModel.QuantidadePorPagina);
            var totalRegistros = cupons.Count();
            return new
            {
                totalRegistros,
                totalPages,
                data = cupons.Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1)).Take(viewModel.QuantidadePorPagina).Select(s => new
                {
                    s.Comerciante.Nome,
                    Usuario = s.Comerciante.Nome,
                    s.Descricao,
                    s.Valor,
                    s.DataCompra,
                    s.ComprovanteCompra,
                    MeioPagamento = ((EnumTipoPagamento)s.MeioPagamento).GetDescription(),
                    s.CompraUsuarioAprovada,
                    s.PercentualCashback,
                    s.Token,
                    ValorRecebido = s.Valor - (s.Valor * s.PercentualCashback),
                    ValorCashbackUsuario = s.Valor * s.PercentualCashback,
                    s.Status
                }).OrderByDescending(o => o.DataCompra).ToList()
            };
        }

        public async Task<object> ObterCuponsCompraUsuarioCredenciado(FiltroViewModel.FiltroVendasCredenciando viewModel)
        {
            try
            {
                return await _repositorio.ObterCuponsCompraUsuarioCredenciado(viewModel);

                //var cupons = _repositorio.Get(g => g.DataCompra >= viewModel.DataInicio && g.DataCompra <= viewModel.DataFim && g.IdComerciante == viewModel.IdCredenciado && g.Status != 9)
                //    .Include(x => x.Comerciante)
                //    .Where(g => g.CompraUsuario == true);

                //var totalPages = (int)Math.Ceiling((double)cupons.Count() / viewModel.QuantidadePorPagina);
                //var totalRegistros = cupons.Count();

                //cupons = cupons.Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1)).Take(viewModel.QuantidadePorPagina);

                //return new
                //{
                //    totalRegistros,
                //    totalPages,
                //    data = cupons.Select(s => new
                //    {
                //        s.Comerciante.Nome,
                //        Usuario = s.Comerciante.Nome,
                //        s.Descricao,
                //        s.Valor,
                //        s.DataCompra,
                //        s.ComprovanteCompra,
                //        MeioPagamento = ((EnumTipoPagamento)s.MeioPagamento).GetDescription(),
                //        s.CompraUsuarioAprovada,
                //        s.PercentualCashback,
                //        s.Token,
                //        ValorRecebido = s.Valor - (s.Valor * s.PercentualCashback),
                //        ValorCashbackUsuario = s.Valor * s.PercentualCashback,
                //        s.Status
                //    }).OrderByDescending(o => o.DataCompra).ToList()
                //};
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ChaveUrlNFCeViewModel>> ObterChavesDeAcessoNF()
        {
            Expression<Func<CupomCashback, bool>> filtro = x => !string.IsNullOrEmpty(x.ChaveDeAcessoNF) && (x.Status == 9 || x.Status == 11);

            var chavesDeAcesso = await _repositorio.Get(filtro)
                                             .Select(x => new ChaveUrlNFCeViewModel
                                             {
                                                 UrlChaveDeAcessoNF = x.UrlChaveDeAcessoNF,
                                                 ChaveDeAcessoNF = x.ChaveDeAcessoNF,
                                                 ChaveManual = x.ChaveManual,
                                             })
                                             .ToListAsync();
            return chavesDeAcesso;
        }

        public async Task<(bool status, string message)> CriarDadosNF(CupomCashbackDadosNF dadosNF)
        {
            return await _repositorio.CriarDadosNF(dadosNF);
        }

        public async Task<CupomCashback> BuscarPelaChaveDeAcessoAsync(string chaveDeAcesso)
        {
            return await _repositorio.BuscarPelaChaveDeAcessoAsync(chaveDeAcesso);
        }

        public async Task<(bool status, string message)> Atualizar(CupomCashback cupom)
        {
            try
            {
                _repositorio.Update(cupom);
                await _repositorio.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                return (false, message);
            }
        }

        public async Task<CupomCashback> CriarCuponCadastroFacilitadoAsync(decimal valorCompra, string comprovante, Usuario comerciante, Usuario usuarioPreCadastro)
        {
            try
            {
                var usuarioLogado = _usuarioNegocio.GetById(comerciante.IdUsuario, "Grupo", "Credenciamento");

                if (usuarioLogado.Grupo.Descricao != "Comerciante")
                {
                    throw new UnauthorizedException("token_venda_nao_comerciante");
                }

                if (usuarioLogado.Credenciamento.Status != StatusCredenciamento.Aprovado)
                {
                    throw new PadraoException("comerciante_nao_aprovado");
                }


                var credenciado = _credenciamentoRepositorio.FirstNoTracking(c => c.IdUsuario == comerciante.IdUsuario);

                var cupomGerado = new CupomCashback
                {
                    Token = Guid.NewGuid().ToString(),
                    Valor = valorCompra,
                    PercentualCashback = (decimal)credenciado.PercentualCashback / 100,
                    Documento = usuarioPreCadastro.Documento,
                    DataCompra = DateTime.Now,
                    Descricao = "Primeira compra",
                    MeioPagamento = 19,
                    IdComerciante = comerciante.IdUsuario,
                    CompraUsuario = true,
                    ComprovanteCompra = comprovante,
                    CompraUsuarioAprovada = false,
                    Status = 1,
                    UrlChaveDeAcessoNF = null,
                    ChaveManual = false,
                };

                _repositorio.Insert(cupomGerado);
                _repositorio.SaveChanges();


                return cupomGerado;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<bool> CheckCompra(Guid idUsuarioLogado, CupomCashback cupom)
        {
            var ultimoCupom = await _repositorio.Get(c => c.Documento == cupom.Documento).OrderByDescending(c => c.DataCompra).FirstOrDefaultAsync();

            if (ultimoCupom != null  && ultimoCupom.IdComerciante == cupom.IdComerciante)
            {
                if (ultimoCupom.Valor == cupom.Valor && ultimoCupom.DataCompra.Date == cupom.DataCompra.Date && ultimoCupom.Descricao == cupom.Descricao)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public void DeletarRegistrosFalhaProcedure(CupomCashback cupom)
        {
            var cupomPedido = _cuponCashbackPedidoRepositorio.FirstNoTracking(ccp => ccp.IdCuponCashback == cupom.IdCuponCashback);

            var pedido = _pedidoRepositorio.FirstNoTracking(p => p.IdPedido == cupomPedido.IdPedido);

            var transacao = _transacaoRepositorio.FirstNoTracking(t => t.IdTransacao == pedido.IdTransacao);

            _transacaoRepositorio.Delete(transacao.IdTransacao);
            _cuponCashbackPedidoRepositorio.Delete(cupomPedido.IdCuponCashback);
            _pedidoRepositorio.Delete(pedido.IdPedido);
            _repositorio.Delete(cupom.IdCuponCashback);

            _repositorio.SaveChanges();
        }
    }
}
