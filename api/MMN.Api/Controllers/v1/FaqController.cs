using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using System;
using System.Linq;

namespace MMN.Api.Controllers.v1
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FaqController : LoggedControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IFaqNegocio _faqNegocio;

        public FaqController(
            IUsuarioNegocio usuarioNegocio,
            IFaqNegocio faqNegocio)
        {
            _usuarioNegocio = usuarioNegocio;
            _faqNegocio = faqNegocio;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarFaq")]
        public IActionResult BuscarFaq()
        {
            try
            {
                var result = _faqNegocio.GetAllNoTracking();
                return Ok(result.Select(r => new { r.IdFaq, r.Pergunta, r.DataCadastro, r.UltimaAtualizacao, r.Ativo, r.Resposta }));
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("buscarFaqUsuario")]
        //public IActionResult BuscarFaqUsuario()
        //{
        //    try
        //    {
        //        var result = _faqNegocio.GetAllNoTracking();
        //        return Ok(result.Select(r => new { r.IdFaq, r.Pergunta, r.DataCadastro, r.UltimaAtualizacao, r.Ativo }).Where(r => r.Ativo == true));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new { e.Message });
        //    }
        //}

        [HttpGet]
        [Route("obterFaq/{idFaq}")]
        [AllowAnonymous]
        public IActionResult ObterFaq(long idFaq)
        {
            try
            {
                var item = _faqNegocio.FirstNoTracking(f => f.IdFaq == idFaq);
                var dadosFaq = new
                {
                    idFaq = item.IdFaq,
                    pergunta = item.Pergunta,
                    resposta = item.Resposta,
                    ativo = item.Ativo

                };

                return Ok(new { dadosFaq });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("criarFaq")]
        [Authorize(Roles = "Admin")]
        public IActionResult CriarFaq(FaqViewModel viewModel)
        {
            try
            {
                _faqNegocio.CriarFaq(viewModel, IdUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("EditarFaq")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditarFaq(FaqViewModel editar)
        {
            try
            {
                _faqNegocio.EditarFaq(editar, IdUsuarioLogado);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("FiltrarFaq")]
        [Authorize(Roles = "Admin")]
        public IActionResult FiltarFaq(FiltroFaqAdmin filtroFaqAdmin)
        {
            try
            {
                var filtros = _faqNegocio.FiltrarFAQ(filtroFaqAdmin, IdUsuarioLogado);
                return Ok(filtros);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
