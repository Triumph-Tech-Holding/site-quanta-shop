using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Api.Helpers;
using MMN.Api.Services;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Contexto;
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
    public class CategoriaController : LoggedControllerBase
    {
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly IAnuncianteNegocio _anuncianteNegocio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;

        public CategoriaController(
            ICategoriaNegocio categoriaNegocio,
            IAnuncianteNegocio anuncianteNegocio,
            ICredenciamentoNegocio credenciamentoNegocio
            )
        {
            _categoriaNegocio = categoriaNegocio;
            _anuncianteNegocio = anuncianteNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
        }

        [HttpGet]
        [Route("obterDadosMapeamentoCategorias")]
        public IActionResult ObterDadosMapeamentoCategorias()
        {
            var categorias = _categoriaNegocio.Get(c => c.Ativo).OrderBy(c => c.Nome);
            var lojas = _anuncianteNegocio.Get(a =>
            a.Ativo &&
            (!string.IsNullOrEmpty(a.IdAwin) || !string.IsNullOrEmpty(a.IdAfilio)))
            .OrderBy(l => l.Nome);

            var resultCategorias = new List<object>();
            var resultLojas = new List<object>();

            foreach (var loja in lojas)
            {
                resultLojas.Add(new
                {
                    key = loja.IdAnunciante,
                    value = loja.Nome + (loja.IdAwin != null ? " - Awin" : "- Afilio")
                });
            }

            foreach (var cat in categorias)
            {
                resultCategorias.Add(new
                {
                    key = cat.IdCategoria,
                    value = cat.Nome
                });
            }

            var mapeamentos = _categoriaNegocio.ObterMapeamentos();

            var result = new List<object>();

            foreach (var map in mapeamentos)
            {
                result.Add(new
                {
                    idMapeamento = map.IdCategoriaAnunciante,
                    map.IdCategoria,
                    nomeCategoria = map.Categoria.Nome,
                    map.IdAnunciante,
                    nomeAnunciante = map.Anunciante.Nome
                });
            }

            return Ok(new
            {
                mapeamentos = result,
                categorias = resultCategorias,
                anunciantes = resultLojas
            });
        }

        [HttpPost]
        [Route("filtrarMapeamentos")]
        public IActionResult FiltrarMapeamentos([FromBody] FiltroMapeamentoViewModel filtro)
        {
            var mapeamentos = _categoriaNegocio.ObterMapeamentos();

            if (!string.IsNullOrEmpty(filtro.NomeAnunciante))
            {
                mapeamentos = mapeamentos.Where(m => m.Anunciante.Nome.Contains(filtro.NomeAnunciante)).ToList();
            }

            if (filtro.IdCategoria.HasValue)
            {
                mapeamentos = mapeamentos.Where(m => m.IdCategoria == filtro.IdCategoria).ToList();
            }

            var result = new List<object>();

            foreach (var map in mapeamentos)
            {
                result.Add(new
                {
                    idMapeamento = map.IdCategoriaAnunciante,
                    map.IdCategoria,
                    nomeCategoria = map.Categoria.Nome,
                    map.IdAnunciante,
                    nomeAnunciante = map.Anunciante.Nome
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("ObterCategorias")]
        public IActionResult ObterCategorias([FromBody] FiltroCategoriaViewModel filtro)
        {
            var categorias = _categoriaNegocio.BuscarCategorias();

            categorias = !string.IsNullOrEmpty(filtro.Nome) ? categorias.Where(c => c.Nome.ToLower().Contains(filtro.Nome.ToLower())).ToList() : categorias;
            categorias = !string.IsNullOrEmpty(filtro.NomePai) ? categorias.Where(c => c.CategoriaPai.Nome.ToLower().Contains(filtro.Nome.ToLower())).ToList() : categorias;
            categorias = filtro.Ativo ? categorias.Where(c => c.Ativo).ToList() : categorias;

            categorias = filtro.DataCriacaoInicio.HasValue ? categorias.Where(c => c.DataCriacao >= filtro.DataCriacaoInicio).ToList() : categorias;
            categorias = filtro.DataCriacaoFim.HasValue ? categorias.Where(c => c.DataCriacao < filtro.DataCriacaoFim).ToList() : categorias;
            categorias = filtro.DataAtualizacaoInicio.HasValue ? categorias.Where(c => c.DataAtualizacao >= filtro.DataAtualizacaoInicio).ToList() : categorias;
            categorias = filtro.DataAtualizacaoFim.HasValue ? categorias.Where(c => c.DataAtualizacao < filtro.DataAtualizacaoFim).ToList() : categorias;

            var result = new List<object>();

            foreach (var cat in categorias)
            {
                result.Add(new
                {
                    cat.IdCategoria,
                    cat.Nome,
                    nomePai = cat.CategoriaPai != null ? cat.CategoriaPai.Nome : string.Empty,
                    cat.Ativo,
                    cat.DataAtualizacao,
                    cat.DataCriacao,
                });
            }

            return Ok(result);
        }

        [HttpGet, AllowAnonymous]
        [Route("ObterCategoriasDestaque")]
        public IActionResult ObterCategoriasDestaque()
        {
            var categorias = _categoriaNegocio.Get(x => x.Destaque, new[] { "CategoriaAnunciante", "CategoriaAnunciante.Anunciante" }).ToList();

            var result = new List<object>();

            foreach (var cat in categorias)
            {
                var anunciantes_ids = cat.CategoriaAnunciante.Select(x => x.IdAnunciante).ToArray();
                var anunciantes = _anuncianteNegocio.Get(x => anunciantes_ids.Contains(x.IdAnunciante)).ToList();

                result.Add(new
                {
                    cat.IdCategoria,
                    cat.Nome,
                    nomePai = cat.CategoriaPai != null ? cat.CategoriaPai.Nome : string.Empty,
                    cat.Ativo,
                    cat.DataAtualizacao,
                    cat.DataCriacao,
                    cat.CategoriaAnunciante,
                    anunciantes
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("ObterCategoriasAleatorias")]
        public IActionResult ObterCategoriasAleatorias()
        {
            var categorias = _categoriaNegocio.BuscarCategoriasAleatorias();
            return Ok(categorias);
        }

        [HttpGet]
        [Route("obterMapeamentos")]
        public IActionResult ObterMapeamentos()
        {
            var mapeamentos = _categoriaNegocio.ObterMapeamentos();

            var result = new List<object>();

            foreach (var map in mapeamentos)
            {
                result.Add(new
                {
                    map.IdCategoria,
                    nomeCategoria = map.Categoria.Nome,
                    map.IdAnunciante,
                    nomeAnunciante = map.Anunciante.Nome
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("obterOpcoes")]
        public IActionResult ObterOpcoesMapeamento()
        {
            var categorias = _categoriaNegocio.Get(c => c.Ativo);
            var lojas = _anuncianteNegocio.Get(a => a.Ativo);

            var resultCategorias = new List<object>();
            var resultLojas = new List<object>();

            foreach (var loja in lojas)
            {
                resultLojas.Add(new
                {
                    key = loja.IdAnunciante,
                    value = loja.Nome
                });
            }

            foreach (var cat in categorias)
            {
                resultCategorias.Add(new
                {
                    key = cat.IdCategoria,
                    value = cat.Nome
                });
            }

            return Ok(new
            {
                categorias = resultCategorias,
                lojas = resultLojas
            });
        }

        [HttpGet]
        [Route("obterCategoria/{idCategoria}")]
        public IActionResult ObterDadosCategoria(int idCategoria)
        {
            var categoria = _categoriaNegocio.BuscarCategorias().FirstOrDefault(c => c.IdCategoria == idCategoria);

            return Ok(new
            {
                categoria.IdCategoria,
                categoria.Nome,
                categoria.Ativo,
                categoria.DataAtualizacao,
                categoria.DataCriacao,
                categoria.IdCategoriaPai,
            });
        }

        [HttpGet]
        [Route("obterDadosCategorias")]
        public IActionResult ObterDadosCategorias()
        {
            var categorias = _categoriaNegocio.BuscarCategorias().Where(c => c.Ativo);

            var categoriaPaiOptions = new List<object>();

            foreach (var cat in categorias)
            {
                categoriaPaiOptions.Add(new { key = cat.IdCategoria, value = cat.Nome });
            }

            return Ok(new { categoriaPaiOptions });
        }

        [HttpPost]
        [Route("CriarCategoria")]
        public IActionResult CriarCategoria([FromBody] CriarCategoriaViewModel categoria)
        {
            _categoriaNegocio.CriarCategoria(categoria);

            return Ok();
        }

        [HttpPost]
        [Route("AtualizarCategoria")]
        public IActionResult AtualizarCategoria([FromBody] CriarCategoriaViewModel categoria)
        {
            _categoriaNegocio.AtualizarCategoria(categoria);

            return Ok();
        }

        [HttpDelete]
        [Route("ExcluirCategoria/{idCategoria}")]
        public IActionResult ExcluirCategoria(int idCategoria)
        {
            if (_credenciamentoNegocio.Get(c => c.IdCategoria == idCategoria).Any())
            {
                throw new PadraoException("categoria_credenciamento_mapeado");
            }

            _categoriaNegocio.DeletarCategoria(idCategoria);
            return Ok();
        }

        [HttpPost]
        [Route("Mapear")]
        public IActionResult Mapear(CategoriaAnuncianteMapViewModel map)
        {
            var newMap = _categoriaNegocio.Map(map);
            return Ok(new
            {
                nomeAnunciante = newMap.Anunciante.Nome,
                nomeCategoria = newMap.Categoria.Nome
            });
        }

        [HttpDelete]
        [Route("removerMapeamento/{idMap}")]
        public IActionResult RemoverMapeamento(int idMap)
        {
            _categoriaNegocio.RemoveMap(idMap);
            return Ok();
        }
    }
}
