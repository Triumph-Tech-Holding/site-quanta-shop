using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.Models.Request;
using MMN.Api.Models.Response;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnuncianteController : LoggedControllerBase
    {
        private readonly IAnuncianteNegocio _anuncianteNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly ICategoriaNegocio _categoriaNegocio;
        private readonly ICache _cache;

        public AnuncianteController(IAnuncianteNegocio anuncianteNegocio,
            ICategoriaNegocio categoriaNegocio,
            IUsuarioNegocio usuarioNegocio, ICache cache)
        {
            _anuncianteNegocio = anuncianteNegocio;
            _categoriaNegocio = categoriaNegocio;
            _usuarioNegocio = usuarioNegocio;
            _cache = cache;
        }

        [HttpPost]
        [Route("obterAnunciantesZanox")]
        public async Task<IActionResult> ObterAnunciantes(FiltroViewModel.AnunciantePaginado filtro)
        {
            var anunciantes = await _anuncianteNegocio.BuscarAnunciantes(filtro);

            return Ok(anunciantes);
        }

        [HttpGet]
        [Route("obterAnunciantesAleatorio")]
        public IActionResult ObterAnunciantesAleatorio()
        {
            var anunciantes = _anuncianteNegocio.BuscarAnunciantesAleatorio();
            return Ok(anunciantes);
        }

        [Authorize]
        [HttpGet]
        [Route("gerarUrl/{anuncianteId}")]
        public async Task<IActionResult> GerarUrl(int anuncianteId)
        {
            var usuario = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == IdUsuarioLogado);
            if (!usuario.TermosDeAceite)
            {
                throw new UnauthorizedException("politica_escritorio_requerida");
            }

            string url = await _anuncianteNegocio.GerarUrlAsync(anuncianteId);

            if (url == null)
            {
                throw new Exception();
            }

            url = url.Replace("[IdUsuario]", IdUsuarioLogado.ToString());

            return Ok(url);
        }

        [HttpPost]
        [Route("obterCategorias")]
        public IActionResult ObterCategorias([FromBody] FiltroHomeCategoriaViewModel filtro)
        {
            return Ok(_categoriaNegocio.GetCategorias(filtro).Where(x => x.TotalCadastros > 0).OrderBy(c => c.Nome));
        }

        [HttpGet]
        [Route("obterAnunciante/{idAnunciante}")]
        public IActionResult OberAnunciante(int IdAnunciante)
        {
            try
            {
                var anunciante = _anuncianteNegocio.FirstNoTracking(p => p.IdAnunciante == IdAnunciante);
                var cashback = _anuncianteNegocio.BuscarCashbackAnunciante(anunciante);
                var dadosAnunciante = new
                {
                    anunciante.IdAnunciante,
                    nome = anunciante.Nome,
                    url = anunciante.ImagemUrl,
                    ativo = anunciante.Ativo,
                    conexao = anunciante.IdAwin,
                    dataAtualizacao = anunciante.DataAtualizacao,
                    prioridade = anunciante.Prioridade,

                };


                return Ok(new { dadosAnunciante });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("editarAnunciante/{idAnunciante}")]
        public IActionResult editarAnunciante(AnuncianteViewModel viewModel)
        {
            _anuncianteNegocio.EditarAnunciante(viewModel, IdUsuarioLogado);
            return Ok();
        }

        [HttpPost]
        [Route("obterAnuncios")]
        public async Task<IActionResult> ObterAnunciosAsync(FiltroViewModel.AnunciantePaginado filtro)
        {
            var anuncios = await _anuncianteNegocio.ObterOrdenacaoAnunciosFromCashAsync();

            if (!string.IsNullOrEmpty(filtro.Descricao))
                anuncios = anuncios.Where(w => w.Nome.Contains(filtro.Descricao, StringComparison.InvariantCultureIgnoreCase));

            if (filtro.IdCategoria != null)
                anuncios = anuncios.Where(w => w.IdCategoria == filtro.IdCategoria);

            if (filtro.TipoAnunciante.Count > 0)
            {
                int valorEnum = Convert.ToInt32(filtro.TipoAnunciante[0]);
                anuncios = anuncios.Where(w => w.ParceiroOnline == Convert.ToBoolean(valorEnum));
            }

            if (IdUsuarioLogado == null)
            {
                foreach (var anuncio in anuncios)
                    anuncio.UrlAnuncio = null;
            }
            else
            {
                var userId = IdUsuarioLogado.ToString();
                foreach (var anuncio in anuncios)
                    if (anuncio.UrlAnuncio != null)
                        anuncio.UrlAnuncio = anuncio.UrlAnuncio.Replace("[IdUsuario]", userId);
            }

            return Ok(new
            {
                totalPaginas = Math.Ceiling((float)anuncios.Count() / filtro.Quantidade),
                quantidade = filtro.Quantidade,
                page = filtro.Page,
                anunciantes = anuncios.Skip(filtro.Quantidade * (filtro.Page - 1)).Take(filtro.Quantidade).ToList(),
            });
        }

        [HttpGet, Route("ObterCuponsOfertas")]
        public async Task<IActionResult> ObterCuponsOfertasAsync(int? advertiserId, int? page, int? pageSize, string userId)
        {
            page ??= 1;
            pageSize ??= 200;

            var advertiserIds = _anuncianteNegocio.Get(x => x.Ativo && !string.IsNullOrEmpty(x.IdAwin)).Select(x => Convert.ToInt32(x.IdAwin)).ToList();

            if (advertiserId is not null)
                advertiserIds = [advertiserId.Value];

            var url = "https://api.awin.com/publisher/689359/promotions?accessToken=67297973-d629-448d-a704-326fa1f8f7ca";

            var requestBody = new
            {
                filters = new FiltroAwinCuponOfertaRequest
                {
                    AdvertiserIds = advertiserIds,
                    ExclusiveOnly = false,
                    Membership = "joined",
                    RegionCodes = ["BR"],
                    Status = "active",
                    Type = "all",
                    UpdatedSince = "2022-05-06",
                    Pagination = new Models.Request.Pagination(page.Value, pageSize.Value)
                }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);

            using (HttpClient client = new())
            {
                try
                {
                    HttpContent content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<FiltroAwinCuponOfertaResponse>(responseBody);

                        foreach (var item in responseObject.Data)
                        {
                            if (string.IsNullOrEmpty(userId))
                                item.UrlTracking = null;
                            else
                            {
                                item.UrlTracking += $"&clickref={userId}";
                            }
                        }

                        return Ok(responseObject);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response);
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                    return BadRequest(message);
                }
            }
        }
    }
}