using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using MMN.Api.Services;
using MMN.Api.ViewModel.Cupom;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [Route("api/venda")]
    [ApiController]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status500InternalServerError)]
    public class VendaController : LoggedControllerBase
    {
        private readonly ICupomCashbackNegocio _cuponCashbackNegocio;
        private readonly ITipoPagamentoNegocio _tipoPagamentoNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly IMapper _mapper;
        private readonly WhatsAppService _whatsAppService;
        private readonly AppSettings _appSettings;

        /// <param name="usuarioNegocio"></param>
        /// <param name="pedidoNegocio"></param>
        /// <param name="cuponCashbackNegocio"></param>
        /// <param name="tipoPagamentoNegocio"></param>
        /// <param name="credenciamentoNegocio"></param>
        /// <param name="proceduresRepositorio"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        public VendaController(
            IUsuarioNegocio usuarioNegocio,
            IPedidoNegocio pedidoNegocio,
            ICupomCashbackNegocio cuponCashbackNegocio,
            ITipoPagamentoNegocio tipoPagamentoNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            IProceduresRepositorio proceduresRepositorio,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            WhatsAppService whatsAppService)
        {
            _cuponCashbackNegocio = cuponCashbackNegocio;
            _tipoPagamentoNegocio = tipoPagamentoNegocio;
            _usuarioNegocio = usuarioNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _proceduresRepositorio = proceduresRepositorio;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _whatsAppService = whatsAppService;
        }

        /// <summary>
        /// Cria uma nova venda do credenciado logado.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <response code="201">Criação de venda bem sucedida</response>
        [ProducesResponseType(typeof(CriarCupomViewModel), StatusCodes.Status201Created)]
        [HttpPost("criarVenda")]
        public async Task<ActionResult<CupomCashbackViewModel>> CriarVenda(CriarCupomViewModel viewModel)
        {
            var digitsOnly = new Regex(@"[^\d]");
            viewModel.Documento = digitsOnly.Replace(viewModel.Documento, "");

            //validar a regra de cashback para enviar quando for um comprador e não um vendedor
            var viewModelValidator = new CriarCuponViewModelValidator();
            var result = viewModelValidator.Validate(viewModel);
            var usuario = _usuarioNegocio.FirstNoTracking(f => f.IdUsuario == IdUsuarioLogado);
            var idUsuario = viewModel.CompraUsuario ? viewModel.IdComerciante : usuario.IdUsuario;
            var comprovanteUrl = viewModel.CompraUsuario == true ? SalvaImagem(viewModel.ComprovanteCompra, IdUsuarioLogado.ToString()) : null;
            var dadosComerciante = _usuarioNegocio.FirstNoTracking(f => f.IdUsuario == idUsuario);
            var dadosEstabelecimento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == idUsuario);

            if (viewModel.PercentualCashback < dadosEstabelecimento.PercentualCashback)
            {
                throw new PadraoException("cashback_menor_que_o_cadastrado");
            }

            var tiposPagamento = _tipoPagamentoNegocio.GetTipoPagamentoVendaOffline();

            if (!tiposPagamento.Any(a => a.Chave == (int)viewModel.MeioPagamento))
            {
                throw new PadraoException("pagamento_tipo_invalido");
            }

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var comprovanteBlob = SalvaImagem(viewModel.ComprovanteCompra, IdUsuarioLogado.ToString());

            var cupom = new CupomCashback
            {
                Token = viewModel.Token.ToUpper(),
                Valor = viewModel.Valor,
                PercentualCashback = (decimal)(viewModel.PercentualCashback is not null ? viewModel.PercentualCashback / 100 : dadosEstabelecimento.PercentualCashback / 100),
                Documento = viewModel.CompraUsuario == true ? usuario.Documento : viewModel.Documento,
                DataCompra = viewModel.DataVenda?.HorarioBrasilia() ?? DateTime.UtcNow.HorarioBrasilia(),
                Descricao = viewModel.Descricao,
                MeioPagamento = (int)viewModel.MeioPagamento,
                IdComerciante = viewModel.CompraUsuario == true ? viewModel.IdComerciante : IdUsuarioLogado,
                CompraUsuario = viewModel.CompraUsuario,
                ComprovanteCompra = viewModel.CompraUsuario == true ? comprovanteBlob : null,
                CompraUsuarioAprovada = viewModel.CompraUsuario == true ? false : true,
                UrlChaveDeAcessoNF = viewModel.UrlChaveDeAcessoNF,
                ChaveManual = viewModel.ChaveManual,
            };

            cupom = await _cuponCashbackNegocio.CriarCuponAsync(cupom, IdUsuarioLogado, viewModel.CompraUsuario == true ? false : true);

            if (cupom.MeioPagamento == (int)Util.Enum.EnumTipoPagamento.Saldo)
            {
                await _cuponCashbackNegocio.AprovarReprovarCupomAsync(cupom, true, 6);
                try
                {
                    _proceduresRepositorio.spc_PagamentoComSaldo(new Guid(cupom.IdCuponCashback));
                }
                catch (Exception)
                {
                    _cuponCashbackNegocio.DeletarRegistrosFalhaProcedure(cupom);
                    throw new Exception("Falha ao inserir compra, por favor tente novamente");
                }
            }

            cupom = await _cuponCashbackNegocio.ObterCuponAsync(viewModel.Token);

            if (cupom == null)
            {
                throw new Exception("token_venda_nao_encontrado");
            }

            if (cupom.CompraUsuario)
            {
                var cashback = string.Format("{0:C2}", Convert.ToDouble(cupom.Valor * cupom.PercentualCashback));
                var valor = string.Format("{0:C2}", Convert.ToDouble(cupom.Valor));
                var emailUtil = new EmailUtilitis();
                Dictionary<string, string> body = null;
                ObjEmailUtilitis objectEmailComprador = new()
                {
                    Data = DateTime.Now.HorarioBrasilia(),
                    From = "contato@quantashop.com.br",
                    FromName = _appSettings.FromName,
                    Subject = "Comprovante de compra enviado com sucesso! ✔️",
                    EmailSuporte = _appSettings.EmailSuporte,
                    SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"),
                    DestinationName = usuario.Nome,
                    To = usuario.Email
                };

                body = new Dictionary<string, string> { { "{{ name }}", objectEmailComprador.DestinationName }, { "{{ estabelecimento }}", dadosEstabelecimento.Estabelecimento }, { "{{ valor }}", valor }, { "{{ data }}", cupom.DataCompra.HorarioBrasilia().ToString("dd/MM/yyyy") } };

                await emailUtil.EnviarEmail(body, _appSettings.CupomNaoFiscalEnviadoComSucesso, null, objectEmailComprador);

                // Consumidor receberá uma mensagem em seu WhatsApp sobre esta nova compra
                var mensagemConsumidor = $"Olá, {usuario.Nome}! \nSeu comprovante de compra de {cupom.Valor:C2}, realizada em {cupom.DataCompra.HorarioBrasilia():dd/MM/yyyy}, foi enviado com sucesso! ✔️ \n\nAgora é só aguardar *{dadosEstabelecimento.Estabelecimento}* confirmar. \n\nAbraços,\nEquipe Quanta Shop";
                await _whatsAppService.SendMessageAsync(usuario.Celular, mensagemConsumidor);

                Dictionary<string, string> bodyVendedor = null;
                ObjEmailUtilitis objectEmailVendedor = new()
                {
                    Data = DateTime.Now.HorarioBrasilia(),
                    From = "contato@quantashop.com.br",
                    FromName = _appSettings.FromName,
                    Subject = "Comprovante de compra recebido com sucesso! ✔️",
                    EmailSuporte = _appSettings.EmailSuporte,
                    SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"),
                    DestinationName = dadosComerciante.Nome,
                    To = dadosComerciante.Email
                };

                body = new Dictionary<string, string> { { "{{ name }}", objectEmailComprador.DestinationName }, { "{{ valor }}", valor }, { "{{ data }}", cupom.DataCompra.HorarioBrasilia().ToString("dd/MM/yyyy") } };

                await emailUtil.EnviarEmail(bodyVendedor, _appSettings.CupomNaoFiscalEnviadoComSucessoComerciante, null, objectEmailVendedor);

                // Comerciante receberá uma mensagem em seu WhatsApp sobre esta nova venda
                var mensagemVendedor = $"Olá, {dadosComerciante.Nome}! \nUma venda de {cupom.Valor:C2}, realizada em {cupom.DataCompra.HorarioBrasilia():dd/MM/yyyy}, foi recebida com sucesso! ✔️ \n\nNão esqueça de aprovar esta venda na sua agência para *{usuario.Nome}* receber seu cashback. \n\nAbraços,\nEquipe Quanta Shop";
                await _whatsAppService.SendMessageAsync(dadosComerciante.Celular, mensagemVendedor);
            }
            else
            {
                var consumidor = _usuarioNegocio.Get(x => x.Documento == viewModel.Documento).FirstOrDefault();

                if (consumidor is not null)
                {
                    var cashback = string.Format("{0:C2}", Convert.ToDouble(cupom.Valor * cupom.PercentualCashback));
                    var valor = string.Format("{0:C2}", Convert.ToDouble(cupom.Valor));
                    var emailUtil = new EmailUtilitis();
                    Dictionary<string, string> body = null;
                    ObjEmailUtilitis objectEmailComprador = new()
                    {
                        Data = DateTime.Now.HorarioBrasilia(),
                        From = "contato@quantashop.com.br",
                        FromName = _appSettings.FromName,
                        Subject = "Sua compra foi registrada com sucesso! ✔️",
                        EmailSuporte = _appSettings.EmailSuporte,
                        SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"),
                        DestinationName = consumidor.Nome,
                        To = consumidor.Email
                    };

                    body = new Dictionary<string, string> { { "{{ name }}", objectEmailComprador.DestinationName }, { "{{ estabelecimento }}", dadosEstabelecimento.Estabelecimento }, { "{{ valor }}", valor }, { "{{ data }}", cupom.DataCompra.HorarioBrasilia().ToString("dd/MM/yyyy") } };

                    await emailUtil.EnviarEmail(body, _appSettings.CupomNaoFiscalEnviadoComSucesso, null, objectEmailComprador);

                    var mensagemConsumidor = $"Olá, {consumidor.Nome}! \nSua compra de {cupom.Valor:C2}, realizada em {cupom.DataCompra.HorarioBrasilia():dd/MM/yyyy}, foi registrada com sucesso! ✔️ \n\nAgora é só aguardar *{dadosEstabelecimento.Estabelecimento}* confirmar seu cashback. \n\nAbraços,\nEquipe Quanta Shop";
                    await _whatsAppService.SendMessageAsync(consumidor.Celular, mensagemConsumidor);
                }
            }

            var cuponResult = _mapper.Map<CupomCashbackViewModel>(cupom);

            return Created($"/api/venda/consultarVenda?token={cupom.Token}", cuponResult);
        }

        /// <summary>
        /// Consulta as informações sobre a venda, caso criada para o usuário logado.
        /// </summary>
        /// <param name="token"></param>
        [ProducesResponseType(typeof(CriarCupomViewModel), StatusCodes.Status200OK)]
        [HttpGet("consultarVenda")]
        public async Task<ActionResult<CupomCashbackViewModel>> ConsultarVenda(string token)
        {
            if (!DigitoTokenVendaValidator.ValidarDigitoVerificador(token))
            {
                throw new PadraoException("token_venda_invalido");
            }

            var cupon = await _cuponCashbackNegocio.ObterCuponAsync(token);

            if (cupon.IdComerciante != IdUsuarioLogado && cupon.CuponCashbackPedido.Pedido.IdUsuario != IdUsuarioLogado)
            {
                throw new UnauthorizedException("");
            }

            if (cupon == null)
            {
                throw new NotFoundException("token_venda_nao_encontrado");
            }

            var result = _mapper.Map<CupomCashbackViewModel>(cupon);

            return result;
        }

        /// <summary>
        /// Consulta os tipos de pagamento para venda offline.
        /// </summary>
        [ProducesResponseType(typeof(CriarCupomViewModel), StatusCodes.Status200OK)]
        [HttpGet("obterTiposPagamento")]
        public ActionResult<IEnumerable<TipoPagamentoViewModel>> ObterTiposPagamento()
        {
            return Ok(_tipoPagamentoNegocio.GetTipoPagamentoVendaOffline());
        }

        /// <summary>
        /// Aprova os cupons inseridos pelo usuário
        /// </summary>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpPost("aprovarCupom")]
        public async Task<ActionResult<bool>> AprovarCupom(AprovarCupomViewModel viewModel)
        {
            var cupom = await _cuponCashbackNegocio.AprovarReprovarCupomAsync(viewModel.Token, IdUsuarioLogado, viewModel.Aprovado, viewModel.Status, viewModel.Justificativa, viewModel.InformarCliente);
            return Ok(true);
        }

        /// <summary>
        /// Aprova os cupons inseridos pelo usuário
        /// </summary>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpPost("aprovarCupoms")]
        public async Task<ActionResult<bool>> AprovarCupom(List<AprovarCupomViewModel> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                var cupom = await _cuponCashbackNegocio.AprovarReprovarCupomAsync(
                    viewModel.Token,
                    IdUsuarioLogado,
                    viewModel.Aprovado,
                    viewModel.Status,
                    $"Aprovado pelo usuario: {IdUsuarioLogado} na data {DateTime.Now}",
                    viewModel.InformarCliente
                );
            }

            return Ok(true);
        }


        private string SalvaImagem(string imageBase64, string idUsuario)
        {
            if (!string.IsNullOrEmpty(imageBase64))
            {
                var logoUrl = AzureStorage.CreateBlob(
                        imageBase64,
                new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        "imagens-comprovante",
                        idUsuario + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                        true
                    ).Result;

                return logoUrl;
            }

            return string.Empty;
        }

    }
}
