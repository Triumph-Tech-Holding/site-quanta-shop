using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    public partial class UsuarioController
    {
        [HttpGet]
        [Route("diretosUsaurioLogado")]
        public async Task<IActionResult> DiretosUsuarioLogado()
        {
            var mesAnterior = ObterPrimeiroEUltimoDiaMesAnterior();
            var diretos = _negocio.ListaUsuarioDiretos(IdUsuarioLogado);

            if (!diretos.Any())
                return Ok(new List<object>());

            var idsUsuarios = diretos.Select(u => u.IdUsuario).ToList();

            var dataUltimasComprasTask = _usersService.GetLastBuyBatch(idsUsuarios);
            var consumosMesAnteriorTask = _saqueNegocio.ObterConsumoSaqueBatch(
                idsUsuarios, mesAnterior.primeiroDia, mesAnterior.ultimoDia);

            await Task.WhenAll(dataUltimasComprasTask, consumosMesAnteriorTask);

            var dataUltimasCompras  = await dataUltimasComprasTask;
            var consumosMesAnterior = await consumosMesAnteriorTask;

            var result = diretos.Select(u => new
            {
                produtoAtivo = u.UsuarioProduto != null && u.UsuarioProduto.Count > 0 && u.UsuarioProduto.Any(p => p.Ativo)
                    ? u.UsuarioProduto.FirstOrDefault(p => p.Ativo).Produto.Nome
                    : "Baf Vision",
                u.IdUsuario,
                u.Nome,
                u.UrlImg,
                u.Login,
                u.Email,
                u.DataCadastro,
                u.Celular,
                Graduacao = new { u.Graduacao.Nome },
                TemFilhos = u.Filhos.Count > 0,
                Diretos = u.Filhos.Count,
                Pontuacao = _negocio.GetPontosFromCache(u.IdUsuario).TotalPontosUsuario,
                u.DataUltimoAcesso,
                dataUltimaCompra    = dataUltimasCompras.ContainsKey(u.IdUsuario) ? dataUltimasCompras[u.IdUsuario] : null,
                consumoMesAnterior  = consumosMesAnterior.ContainsKey(u.IdUsuario)
                    ? consumosMesAnterior[u.IdUsuario].ToString("C2")
                    : "R$ 0,00"
            }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("obterDistribuicao")]
        public IActionResult ObterDistribuicao()
        {
            var distribuicao = _negocio.ObterDistruibuicao(IdUsuarioLogado);
            var lancamentos = new List<object>();
            decimal total = 0;
            distribuicao.ForEach(d =>
            {
                lancamentos.Add(new
                {
                    descricao = d.Descricao,
                    valor = d.Valor,
                    validade = System.Math.Round((d.DataLancamento.AddDays(60) - System.DateTime.Now).TotalDays + 1)
                });
                total += d.Valor;
            });
            return Ok(new { lancamentos, total });
        }

        [HttpGet]
        [Route("obterUltimosDiretos")]
        public IActionResult ObterUltimosDiretos()
        {
            var lstUsuario = _negocio.ObterUltimosDiretos(IdUsuarioLogado);
            return Ok(lstUsuario.Select(u => new { u.Login, u.Email, u.DataCadastro, Graduacao = new { u.Graduacao.Nome } }));
        }

        [HttpGet]
        [Route("verificarQualificacao")]
        public IActionResult VerificarQualificacao()
        {
            var usuarioLogado = _negocio.GetById(IdUsuarioLogado);
            return Ok(new { qualificado = usuarioLogado.DataQualificacao.HasValue, usuarioLogado.DataQualificacao });
        }

        [HttpGet]
        [Route("obterDadosUsuarioRede/{idUsuario}")]
        public IActionResult ObterDadosUsuarioRede(System.Guid idUsuario)
        {
            return Ok(_negocio.ObterDadosPessoais(idUsuario));
        }

        [HttpGet]
        [Route("obterPerformanceRede")]
        public IActionResult ObterPerfomanceRede(string nome = "", string login = "")
        {
            if (nome == null) nome = "";
            if (login == null) login = "";
            var performanceRede = _proceduresRepositorio.spc_PerformanceRede(IdUsuarioLogado, nome, login);
            var dadosRede = performanceRede
                .Select(async s => new
                {
                    s.IdUsuario, s.Nome, s.Login, s.Direto, s.ValorGerado, s.NomeProduto, s.URLIMG,
                    Pontuacao = _negocio.GetPontosFromCache(s.IdUsuario).TotalPontosUsuario,
                })
                .Select(s => s.Result);
            return Ok(dadosRede);
        }

        [HttpGet]
        [Route("obterPerformanceRedeDireto/{idUsuario}")]
        public IActionResult ObterPerfomanceRedeDireto(System.Guid idUsuario, string nome = "", string login = "")
        {
            if (nome == null) nome = "";
            if (login == null) login = "";
            var dadosRedeUsuario = _proceduresRepositorio.spc_PerformanceRede(idUsuario, nome, login);
            return Ok(dadosRedeUsuario);
        }

        [HttpGet]
        [Route("obterLancamentoRede")]
        public IActionResult ObterLancamentoRede()
        {
            return Ok(_proceduresRepositorio.spc_LancamentoRedeUsuario(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("obterRedeUsuario")]
        public IActionResult ObterRedeUsuario()
        {
            return Ok(_proceduresRepositorio.spc_UsuarioDownLine(IdUsuarioLogado));
        }

        [HttpPost]
        [Route("obterRankUsuarioFiltrado")]
        public IActionResult ObterRankUsuarioFiltrado(FiltroViewModel.FiltroRank filtro)
        {
            return Ok(_negocio.ObterRankFiltrado(IdUsuarioLogado, filtro.Login, filtro.Ordenacao));
        }
    }
}
