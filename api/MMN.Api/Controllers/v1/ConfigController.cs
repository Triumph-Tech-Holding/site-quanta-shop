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

namespace MMN.Api.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : LoggedControllerBase
    {
        private readonly IConfiguracaoNegocio _configuracaoNegocio;
        private readonly AppSettings _appSettings;

        public ConfigController(
            IConfiguracaoNegocio configuracaoNegocio,
            IOptions<AppSettings> appSettings
            )
        {
            _configuracaoNegocio = configuracaoNegocio;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("ImagensApp")]
        public IActionResult ObterImagensApp()
        {
            var imagensS = new List<object>();
            var imagensC = new List<object>();

            var imagensSlider = AzureStorage.GetImages(_appSettings.StorageAccountConnectionString, "screenshots-app-slider").Result;

            foreach (var img in imagensSlider)
            {
                imagensS.Add(new
                {
                    url = img,
                    Diretorio = "screenshots-app-slider",
                    Nome = img.Split('/').Last(),
                });
            }

            var imagensCarrossel = AzureStorage.GetImages(_appSettings.StorageAccountConnectionString, "screenshots-app-carrossel").Result;
            foreach (var img in imagensCarrossel)
            {
                imagensC.Add(new
                {
                    url = img,
                    Diretorio = "screenshots-app-carrossel",
                    Nome = img.Split('/').Last(),
                });
            }

            return Ok(new { imagensS, imagensC, imagensSlider, imagensCarrossel });
        }

        [HttpPost]
        [Route("UploadImagem")]
        public IActionResult UploadImagensApp([FromBody] UploadImageModel model)
        {
            if (!string.IsNullOrEmpty(model.Base64))
            {
                var image = Convert.FromBase64String(model.Base64);
                using (var ms = new MemoryStream(image))
                {
                    var img = Image.FromStream(ms);

                    //if (img.Width > 320 || img.Height > 320)
                    //{
                    //    throw new PadraoException("imagem_app_resolucao_maxima");
                    //}
                }

                var logoUrl = AzureStorage.CreateBlob(
                        model.Base64,
                        new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        model.Diretorio,
                        "image-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                        true
                    ).Result;

                return Ok(logoUrl);
            }

            throw new Exception();
        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile([FromBody] UploadFileModel model)
        {
            var FileUrl = AzureStorage.CreateBlob(
                    model.Base64,
                    new Guid(),
                    _appSettings.StorageAccountConnectionString,
                    model.Diretorio,
                    model.Nome,
                    true
                ).Result;

            return Ok(FileUrl);
        }

        [HttpPost]
        [Route("DeletarImagem/")]
        public IActionResult DeletarImagensApp([FromBody] UploadImageModel model)
        {
            var result = AzureStorage.Delete(
                    model.Diretorio,
                    model.Nome,
                    _appSettings.StorageAccountConnectionString
                ).Result;

            return Ok(result);
        }

        [HttpGet]
        [Route("ObterConfiguracoes")]
        public IList<ConfiguracaoViewModel> ObterConfiguracoes()
        {
            var config = _configuracaoNegocio.Get(w => w.Editavel == true);

            return config;
        }

        [HttpGet]
        [Route("obterConfig/{idConfiguracao}")]
        public IActionResult ObterTutorial(int idConfiguracao)
        {
            var dados = _configuracaoNegocio.FirstNoTracking(p => p.IdConfiguracao == idConfiguracao);
            var dadosConfig = new
            {
                idConfiguracao = dados.IdConfiguracao,
                descricao = dados.Descricao,
                valor = dados.Valor,

            };

            return Ok(new { dadosConfig });

        }

        [HttpPost]
        [Route("editarConfig")]
        public IActionResult EditarTutorial(ConfiguracaoViewModel viewModel)
        {
            _configuracaoNegocio.EditarConfig(viewModel);
            return Ok();

        }
    }
}