using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Models.Config;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MMN.Api.Controllers.v1
{

    [Route("api/[controller]")]
    [ApiController]
    public class TutorialController : LoggedControllerBase
    {
        private readonly ITutorialNegocio _tutorialNegocio;
        private readonly Tutorial _tutorial;

        public TutorialController(
            ITutorialNegocio tutorialNegocio,
            IOptions<Tutorial> tutorial
            )
        {
            _tutorialNegocio = tutorialNegocio;
            _tutorial = tutorial.Value;
        }

        [HttpGet]
        [Route("ObterTutoriais")]
        public IActionResult ObterTutoriais()
        {
            try
            {
                return Ok(_tutorialNegocio.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("CriarTutorial")]
        public IActionResult CriarTutorial(TutorialViewModel viewModel)
        {
            try
            {
                _tutorialNegocio.CriarTutorial(viewModel);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("obterTutorial/{idTutorial}")]
        public IActionResult OberTutorial(int idTutorial)
        {
            var tutorial = _tutorialNegocio.FirstNoTracking(p => p.IdTutorial == idTutorial);
            var dadosTutorial = new
            {
                idTutorial = tutorial.IdTutorial,
                nome = tutorial.Nome,
                descricao = tutorial.Descricao,
                ativo = tutorial.Ativo,
                url = tutorial.URL,
                dataAtualizacao = tutorial.DataAtualizacao,
                dataCadastro = tutorial.DataCadastro,
            };

            return Ok(new { dadosTutorial });

        }

        [HttpPost]
        [Route("editarTutorial")]
        public IActionResult EditarTutorial(TutorialViewModel viewModel)
        {
            _tutorialNegocio.EditarTutorial(viewModel);
            return Ok();

        }


        [HttpPost]
        [Route("DeletarTutorial")]
        public IActionResult DeletarTutorial([FromBody] Tutorial model)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
