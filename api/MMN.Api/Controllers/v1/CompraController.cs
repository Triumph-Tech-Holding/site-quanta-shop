using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.ViewModel.Compra;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : LoggedControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly IPedidoNegocio _negocio;
        private readonly ICashbackNegocio _cashbackNegocio;
        private readonly ICupomCashbackNegocio _cupomCashbackNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IMapper _mapper;

        public CompraController(
            IOptions<AppSettings> appSettings,
            IProceduresRepositorio proceduresRepositorio,
            IPedidoNegocio negocio,
            ICashbackNegocio cashbackNegocio,
            ICupomCashbackNegocio cupomCashbackNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            IUsuarioNegocio usuarioNegocio,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _proceduresRepositorio = proceduresRepositorio;
            _negocio = negocio;
            _cashbackNegocio = cashbackNegocio;
            _cupomCashbackNegocio = cupomCashbackNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _usuarioNegocio = usuarioNegocio;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("obterMetodosDeCompra")]
        public IActionResult ObterMetodosDeCompra()
        {
            var lista = new Dictionary<string, int>
            {
                { EnumTipoPagamento.PGPAGARME.GetDescription(), (int)EnumTipoPagamento.PGPAGARME }
            };
            return Ok(lista);
        }

        /// <summary>
        /// Remove a compra do banco de dados.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("removerCompra")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoverCompraAsync(RemoverCompraViewModel model)
        {
            if (!model.ConfirmarRemover)
            {
                throw new PadraoException("confirmacao_requerida");
            }

            await _negocio.RemoverCompra(model.IdPedido);

            return Ok("Compra removida");
        }

        /// <summary>
        /// Faz a aprovação e lançamento manual do cashback.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("aprovarCashback")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AprovarCashback(AprovarCashbackViewModel model)
        {
            var pedido = _negocio.FirstNoTracking(
                f => f.IdPedido == model.IdPedido,
                "Transacao.Anunciante");

            if (pedido == null)
            {
                throw new PadraoException("pedido_nao_encontrado");
            }

            await _cashbackNegocio.LancarCashback(
                model.IdPedido,
                IdUsuario: IdUsuarioLogado,
                finalizado: false);

            return Ok("Cashback aprovado.");
        }

        /// <summary>
        /// Retorna um relatório com um resumo mensal de cashback pago
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="datafinal"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("relatorioMensalCashback")]
        [Authorize(Roles = "Admin")]
        public IActionResult RelatorioMensalCashback(
            DateTime? dataInicial = null,
            DateTime? datafinal = null)
        {
            var relatorio = _proceduresRepositorio
                .spc_RelatorioMensalCashback(dataInicial, datafinal);

            return Ok(relatorio
                .GroupBy(g => new { g.Mes, g.Ano })
                .Select(s => new
                {
                    data = new DateTime(s.Key.Ano, s.Key.Mes, 1),
                    Lancamentos = s.Select(s => new
                    {
                        s.StatusPagamento,
                        s.IdTipoLancamento,
                        s.TipoLancamento,
                        s.TipoPedido,
                        s.DescricaoTipoPedido,
                        Valor = s.CashbackPago != 0 ? s.CashbackPago : s.CashbackAPagar
                    })
                })
            );
        }

        /// <summary>
        /// Inserir cupom fiscal de compra
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        [Route("getListaCupomFiscal")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCupomFiscal()
        {
            return Ok("teste");
        }

        /// <summary>
        /// Inserir cupom fiscal de compra
        /// </summary>
        /// /// <param name="cupomFiscal"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("inserirCupomFiscal")]
        //[Authorize(Roles = "User")]
        //public async Task<IActionResult> InserirCupomFiscal(InserirCupomFiscalViewModel cupomFiscal)
        //{
        //    await _cashbackNegocio.InserirCupomFiscal
        //        (
        //            cupomFiscal.Valor,
        //            cupomFiscal.CNPJ,
        //            cupomFiscal.Imagem,
        //            IdUsuarioLogado
        //        );
        //    return Ok();
        //}

        [HttpGet("obterChavesDeAcessoNF"), AllowAnonymous]
        public async Task<IActionResult> ObterChavesDeAcessoNF()
        {
            try
            {
                var request = HttpContext.Request;
                var authHeader = request.Headers.Authorization.ToString();

                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
                {
                    var encodedCredentials = authHeader["Basic ".Length..].Trim();
                    var encoding = Encoding.GetEncoding("iso-8859-1");

                    try
                    {
                        var credentials = encoding.GetString(Convert.FromBase64String(encodedCredentials));

                        int separator = credentials.IndexOf(':');
                        string name = credentials[..separator];
                        string password = credentials[(separator + 1)..];

                        if (name.Equals("rpa", StringComparison.OrdinalIgnoreCase) && password.Equals("python", StringComparison.OrdinalIgnoreCase))
                        {
                            var chavesDeAcesso = await _cupomCashbackNegocio.ObterChavesDeAcessoNF();

                            return Ok(chavesDeAcesso);
                        }
                        else
                            return Unauthorized();
                    }
                    catch (FormatException)
                    {
                        return BadRequest("O cabeçalho de autorização não está em um formato válido Base64.");
                    }
                }
                else
                {
                    return BadRequest("O cabeçalho de autorização está ausente ou em formato inválido.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Inserir cupom fiscal de compra
        /// </summary>
        /// /// <param name="cupomFiscal"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("inserirCupomFiscalSemDadosCompra")]
        public async Task<IActionResult> InserirCupomFiscalSemDadosCompra(InserirCupomFiscalViewModel cupomFiscal)
        {
            try
            {
                await _cashbackNegocio.InserirCupomFiscal
                (
                    cupomFiscal.ChaveAcesso,
                    cupomFiscal.UrlChaveDeAcessoNF,
                    cupomFiscal.ChaveManual,
                    IdUsuarioLogado
                );
                return Ok();
            }
            catch (Exception ex)
            {
                //Retorno do erro de nota ja cadastrada
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// Armazena os dados obtidos das NF a partir da consulta pela chave de acesso no banco de dados
        /// </summary>
        /// <param name="dadosNF"></param>
        /// <returns></returns>
        [HttpPost("enviarDadosNF"), AllowAnonymous]
        public async Task<IActionResult> EnviarDadosNF(DadosNFViewModel dadosNF)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(dadosNF);

                var request = HttpContext.Request;
                var authHeader = request.Headers.Authorization.ToString();

                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
                {
                    var encodedCredentials = authHeader["Basic ".Length..].Trim();
                    var encoding = Encoding.GetEncoding("iso-8859-1");

                    try
                    {
                        var credentials = encoding.GetString(Convert.FromBase64String(encodedCredentials));

                        int separator = credentials.IndexOf(':');
                        string name = credentials[..separator];
                        string password = credentials[(separator + 1)..];

                        if (name.Equals("rpa", StringComparison.OrdinalIgnoreCase) && password.Equals("python", StringComparison.OrdinalIgnoreCase))
                        {
                            if (dadosNF.ChaveDeAcesso.Length != 44)
                                return BadRequest("A chave de acesso deve ter 44 dígitos");

                            var cupomCashback = await _cupomCashbackNegocio.BuscarPelaChaveDeAcessoAsync(dadosNF.ChaveDeAcesso);

                            if (cupomCashback is null)
                                return NotFound("Compra não encontrada");

                            CupomCashbackDadosNF cupomCashbackDadosNF = new()
                            {
                                CNPJ = dadosNF.CNPJ,
                                CPF = dadosNF.CPF,
                                ChaveDeAcesso = dadosNF.ChaveDeAcesso,
                                QtdTotalDeItens = dadosNF.QtdTotalDeItens,
                                ValorAPagar = dadosNF.ValorAPagar,
                                ValorTotal = dadosNF.ValorTotal,
                                Descontos = dadosNF.Descontos,
                                Itens = [],
                                IdCuponCashback = cupomCashback.IdCuponCashback,
                                DataCadastro = DateTime.Now.HorarioBrasilia()
                            };

                            foreach (var item in dadosNF.Itens)
                            {
                                cupomCashbackDadosNF.Itens.Add(new CupomCashbackItemNF
                                {
                                    NomeProduto = item.NomeProduto,
                                    CodigoProduto = item.CodigoProduto,
                                    QuantidadeProduto = item.QuantidadeProduto,
                                    ValorUnitarioProduto = item.ValorUnitarioProduto,
                                });
                            }

                            var (status, message) = await _cupomCashbackNegocio.CriarDadosNF(cupomCashbackDadosNF);
                            var emailUtil = new EmailUtilitis();
                            Dictionary<string, string> body = null;
                            ObjEmailUtilitis objectEmail = new()
                            {
                                Data = DateTime.UtcNow.HorarioBrasilia(),
                                From = "contato@quantashop.com.br",
                                FromName = _appSettings.FromName,
                                Subject = "Comprovante de compra processado com sucesso! ✔️",
                                EmailSuporte = _appSettings.EmailSuporte,
                                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                            };

                            if (status)
                            {
                                var comprador = _usuarioNegocio.FirstNoTracking(x => x.Documento == (dadosNF.CPF == "Consumidor nao identificado" ? cupomCashback.Documento : dadosNF.CPF));

                                if (comprador is null)
                                {
                                    cupomCashback.Descricao = "Consumidor não encontrado";
                                    cupomCashback.Status = 11;

                                    await _cupomCashbackNegocio.Atualizar(cupomCashback);

                                    var usuario = _usuarioNegocio.FirstNoTracking(x => x.Documento == cupomCashback.Documento);

                                    objectEmail.DestinationName = usuario.Nome;
                                    objectEmail.Subject = "Erro no processamento do comprovante de compra 😕";
                                    objectEmail.To = usuario.Email;

                                    body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName }, { "{{ cause }}", "O documento informado não corresponde a sua conta." } };

                                    await emailUtil.EnviarEmail(body, _appSettings.NotaProcessadaComErro, null, objectEmail);

                                    return NotFound($"Consumidor não encontrado: {dadosNF.CPF}");
                                }

                                var comerciante = _usuarioNegocio.FirstNoTracking(x => x.Documento == dadosNF.CNPJ);

                                if (comerciante is null)
                                {
                                    cupomCashback.Descricao = "Comerciante não encontrado";
                                    cupomCashback.Status = 11;

                                    await _cupomCashbackNegocio.Atualizar(cupomCashback);

                                    objectEmail.DestinationName = comprador.Nome;
                                    objectEmail.Subject = "Erro no processamento do comprovante de compra 😕";
                                    objectEmail.To = comprador.Email;

                                    body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName }, { "{{ cause }}", "O estabelecimento emissor não está cadastrado no Quanta Shop." } };

                                    await emailUtil.EnviarEmail(body, _appSettings.NotaProcessadaComErro, null, objectEmail);

                                    return NotFound($"Comerciante não encontrado: {dadosNF.CNPJ}");
                                }

                                var credenciamento = _credenciamentoNegocio.FirstNoTracking(x => x.IdUsuario == comerciante.IdUsuario);

                                if (credenciamento is null)
                                {
                                    cupomCashback.Descricao = "Credenciamento não encontrado";
                                    cupomCashback.Status = 11;

                                    await _cupomCashbackNegocio.Atualizar(cupomCashback);

                                    objectEmail.DestinationName = comprador.Nome;
                                    objectEmail.Subject = "Erro no processamento do comprovante de compra 😕";
                                    objectEmail.To = comprador.Email;

                                    body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName }, { "{{ cause }}", "O estabelecimento emissor não está cadastrado no Quanta Shop." } };

                                    await emailUtil.EnviarEmail(body, _appSettings.NotaProcessadaComErro, null, objectEmail);

                                    return NotFound($"Credenciamento não encontrado: {dadosNF.CNPJ}");
                                }

                                cupomCashback.IdComerciante = comerciante.IdUsuario;
                                cupomCashback.Descricao = $"Compra realizada em {credenciamento.Estabelecimento.ToUpper()} usando o SCAN 'N GO";
                                cupomCashback.Valor = (decimal)cupomCashbackDadosNF.ValorAPagar;
                                cupomCashback.PercentualCashback = (decimal)credenciamento.PercentualCashback / 100;
                                cupomCashback.Status = 10;

                                await _cupomCashbackNegocio.Atualizar(cupomCashback);

                                await _cupomCashbackNegocio.AprovarReprovarCupomAsync(cupomCashback.Token, comprador.IdUsuario, true, 10, null, false);

                                objectEmail.DestinationName = comprador.Nome;
                                objectEmail.Subject = "Comprovante de compra processado com sucesso! ✔️";
                                objectEmail.To = comprador.Email;

                                body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName } };

                                await emailUtil.EnviarEmail(body, _appSettings.NotaProcessadaComSucesso, null, objectEmail);

                                return Ok("Cupom registrado com sucesso");
                            }
                            else
                            {
                                cupomCashback.Descricao = message;
                                cupomCashback.Status = 11;

                                await _cupomCashbackNegocio.Atualizar(cupomCashback);

                                var usuario = _usuarioNegocio.FirstNoTracking(x => x.Documento == cupomCashback.Documento);

                                objectEmail.DestinationName = usuario.Nome;
                                objectEmail.Subject = "Erro no processamento do comprovante de compra 😕";
                                objectEmail.To = usuario.Email;

                                body = new Dictionary<string, string> { { "{{ name }}", objectEmail.DestinationName }, { "{{ cause }}", "O estabelecimento emissor não está cadastrado no Quanta Shop." } };

                                await emailUtil.EnviarEmail(body, _appSettings.NotaProcessadaComErro, null, objectEmail);

                                return BadRequest(message);
                            }
                        }
                        else
                            return Unauthorized();
                    }
                    catch (FormatException)
                    {
                        return BadRequest("O cabeçalho de autorização não está em um formato válido Base64.");
                    }
                }
                else
                    return BadRequest("O cabeçalho de autorização está ausente ou em formato inválido.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("compras-aguardando-confirmacao")]
        public async Task<IActionResult> ComprasAguardandoConfirmacao()
        {
            try
            {
                var usuario = _usuarioNegocio.GetById(IdUsuarioLogado);
                var data = _cupomCashbackNegocio.GetAsync(x => (x.Status == 1 || x.Status == 9) && x.Documento == usuario.Documento).Result.Adapt<IList<CupomCashbackVM>>();

                if (data == null)
                    return NotFound(new { status = 404, data = Array.Empty<CupomCashback>() });

                foreach (var item in data)
                {
                    if (item.IdComerciante is not null)
                    {
                        var credenciamento = _credenciamentoNegocio.FirstNoTracking(x => x.IdUsuario == item.IdComerciante.Value);

                        item.Credenciamento = new Credenciamento
                        {
                            IdCredenciamento = credenciamento.IdCredenciamento,
                            IdUsuario = credenciamento.IdUsuario,
                            Estabelecimento = credenciamento.Estabelecimento,
                            Rua = credenciamento.Rua,
                            Numero = credenciamento.Numero,
                            Bairro = credenciamento.Bairro,
                            Complemento = credenciamento.Complemento,
                            LogoUrl = credenciamento.LogoUrl
                        };
                    }
                }

                return Ok(new { status = 200, data = data.OrderByDescending(x => x.DataCompra).ToList() });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException is null ? ex.Message : ex.InnerException.ToString();

                return StatusCode(500, new { status = 500, message });

            }
        }
    }
}