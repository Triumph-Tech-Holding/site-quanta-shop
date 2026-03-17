using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Model;
using MMN.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MMN.Api.Models.Produto;
using MMN.Dominio.Enum;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]"), ApiController]
    public class ProdutoController : LoggedControllerBase
    {
        private readonly IProdutoNegocio _negocio;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
        private readonly IProdutoNivelNegocio _produtoNivelNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly AppSettings _appSettings;

        public ProdutoController(IOptions<AppSettings> appSettings,
            IProdutoNegocio negocio,
            ICategoriaNegocio categoriaNegocio,
            IUsuarioProdutoNegocio usuarioProdutoNegocio,
            IProdutoNivelNegocio produtoNivelNegocio,
            IUsuarioNegocio usuarioNegocio,
            IPedidoNegocio pedidoNegocio,
            ILancamentoNegocio lancamentoNegocio)
        {
            _appSettings = appSettings.Value;
            _negocio = negocio;
            _categoriaNegocio = categoriaNegocio;
            _usuarioProdutoNegocio = usuarioProdutoNegocio;
            _produtoNivelNegocio = produtoNivelNegocio;
            _pedidoNegocio = pedidoNegocio;
            _usuarioNegocio = usuarioNegocio;
            _lancamentoNegocio = lancamentoNegocio;
        }

        [HttpGet]
        [Route("buscar")]
        public IActionResult Buscar(string categoria = "INV", bool? ativo = null)
        {
            var produtos = _negocio.GetNoTracking(g =>
                (categoria == null || g.Categoria.Chave == categoria) &&
                (ativo == null || g.Ativo == ativo));

            return Ok(produtos.Select(p => new
            {
                p.IdProduto,
                p.Nome,
                p.Valor,
                p.Pontos,
                p.TetoBinario,
                p.Ativo,
                p.DataCriacao,
                p.DataAutalizacao,
                p.Visivel
            }));
        }

        [HttpGet]
        [Route("buscarAtivos")]
        public IActionResult BuscarAtivos()
        {
            var produtos = _negocio.BuscarAtivos();
            produtos = produtos.Where(p => p.Visivel).ToList();
            return Ok(produtos.Select(p => new { p.IdProduto, p.Nome, p.Valor }));

        }

        [HttpGet]
        [Route("buscarTodos")]
        public IActionResult BuscarTodos()
        {
            var produtos = _negocio.BuscarTodos();
            return Ok(produtos.Select(p => new { p.IdProduto, p.Nome, p.Valor, p.Pontos, p.TetoBinario, p.Ativo, p.DataCriacao, p.DataAutalizacao, p.Visivel }));
        }

        [HttpGet]
        [Route("buscarAgrupadoPorCategoria")]
        public IActionResult BuscarAgrupadoPorCategoria()
        {
            var categoria = _categoriaNegocio.BuscarAtivos("INVST");
            var niveis = _produtoNivelNegocio.GetAll().ToList();
            UsuarioProdutoViewModel produtoAtivo = _usuarioProdutoNegocio.BuscarProdutoAtivo(IdUsuarioLogado);
            var valorPago = _pedidoNegocio.ObterValorPagoBaf(IdUsuarioLogado);

            var response = categoria.Select(c => new
            {
                c.IdCategoria,
                c.ChaveTraducao,
                c.Nome,
                Produtos = c.Produtos.Where(p => (produtoAtivo == null || p.Pontos > produtoAtivo.Produto.Pontos) && p.Visivel && p.IdProduto != 12).Select(p => new
                {
                    p.IdProduto,
                    p.Descricao,
                    p.ImagemUrl,
                    p.Nome,
                    p.Pontos,
                    p.Parcelas,
                    valorOriginal = p.Valor,
                    valor = p.Valor - valorPago,
                    DisponivelCompra = true,
                    Upgrade = produtoAtivo != null,
                    p.TetoBinario
                }).OrderBy(p => p.Pontos).ToList(),
                niveis
            });

            return Ok(response);
        }

        [HttpGet]
        [Route("buscarPlanoAssinatura")]
        public IActionResult BuscarPlanoAssinatura()
        {
            var categoria = _categoriaNegocio.BuscarAtivos("ASS");
            var niveis = _produtoNivelNegocio.GetAll().ToList();
            UsuarioProdutoViewModel produtoAtivo = _usuarioProdutoNegocio.BuscarAssinaturaAtiva(IdUsuarioLogado);
            var valorPago = _pedidoNegocio.ObterValorPagoBaf(IdUsuarioLogado);

            var pedido = _pedidoNegocio.Get(x => x.IdUsuario == IdUsuarioLogado && x.Tipo == (int)TipoPedido.Assinatura && !x.Cancelado.Value && x.Pago).FirstOrDefault();

            var response = categoria.Select(c => new
            {
                c.IdCategoria,
                c.ChaveTraducao,
                c.Nome,
                Produtos = c.Produtos.Where(x => x.Ativo).Select(p => new
                {
                    p.IdProduto,
                    p.Descricao,
                    p.ImagemUrl,
                    p.Nome,
                    p.Pontos,
                    p.Parcelas,
                    valorOriginal = p.Valor,
                    valor = p.Valor - valorPago,
                    DisponivelCompra = produtoAtivo != null ? false : true,
                    Upgrade = produtoAtivo != null,
                    p.TetoBinario,
                    dataAssinatura = pedido != null ? pedido.DataPagamento : DateTime.Now,
                }).OrderBy(p => p.Pontos).ToList(),
                niveis
            }); ;



            return Ok(response);
        }

        [HttpGet]
        [Route("obterDadosAtivacao/{login}")]
        public IActionResult DadosAtivacaoProduto(string login)

        {
            try
            {
                var usuario = _usuarioNegocio.FirstNoTracking(u => u.Login == login, "UsuarioProduto", "UsuarioProduto.Produto");

                if (usuario == null)
                {
                    throw new Exception("Usuário não encontrado");
                }

                var produtoAtivo = usuario.UsuarioProduto.FirstOrDefault(p => p.Ativo);


                var dadosUsuario = new
                {
                    usuario.Login,
                    usuario.Nome,
                    produtoAtivo = produtoAtivo != null ? produtoAtivo.Produto.Nome : "Sem produto ativo"
                };

                var produtos = _negocio.GetNoTracking(p => p.Ativo/* && (produtoAtivo != null ? p.IdProduto != produtoAtivo.IdProduto : true)*/);

                var opcoes = produtos.Select(p => new
                {
                    p.Nome,
                    p.IdProduto,
                    p.Valor,
                    reaisPorPonto = 700  // TODO: Quando este valor existir no banco, usar ele
                });

                return Ok(new { opcoes, dadosUsuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("ativarProduto")]
        public async Task<IActionResult> AtivarProduto([FromBody] AtivarProdutoRequest request)
        {
            _pedidoNegocio.AtivarProduto(
                request.IdProduto,
                request.Login,
                request.ValorPedido,
                request.DistribuirNaRede,
                request.GerarPontos);

            return Ok();
        }


        [HttpGet]
        [Route("obterAnunciantesZanox")]
        public async Task<IActionResult> ObterAnunciantesZanox()
        {
            string teste = string.Empty;
            //var zanox = new ZanoxBLL();

            //var ids = await zanox.GetProgramAppArrayId();

            //foreach (var id in ids)
            //{
            //    var jao = await zanox.GetProgramsById(id);

            //    if (teste.Length == 0)
            //        teste = jao;
            //    else
            //        teste += "," + jao;
            //}

            var zanox = new ZanoxBLL();

            var ids = await zanox.GetProgramApp();
            return Ok(ids);
        }

        [HttpGet]
        [Route("obterPlano/{idPlano}")]
        public IActionResult OberPlano(long idPlano)
        {
            var produto = _negocio.FirstNoTracking(p => p.IdProduto == idPlano);
            var dadosProduto = new
            {
                idProduto = produto.IdProduto,
                nome = produto.Nome,
                valor = produto.Valor,
                parcelas = produto.Parcelas,
                dataCriacao = produto.DataCriacao,
                dataAtualizacao = produto.DataAutalizacao,
                ativo = produto.Ativo,
                visivel = produto.Visivel,
                reaisPorPonto = produto.ReaisPorPonto
            };

            var historico = _negocio.ObterHistorico(idPlano);

            var dadosHistorico = historico.Select(s => new
            {
                s.Texto,
                s.DataAtualizacao,
                s.Usuario.Nome
            });

            return Ok(new { dadosProduto, dadosHistorico });

        }

        [HttpPost]
        [Route("editarPlano")]
        public IActionResult EditarPlano(ProdutoViewModel viewModel)
        {
            _negocio.EditarPlano(viewModel, IdUsuarioLogado);
            return Ok();

        }
        [HttpPost]
        [Route("criarPlano")]
        public IActionResult CriarPlano(ProdutoViewModel viewModel)
        {
            _negocio.CriarPlano(viewModel, IdUsuarioLogado);
            return Ok();
        }
    }
}