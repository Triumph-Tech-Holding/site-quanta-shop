using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Translation;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExcecaoViewModel), StatusCodes.Status500InternalServerError)]
    public class EcossistemaController : LoggedControllerBase
    {
        private readonly IEcossistemaNegocio _ecossistemaNegocio;
        public EcossistemaController(IEcossistemaNegocio ecossistemaNegocio)
        {
            _ecossistemaNegocio = ecossistemaNegocio;
        }

        [HttpGet]
        [Route("obterEcossistemas")]
        public async Task<IActionResult> ObterEcossistemas([FromQuery] FiltroEcossistemas filtro)
        {
            var ecossistemas = await _ecossistemaNegocio.BuscarEcossistemas(filtro);
            return Ok(ecossistemas);
        }

        [HttpGet]
        [Route("obterEcossistema/{id}")]
        public async Task<IActionResult> ObterEcossistema(int id)
        {
            var ecossistema = await _ecossistemaNegocio.BuscarEcossistemaPorId(id);
            if (ecossistema == null)
            {
                return NotFound();
            }
            return Ok(ecossistema);
        }

        [HttpPost]
        [Route("criarEcossistema")]
        public async Task<IActionResult> CriarEcossistema([FromBody] Ecossistema ecossistema)
        {
            ecossistema.DataCadastro = DateTime.UtcNow;

            await _ecossistemaNegocio.CriarEcossistema(ecossistema);
            return CreatedAtAction(nameof(ObterEcossistema), new { id = ecossistema.IdEcossistema }, ecossistema);
        }

        [HttpPost]
        [Route("Ecossistema/{id}")]
        public async Task<IActionResult> EditarEcossistema(int id, [FromBody] Ecossistema ecossistema)
        {
            if (id != ecossistema.IdEcossistema)
            {
                return BadRequest();
            }

            await _ecossistemaNegocio.AtualizarEcossistema(ecossistema);
            return NoContent();
        }

        [HttpDelete]
        [Route("deletarEcossistema/{id}")]
        public async Task<IActionResult> DeletarEcossistema(int id)
        {
            //TODO: Tratar o erro na hora de excluír os credenciamentos SQL Exception 547
            var ecossistema = await _ecossistemaNegocio.BuscarEcossistemaPorId(id);
            if (ecossistema == null)
            {
                return NotFound();
            }

            await _ecossistemaNegocio.DeletarEcossistema(id);
            return NoContent();
        }
    }
}
