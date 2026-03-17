using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Translation;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Authorize, ApiController, Route("api/[controller]")]
    public class ComunicadoController : LoggedControllerBase
    {
        private readonly IMensagemNegocio _mensagemNegocio;
        private readonly IMensagemGraduacaoNegocio _mensagemGraduacaoNegocio;
        private readonly AppSettings _appSettings;
        private readonly ILocation _location;

        public ComunicadoController(IMensagemNegocio mensagemNegocio, IMensagemGraduacaoNegocio mensagemGraduacaoNegocio, IOptions<AppSettings> appSettings, ILocation location)
        {
            _mensagemNegocio = mensagemNegocio;
            _mensagemGraduacaoNegocio = mensagemGraduacaoNegocio;
            _appSettings = appSettings.Value;
            _location = location;
        }

        [HttpPost]
        public async Task<IActionResult> SalvarComunicado(MensagemViewModel viewModel)
        {
            MensagemViewModelValidator validator = new MensagemViewModelValidator();

            var result = validator.Validate(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            viewModel.DataInicio = viewModel.DataInicio.Value.HorarioBrasilia();
            viewModel.DataFim = viewModel.DataFim.Value.HorarioBrasilia();

            viewModel.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
            viewModel.TipoMensagem = TipoMensagem.Comunicado;
            viewModel.Ativo = true;

            string image = null;
            if (!string.IsNullOrEmpty(viewModel.Base64Arquivo))
                image = await AzureStorage.CreateBlob(viewModel.Base64Arquivo, IdUsuarioLogado, _appSettings.StorageAccountConnectionString, "comunicados", viewModel.Titulo);

            viewModel.UrlArquivo = image;
            foreach (var mensagemGraduacao in viewModel.MensagemGraduacao)
            {
                mensagemGraduacao.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                mensagemGraduacao.Ativo = true;
            }

            _mensagemNegocio.Insert(viewModel);
            return Ok(new { message = "Comunicado cadastrado com sucesso." });
        }

        [HttpPut]
        public async Task<IActionResult> EditarComunicado(MensagemViewModel viewModel)
        {
            MensagemViewModelUpdateValidator validator = new MensagemViewModelUpdateValidator();
            var result = validator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var mensagemToUpdate = _mensagemNegocio.FirstNoTracking(c => c.IdMensagem == viewModel.IdMensagem, "MensagemGraduacao");

            if (mensagemToUpdate == null)
                throw new NotFoundException("mensagem_nao_encontrada");

            if (string.IsNullOrEmpty(viewModel.Texto) && string.IsNullOrEmpty(viewModel.Base64Arquivo) && string.IsNullOrEmpty(viewModel.UrlArquivo))
                throw new PadraoException("mensagem_imagem_nao_preenchida");


            if (!string.IsNullOrEmpty(viewModel.Base64Arquivo) && string.IsNullOrEmpty(viewModel.UrlArquivo))
            {
                string image = await AzureStorage.CreateBlob(viewModel.Base64Arquivo, IdUsuarioLogado, _appSettings.StorageAccountConnectionString, "comunicados", viewModel.Titulo);
                mensagemToUpdate.UrlArquivo = image;
            }

            foreach (var mensagemGraduacao in mensagemToUpdate.MensagemGraduacao)
            {
                mensagemGraduacao.Ativo = false;
                _mensagemGraduacaoNegocio.Update(mensagemGraduacao);
            }
            mensagemToUpdate.Titulo = viewModel.Titulo;
            mensagemToUpdate.Texto = viewModel.Texto;
            mensagemToUpdate.DataInicio = viewModel.DataInicio.Value.HorarioBrasilia();
            mensagemToUpdate.DataFim = viewModel.DataFim.Value.HorarioBrasilia();
            mensagemToUpdate.MensagemGraduacao = viewModel.MensagemGraduacao;

            foreach (var mensagemGraduacao in mensagemToUpdate.MensagemGraduacao)
            {
                mensagemGraduacao.DataCadastro = DateTime.UtcNow.HorarioBrasilia();
                mensagemGraduacao.IdMensagem = mensagemToUpdate.IdMensagem;
                mensagemGraduacao.Ativo = true;
                _mensagemGraduacaoNegocio.Insert(mensagemGraduacao);

            }
            _mensagemNegocio.Update(mensagemToUpdate);
            _mensagemNegocio.SaveChanges();
            return Ok(new { message = "Comunicado atualizado com sucesso." });
        }

        [HttpGet, Route("ListaComunicados")]
        public IActionResult ListaComunicados()
        {
            var comunicados = _mensagemNegocio.BuscarComunicados();

            return Ok(comunicados.Select(c => new
            {
                c.IdMensagem,
                c.Titulo,
                c.Texto,
                c.DataInicio,
                c.DataFim,
                c.UrlArquivo,
                c.Ativo
            }).OrderByDescending(c => c.DataInicio));
        }

        [HttpPut, Route("AtivarDesativarMensagem")]
        public IActionResult AtivarDesativarMensagem(MensagemViewModel viewModel)
        {
            string retorno = string.Empty;
            var mensagem = _mensagemNegocio.FirstNoTracking(m => m.IdMensagem == viewModel.IdMensagem);
            if (mensagem != null)
            {
                if (mensagem.Ativo)
                    retorno = _location.GetTranslation("ComunicadoDesativado");
                else
                    retorno = _location.GetTranslation("ComunicadoAtivado");

                mensagem.Ativo = !mensagem.Ativo;
                _mensagemNegocio.Update(mensagem);
            }

            return Ok(new { message = retorno });
        }

        [HttpGet, Route("ObterComunicado/{idMensagem}")]
        public IActionResult ObterComunicado(int idMensagem)
        {
            var comunicado = _mensagemNegocio.First(c => c.IdMensagem == idMensagem, "MensagemGraduacao");
            if (comunicado == null)
                throw new NotFoundException("comunicado_nao_encontrado");

            return Ok(new
            {
                comunicado.IdMensagem,
                comunicado.Titulo,
                comunicado.Texto,
                Graduacoes = comunicado.MensagemGraduacao.Where(mg => mg.Ativo).Select(mg => mg.IdGraduacao).ToList(),
                comunicado.DataInicio,
                comunicado.DataFim,
                comunicado.UrlArquivo
            });
        }

        [HttpPut, Route("ExcluirImagem")]
        public async Task<IActionResult> ExcluirImagem(MensagemViewModel viewModel)
        {
            var mensagem = _mensagemNegocio.FirstNoTracking(m => m.IdMensagem == viewModel.IdMensagem);
            if (mensagem != null)
            {
                if (!string.IsNullOrEmpty(mensagem.UrlArquivo))
                {
                    string[] partesNome = mensagem.UrlArquivo.Split('/');

                    string nomeArquivo = partesNome[partesNome.Length - 1];
                    string folder = partesNome[partesNome.Length - 2];

                    bool result = await AzureStorage.Delete(folder, nomeArquivo, _appSettings.StorageAccountConnectionString);

                    mensagem.UrlArquivo = "";

                    _mensagemNegocio.Update(mensagem);
                }
            }

            return Ok();
        }

        [HttpGet, Route("ObterComunicadosPorGraduacao")]
        public IActionResult ObterComunicadosPorGraduacao()
        {
            var data = DateTime.UtcNow.HorarioBrasilia();

            var comunicados = _mensagemGraduacaoNegocio.Get(m => m.IdGraduacao == IdGraduacaoLogado
                && m.Ativo
                && m.Mensagem.Ativo
                && m.Mensagem.DataInicio < data
                && m.Mensagem.DataFim > data
                && m.Mensagem.TipoMensagem == TipoMensagem.Comunicado, "Mensagem");

            return Ok(comunicados.Select(c => new
            {
                c.MensagemViewModel.Titulo,
                c.MensagemViewModel.Texto,
                c.MensagemViewModel.UrlArquivo,
                data = c.MensagemViewModel.DataInicio
            }).OrderByDescending(c => c.data));
        }

        [HttpGet, Route("ObterNotificacoesNaoLidas")]
        public IActionResult ObterNotificacoesNaoLidas()
        {
            return Ok(_mensagemNegocio.Get(m => m.Ativo &&
                m.IdUsuarioDestino == IdUsuarioLogado &&
                m.TipoMensagem == TipoMensagem.Aviso).Select(m => new
                {
                    m.IdMensagem,
                    m.Titulo,
                    m.Texto,
                    m.UrlArquivo,
                    data = m.DataCadastro,
                    lida = m.DataLeitura.HasValue
                }).OrderByDescending(m => m.data).Take(30));
        }

        [HttpPut, Route("MarcarNotificacaoComoLida")]
        public IActionResult MarcarNotificacaoComoLida(MensagemViewModel viewModel)
        {
            var notificacao = _mensagemNegocio.FirstNoTracking(m => m.IdMensagem == viewModel.IdMensagem && m.IdUsuarioDestino == IdUsuarioLogado);
            if (notificacao != null)
            {
                notificacao.DataLeitura = DateTime.Now.HorarioBrasilia();
                _mensagemNegocio.Update(notificacao);
            }
            return Ok();
        }
    }
}