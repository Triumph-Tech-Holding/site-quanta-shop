using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : LoggedControllerBase
    {

        private readonly AppSettings _appSettings;
        private readonly IUsuarioNegocio _usuarionegocio;
        private readonly ISuporteNegocio _suporteNegocio;
        public ContatoController(IOptions<AppSettings> appSettings,
            IUsuarioNegocio usuarionegocio,
            ISuporteNegocio suporteNegocio)
        {
            _appSettings = appSettings.Value;
            _usuarionegocio = usuarionegocio;
            _suporteNegocio = suporteNegocio;
        }

        [HttpPost, Route("EnviarContato")]
        public async Task<IActionResult> EnviarContato(ContatoViewModel viewModel)
        {
            var validator = new ContatoViewModelValidator();
            var result = validator.Validate(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                Subject = $"{_appSettings.FromName} - Novo contato",
                To = "contato@quantabank.com.br",
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var body = new Dictionary<string, string>
                {
                    { "#NomeContato#", viewModel.Nome },
                    { "#EmailContato#", viewModel.Email},
                    { "#MensagemContato#", viewModel.Mensagem}
                };

            await new EmailUtilitis().EnviarEmail(body, _appSettings.ContatoTemplate, "", objectEmail);

            return Ok();
        }

        [HttpPost, Route("EnviarSuporte"), Authorize]
        public async Task<IActionResult> EnviarSuporte(dynamic teste)
        {
            SuporteViewModel viewModel = JsonConvert.DeserializeObject<SuporteViewModel>(teste.ToString());
            var objectEmail = new ObjEmailUtilitis
            {
                Data = DateTime.UtcNow.HorarioBrasilia(),
                From = _appSettings.EmailToSmtp,
                FromName = _appSettings.FromName,
                Subject = $"{_appSettings.FromName} - {viewModel.TipoContato.GetDescription()}",
                To = "bigsuporte@triumphmarketing.me",
                EmailSuporte = _appSettings.EmailSuporte,
                SendGridClient = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
            };

            var usuario = _usuarionegocio.GetById(IdUsuarioLogado);
            Dictionary<string, string> body;
            switch (viewModel.TipoContato)
            {
                case TipoContatoEnum.CashbackNaoPago:
                    await _suporteNegocio.SolicitarCashbackNaoPago(viewModel, usuario, objectEmail);
                    break;
                case TipoContatoEnum.CancelamentoParcelas:
                    await _suporteNegocio.SolicitarCancelamentoParcela(viewModel, usuario, objectEmail);
                    break;
                case TipoContatoEnum.Contato:
                    if (!string.IsNullOrEmpty(viewModel.Observacao))
                    {
                        body = new Dictionary<string, string>
                            {
                                { "#NomeContato#", usuario.Nome },
                                { "#EmailContato#", usuario.Email },
                                { "#MensagemContato#", viewModel.Observacao }
                            };

                        await new EmailUtilitis().EnviarEmail(body, _appSettings.ContatoTemplate, "", objectEmail);
                    }
                    else
                        throw new PadraoException("observacao_requerida");
                    break;
                default:
                    throw new PadraoException("solicitacao_indisponivel");
            }

            return Ok();
        }


        [HttpPost, Route("obterMinhasSolicitacoes/")]
        public IActionResult ObterMinhasSolicitacoes([FromBody] FiltroSuporteViewModel filtro)
        {
            var solicitacoes = _suporteNegocio.Get(s => s.IdUsuario == IdUsuarioLogado, "Usuario", "Usuario.UsuarioPai");

            solicitacoes = filtro.DataInicioInicio.HasValue ? solicitacoes.Where(s => s.DataSolicitacao >= filtro.DataInicioInicio).ToList() : solicitacoes;
            solicitacoes = filtro.DataInicioFim.HasValue ? solicitacoes.Where(s => s.DataSolicitacao <= filtro.DataInicioFim).ToList() : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoInicio.HasValue ? solicitacoes.Where(s => s.DataAtualizacao >= filtro.DataAtualizacaoInicio).ToList() : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoFim.HasValue ? solicitacoes.Where(s => s.DataAtualizacao <= filtro.DataAtualizacaoFim).ToList() : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginPatrocinador) ? solicitacoes.Where(s => s.Usuario.UsuarioPai.Login.Contains(filtro.LoginPatrocinador)).ToList() : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginUsuario) ? solicitacoes.Where(s => s.Usuario.Login.Contains(filtro.LoginUsuario)).ToList() : solicitacoes;
            solicitacoes = filtro.IdStatus.HasValue ? solicitacoes.Where(s => s.IdStatus == filtro.IdStatus.Value).ToList() : solicitacoes;
            solicitacoes = filtro.IdTipo.HasValue ? solicitacoes.Where(s => s.TipoContato == (TipoContatoEnum)filtro.IdTipo.Value).ToList() : solicitacoes;
            solicitacoes = filtro.IdStatus.HasValue ? solicitacoes.Where(s => s.IdStatus == filtro.IdStatus.Value).ToList() : solicitacoes;

            var result = new List<object>();

            foreach (var solicitacao in solicitacoes)
            {
                result.Add(new
                {
                    idSolicitacao = solicitacao.IdSuporte,
                    dataAtualizacao = solicitacao.DataAtualizacao,
                    dataSolicitacao = solicitacao.DataSolicitacao,
                    tipo = solicitacao.TipoContato,
                    status = solicitacao.IdStatus,
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("detalhesSolicitacao/{idSolicitacao}")]
        public IActionResult DetalhesSolicitacap(long idSolicitacao)
        {
            var solicitacao = _suporteNegocio.First(s => s.IdSuporte == idSolicitacao, "SuporteLog", "Usuario", "Usuario.UsuarioPai");

            return Ok(new
            {
                solicitacao.IdStatus,
                solicitacao.DataAtualizacao,
                solicitacao.DataSolicitacao,
                solicitacao.DataCompra,
                solicitacao.NumeroPedido,
                solicitacao.ValorPedido,
                nomeUsuario = solicitacao.Usuario.Nome,
                loginUsuario = solicitacao.Usuario.Login,
                loginPatrocinador = solicitacao.Usuario.UsuarioPai.Login,
                solicitacao.TipoContato,
                solicitacao.UrlComprovante,
                solicitacao.Observacao,
                solicitacao.ObservacaoAdmin,
                historicoSuporte = solicitacao.SuporteLog.OrderByDescending(o => o.DataUpdate).Select(x => new
                {
                    DataUpdate = x.DataUpdate.ToString("dd/MM/yyyy - HH:mm"),
                    ObservacaoAdmin = ((StatusTransacaoEnum)x.IdStatus).GetDescription() + " - " + x.ObservacaoAdmin
                }).ToList()
            });
        }


        [HttpGet, Route("obterMinhasSolicitacoes/{idStatus:int?}"), Authorize]
        public IActionResult ObterMinhasSolicitacoes(int? idStatus)
        {
            return Ok(_suporteNegocio.ObterMinhasSolicitacoes(IdUsuarioLogado, idStatus));
        }

        [HttpGet, Route("Detalhes/{idStatus}"), Authorize]
        public IActionResult ObterDetalhesSolicitacao(int idSUporte)
        {
            var solicitacao = _suporteNegocio.FirstNoTracking(s => s.IdSuporte == idSUporte);

            return Ok(solicitacao);
        }
    }
}