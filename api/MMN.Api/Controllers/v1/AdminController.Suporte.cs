using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Enum;
using MMN.Dominio.ViewModel;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static MMN.Dominio.ViewModel.FiltroViewModel;

namespace MMN.Api.Controllers.v1
{
    public partial class AdminController
    {
        [HttpPost, Route("obterSolicitacoes/"), Authorize]
        public IActionResult ObterSolicitacoesAdmin([FromBody] FiltroSuporteViewModel filtro)
        {
            var solicitacoes = _suporteNegocio.GetAll("Usuario", "Usuario.UsuarioPai", "Status");

            solicitacoes = filtro.DataInicioInicio.HasValue   ? solicitacoes.Where(s => s.DataSolicitacao  >= filtro.DataInicioInicio).ToList()   : solicitacoes;
            solicitacoes = filtro.DataInicioFim.HasValue      ? solicitacoes.Where(s => s.DataSolicitacao  <= filtro.DataInicioFim).ToList()       : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoInicio.HasValue ? solicitacoes.Where(s => s.DataAtualizacao >= filtro.DataAtualizacaoInicio).ToList() : solicitacoes;
            solicitacoes = filtro.DataAtualizacaoFim.HasValue ? solicitacoes.Where(s => s.DataAtualizacao  <= filtro.DataAtualizacaoFim).ToList()  : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginPatrocinador) ? solicitacoes.Where(s => s.Usuario.UsuarioPai != null && s.Usuario.UsuarioPai.Login.Contains(filtro.LoginPatrocinador)).ToList() : solicitacoes;
            solicitacoes = !string.IsNullOrEmpty(filtro.LoginUsuario) ? solicitacoes.Where(s => s.Usuario.Login.Contains(filtro.LoginUsuario)).ToList() : solicitacoes;
            solicitacoes = filtro.IdStatus.HasValue ? solicitacoes.Where(s => s.IdStatus == filtro.IdStatus.Value).ToList() : solicitacoes;
            solicitacoes = filtro.IdTipo.HasValue   ? solicitacoes.Where(s => s.TipoContato == (TipoContatoEnum)filtro.IdTipo.Value).ToList() : solicitacoes;

            var result = new List<object>();
            foreach (var solicitacao in solicitacoes.OrderByDescending(s => s.DataSolicitacao))
            {
                result.Add(new
                {
                    idSolicitacao    = solicitacao.IdSuporte,
                    dataPedido       = solicitacao.DataCompra,
                    dataAtualizacao  = solicitacao.DataAtualizacao,
                    dataSolicitacao  = solicitacao.DataSolicitacao,
                    tipoId           = (int)solicitacao.TipoContato,
                    tipo             = solicitacao.TipoContato.GetDescription(),
                    statusId         = solicitacao.Status.IdStatus,
                    status           = solicitacao.Status.Nome,
                    loginUsuario     = solicitacao.Usuario.Login,
                    loja             = solicitacao.SiteCompra,
                    nome             = solicitacao.Usuario.Nome,
                    email            = solicitacao.Usuario.Email,
                    comprovante      = solicitacao.UrlComprovante,
                    descricao        = solicitacao.Observacao,
                    observacao       = solicitacao.ObservacaoAdmin,
                    valor            = solicitacao.ValorPedido,
                    telefone         = solicitacao.Usuario.Celular,
                    loginPatrocinador = solicitacao.Usuario.UsuarioPai?.Login ?? string.Empty,
                });
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("obterSolicitacoesSuporte")]
        public IActionResult ObterSolicitacoesSuporte(FiltroSuporte viewModel)
        {
            return Ok(_suporteNegocio.FiltrarSolicitacoes(viewModel));
        }

        [HttpPost]
        [Route("atualizarStatusSuporte")]
        public IActionResult AtualizarStatusSuporte(dynamic viewModel)
        {
            var parsed = JsonConvert.DeserializeObject(viewModel.ToString());
            int idSuporte = Convert.ToInt32(parsed.idSuporte.Value);
            var suporte = _suporteNegocio.FirstNoTracking(f => f.IdSuporte == idSuporte);
            suporte.IdStatus = Convert.ToInt32(parsed.idStatus.Value);
            suporte.ObservacaoAdmin = parsed.observacaoAdmin.Value;
            suporte.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            suporte.IdUsuarioAcao = IdUsuarioLogado;
            _suporteNegocio.Update(suporte);
            return Ok("Atualizado com sucesso!");
        }
    }
}
