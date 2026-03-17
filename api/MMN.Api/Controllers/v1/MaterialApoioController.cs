using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Models.MaterialApoio;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialApoioController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IMaterialApoioNegocio _materialapoioNegocio;
        private readonly AppSettings _appSettings;

        public MaterialApoioController(
            IUsuarioNegocio usuarioNegocio,
            IOptions<AppSettings> appSettings,
            IMaterialApoioNegocio materialApoioNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
            _appSettings = appSettings.Value;
            _materialapoioNegocio = materialApoioNegocio;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("buscarMaterial")]
        public IActionResult BuscarMaterial()
        {
            try
            {
                var result = _materialapoioNegocio.GetAllNoTracking();
                return Ok(result.Select(r => new { r.IdMaterial, r.Nome, r.Descricao, r.DataCadastro, r.UltimaAtualizacao, r.Ativo, r.URLMaterial }));
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarMaterialUsuario")]
        public IActionResult BuscarMaterialUsuario()
        {
            try
            {
                var result = _materialapoioNegocio.GetAllNoTracking();
                return Ok(result.Select(r => new { r.IdMaterial, r.Nome, r.Descricao, r.DataCadastro, r.UltimaAtualizacao, r.Ativo, r.URLMaterial }).Where(r => r.Ativo == true));
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [Route("obterMaterial/{idMaterial}")]
        public IActionResult ObterMaterial(long idMaterial)
        {
            try
            {
                var item = _materialapoioNegocio.FirstNoTracking(f => f.IdMaterial == idMaterial);
                var dadosMaterial = new
                {
                    idMaterial = item.IdMaterial,
                    nome = item.Nome,
                    descricao = item.Descricao,
                    ativo = item.Ativo

                };

                return Ok(new { dadosMaterial });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("criarMaterial")]
        public IActionResult CriarMaterial(MaterialApoioRequest request)
        {
            try
            {
                // Chama método do AzureStorage para subir arquivo e retornar URL
                //string  = replace.Replace("blablabla", " "); 
                // TODO: Incluir appsettings la em cima igual config controller
                var url = AzureStorage.CreateBlob(
                        request.Base64,
                        new Guid(),
                        _appSettings.StorageAccountConnectionString,
                        "material-apoio",
                        request.Nome,
                        true
                    ).Result;

                // Salva novo Material de Apoio no banco com a url retornada
                _materialapoioNegocio.CriarMaterial(new MaterialApoioViewModel
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                    Ativo = request.Ativo,
                    // TODO: Adicionar Url no banco,
                    URLMaterial = url
                }, IdUsuarioLogado);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("EditarMaterial")]
        public IActionResult EditarMaterial(MaterialApoioRequest model)
        {
            try
            {
                var base64 = model.Base64;

                if (base64 != null)
                {
                    var url = AzureStorage.CreateBlob(
                            model.Base64,
                            new Guid(),
                            _appSettings.StorageAccountConnectionString,
                            "material-apoio",
                            model.Nome,
                            true
                        ).Result;

                    _materialapoioNegocio.EditarMaterial(new MaterialApoioViewModel
                    {
                        IdMaterial = (int)model.IdMaterial,
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                        Ativo = model.Ativo,
                        URLMaterial = url
                    }
                    , IdUsuarioLogado);
                }
                else
                {
                    _materialapoioNegocio.EditarMaterial(new MaterialApoioViewModel
                    {
                        IdMaterial = (int)model.IdMaterial,
                        Nome = model.Nome,
                        Descricao = model.Descricao,
                        DataCadastro = DateTime.UtcNow.HorarioBrasilia(),
                        Ativo = model.Ativo,
                    }
                    , IdUsuarioLogado); ;
                };




                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeletarMaterial/{idMaterial}")]
        public IActionResult DeletarMaterial(int idMaterial)
        {
            _materialapoioNegocio.Delete(idMaterial);

            return Ok();

        }
    }
}
