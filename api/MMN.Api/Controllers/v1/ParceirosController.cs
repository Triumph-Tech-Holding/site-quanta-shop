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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParceirosController : LoggedControllerBase
    {
        private readonly IParceiroNegocio _negocio;

        public ParceirosController(IParceiroNegocio negocio)
        {
            _negocio = negocio;
        }

        [HttpGet]
        [Route("obterDadosParceiro/{idParceiro}")]
        public IActionResult ListarParceiros(int idParceiro)
        {
            var parceiro = _negocio.ObterParceiro(idParceiro);

            return Ok(parceiro);
        }

        [HttpPost]
        [Route("obterParceiros")]
        public IActionResult ListarParceiros(FiltroParceiroViewModel filtro)
        {
            var parceiros = _negocio.ObterParceiros(IdUsuarioLogado);

            parceiros = filtro.DataCriacaoInicio.HasValue ? parceiros.Where(p => p.DataCriacao >= filtro.DataCriacaoInicio.Value.AddHours(DateTime.Now.Hour + 1)).ToList() : parceiros;
            parceiros = filtro.DataCriacaoFim.HasValue ? parceiros.Where(p => p.DataCriacao <= filtro.DataCriacaoFim.Value.AddHours(DateTime.Now.Hour + 1)).ToList() : parceiros;

            parceiros = filtro.DataAtualizacaoInicio.HasValue ? parceiros.Where(p => p.DataAtualizacao >= filtro.DataAtualizacaoInicio.Value.AddHours(DateTime.Now.Hour + 1)).ToList() : parceiros;
            parceiros = filtro.DataAtualizacaoFim.HasValue ? parceiros.Where(p => p.DataAtualizacao <= filtro.DataAtualizacaoFim.Value.AddHours(DateTime.Now.Hour + 1)).ToList() : parceiros;

            parceiros = filtro.Ativo.HasValue ? parceiros.Where(p => p.Ativo == filtro.Ativo).ToList() : parceiros;
            parceiros = !string.IsNullOrEmpty(filtro.Nome) ? parceiros.Where(p => p.Nome.Contains(filtro.Nome)).ToList() : parceiros;
            parceiros = !string.IsNullOrEmpty(filtro.Celular) ? parceiros.Where(p => p.Celular.Contains(filtro.Celular)).ToList() : parceiros;
            parceiros = !string.IsNullOrEmpty(filtro.Descricao) ? parceiros.Where(p => p.Descricao.Contains(filtro.Descricao)).ToList() : parceiros;

            return Ok(parceiros);
        }

        [HttpPost]
        [Route("CriarParceiro")]
        public IActionResult CriarParceiro(ParceiroViewModel parceiro)
        {
            parceiro.IdCredenciado = IdUsuarioLogado;
            parceiro.Celular = parceiro.Celular.Replace(" ", string.Empty);
            return Ok(_negocio.CriarParceiro(parceiro));
        }

        [HttpPost]
        [Route("AtualizarParceiro")]
        public IActionResult AtualizarParceiro(ParceiroViewModel parceiro)
        {
            parceiro.IdCredenciado = IdUsuarioLogado;
            parceiro.Celular = parceiro.Celular.Replace(" ", string.Empty);
            return Ok(_negocio.AtualizarParceiro(parceiro));
        }

        [HttpDelete]
        [Route("ExcluirParceiro/{idParceiro}")]
        public IActionResult ExcluirParceiro(int idParceiro)
        {
            _negocio.Delete(idParceiro);

            return Ok();
        }
    }
}
