using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Models.Pedido;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Negocio.Negocio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Model.Pedido;
using MMN.Util.Translation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : LoggedControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IPedidoNegocio _negocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IUsuarioEnderecoNegocio _usuarioEnderecoNegocio;
        private readonly IProdutoNegocio _produtoNegocio;
        private readonly IConfiguracaoNegocio _configuracaoNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly ILocation _location;

        public PedidosController(IOptions<AppSettings> appSettings,
            IPedidoNegocio negocio,
            ILocation location,
            IUsuarioNegocio usuarioNegocio,
            ILancamentoNegocio lancamentoNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            IProdutoNegocio produtoNegocio,
            IConfiguracaoNegocio configuracaoNegocio,
            IUsuarioEnderecoNegocio usuarioEnderecoNegocio)
        {
            _negocio = negocio;
            _usuarioNegocio = usuarioNegocio;
            _location = location;
            _appSettings = appSettings.Value;
            _lancamentoNegocio = lancamentoNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _produtoNegocio = produtoNegocio;
            _configuracaoNegocio = configuracaoNegocio;
            _usuarioEnderecoNegocio = usuarioEnderecoNegocio;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("detalhes/{codigo}")]
        public IActionResult Detalhes(string codigo)
        {
            var pedido = _negocio.ObterDetalhes(codigo);
            var produto = pedido.UsuarioProduto.First().Produto;
            var status = "Pago";

            if (pedido.Cancelado.HasValue && pedido.Cancelado.Value)
            {
                status = "Cancelado";
            }
            else if (pedido.Ativo.HasValue && pedido.Ativo.Value && !pedido.Pago)
            {
                status = "Em pagamento";
            }
            else if (pedido.Ativo.HasValue && !pedido.Ativo.Value && !pedido.Pago)
            {
                status = "Inativo";
            }

            var parcelas = new List<object>();
            foreach (var par in pedido.Pagamentos)
            {
                parcelas.Add(new
                {
                    dataValidade = par.DataValidade,
                    dataPagamento = par.DataReferencia ?? par.DataPagamento,
                    boletoPago = par.Pago,
                    url = par.UrlBoleto,
                    par.NumeroParcela,
                    valorParcela = par.Valor
                });
            }

            return Ok(new
            {
                pedido.IdPedido,
                pedidoPago = pedido.Pago,
                valorProduto = produto.Valor,
                valorPedido = pedido.ValorPedido,
                valorPago = pedido.ValorPago,
                produto = produto.Nome,
                dataPedido = pedido.DataReferencia ?? pedido.DataPedido,
                dataPagamento = pedido.DataPagamento,
                status,
                parcelas,
                numParcelas = pedido.NumeroParcelas
            });
        }

        [HttpPost]
        [Route("listaPedidos")]
        public IActionResult ListaPedidos([FromBody] BuscarPedido filtro)
        {
            var lstPedidos = _negocio.ListarPedidos(filtro, IdUsuarioLogado.ToString());
            return Ok(lstPedidos);
        }

        [HttpGet]
        //[Authorize("Admin")]
        [Route("detalheseditar/{idPedido}")]
        public IActionResult DetalhesPedido(long idPedido)
        {
            var pedido = _negocio
                .FirstNoTracking(p =>
                p.IdPedido == idPedido,
                "Pagamentos",
                "Usuario",
                "Usuario.UsuarioPai",
                "UsuarioProduto",
                "UsuarioProduto.Produto.Categoria");

            var parcelas = new List<object>();

            if (pedido is not null && pedido.NumeroParcelas == 1)
            {
                parcelas.Add(new
                {
                    idPagamento = pedido.IdPedido,
                    pedido.DataPagamento,
                    dataValidade = pedido.DataPedido.AddDays(8),
                    valor = pedido.ValorPedido,
                    pedido.Pago,
                    numeroParcela = 1
                });
            }
            else
            {
                parcelas.AddRange(pedido.Pagamentos.OrderBy(s => s.NumeroParcela).Select(s => new
                {
                    s.IdPagamento,
                    dataPagamento = s.DataReferencia ?? s.DataPagamento,
                    s.DataValidade,
                    s.Valor,
                    s.Pago,
                    s.NumeroParcela
                }));
            }
            if (pedido.DataReferencia != null)
            {
                return Ok(new
                {
                    pedido.IdPedido,
                    pedido.Ativo,
                    dataPedido = pedido.DataReferencia,
                    pedido.DataPagamento,
                    pedido.Pago,
                    status = pedido.Ativo.HasValue && pedido.Ativo.Value ? "Ativo" : "Inativo",
                    valorPago = pedido.ValorPago.HasValue ? pedido.ValorPago.Value : 0,
                    pedido.ValorPedido,
                    nomeUsuario = pedido.Usuario.Nome,
                    produto = pedido.UsuarioProduto.FirstOrDefault()?.Produto?.Nome,
                    categoriaProduto = pedido.UsuarioProduto.FirstOrDefault()?.Produto?.Categoria?.Chave,
                    loginUsuario = pedido.Usuario.Login,
                    loginPatrocinador = pedido.Usuario.UsuarioPai?.Login,
                    numeroParcelas = pedido.NumeroParcelas,
                    pedido.ReaisPorPonto,
                    parcelas
                });
            }
            else
            {
                return Ok(new
                {
                    pedido.IdPedido,
                    pedido.Ativo,
                    pedido.DataPedido,
                    pedido.DataPagamento,
                    pedido.Pago,
                    status = pedido.Ativo.HasValue && pedido.Ativo.Value ? "Ativo" : "Inativo",
                    valorPago = pedido.ValorPago.HasValue ? pedido.ValorPago.Value : 0,
                    pedido.ValorPedido,
                    nomeUsuario = pedido.Usuario.Nome,
                    produto = pedido.UsuarioProduto.FirstOrDefault()?.Produto?.Nome,
                    categoriaProduto = pedido.UsuarioProduto.FirstOrDefault()?.Produto?.Categoria?.Chave,
                    loginUsuario = pedido.Usuario.Login,
                    loginPatrocinador = pedido.Usuario.UsuarioPai?.Login,
                    numeroParcelas = pedido.NumeroParcelas,
                    pedido.ReaisPorPonto,
                    parcelas
                });
            }
        }


        [HttpPut]
        //[Authorize("Admin")]
        [Route("Editar")]
        public IActionResult EditarPedido([FromBody] PedidoViewModel pedido)
        {
            var result = _negocio.EditarPedido(pedido);

            return Ok(result);
        }

        [HttpPut]
        //[Authorize("Admin")]
        [Route("RemoverParcela/{idPedido}")]
        public IActionResult RemoverParcela(long idPedido)
        {
            var result = _negocio.RemoverParcela(idPedido);

            return Ok(result);
        }

        [HttpPut]
        //[Authorize("Admin")]
        [Route("AdicionarParcela/{idPedido}")]
        public IActionResult AdicionarParcela(long idPedido)
        {
            var result = _negocio.AdicionarParcela(idPedido);

            return Ok(result);
        }

        [HttpPost]
        [Route("cancelar/{idPedido}")]
        public IActionResult CancelarParcelamento(long idPedido)
        {
            _negocio.CancelarPedido(idPedido, IdUsuarioLogado);

            return Ok();
        }

        [HttpPost]
        [Route("reativar/{idPedido}")]
        public IActionResult ReativarParcelamento(long idPedido)
        {
            _negocio.ReativarPedido(idPedido, IdUsuarioLogado);

            return Ok();
        }

        [HttpGet]
        [Route("listaPedidosNovo")]
        //public IActionResult ListaPedidosNovo(string categoria = "INV")
        public IActionResult ListaPedidosNovo(string categoria)
        {
            var result = new List<object>();

            var categoriaINV = "INV";

            var pedidos = _negocio
                .Get(w =>
                    !string.IsNullOrEmpty(w.Codigo) &&
                    w.IdUsuario == IdUsuarioLogado &&
                    (categoriaINV == null || w.UsuarioProduto.Any(a =>
                        a.Produto.Categoria.Chave == categoriaINV)),
                "UsuarioProduto",
                "Pagamentos");

            foreach (var pedido in pedidos.OrderByDescending(o => o.DataPedido))
            {
                if (categoria == "INV")
                {
                    if (pedido.UsuarioProduto.FirstOrDefault().IdProduto != 12)
                    {
                        var nomeProduto = string.Empty;
                        if (pedido.UsuarioProduto != null && pedido.UsuarioProduto.Count() > 0)
                        {
                            var produto = _produtoNegocio.FirstNoTracking(p => p.IdProduto == pedido.UsuarioProduto.First().IdProduto);
                            nomeProduto = produto.Nome;
                        }
                        if (pedido.DataReferencia != null)
                        {
                            result.Add(new
                            {
                                pedido.IdPedido,
                                dataPedido = pedido.DataReferencia,
                                pedido.ValorPedido,
                                pedido.UrlBoleto,
                                pedido.Codigo,
                                pedido.Ativo,
                                pedido.Cancelado,
                                pedido.LinhaDigitavelBoleto,
                                pedido.Pago,
                                pedido.Pagamentos,
                                nomeProduto,
                                numParcelas = pedido.NumeroParcelas
                            });
                        }
                        else
                        {
                            result.Add(new
                            {
                                pedido.IdPedido,
                                pedido.DataPedido,
                                pedido.ValorPedido,
                                pedido.UrlBoleto,
                                pedido.Codigo,
                                pedido.Ativo,
                                pedido.Cancelado,
                                pedido.LinhaDigitavelBoleto,
                                pedido.Pago,
                                pedido.Pagamentos,
                                nomeProduto,
                                numParcelas = pedido.NumeroParcelas
                            });
                        }
                    }
                }
                else if (categoria == "ASNT")
                {
                    if (pedido.UsuarioProduto.FirstOrDefault().IdProduto == 12)
                    {
                        var nomeProduto = string.Empty;
                        if (pedido.UsuarioProduto != null && pedido.UsuarioProduto.Count() > 0)
                        {
                            var produto = _produtoNegocio.FirstNoTracking(p => p.IdProduto == pedido.UsuarioProduto.First().IdProduto);
                            nomeProduto = produto.Nome;
                        }
                        if (pedido.DataReferencia != null)
                        {
                            result.Add(new
                            {
                                pedido.IdPedido,
                                dataPedido = pedido.DataReferencia,
                                pedido.ValorPedido,
                                pedido.UrlBoleto,
                                pedido.Codigo,
                                pedido.Ativo,
                                pedido.Cancelado,
                                pedido.LinhaDigitavelBoleto,
                                pedido.Pago,
                                pedido.Pagamentos,
                                nomeProduto,
                                numParcelas = pedido.NumeroParcelas
                            });
                        }
                        else
                        {
                            result.Add(new
                            {
                                pedido.IdPedido,
                                pedido.DataPedido,
                                pedido.ValorPedido,
                                pedido.UrlBoleto,
                                pedido.Codigo,
                                pedido.Ativo,
                                pedido.Cancelado,
                                pedido.LinhaDigitavelBoleto,
                                pedido.Pago,
                                pedido.Pagamentos,
                                nomeProduto,
                                numParcelas = pedido.NumeroParcelas
                            });
                        }
                    }
                }

            }

            return Ok(result);
        }

        [HttpPost]
        [Route("gerarPedidoManual")]
        public IActionResult AtivarPacote([FromBody] GerarPedidoManualViewModel model)
        {
            var usuario = _usuarioNegocio.GetById(IdUsuarioLogado, "Grupo");
            if (usuario.Grupo.Descricao.Equals("Admin"))
            {
                var pedido = _negocio.GerarPedidoManual(model, IdUsuarioLogado);

                return Ok(new
                {
                    pedido.IdPedido,
                });
            }
            throw new UnauthorizedException("sem_permissao");
        }

        [HttpPost]
        [Route("adquirirPlano")]
        public IActionResult AdquirirPlano([FromBody] AdquirirPlanoViewModel model)
        {
            var adquirirPlanoViewModelValidator = new AdquirirPlanoViewModelValidator();
            var result = adquirirPlanoViewModelValidator.Validate(model);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _usuarioNegocio.GetById(IdUsuarioLogado, "Grupo");

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Documento))
                    throw new PadraoException("compra_cadastre_dados");

                var pedido = _negocio.AdquirirPlano(model, IdUsuarioLogado);

                return Ok(new
                {
                    pedido.IdPedido,
                    pedido.UrlBoleto,
                    pedido.CodigoReferenciaBoleto,
                    pedido.LinhaDigitavelBoleto,
                    pedido.MeioPagamento
                });
            }

            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("adquirirPlanoMensal")]
        public IActionResult AdquirirPlanoMensal([FromBody] AdquirirPlanoViewModel model)
        {
            if (model.Card.Card_expiration_year.Length == 2)
            {
                model.Card.Card_expiration_year = "20" + model.Card.Card_expiration_year;
            }

            var adquirirPlanoViewModelValidator = new AdquirirPlanoViewModelValidator();
            var result = adquirirPlanoViewModelValidator.Validate(model);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var assinaturaAtiva = _negocio.GetAssinatura(IdUsuarioLogado);

            if (assinaturaAtiva)
            {
                return Ok(new
                {
                    assinaturaAtiva
                });
            }

            var usuario = _usuarioNegocio.GetById(IdUsuarioLogado, "Grupo");
            
            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Documento))
                    throw new PadraoException("compra_cadastre_dados");

                if (usuario.PreCadastro)
                    throw new PadraoException("compra_cadastre_dados");

                var usuarioEndereco = _usuarioEnderecoNegocio.Get(x => x.IdUsuario == IdUsuarioLogado, ["Cidade", "Cidade.Estado"]).FirstOrDefault();

                if (usuarioEndereco is null)
                    throw new PadraoException("usuario_sem_endereco");

                var pedido = _negocio.AdquirirPlanoMensal(model, IdUsuarioLogado);

                if (pedido.Pago == true)
                {
                    return Ok(new
                    {
                        pedido.CodigoReferenciaAssinatura,
                        pedido.MeioPagamento
                    });
                }
                else
                {
                    return BadRequest("Algo deu errado:");
                }
            }

            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("adquirir-assinatura-anual")]
        public IActionResult AdquirirAssinaturaAnual([FromBody] AdquirirAssinaturaAnualViewModel model)
        {
            if (model.Card.Card_expiration_year.Length == 2)
            {
                model.Card.Card_expiration_year = "20" + model.Card.Card_expiration_year;
            }

            var adquirirPlanoViewModelValidator = new AdquirirAssinaturaAnualViewModelValidator();
            var result = adquirirPlanoViewModelValidator.Validate(model);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var assinaturaAtiva = _negocio.GetAssinatura(IdUsuarioLogado);

            if (assinaturaAtiva)
            {
                return Ok(new
                {
                    assinaturaAtiva
                });
            }

            var usuario = _usuarioNegocio.GetById(IdUsuarioLogado, "Grupo");

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Documento))
                    throw new PadraoException("compra_cadastre_dados");

                var pedido = _negocio.AdquirirAssinaturaAnual(model, IdUsuarioLogado);

                if (pedido.Pago == true)
                {
                    return Ok(new
                    {
                        pedido.CodigoReferenciaAssinatura,
                        pedido.MeioPagamento
                    });
                }
                else
                {
                    return BadRequest("Algo deu errado");
                }
            }

            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("cancelarAssinatura")]
        public IActionResult CancelarAssinatura()
        {
            try
            {
                var usuario = _usuarioNegocio.GetById(IdUsuarioLogado, "Grupo");

                if (usuario != null)
                {
                    if (string.IsNullOrEmpty(usuario.Documento))
                        throw new PadraoException("compra_cadastre_dados");

                    var resultado = _negocio.CancelarAssinatura(IdUsuarioLogado, "usuario");

                    if (resultado.Success)
                    {
                        return Ok(new
                        {

                        });
                    }
                    else
                    {
                        return BadRequest("Algo deu errado: " + resultado.Message);
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("adquirirSaldo")]
        public IActionResult AdquirirSaldo([FromBody] AdquirirSaldoViewModel model)
        {
            var adquirirSaldoViewModelValidator = new AdquirirSaldoViewModelValidator();
            var result = adquirirSaldoViewModelValidator.Validate(model);

            model.FormaPagamento = EnumTipoPagamento.PGPAGARME;

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _usuarioNegocio.GetById(IdUsuarioLogado);

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Documento))
                {
                    throw new PadraoException("compra_cadastre_dados");
                }

                var pedido = _negocio.AdquirirSaldo(model, IdUsuarioLogado);

                return Ok(new
                {
                    pedido.IdPedido,
                    pedido.UrlBoleto,
                    pedido.CodigoReferenciaBoleto,
                    pedido.LinhaDigitavelBoleto,
                    pedido.MeioPagamento
                });
            }

            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpGet]
        [Route("taxaSaldo")]
        public ActionResult<decimal> TaxaSaldo()
        {
            var configuracao = _configuracaoNegocio.BuscarPelaChave("TAXA_COMPRA_SALDO");

            return Ok(decimal.Parse(configuracao.Valor, CultureInfo.InvariantCulture));
        }

        [HttpGet]
        [Route("buscarProdutoPorCodigo/{codigo}")]
        public IActionResult BuscarProdutoPorCodigo(string codigo)
        {
            var produto = _negocio.BuscarProdutoPorCodigo(codigo, IdUsuarioLogado);

            return Ok(produto);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("validarPedidosLionbit")]
        public IActionResult ValidarPedidosLionbit()
        {
            //_negocio.ProcessarPagamentoLionbit();
            return Ok(new { sucesso = true });
        }

        [HttpPost]
        [Route("listaPedidosAfiliados")]
        public IActionResult ListaPedidosAfiliados(PedidoAfiliados filtro)
        {
            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
            {
                if (filtro.DataFim.Value <= filtro.DataInicio.Value)
                {
                    return BadRequest(new { message = "A data de fim deve ser maior que a data de início." });
                }

                var intervalo = filtro.DataFim.Value - filtro.DataInicio.Value;

                if (intervalo.Days > 120)
                {
                    return BadRequest(new { message = "O intervalo máximo permitido é de 120 dias." });
                }
            }

            var resultado = _negocio.ListarPedidosAfiliados(filtro, IdUsuarioLogado);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            return new JsonResult(resultado, options);
        }

        [HttpPost]
        [Route("atualizarPedido")]
        public IActionResult AtualizarPedido(PedidoViewModel model)
        {
            _negocio.EditarPedido(model);
            return Ok();
        }

        [HttpPost]
        [Route("atualizarParcela")]
        public IActionResult AtualizarParcela(UpdateParcela model)
        {
            var result = false;
            switch (model.Acao)
            {
                case AcaoUpdateParcela.Pagar:
                    result = _negocio.PagarParcela(model.IdPedido, model.NumeroParcela, model.DataReferencia, model.DistribuirNaRede.HasValue ? model.DistribuirNaRede.Value : false);
                    break;

                case AcaoUpdateParcela.Apagar:
                    result = _negocio.ApagarParcela(model.IdPedido, model.NumeroParcela);
                    break;

                case AcaoUpdateParcela.RenovarBoleto:
                    result = _negocio.RenovarBoleto(model.IdPedido, model.NumeroParcela);
                    break;
                case AcaoUpdateParcela.AlterarValor:
                    result = _negocio.AlterarValorParcela(model.IdPedido, model.NumeroParcela, model.Valor.Value);
                    break;

                default:
                    throw new PadraoException("acao_requerida");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("ListarComprasEmLojas")]
        public IActionResult ListarComprasEmLojas(FiltroComprasEmLojas viewModel)
        {
            var viewModelValidator = new FiltroComprasEmLojasValidator();
            var result = viewModelValidator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            bool procurarEstabelecimentos = false;
            List<Guid> idsUsuariosCredenciamentos = new List<Guid>();
            if (!string.IsNullOrEmpty(viewModel.Estabelecimento))
            {
                procurarEstabelecimentos = true;
                idsUsuariosCredenciamentos = _credenciamentoNegocio.Get(c => c.Estabelecimento.ToLower().Contains(viewModel.Estabelecimento.ToLower()) && c.IdUsuario.HasValue).Select(s => s.IdUsuario.Value).ToList();
            }

            var compras = _negocio.Get(p =>
                p.IdUsuario == IdUsuarioLogado
                && p.IdUsuarioComerciante.HasValue
                && p.Ativo == true
                && (!procurarEstabelecimentos || idsUsuariosCredenciamentos.Any(i => i == p.IdUsuarioComerciante.Value))
                && (!viewModel.TipoPagamento.HasValue || viewModel.TipoPagamento == (EnumTipoPagamento)p.MeioPagamento)
                && (!viewModel.ValorInicial.HasValue || viewModel.ValorInicial <= p.ValorPedido)
                && (!viewModel.ValorFinal.HasValue || viewModel.ValorFinal >= p.ValorPedido), "Transacao.Status", "UsuarioComerciante", "Usuario")
                //.Select(s => new
                //{
                //    s.IdPedido,
                //    s.DataPedido,
                //    s.Transacao.Descricao,
                //    s.ValorPedido,
                //    s.PercentualCashback,
                //    Status = s.Transacao.StatusViewModel.Nome,
                //    s.MeioPagamento,
                //    MeioPagamentoDesc = ((EnumTipoPagamento)s.MeioPagamento).GetDescription(),
                //    _credenciamentoNegocio.First(c => c.IdUsuario == s.IdUsuarioComerciante).Estabelecimento
                //})
                .OrderByDescending(o => o.DataPedido)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)compras.Count() / viewModel.QuantidadePorPagina);

            var comprasFiltradas = compras
               .Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1))
               .Take(viewModel.QuantidadePorPagina)
               .ToList();

            List<object> retorno = new List<object>();

            var arrayTipo = new int[2] { 51, 52 };

            foreach (var compra in comprasFiltradas)
            {
                decimal valorParaRede = 0;
                var lancamentosParaRede = _lancamentoNegocio.Get(l => l.IdTransacao == compra.IdTransacao
                                               && arrayTipo.Contains(l.IdTipo)
                                               && l.IdUsuario != new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B"), "LancamentoRetido");

                foreach (var lancamento in lancamentosParaRede
                    .Where(w => compra.Usuario.IdUsuarioPai == null || w.IdUsuario != compra.Usuario.IdUsuarioPai
                    && w.IdTipo != 53
                    && w.IdUsuario != IdUsuarioLogado)
                    .ToList()
                )
                {
                    valorParaRede += lancamento.Valor;
                    //valorParaRede += lancamento.LancamentoRetido.Sum(l => l.Valor);
                }

                var valorPatrocinador = lancamentosParaRede.Where(w => compra.Usuario.IdUsuarioPai == null || w.IdUsuario == compra.Usuario.IdUsuarioPai).Sum(s => s.Valor);
                var valorRecebido = lancamentosParaRede.Where(w => w.IdUsuario == IdUsuarioLogado && w.IdTipo == 52).Sum(s => s.Valor);

                retorno.Add(new
                {
                    compra.IdPedido,
                    compra.DataPedido,
                    compra.Transacao.Descricao,
                    compra.ValorPedido,
                    compra.PercentualCashback,
                    Status = compra.Transacao.StatusViewModel.Nome,
                    compra.MeioPagamento,
                    MeioPagamentoDesc = ((EnumTipoPagamento)compra.MeioPagamento).GetDescription(),
                    _credenciamentoNegocio.First(c => c.IdUsuario == compra.IdUsuarioComerciante).Estabelecimento,
                    valorParaRede,
                    valorPatrocinador,
                    valorRecebido,
                    ValorCashback = compra.ValorPago * (compra.PercentualCashback / 100),
                    ValorRecebidoPelaBigcash = _lancamentoNegocio.Get(l => l.IdTransacao == compra.IdTransacao && l.IdUsuario == new Guid("30C0BF78-879A-47C1-9869-D485B8D84E0B")).Sum(s => s.Valor)
                });
            }

            return Ok(
                new
                {
                    totalPages,
                    viewModel.QuantidadePorPagina,
                    viewModel.Pagina,
                    comprasFiltradas = retorno,
                    quantidadeTotal = compras.Count(),
                    valorMaisAlto = compras.Count > 0 ? compras.Max(c => c.ValorPedido) : 0
                }
            );
        }

        [HttpPost, Route("listaPedidosAfiliadosAdmin"), Authorize(Roles = "Admin")]
        public IActionResult ListaPedidosAfiliadosAdmin(PedidoAfiliadosAdmin viewModel)
        {
            PedidoAfiliadosAdminValidator validator = new PedidoAfiliadosAdminValidator();
            var result = validator.Validate(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            return Ok(_negocio.ListarPedidosAfiliadosAdmin(viewModel));
        }

        [HttpGet]
        [Route("obterDistribuicaoCashback/{idPedido}")]
        public IActionResult ObterDistribuicaoCashback(int idPedido)
        {
            var pedido = _negocio.First(f => f.IdPedido == idPedido, "Transacao");

            if (pedido.Transacao != null)
            {
                var lancamentos = _lancamentoNegocio.Get(w => w.IdTransacao == pedido.Transacao.IdTransacao, "Usuario").OrderBy(x => x.OrdemExibicao).ThenBy(x => x.Valor).ThenBy(x => x.IdUsuario);

                return Ok(lancamentos.Select(s => new
                {
                    s.Valor,
                    Login = s.Usuario.Login.Equals("bigcashAdm") ? "Quanta Shop" : s.Usuario.Login,
                    s.Descricao,
                    s.DataLancamento
                }));
            }

            throw new PadraoException("pedido_sem_transacao");
        }

    }
}