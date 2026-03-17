using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.INegocio.Negocio;
using MMN.Util.Translation;
using System;
using System.Linq;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RedeController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarionegocio;

        public RedeController(IUsuarioNegocio usuarioNegocio, ILocation location)
        {
            _usuarionegocio = usuarioNegocio;
        }

        [HttpGet]
        [Route("obterDiretos/{idUsuario}")]
        public IActionResult ObterDiretos(Guid idUsuario)
        {
            var diretos = _usuarionegocio.ListaUsuarioDiretos(idUsuario);

            var result = diretos
                .Select(async u => new
                {
                    produtoAtivo = u.UsuarioProduto != null && u.UsuarioProduto.Count > 0 && u.UsuarioProduto.Any(p => p.Ativo) ? u.UsuarioProduto.FirstOrDefault(p => p.Ativo).Produto.Nome : "Baf Vision",
                    u.IdUsuario,
                    u.Nome,
                    u.UrlImg,
                    u.Login,
                    u.Email,
                    u.DataCadastro,
                    u.Celular,
                    AssinaturaHabilitada = u.UsuarioProduto.Any(p => p.AssinaturaHabilitada),
                    Graduacao = new { u.Graduacao.Nome },
                    TemFilhos = u.Filhos.Count > 0,
                    Pontuacao = _usuarionegocio.GetPontosFromCache(u.IdUsuario).TotalPontosUsuario
                })
                .Select(s => s.Result);

            return Ok(result);
        }
    }
}