using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaqueController : LoggedControllerBase
    {
        private readonly ISaqueNegocio _saqueNegocio;
        private readonly ILancamentoNegocio _lancamentoNegocio;
        private readonly IUsuarioNegocio _negocioUsuario;
        private readonly IConfiguracaoNegocio _configNegocio;
        private readonly IProceduresRepositorio _proceduresRepositorio;

        public SaqueController(ILancamentoNegocio lancamentoNegocio, IUsuarioNegocio negociousuario, ISaqueNegocio saqueNegocio, IConfiguracaoNegocio configNegocio, IProceduresRepositorio proceduresRepositorio)
        {
            _lancamentoNegocio = lancamentoNegocio;
            _negocioUsuario = negociousuario;
            _saqueNegocio = saqueNegocio;
            _configNegocio = configNegocio;
            _proceduresRepositorio = proceduresRepositorio;
        }
        /// <summary>
        /// Solicitar saque - Enviar apenas valor, IdUsuarioBanco e tipo de saque (cashback ou Rede)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("solicitarSaque")]
        public async Task<IActionResult> SolicitarSaqueAsync(SaqueViewModel viewModel)
        {
            var validator = new SaqueViewModelValidator();
            var result = validator.Validate(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var saldo = _lancamentoNegocio.Get(l => l.Ativo && l.IdUsuario == IdUsuarioLogado && l.IdStatus != 3 && !l.Bloqueado, "LancamentoRetido")
                        .Sum(l => l.Valor - l.LancamentoRetido.Where(w => w.Ativo).Sum(lr => lr.Valor));

            var mesAnterior = ObterPrimeiroEUltimoDiaMesAnterior();

            //var consumo = _proceduresRepositorio.fnc_ObterResumoConsumoMensal(IdUsuarioLogado, mesAnterior.primeiroDia, mesAnterior.ultimoDia);

            var consumo = await _saqueNegocio.ObterConsumoSaque(IdUsuarioLogado, mesAnterior.primeiroDia, mesAnterior.ultimoDia);

            var configConsumo = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "VALOR_MINIMO_CONSUMO").Valor);

            if (saldo < viewModel.Valor)
            {
                throw new PadraoException("saldo_insuficiente");
            }

            if (consumo < configConsumo)
            {
                throw new PadraoException("consumo_insuficiente");
            }

            viewModel.IdUsuario = IdUsuarioLogado;
            _saqueNegocio.InserirPedidoSaque(viewModel);

            return Ok();
        }

        [HttpGet]
        [Route("obterConsumo")]
        public async Task<IActionResult> ObterConsumoAsync()
        {
            var mesAtual = ObterPrimeiroEUltimoDiaMesAtual();
            var mesAnterior = ObterPrimeiroEUltimoDiaMesAnterior();

            var consumoMesAnterior = await _saqueNegocio.ObterConsumoSaque(IdUsuarioLogado, mesAnterior.primeiroDia, mesAnterior.ultimoDia);
            var consumoMesAtual = await _saqueNegocio.ObterConsumoSaque(IdUsuarioLogado, mesAtual.primeiroDia, mesAtual.ultimoDia);
            var configConsumo = Convert.ToDecimal(_configNegocio.FirstNoTracking(c => c.Chave == "VALOR_MINIMO_CONSUMO").Valor);

            return Ok(
                new
                {
                    ConsumoMesAnteriorFormatado = consumoMesAnterior.ToString("C2"),
                    ConsumoMesAtualFormatado = consumoMesAtual.ToString("C2"),
                    ConsumoMesAtual = consumoMesAtual,
                    ConsumoMesAnterior = consumoMesAnterior,
                    ConfigConsumo = configConsumo.ToString("C2"),
                }); ;
        }

        /// <summary>
        /// Saques solicitados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("saquesSolicitados")]
        public IActionResult SaquesSolicitados()
        {
            var lista = _saqueNegocio.Get(s => s.IdUsuario == IdUsuarioLogado, "Status", "UsuarioBanco")
                .Select(s => new
                {
                    Status = s.Status.Nome,
                    s.Valor,
                    s.DataSolicitacao,
                    s.DataAprovacao,
                    s.UsuarioBancoViewModel.NomeConta
                });

            return Ok(lista);
        }

        /// <summary>
        /// Saques solicitados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("listarTiposDeSaque")]
        public IActionResult ListarTiposDeSaque()
        {
            var lista = new Dictionary<string, int>();
            lista.Add(EnumTipoSaldo.CHBK.GetDescription(), (int)EnumTipoSaldo.CHBK);
            lista.Add(EnumTipoSaldo.SLRD.GetDescription(), (int)EnumTipoSaldo.SLRD);
            return Ok(lista);
        }

        [HttpGet]
        [Route("obterStatusSaque")]
        public IActionResult ObterStatusSaque()
        {
            var lista = new Dictionary<string, int>();
            lista.Add(StatusTransacaoEnum.EmAprovacao.GetDescription(), (int)StatusTransacaoEnum.EmAprovacao);
            lista.Add(StatusTransacaoEnum.Finalizada.GetDescription(), (int)StatusTransacaoEnum.Finalizada);
            lista.Add(StatusTransacaoEnum.EmProcessamento.GetDescription(), (int)StatusTransacaoEnum.EmProcessamento);
            lista.Add(StatusTransacaoEnum.Cancelada.GetDescription(), (int)StatusTransacaoEnum.Cancelada);
            return Ok(lista.Select(i => new { i.Key, i.Value }));
        }

        [HttpPost]
        [Route("listarSaquesAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult BuscarSaques(FiltroViewModel.FiltroSaque model)
        {
            var saques = _saqueNegocio.BuscarPagamentos(model);
            return Ok(saques);
        }


        [HttpGet]
        [Route("obterResumoSaque")]
        [Authorize(Roles = "Admin")]
        public IActionResult ObterResumoSaque()
        {
            var dataInicio = DateTime.Now.HorarioBrasilia().AddDays(-7);

            var dataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            var lista = _saqueNegocio.ObterResumoSaque(dataInicio, dataFim);
            return Ok(lista);
        }

        [HttpPost]
        [Route("cancelarSaques")]
        [Authorize(Roles = "Admin")]
        public IActionResult CancelarSaques(CancelarAprovarSaquesViewModel viewModel)
        {
            CancelarAprovarSaquesViewModelValidator validator = new CancelarAprovarSaquesViewModelValidator();

            var result = validator.Validate(viewModel);
            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _negocioUsuario.FirstNoTracking(u => u.IdUsuario == IdUsuarioLogado);
            var senhaCorreta = _negocioUsuario.Autenticacao(usuario.Login, viewModel.ConfirmPassword, out _);
            if (senhaCorreta != null)
            {
                _saqueNegocio.CancelarSaque(viewModel.Selecionados);
                return Ok(new { success = true });
            }
            throw new UnauthorizedException("senha_incorreta");
        }

        [HttpPost]
        [Route("aprovarSaques")]
        [Authorize(Roles = "Admin")]
        public IActionResult AprovarSaques(CancelarAprovarSaquesViewModel viewModel)
        {
            CancelarAprovarSaquesViewModelValidator validator = new CancelarAprovarSaquesViewModelValidator();

            var result = validator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(
                    result.Errors.Select(e => new PadraoException(e.ErrorMessage)));
            }

            var usuario = _negocioUsuario.FirstNoTracking(u => u.IdUsuario == IdUsuarioLogado);
            var senhaCorreta = _negocioUsuario.Autenticacao(usuario.Login, viewModel.ConfirmPassword, out _);

            if (senhaCorreta != null)
            {
                _saqueNegocio.AprovarSolicitacaoSaque(viewModel.Selecionados, IdUsuarioLogado);
                return Ok(new { success = true });
            }

            throw new UnauthorizedException("senha_incorreta");
        }

        public static (DateTime primeiroDia, DateTime ultimoDia) ObterPrimeiroEUltimoDiaMesAnterior()
        {
            DateTime dataAtual = DateTime.Today;
            DateTime primeiroDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
            DateTime ultimoDiaMesPassado = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddDays(-1);
            return (primeiroDiaMesPassado, ultimoDiaMesPassado);
        }

        public static (DateTime primeiroDia, DateTime ultimoDia) ObterPrimeiroEUltimoDiaMesAtual()
        {
            DateTime dataAtual = DateTime.Today;
            DateTime primeiroDia = new DateTime(dataAtual.Year, dataAtual.Month, 1);
            DateTime ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
            return (primeiroDia, ultimoDia);
        }
    }
}