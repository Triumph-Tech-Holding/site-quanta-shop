using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Api.Helpers;
using MMN.Api.Services;
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
    public class CarrosselController : LoggedControllerBase
    {
        private readonly ICarrosselNegocio _carrosselNegocio;
        private readonly IAnuncianteNegocio _anuncianteNegocio;

        public CarrosselController(ICarrosselNegocio carrosselNegocio, IAnuncianteNegocio anuncianteNegocio)
        {
            _carrosselNegocio = carrosselNegocio;
            _anuncianteNegocio = anuncianteNegocio;
        }

        [HttpGet]
        [Route("listarCarrosseis")]
        public IActionResult ObterCarrosseis()
        {
            var carrosseis = _carrosselNegocio.GetAll().OrderBy(c => c.IdCarrossel);

            return Ok(carrosseis);
        }

        [HttpGet]
        [AllowAnonymous, Route("{idCarrossel}")]
        public IActionResult ObterCarrossel(int idCarrossel)
        {
            var carrossel = _carrosselNegocio.FirstNoTracking(c => c.IdCarrossel == idCarrossel);
            var anunciantes = _anuncianteNegocio.Get(a => a.Ativo && (a.IdAfilio != null || a.IdAwin != null)).OrderBy(l => l.Nome);

            var resultLojas = new List<object>();

            foreach (var an in anunciantes)
            {
                resultLojas.Add(new
                {
                    key = an.IdAnunciante,
                    value = an.Nome + (an.IdAwin != null ? " - Awin" : "- Afilio")
                });
            }

            var result = new
            {
                carrossel.IdCarrossel,
                carrossel.Texto1,
                carrossel.Texto2,
                carrossel.Texto3,
                carrossel.CorFundo,
                carrossel.Ativo,
                carrossel.UltimaAtualizacao,
                carrossel.DataCriacao,
                anunciantes = resultLojas,
                mapeamentos = new { }
            };

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("obterCarrossel/{idCarrossel}")]
        public IActionResult ObterAnunciantesCarrossel(int idCarrossel)
        {
            var carrossel = _carrosselNegocio.First(c => c.IdCarrossel == idCarrossel);

            return Ok(new
            {
                corFundo = carrossel.CorFundo,
                ativo = carrossel.Ativo,
                nome = carrossel.Texto1,
                anunciantes = new { },
            });
        }

        [HttpPost]
        [Route("atualizar")]
        public IActionResult Atualizar([FromBody] CarrosselViewModel carrossel)
        {
            carrossel.UltimaAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            _carrosselNegocio.Update(carrossel);
            return Ok();
        }

        private List<object> BuscarCashback(List<Anunciante> anunciantes)
        {
            var result = new List<object>();

            foreach (var anunciante in anunciantes)
            {
                decimal? minimo = null;
                decimal? maximo = null;
                string tipo = string.Empty;
                string moeda = string.Empty;

                if (anunciante.AnuncianteCashBack != null)
                {
                    foreach (var cashback in anunciante.AnuncianteCashBack)
                    {
                        moeda = cashback.Moeda;
                        if (cashback.Descricao.Equals("Máximo"))
                        {
                            maximo = cashback.Percentual;
                            tipo = cashback.Tipo;
                            continue;
                        }

                        if (cashback.Descricao.Equals("Mínimo"))
                        {
                            minimo = cashback.Percentual;
                            tipo = cashback.Tipo;
                            continue;
                        }

                        if (cashback.Descricao.Equals("Afilio"))
                        {
                            minimo = cashback.Percentual;
                            maximo = cashback.Percentual;
                            tipo = cashback.Descricao;
                            continue;
                        }
                    }
                }

                result.Add(new
                {
                    anunciante.IdAnunciante,
                    anunciante.Ativo,
                    anunciante.IdAwin,
                    anunciante.IdAfilio,
                    anunciante.Nome,
                    anunciante.ImagemUrl,
                    anunciante.Cashback,
                    cashbackMin = minimo,
                    cashbackMax = maximo,
                    tipoCashback = tipo,
                    moeda,
                });
            }

            return result;
        }
    }
}
