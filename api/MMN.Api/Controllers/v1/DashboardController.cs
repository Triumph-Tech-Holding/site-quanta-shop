using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Model;
using System;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly AppSettings _appSettings;
        private readonly IProceduresRepositorio _proceduresRepositorio;

        public DashboardController(IUsuarioNegocio negocio, IProceduresRepositorio proceduresRepositorio, IOptions<AppSettings> appSettings)
        {
            _usuarioNegocio = negocio;
            _appSettings = appSettings.Value;
            _proceduresRepositorio = proceduresRepositorio;
        }

        [HttpGet]
        [Route("obterBarraStatus")]
        public IActionResult ObertBarraStatus()
        {
            var dados = _usuarioNegocio.GetBarraStatusFromCache(IdUsuarioLogado);

            return Ok(dados);
        }

        [HttpGet]
        [Route("obterPontuacao")]
        public IActionResult ObterPontuacao()
        {
            var pontos = _usuarioNegocio.GetPontosFromCache(IdUsuarioLogado).TotalPontosUsuario;

            return Ok(pontos);
        }

        [HttpGet]
        [Route("obterRankUsuario")]
        public IActionResult ObterRankUsuario()
        {
            var rank = _usuarioNegocio.GetRankFromCache(IdUsuarioLogado);

            return Ok(rank);
        }

        [HttpGet]
        [Route("limitesDeGanhos")]
        public IActionResult LimitesDeGanhos()
        {
            var limites = _usuarioNegocio.BuscarLimitesGanhos(IdUsuarioLogado);

            return Ok(limites);
        }
    }
}