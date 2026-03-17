using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Api.Helpers;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Cache;
using MMN.Util.Translation;

namespace MMN.Api.Controllers.v1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioBancoController : LoggedControllerBase
    {
        private readonly IUsuarioBancoNegocio _usuarioBancoNegocio;
        public UsuarioBancoController(IUsuarioBancoNegocio usuarioBancoNegocio)
        {
            _usuarioBancoNegocio = usuarioBancoNegocio;
        }

        [HttpGet]
        [Route("obterDadosBancarios")]
        public IActionResult ObterDadosBancarios()
        {
            return Ok(_usuarioBancoNegocio.Get(w => w.IdUsuario == IdUsuarioLogado && w.Ativo, "Banco", "Tipo").AsEnumerable());
        }

        [HttpPost]
        [Route("cadastrarUsuarioBanco")]
        public IActionResult CadastrarUsuarioBanco(UsuarioBancoViewModel viewModel)
        {
            var valid = new UsuarioBancoViewModelValidator();
            var result = valid.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            if (_usuarioBancoNegocio.CadastrarUsuarioBanco(viewModel, IdUsuarioLogado))
                return Ok(true);

            throw new Exception();
        }

        [HttpGet]
        [Route("listarContasCadastradas")]
        public IActionResult ListarContasCadastradas()
        {
            var listaBancos = new List<UsuarioBancoViewModel>();
            listaBancos.Add(new UsuarioBancoViewModel() { IdUsuarioBanco = 1, Agencia = "0616", Ativo = true, Conta = "06400", DigitoConta = "0", Cpfcnpj = "088.622.866-66", Banco = new BancoViewModel() { IdBanco = 1, Nome = "Itau", Febraban = 341 }, NomeConta = "Conta Principal" });

            //if(result.IsValid)
            //    _usuarioBancoNegocio.


            return Ok(listaBancos);
        }

        [HttpDelete]
        [Route("ExcluirContaBancaria/{idUsuarioBanco}")]
        public IActionResult ExcluirContaBancaria(int idUsuarioBanco)
        {
            var usuarioBanco = _usuarioBancoNegocio.FirstNoTracking(u => u.IdUsuarioBanco == idUsuarioBanco && u.IdUsuario == IdUsuarioLogado);
            if (usuarioBanco == null)
                throw new PadraoException("conta_nao_encontrada");

            usuarioBanco.Ativo = false;
            _usuarioBancoNegocio.Update(usuarioBanco);
            return Ok();
        }
    }
}