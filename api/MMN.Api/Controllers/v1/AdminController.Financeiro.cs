using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.Util.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    public partial class AdminController
    {
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
                throw new PadraoException("informe_plano_upgrade");

            var usuario = _usuarioNegocio.GetById(viewModel.IdUsuario);
            if (usuario == null)
                throw new PadraoException("usuario_invalido");

            var planoSelecionado = _produtoNegocio.FirstNoTracking(p => p.Ativo && p.IdProduto == viewModel.IdProduto);
            if (planoSelecionado == null)
                throw new PadraoException("plano_invalido");

            var planoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (planoAtivo == null || planoAtivo.Produto.IdProduto < planoSelecionado.IdProduto)
            {
                if (_usuarioProdutoNegocio.InserirPlanoManual(usuario, planoSelecionado, planoAtivo.IdUsuarioProduto))
                    return Ok();
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
                throw new PadraoException("usuario_invalido");

            var planoSelecionado = _produtoNegocio.FirstNoTracking(p => p.Ativo && p.IdProduto == viewModel.IdProduto);
            if (planoSelecionado == null)
                throw new PadraoException("plano_invalido");

            var planoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(usuario.IdUsuario);
            if (planoAtivo == null || planoAtivo.Produto.IdProduto < planoSelecionado.IdProduto)
            {
                if (_usuarioProdutoNegocio.InserirPlanoPresente(usuario, planoSelecionado, planoAtivo.IdUsuarioProduto))
                    return Ok();
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
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.Usuario.Email).ToList() : pedidos.OrderBy(p => p.Usuario.Email).ToList(); break;
                case EnumOrdenacaoPedidos.Login:
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.Usuario.Login).ToList() : pedidos.OrderBy(p => p.Usuario.Login).ToList(); break;
                case EnumOrdenacaoPedidos.Patrocinador:
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.Usuario.UsuarioPai?.Login).ToList() : pedidos.OrderBy(p => p.Usuario.UsuarioPai?.Login).ToList(); break;
                case EnumOrdenacaoPedidos.Valor:
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.ValorPedido).ToList() : pedidos.OrderBy(p => p.ValorPedido).ToList(); break;
                case EnumOrdenacaoPedidos.Status:
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.Transacao.StatusViewModel.Nome).ToList() : pedidos.OrderBy(p => p.Transacao.StatusViewModel.Nome).ToList(); break;
                default:
                    pedidos = filtro.OrderDesc ? pedidos.OrderByDescending(p => p.DataPedido).ToList() : pedidos.OrderBy(p => p.DataPedido).ToList(); break;
            }

            var totalPages = (int)Math.Ceiling((double)pedidos.Count() / filtro.PerPage);
            var pedidosFiltrados = pedidos.Skip(filtro.PerPage * (filtro.Page - 1)).Take(filtro.PerPage).Select(s => new
            {
                s.IdPedido, s.Usuario.Login, s.Usuario.Celular,
                LoginPatrocinador = s.Usuario.UsuarioPai?.Login, s.Usuario.Email, s.Codigo,
                s.ValorPedido, s.Transacao?.IdStatus, Status = s.Transacao?.StatusViewModel?.Nome,
                s.DataPedido, s.DataReferencia, s.UrlBoleto, s.Ativo,
                Produto = s.Usuario.UsuarioProduto.FirstOrDefault(f => f.IdPedido == s.IdPedido)?.Produto?.Nome ?? string.Empty,
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

        [HttpPost]
        [Route("ObterFaturas")]
        public IActionResult ObterFaturas(FiltroFaturasAdmin viewModel)
        {
            var validator = new FiltroFaturasAdminValidator();
            var result = validator.Validate(viewModel);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));

            var faturas = _pedidoNegocio.Get(f =>
                f.Tipo == (int)TipoPedido.FaturaCashbackCredenciado
                && (!viewModel.DataInicial.HasValue || viewModel.DataInicial.Value.Date <= f.DataReferencia.Value.Date || viewModel.DataInicial.Value.Date <= f.DataPedido.Date)
                && (string.IsNullOrEmpty(viewModel.LoginCredenciado) || f.Usuario.Login.ToLower().Contains(viewModel.LoginCredenciado.ToLower()))
                && (string.IsNullOrEmpty(viewModel.LoginPatrocinador) || f.Usuario.UsuarioPai.Login.ToLower().Contains(viewModel.LoginPatrocinador.ToLower()))
                && (!viewModel.ValorInicial.HasValue || viewModel.ValorInicial <= f.ValorPedido)
                && (!viewModel.ValorFinal.HasValue || viewModel.ValorFinal >= f.ValorPedido)
                && (!viewModel.IdCidade.HasValue || f.Usuario.Credenciamentos.FirstOrDefault().IdCidade == viewModel.IdCidade)
                && (!viewModel.Status.HasValue && f.Status == f.Status ||
                    f.Status == (int)viewModel.Status && f.Pago && f.DataPagamento.HasValue ||
                    f.Status == (int)viewModel.Status && !string.IsNullOrEmpty(f.UrlBoleto) ||
                    f.Status == (int)viewModel.Status && f.DataPagamento > DateTime.Now && !f.DataPagamento.HasValue),
                "Usuario.UsuarioPai", "Usuario.Credenciamento", "Usuario.Credenciamento.Cidade", "Usuario.Credenciamento.Categoria")
                .Select(s => new
                {
                    s.IdPedido, s.Usuario.Login, Patrocinador = s.Usuario.UsuarioPai?.Login, s.Usuario.Nome,
                    Cashback = string.Format("{0:P2}", s.Usuario?.Credenciamento?.PercentualCashback / 100 ?? 0),
                    s.ValorPedido, s.DataPagamento, s.DataPedido,
                    Cidade = s.Usuario?.Credenciamento?.CidadeViewModel?.Nome,
                    Categoria = s.Usuario?.Credenciamento?.CategoriaViewModel?.Nome,
                    s.Status
                });

            if (viewModel.Ordenacao.HasValue && viewModel.Asc.HasValue)
            {
                switch (viewModel.Ordenacao)
                {
                    case EnumOrdenacaoFaturas.Patrocinador: faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Patrocinador) : faturas.OrderBy(v => v.Patrocinador); break;
                    case EnumOrdenacaoFaturas.Login:        faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Login)        : faturas.OrderBy(v => v.Login);        break;
                    case EnumOrdenacaoFaturas.Nome:         faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Nome)         : faturas.OrderBy(v => v.Nome);         break;
                    case EnumOrdenacaoFaturas.Cashback:     faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Cashback)     : faturas.OrderBy(v => v.Cashback);     break;
                    case EnumOrdenacaoFaturas.Cidade:       faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Cidade)       : faturas.OrderBy(v => v.Cidade);       break;
                    case EnumOrdenacaoFaturas.Categoria:    faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Categoria)    : faturas.OrderBy(v => v.Categoria);    break;
                    case EnumOrdenacaoFaturas.DataFatura:   faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.DataPedido)   : faturas.OrderBy(v => v.DataPedido);   break;
                    case EnumOrdenacaoFaturas.DataPagamento:faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.DataPagamento): faturas.OrderBy(v => v.DataPagamento);break;
                    case EnumOrdenacaoFaturas.Status:       faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.Status)       : faturas.OrderBy(v => v.Status);       break;
                    case EnumOrdenacaoFaturas.Valor:        faturas = viewModel.Asc.Value ? faturas.OrderByDescending(v => v.ValorPedido)  : faturas.OrderBy(v => v.ValorPedido);  break;
                }
            }
            else
            {
                faturas = faturas.OrderByDescending(o => o.DataPedido);
            }

            System.Collections.Generic.IEnumerable<object> faturasFiltradas;
            if (viewModel.ObterTodos.HasValue && (bool)viewModel.ObterTodos)
                faturasFiltradas = faturas;
            else
                faturasFiltradas = faturas.Skip(viewModel.QuantidadePorPagina * (viewModel.Pagina - 1)).Take(viewModel.QuantidadePorPagina).ToList();

            return Ok(new { viewModel.QuantidadePorPagina, viewModel.Pagina, faturasFiltradas, quantidadeTotal = faturas.Count() });
        }

        [HttpPost]
        [Route("ObterCuponsCompraUsuario")]
        public IActionResult ObterCuponsCompraUsuario(FiltroVendasCredenciando viewModel)
        {
            return Ok(_cupomCashbackNegocio.ObterCuponsCompraUsuarioAdmin(viewModel));
        }
    }
}
