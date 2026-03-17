using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Api.ViewModel.Pedido;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Jwt;
using MMN.Util.Model;
using MMN.Util.Translation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly ITokenUtil _token;
        private readonly IGraduacaoNegocio _graduacaoNegocio;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly ILocation _location;
        private readonly ITransacaoNegocio _transacaoNegocio;
        private readonly IAnuncianteNegocio _anuncianteNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly ISaqueNegocio _saqueNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IProdutoNegocio _produtoNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;
        private readonly ISuporteNegocio _suporteNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        private readonly ICupomCashbackNegocio _cupomCashbackNegocio;
        private readonly ICache _cache;
        private readonly AppSettings _appSettings;
        private readonly TokenManagement _tokenManagement;
        private readonly DatabaseContext _context;
        private readonly IUsersService _userService;
        private readonly IAdminService _adminService;

        public AdminController(
            IUsuarioNegocio usuarioNegocio,
            ILocation location,
            IGraduacaoNegocio graduacaoNegocio,
            IConfiguracaoNegocio configNegocio,
            IOptions<AppSettings> appSettings,
            ITokenUtil token,
            ITransacaoNegocio transacaoNegocio,
            IAnuncianteNegocio anuncianteNegocio,
            ILancamentoNegocio lancamentoNegocio,
            IPedidoNegocio pedidoNegocio,
            ICategoriaNegocio categoriaNegocio,
            ISaqueNegocio saqueNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IProdutoNegocio produtoNegocio,
            IProceduresRepositorio proceduresRepositorio,
            IOptions<TokenManagement> tokenManagement,
            ISuporteNegocio suporteNegocio,
            ICredenciamentoNegocio credenciamentoNegocio,
            ICupomCashbackNegocio cupomCashbackNegocio,
            DatabaseContext context,
             ICache cache,
             IUsersService userService,
             IAdminService adminService)
        {
            _usuarioNegocio = usuarioNegocio;
            _location = location;
            _graduacaoNegocio = graduacaoNegocio;
            _configNegocio = configNegocio;
            _appSettings = appSettings.Value;
            _token = token;
            _categoriaNegocio = categoriaNegocio;
            _transacaoNegocio = transacaoNegocio;
            _anuncianteNegocio = anuncianteNegocio;
            _lancamentoNegocio = lancamentoNegocio;
            _pedidoNegocio = pedidoNegocio;
            _saqueNegocio = saqueNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _produtoNegocio = produtoNegocio;
            _proceduresRepositorio = proceduresRepositorio;
            _tokenManagement = tokenManagement.Value;
            _suporteNegocio = suporteNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _cupomCashbackNegocio = cupomCashbackNegocio;
            _context = context;
            _cache = cache;
            _userService = userService;
            _adminService = adminService;
        }

        [HttpPost]
        [Route("filtrarUsuarios")]
        public IActionResult FiltrarUsuarios(FiltroUsuario filtroUsuario)
        {
            var usuarios = _usuarioNegocio.FiltrarUsuarios(filtroUsuario);

            return Ok(usuarios);
        }


        [HttpPost]
        [Route("editarDadosUsuario")]
        public IActionResult EditarDadosUsuario(AdminDadosUsuarioViewModel view)
        {
            var validacao = new AdminDadosUsuarioViewModelValidator();
            var result = validacao.Validate(view);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _usuarioNegocio.GetById(view.IdUsuario);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            //todo: Criar traduções LoginIndisponivel
            if (!_usuarioNegocio.VerificaLoginDisponivel(usuario, view.Login))
            {
                throw new PadraoException("login_em_uso");
            }

            if (!_usuarioNegocio.VerificaPatrocinador(view.LoginPatrocinador))
            {
                throw new NotFoundException("patrocinador_nao_encontrado");
            }

            if (!_usuarioNegocio.AdminAtualizarDadosUsuario(view))
            {
                throw new Exception();
            }

            return Ok(new { message = _location.GetTranslation("DadosUsuarioAtualizado") });
        }

        [HttpGet]
        [Route("dadosUsuario/{idUsuario}")]
        public IActionResult DadosUsuario(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.ObterDadosPessoais(idUsuario);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("listaGraduacoes")]
        public IActionResult listaGraduacoes()
        {
            return Ok(_graduacaoNegocio.GetAll());
        }

        [HttpGet]
        [Route("redefinirSenhaUsuairo/{idUsuario}")]
        public IActionResult RedefinirSenhaUsuairo(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            var webToken = _token.ConstruirToken(usuario);

            var rootSite = _configNegocio.BuscarRootSite().Valor;

            var link = rootSite + _appSettings.RootSiteResetPassword + webToken;

            link = link.Replace("quantashop.com.br", "escritorio.quantashop.com.br");

            return Ok(link);
        }

        [HttpGet]
        [Route("bloquearUsuairoAdmin/{idUsuario}")]
        public IActionResult BloquearUsuairoAdmin(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.GetById(idUsuario);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            return Ok(_usuarioNegocio.BloquearUsuarioAdmin(idUsuario));
        }

        [HttpPost]
        [Route("RelatorioAnunciantes")]
        public async Task<IActionResult> RelatorioAnunciantes(FiltroRelatorioAnunciantesViewModel filtro)
        {
            var anunciantes = _anuncianteNegocio.Get(a =>
                a.IdAfilio != null || a.IdAwin != null
                && (!string.IsNullOrEmpty(a.IdAwin) || !string.IsNullOrEmpty(a.IdAfilio)),
                "AnuncianteCashBack",
                "CategoriaAnunciante");

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                anunciantes = anunciantes.Where(a => a.Nome.Contains(filtro.Nome)).ToList();
            }

            if (!string.IsNullOrEmpty(filtro.Moeda))
            {
                anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(b => !string.IsNullOrEmpty(b.Moeda) && b.Moeda.Contains(filtro.Moeda))).ToList();
            }

            if (filtro.Status.HasValue)
            {
                if (filtro.Status.Value == EnumStatusAnunciante.Ativo)
                {
                    anunciantes = anunciantes.Where(a => a.Ativo).ToList();
                }
                else if (filtro.Status.Value == EnumStatusAnunciante.Inativo)
                {
                    anunciantes = anunciantes.Where(a => !a.Ativo).ToList();
                }
            }

            if (filtro.TipoCashback.HasValue)
            {
                switch (filtro.TipoCashback)
                {
                    case EnumTipoCashbackAnunciante.Percentual:
                        anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(c => !string.IsNullOrEmpty(c.Tipo) && c.Tipo.Equals("percentage"))).ToList();
                        break;

                    case EnumTipoCashbackAnunciante.Valor:
                        anunciantes = anunciantes.Where(a => a.AnuncianteCashBack.Any(c => !string.IsNullOrEmpty(c.Tipo) && c.Tipo.Equals("amount"))).ToList();
                        break;
                }
            }

            if (filtro.Conexao.HasValue)
            {
                switch (filtro.Conexao)
                {
                    case EnumConexaoAnunciante.Awin:
                        anunciantes = anunciantes.Where(a => a.IdAwin != null).ToList();
                        break;

                    case EnumConexaoAnunciante.Afilio:
                        anunciantes = anunciantes.Where(a => a.IdAfilio != null).ToList();
                        break;
                }
            }

            if (filtro.Ordenacao.HasValue && filtro.Asc.HasValue)
            {
                switch (filtro.Ordenacao)
                {
                    case EnumOrdenacaoAnuncitantes.Nome:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.Nome).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.Nome).ToList();
                        break;

                    case EnumOrdenacaoAnuncitantes.CashbackMax:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.CashbackMax).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.CashbackMax).ToList();
                        break;

                    case EnumOrdenacaoAnuncitantes.CashbackMin:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.CashbackMin).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.CashbackMin).ToList();
                        break;

                    case EnumOrdenacaoAnuncitantes.TipoCashback:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.TipoCashback).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.TipoCashback).ToList();
                        break;

                    case EnumOrdenacaoAnuncitantes.Moeda:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.Moeda).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.Moeda).ToList();
                        break;

                    case EnumOrdenacaoAnuncitantes.Status:
                        if (!filtro.Asc.Value) anunciantes = anunciantes.OrderBy(a => a.Ativo).ToList();
                        else anunciantes = anunciantes.OrderByDescending(a => a.Ativo).ToList();
                        break;

                }
            }

            else
            {
                anunciantes = anunciantes.OrderByDescending(a => a.IdAwin).ThenBy(a => a.Nome).ToList();
            }

            var anunciantesFiltrados = anunciantes;

            var totalPages = (int)Math.Ceiling((double)anunciantes.Count() / filtro.QuantidadePorPagina);
            anunciantesFiltrados = _anuncianteNegocio.BuscarCashback(anunciantesFiltrados.ToList());

            return Ok(new { totalPages, filtro.QuantidadePorPagina, filtro.Pagina, anunciantesFiltrados, quantidadeTotal = anunciantes.Count() });
        }

        [HttpGet]
        [Route("RelatorioCadastrosMes")]
        public async Task<IActionResult> RelatorioCadastrosMes()
        {
            return Ok(_usuarioNegocio.Get(u => u.DataCadastro > DateTime.Now.AddMonths(-5))
                .GroupBy(g => g.DataCadastro.Value.ToString("yyyy/MM")).Select(u => new
                {
                    data = u.Key,
                    quantidade = u.Count()
                }).OrderBy(o => o.data));
        }

        [HttpGet]
        [Route("TotalUsuariosGraduacao")]
        public async Task<IActionResult> TotalUsuariosGraduacao()
        {
            return Ok(_graduacaoNegocio.Get(w => true, "Usuario")
                .Select(g => new
                {
                    g.Nome,
                    QuantidadeUsuarios = g.UsuarioViewModel.Count
                })
            );
        }

        [HttpGet]
        [Route("ResumoIntegracaoApi")]
        public IActionResult ResumoIntegracaoApi()
        {
            return Ok(
                _transacaoNegocio.Get(t =>
                    t.Ativo &&
                    t.IdAnunciante.HasValue &&
                    t.IdStatus == 2, "Anunciante").GroupBy(g => new { g.IdAnunciante, g.AnuncianteViewModel }).Select(g => new
                    {
                        loja = new { g.Key.AnuncianteViewModel.Nome, g.Key.AnuncianteViewModel.Ativo },
                        valorTotalComissao = g.Sum(s => s.ComissaoTotal),
                    }).OrderByDescending(o => o.valorTotalComissao).Take(10)
                );
        }

        [HttpGet]
        [Route("DistribuicaoDetalhada")]
        public async Task<IActionResult> DistribuicaoDetalhada()
        {
            return Ok(
                new List<object>() { new {
                        nome = "Cashback",
                        valor = _lancamentoNegocio.Get(l =>
                            l.Ativo &&
                            l.IdStatus == (int) StatusTransacaoEnum.Finalizada &&
                            (l.Tipo.Chave == "CB" || l.Tipo.Chave == "DTCSH"), "Tipo").Sum(s => s.Valor)
                    }, new {
                        nome = "Adesão paga ao patrocinador",
                        valor = _lancamentoNegocio.Get(l =>
                            l.Ativo &&
                            l.IdStatus == (int) StatusTransacaoEnum.Finalizada &&
                            (l.Tipo.Chave == "DTAD" || l.Tipo.Chave == "DTBAD"), "Tipo").Sum(s => s.Valor)
                    } }
            );
        }

        [HttpGet]
        [Route("ObterResumoValores")]
        public IActionResult ObterResumoValores()
        {
            try
            {
                var resumo = _cache.GetItem("ObterResumoValores");

                if (resumo != null)
                    return Ok(resumo);

                var usuarios = _usuarioNegocio.GetAll();

                var totalConsumo = _transacaoNegocio.Get(t => t.Ativo && t.IdStatus == 2 && t.IdAnunciante.HasValue).Sum(s => s.ValorPrincipal);
                var totalCashback = _lancamentoNegocio.ObterCashbackDetalahdo();
                var totalConsumoPlanos = _pedidoNegocio.TotalConsumoPlanos();
                var totalSacado = _saqueNegocio.Get(s => s.IdStatus == 2).Sum(s => s.Valor);
                var usuariosAtivos = usuarios.Where(w => w.Ativo).Count();
                var usuariosInativos = usuarios.Where(w => !w.Ativo).Count();

                resumo = new
                {
                    totalConsumo,
                    totalCashback,
                    totalConsumoPlanos,
                    totalSacado,
                    usuariosAtivos,
                    usuariosInativos
                };

                _cache.SetItem("ObterResumoValores", resumo, DateTime.Now.AddHours(2));

                return Ok(resumo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AtivarDesativarUsuario")]
        public IActionResult AtivarDesativarUsuario(UsuarioViewModel viewModel)
        {
            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario != null)
            {
                usuario.Ativo = !usuario.Ativo;
                _usuarioNegocio.Update(usuario);
                return Ok(usuario.Ativo);
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("confirmar-email")]
        public IActionResult ConfirmarEmail(UsuarioViewModel viewModel)
        {
            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario != null)
            {
                usuario.EmailConfirmado = true;
                _usuarioNegocio.Update(usuario);
                return Ok(usuario.EmailConfirmado);
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpPost]
        [Route("gerarLancamentoManual")]
        public async Task<IActionResult> GerarLancamentoManual(LancamentoManualViewModel viewModel)
        {
            var validate = new LancamentoManualViewModelValidator();
            var result = await validate.ValidateAsync(viewModel);

            if (result.IsValid)
            {
                _lancamentoNegocio.GerarLancamentoManual(viewModel, IdUsuarioLogado);
                return Ok(new { message = "Lançamento manual gerado com sucesso!" });
            }

            throw new NotFoundException("verifique_campos");
        }

        [HttpPost]
        [Route("UpgradeManual")]
        public async Task<IActionResult> UpgradeManual(UpgradeManualViewModel viewModel)
        {
            var validator = new UpgradeManualViewModelValidator();
            var result = await validator.ValidateAsync(viewModel);
            if (!result.IsValid)
            {
                throw new PadraoException("informe_plano_upgrade");
            }

            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario == null)
            {
                throw new PadraoException("usuario_invalido");
            }

            var planoSelecionado = _produtoNegocio.FirstNoTracking(p => p.Ativo && p.IdProduto == viewModel.IdProduto);
            if (planoSelecionado == null)
            {
                throw new PadraoException("plano_invalido");
            }

            var planoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (planoAtivo == null || planoAtivo.Produto.IdProduto < planoSelecionado.IdProduto)
            {
                if (_usuarioProdutoNegocio.InserirPlanoManual(usuario, planoSelecionado, planoAtivo.IdUsuarioProduto))
                {
                    return Ok();
                }

                throw new Exception();
            }
            throw new PadraoException("compra_upgrade_produto_menor");
        }

        [HttpPost]
        [Route("UpgradePresente")]
        public async Task<IActionResult> UpgradePresente(UpgradeManualViewModel viewModel)
        {
            var validator = new UpgradeManualViewModelValidator();
            var result = await validator.ValidateAsync(viewModel);
            if (!result.IsValid)
                throw new PadraoException("informe_plano_upgrade");

            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario == null)
            {
                throw new PadraoException("usuario_invalido");
            }

            var planoSelecionado = _produtoNegocio.FirstNoTracking(p => p.Ativo && p.IdProduto == viewModel.IdProduto);
            if (planoSelecionado == null)
            {
                throw new PadraoException("plano_invalido");
            }

            var planoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (planoAtivo == null || planoAtivo.Produto.IdProduto < planoSelecionado.IdProduto)
            {
                if (_usuarioProdutoNegocio.InserirPlanoPresente(usuario, planoSelecionado, planoAtivo.IdUsuarioProduto))
                {
                    return Ok();
                }

                throw new Exception();
            }
            throw new PadraoException("compra_upgrade_produto_menor");
        }

        [HttpGet]
        [Route("obterPlanoAtivoUsuario/{IdUsuario}")]
        public async Task<IActionResult> obterPlanoAtivoUsuario(Guid IdUsuario)
        {
            var planoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(IdUsuario);
            if (planoAtivo == null) return Ok();

            return Ok(planoAtivo.Produto);
        }

        [HttpPost]
        [Route("obterPedidosPlanos")]
        public IActionResult ObterPedidosPlanos(FiltroPedidosAdmin filtro)
        {
            var pedidos = _pedidoNegocio.Get(w =>
                (w.Tipo == (int)TipoPedido.Baf || w.Tipo == (int)TipoPedido.Saldo)
                && (string.IsNullOrEmpty(filtro.LoginEmail) || w.Usuario.Login.Contains(filtro.LoginEmail) || w.Usuario.Email.Contains(filtro.LoginEmail))
                && (!filtro.IdStatus.HasValue || w.Transacao.IdStatus == filtro.IdStatus)
                && (string.IsNullOrEmpty(filtro.CodigoPedido) || w.Codigo.Equals(filtro.CodigoPedido)),
                "Usuario.UsuarioProduto.Produto", "Usuario.UsuarioPai", "Transacao.Status");

            switch (filtro.Ordenacao)
            {
                case EnumOrdenacaoPedidos.Email:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.Usuario.Email).ToList();
                    else pedidos = pedidos.OrderBy(p => p.Usuario.Email).ToList();
                    break;
                case EnumOrdenacaoPedidos.Login:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.Usuario.Login).ToList();
                    else pedidos = pedidos.OrderBy(p => p.Usuario.Login).ToList();
                    break;
                case EnumOrdenacaoPedidos.Patrocinador:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.Usuario.UsuarioPai?.Login).ToList();
                    else pedidos = pedidos.OrderBy(p => p.Usuario.UsuarioPai?.Login).ToList();
                    break;
                case EnumOrdenacaoPedidos.Valor:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.ValorPedido).ToList();
                    else pedidos = pedidos.OrderBy(p => p.ValorPedido).ToList();
                    break;
                case EnumOrdenacaoPedidos.Status:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.Transacao.StatusViewModel.Nome).ToList();
                    else pedidos = pedidos.OrderBy(p => p.Transacao.StatusViewModel.Nome).ToList();
                    break;
                default:
                    if (filtro.OrderDesc) pedidos = pedidos.OrderByDescending(p => p.DataPedido).ToList();
                    else pedidos = pedidos.OrderBy(p => p.DataPedido).ToList();
                    break;
            }

            var totalPages = (int)Math.Ceiling((double)pedidos.Count() / filtro.PerPage);

            var pedidosFiltrados = pedidos.Skip(filtro.PerPage * (filtro.Page - 1))
            .Take(filtro.PerPage)
            .Select(s => new
            {
                s.IdPedido,
                s.Usuario.Login,
                s.Usuario.Celular,
                LoginPatrocinador = s.Usuario.UsuarioPai?.Login,
                s.Usuario.Email,
                s.Codigo,
                s.ValorPedido,
                s.Transacao?.IdStatus,
                Status = s.Transacao?.StatusViewModel?.Nome,
                s.DataPedido,
                s.DataReferencia,
                s.UrlBoleto,
                s.Ativo,
                Produto = s.Usuario.UsuarioProduto
                    .FirstOrDefault(f => f.IdPedido == s.IdPedido)?.Produto?.Nome
                    ?? string.Empty,
            }).ToList();

            return Ok(new { totalPages, filtro.PerPage, filtro.Page, pedidosFiltrados, quantidadeTotal = pedidos.Count() });
        }

        [HttpPost]
        [Route("cancelarPedido")]
        public IActionResult CancelarPedido(CancelarPedidoViewModel viewModel)
        {
            _pedidoNegocio.CancelarPedido(viewModel.IdPedido, IdUsuarioLogado);
            return Ok();
        }

        [HttpPost]
        [Route("aprovarAlteracaoPedido/{idPedido}")]
        public IActionResult AprovarAlteracaoPedido(long idPedido)
        {
            var pedido = _pedidoNegocio.FirstNoTracking(p => p.IdPedido == idPedido, "Transacao");
            pedido.Transacao.IdStatus = (int)StatusTransacaoEnum.Cancelada;
            _transacaoNegocio.Update(pedido.Transacao);
            return Ok();
        }

        [HttpPost, Route("obterSolicitacoes/"), Authorize]
        public IActionResult ObterSolicitacoesAdmin([FromBody] FiltroSuporteViewModel filtro)
        {

            var solicitacoes = _suporteNegocio.GetAll("Usuario", "Usuario.UsuarioPai", "Status");

            solicitacoes = filtro.DataInicioInicio.HasValue ? solicitacoes.Where(s => s.DataSolicitacao >= filtro.DataInicioInicio).ToList() : solicitacoes;
            solicitacoes = filtro.DataInicioFim.HasValue ? solicitacoes.Where(s => s.DataSolicitacao <= filtro.DataInicioFim).ToList() : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoInicio.HasValue ? solicitacoes.Where(s => s.DataAtualizacao >= filtro.DataAtualizacaoInicio).ToList() : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoFim.HasValue ? solicitacoes.Where(s => s.DataAtualizacao <= filtro.DataAtualizacaoFim).ToList() : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginPatrocinador) ? solicitacoes.Where(s => s.Usuario.UsuarioPai != null && s.Usuario.UsuarioPai.Login.Contains(filtro.LoginPatrocinador)).ToList() : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginUsuario) ? solicitacoes.Where(s => s.Usuario.Login.Contains(filtro.LoginUsuario)).ToList() : solicitacoes;
            solicitacoes = filtro.IdStatus.HasValue ? solicitacoes.Where(s => s.IdStatus == filtro.IdStatus.Value).ToList() : solicitacoes;
            solicitacoes = filtro.IdTipo.HasValue ? solicitacoes.Where(s => s.TipoContato == (TipoContatoEnum)filtro.IdTipo.Value).ToList() : solicitacoes;
            solicitacoes = filtro.IdStatus.HasValue ? solicitacoes.Where(s => s.IdStatus == filtro.IdStatus.Value).ToList() : solicitacoes;

            var result = new List<object>();

            foreach (var solicitacao in solicitacoes.OrderByDescending(s => s.DataSolicitacao))
            {
                result.Add(new
                {
                    idSolicitacao = solicitacao.IdSuporte,
                    dataPedido = solicitacao.DataCompra,
                    dataAtualizacao = solicitacao.DataAtualizacao,
                    dataSolicitacao = solicitacao.DataSolicitacao,
                    tipoId = (int)solicitacao.TipoContato,
                    tipo = solicitacao.TipoContato.GetDescription(),
                    statusId = solicitacao.Status.IdStatus,
                    status = solicitacao.Status.Nome,
                    loginUsuario = solicitacao.Usuario.Login,
                    loja = solicitacao.SiteCompra,
                    nome = solicitacao.Usuario.Nome,
                    email = solicitacao.Usuario.Email,
                    comprovante = solicitacao.UrlComprovante,
                    descricao = solicitacao.Observacao,
                    observacao = solicitacao.ObservacaoAdmin,
                    valor = solicitacao.ValorPedido,
                    telefone = solicitacao.Usuario.Celular,
                    loginPatrocinador = solicitacao.Usuario.UsuarioPai?.Login ?? string.Empty,
                });
            }

            return Ok(result);
        }



        [HttpGet]
        [Route("efetuarAcessoRemoto/{idUsuario}")]
        public IActionResult EfetuarAcessoRemoto(Guid idUsuario)
        {
            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == idUsuario, "Grupo", "UsuarioEndereco");
            if (usuario == null)
                throw new NotFoundException("usuario_nao_encontrado");

            if (!usuario.EmailConfirmado)
                throw new PadraoException("usuario_remoto_email_confirmado");

            var claim = new[]
            {
                    new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Grupo.Descricao),
                    new Claim(ClaimTypes.PrimarySid, usuario.Grupo.IdGrupo.ToString()),
                    new Claim(ClaimTypes.PrimaryGroupSid, usuario.IdGraduacao.HasValue ? usuario.IdGraduacao.ToString() : "0"),
                    new Claim("Cultura", usuario.Cultura),
                    new Claim("IdUsurioAdminRemoto", IdUsuarioLogado.ToString())
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            var comerciante = false;
            var credenciamento = new CredenciamentoViewModel();
            if (usuario.Grupo.Descricao == "Comerciante")
            {
                comerciante = true;
                credenciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == usuario.IdUsuario);
            }

            if (usuario.Empreendedor)
            {
                usuario.Perfil = 'E';
            }

            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new
            {
                Id = usuario.IdUsuario,
                Username = usuario.Nome,
                usuario.Login,
                usuario.Email,
                token,
                usuario.Grupo.Descricao,
                usuario.Cultura,
                usuario.UrlImg,
                logoUrl = comerciante && credenciamento != null && !string.IsNullOrEmpty(credenciamento.LogoUrl) ? credenciamento.LogoUrl : string.Empty,
                admin = usuario.IdGrupo == 1,
                usuario.Perfil,
                dadosCompletos = usuario.UsuarioEndereco.Count > 0,
                usuario.Empreendedor,
                comerciante
            });
        }

        [HttpGet]
        [Route("obterRedeUsuarioEspecifico/{login}")]
        public IActionResult ObterRedeUsuarioEspecifico(string login)
        {
            var usuario = _usuarioNegocio.First(f => f.Login == login);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            var diretos = _usuarioNegocio.ListaUsuarioDiretos(usuario.IdUsuario);

            var result = diretos
                .Select(async u => new
                {
                    u.IdUsuario,
                    u.Nome,
                    u.UrlImg,
                    u.Login,
                    u.Email,
                    u.DataCadastro,
                    u.Celular,
                    Graduacao = new { u.Graduacao.Nome },
                    TemFilhos = u.Filhos.Count > 0,
                    Pontuacao = _usuarioNegocio.GetPontosFromCache(u.IdUsuario).TotalPontosUsuario
                })
                .Select(s => s.Result);

            return Ok(result);
        }

        [HttpGet]
        [Route("obterRedeUsuarioEspecificoExport/{login}")]
        public IActionResult ObterRedeUsuarioEspecificoExport(string login)
        {
            var usuario = _usuarioNegocio.First(f => f.Login == login);

            if (usuario == null)
            {
                throw new NotFoundException("usuario_nao_encontrado");
            }

            //var filhos = _proceduresRepositorio.spc_UsuarioDownLine(usuario.IdUsuario);
            var lstUsuario = _usuarioNegocio.ListaUsuarioDiretos(usuario.IdUsuario);

            var listaRedeExport = new List<UsuarioExportViewModel>();

            foreach (var u in lstUsuario)
            {
                var user = new UsuarioExportViewModel
                {
                    Nivel = 0,
                    Login = u.Login,
                    Nome = u.Nome,
                    Email = u.Email,
                    Celular = u.Celular,
                    Graduacao = u.Graduacao.Nome,
                    DataCadastro = u.DataCadastro.Value.ToString("dd/mm/yyyy hh:mm")
                };
                listaRedeExport.Add(user);
                if (u.Filhos.Count > 0)
                {
                    var filhos = _proceduresRepositorio.spc_UsuarioDownLine(u.IdUsuario);
                    listaRedeExport.AddRange(filhos.Select(f => new UsuarioExportViewModel
                    {
                        Nivel = f.Nivel,
                        LoginFilho = f.Login,
                        Nome = f.Nome,
                        Email = f.Email,
                        Celular = f.Celular,
                        Graduacao = ((TipoGraduacao)f.IdGraduacao).GetDescription(),
                        DataCadastro = u.DataCadastro.Value.ToString("dd/mm/yyyy hh:mm"),
                    }));
                }
            }

            return Ok(listaRedeExport);
        }

        [HttpPost]
        [Route("obterSolicitacoesSuporte")]
        public IActionResult ObterSolicitacoesSuporte(FiltroSuporte viewModel)
        {
            return Ok(_suporteNegocio.FiltrarSolicitacoes(viewModel));
        }

        [HttpPost]
        [Route("atualizarStatusSuporte")]
        public IActionResult AtualizarStatusSuporte(dynamic viewModel)
        {
            var parsed = JsonConvert.DeserializeObject(viewModel.ToString());
            int idSuporte = Convert.ToInt32(parsed.idSuporte.Value);
            var suporte = _suporteNegocio.FirstNoTracking(f => f.IdSuporte == idSuporte);
            suporte.IdStatus = Convert.ToInt32(parsed.idStatus.Value);
            suporte.ObservacaoAdmin = parsed.observacaoAdmin.Value;
            suporte.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            suporte.IdUsuarioAcao = IdUsuarioLogado;

            _suporteNegocio.Update(suporte);

            return Ok("Atualizado com sucesso!");
        }

        [HttpPost]
        [Route("ObterFaturas")]
        public IActionResult ObterFaturas(FiltroFaturasAdmin viewModel)
        {
            var validator = new FiltroFaturasAdminValidator();
            var result = validator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(s => new PadraoException(s.ErrorMessage)));
            }

            var faturas = _pedidoNegocio.Get(f =>
                    f.Tipo == (int)TipoPedido.FaturaCashbackCredenciado
                    && (!viewModel.DataInicial.HasValue || viewModel.DataInicial.Value.Date <= f.DataReferencia.Value.Date || viewModel.DataInicial.Value.Date <= f.DataPedido.Date)
                    && (string.IsNullOrEmpty(viewModel.LoginCredenciado) || f.Usuario.Login.ToLower().Contains(viewModel.LoginCredenciado.ToLower()))
                    && (string.IsNullOrEmpty(viewModel.LoginPatrocinador) || f.Usuario.UsuarioPai.Login.ToLower().Contains(viewModel.LoginPatrocinador.ToLower()))
                    && (!viewModel.ValorInicial.HasValue || viewModel.ValorInicial <= f.ValorPedido)
                    && (!viewModel.ValorFinal.HasValue || viewModel.ValorFinal >= f.ValorPedido)
                    && (!viewModel.IdCidade.HasValue || f.Usuario.Credenciamentos.FirstOrDefault().IdCidade == viewModel.IdCidade)
                    && (
                        !viewModel.Status.HasValue && f.Status == f.Status ||
                        f.Status == (int)viewModel.Status && f.Pago && f.DataPagamento.HasValue ||
                        f.Status == (int)viewModel.Status && !string.IsNullOrEmpty(f.UrlBoleto) ||
                        f.Status == (int)viewModel.Status && f.DataPagamento > DateTime.Now && !f.DataPagamento.HasValue)
                , "Usuario.UsuarioPai", "Usuario.Credenciamento", "Usuario.Credenciamento.Cidade", "Usuario.Credenciamento.Categoria")
                    .Select(s => new
                    {
                        s.IdPedido,
                        s.Usuario.Login,
                        Patrocinador = s.Usuario.UsuarioPai?.Login,
                        s.Usuario.Nome,
                        Cashback = string.Format("{0:P2}", s.Usuario?.Credenciamento?.PercentualCashback / 100 ?? 0),
                        s.ValorPedido,
                        s.DataPagamento,
                        s.DataPedido,
                        Cidade = s.Usuario?.Credenciamento?.CidadeViewModel?.Nome,
                        Categoria = s.Usuario?.Credenciamento?.CategoriaViewModel?.Nome,
                        s.Status
                    });

            if (viewModel.Ordenacao.HasValue && viewModel.Asc.HasValue)
            {
                switch (viewModel.Ordenacao)
                {
                    case EnumOrdenacaoFaturas.Patrocinador:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Patrocinador).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Patrocinador).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Login:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Login).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Login).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Nome:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Nome).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Nome).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Cashback:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Cashback).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Cashback).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Cidade:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Cidade).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Cidade).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Categoria:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Categoria).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Categoria).ToList();
                        break;
                    case EnumOrdenacaoFaturas.DataFatura:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.DataPedido).ToList();
                        else faturas = faturas.OrderByDescending(v => v.DataPedido).ToList();
                        break;
                    case EnumOrdenacaoFaturas.DataPagamento:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.DataPagamento).ToList();
                        else faturas = faturas.OrderByDescending(v => v.DataPagamento).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Status:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.Status).ToList();
                        else faturas = faturas.OrderByDescending(v => v.Status).ToList();
                        break;
                    case EnumOrdenacaoFaturas.Valor:
                        if (!viewModel.Asc.Value) faturas = faturas.OrderBy(v => v.ValorPedido).ToList();
                        else faturas = faturas.OrderByDescending(v => v.ValorPedido).ToList();
                        break;
                }
            }
            else
            {
                faturas = faturas.OrderByDescending(o => o.DataPedido);
            }

            IEnumerable<object> faturasFiltradas;
            if (viewModel.ObterTodos.HasValue && (bool)viewModel.ObterTodos)
            {
                faturasFiltradas = faturas;
            }
            else
            {
                faturasFiltradas = faturas
                    .Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1))
                    .Take(viewModel.QuantidadePorPagina).ToList();
            }

            return Ok(new
            {
                viewModel.QuantidadePorPagina,
                viewModel.Pagina,
                faturasFiltradas,
                quantidadeTotal = faturas.Count()
            });
        }

        [HttpPost]
        [Route("ObterCuponsCompraUsuario")]
        public IActionResult ObterCuponsCompraUsuario(FiltroVendasCredenciando viewModel)
        {
            return Ok(_cupomCashbackNegocio.ObterCuponsCompraUsuarioAdmin(viewModel));
        }

        [HttpGet("obter-aniversariantes")]
        public async Task<IActionResult> ObterAniversariantes(int mes)
        {
            try
            {
                var aniversarios = await _userService.GetBirthdays(mes);
                return Ok(aniversarios);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-total-cadastros-por-dia")]
        public async Task<IActionResult> ObterTotalCadastrosPorDia(int mes, int ano)
        {
            try
            {
                var cadastros = await _userService.GetRegistrationsPerDay(mes, ano);
                return Ok(cadastros);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-cadastros-por-dia")]
        public async Task<IActionResult> ObterCadastrosPorDia(DateTime dia)
        {
            try
            {
                var cadastros = await _userService.GetRegistrationsPerDay(dia);
                return Ok(cadastros);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("obter-faturamento-ecossistema/{ecossistemaId}")]
        public async Task<IActionResult> ObterFaturamentoEcossistema(int ecossistemaId)
        {
            try
            {
                var faturamento = await _adminService.GetEcosystemBilling(ecossistemaId);
                return Ok(faturamento);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("obter-empresas-ecossistema/{ecossistemaId}")]
        public async Task<IActionResult> ObterEmpresasEcossistema(int ecossistemaId)
        {
            try
            {
                var empresas = await _adminService.GetCompaniesByEcosystem(ecossistemaId);
                return Ok(empresas);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
