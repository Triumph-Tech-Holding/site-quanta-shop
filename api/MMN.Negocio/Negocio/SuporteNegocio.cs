using AutoMapper;
using Microsoft.Extensions.Options;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.Dominio.ViewModel.Suporte;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class SuporteNegocio : BaseNegocio<SuporteViewModel, Suporte>, ISuporteNegocio
    {
        private readonly AppSettings _appSettings;
        private readonly ISuporteRepositorio _repositorio;
        private readonly IPedidoNegocio _pedidoNegocio;
        private readonly IMapper _mapper;
        public SuporteNegocio(
            IOptions<AppSettings> appSettings,
            ISuporteRepositorio repositorio,
            IPedidoNegocio pedidoNegocio,
            IMapper mapper) : base(repositorio, mapper)
        {
            _appSettings = appSettings.Value;
            _repositorio = repositorio;
            _mapper = mapper;
            _pedidoNegocio = pedidoNegocio;
        }

        public object FiltrarSolicitacoes(FiltroViewModel.FiltroSuporte viewModel)
        {
            var suportes = Get(w => (!viewModel.IdStatus.HasValue || w.IdStatus == viewModel.IdStatus.Value)
                && (string.IsNullOrEmpty(viewModel.Login) || (w.Usuario.Login.Contains(viewModel.Login) || w.Usuario.Email.Contains(viewModel.Login)))
                , "Usuario", "Status", "SuporteLog");

            var totalPages = (int)Math.Ceiling((double)suportes.Count() / viewModel.PerPage);
            var totalRegistros = suportes.Count();
            return new
            {
                totalRegistros,
                totalPages,
                data = suportes.Skip(viewModel.PerPage * (viewModel.Page - 1)).Take(viewModel.PerPage).Select(s => new
                {
                    s.IdSuporte,
                    s.Usuario.Login,
                    s.Usuario.Celular,
                    s.Usuario.Email,
                    s.SiteCompra,
                    s.ValorPedido,
                    s.DataCompra,
                    s.DataSolicitacao,
                    s.Observacao,
                    s.NumeroPedido,
                    s.Usuario.Nome,
                    s.UrlComprovante,
                    s.ObservacaoAdmin,
                    Status = new { s.Status.Nome, s.Status.IdStatus },
                    HistoricoSuporte = s.SuporteLog.OrderByDescending(o => o.DataUpdate).Select(x => new
                    {
                        x.DataUpdate,
                        ObservacaoAdmin = EnumExtensions.GetDescription((StatusTransacaoEnum)x.IdStatus) + " - " + x.ObservacaoAdmin
                    }).ToList()
                }).OrderByDescending(o => o.DataSolicitacao).ToList()
            };
        }

        public object ObterMinhasSolicitacoes(Guid idUsuario, int? idStatus)
        {
            var suportes = Get(w => w.IdUsuario == idUsuario && (!idStatus.HasValue || w.IdStatus == idStatus), "Usuario", "Status", "SuporteLog");

            return new
            {
                data = suportes.Select(s => new
                {
                    s.IdSuporte,
                    s.Usuario.Login,
                    s.Usuario.Celular,
                    s.Usuario.Email,
                    s.SiteCompra,
                    s.ValorPedido,
                    s.DataCompra,
                    s.DataSolicitacao,
                    s.Observacao,
                    s.UrlComprovante,
                    s.DataAtualizacao,
                    s.ObservacaoAdmin,
                    Status = new { s.Status.Nome, s.Status.IdStatus },
                    HistoricoSuporte = s.SuporteLog.OrderByDescending(o => o.DataUpdate).Select(s => new { s.DataUpdate, s.ObservacaoAdmin }).ToList()
                }).OrderByDescending(o => o.DataSolicitacao).ToList()
            };
        }

        public async Task<bool> SolicitarCashbackNaoPago(SuporteViewModel viewModel, UsuarioViewModel usuario, ObjEmailUtilitis objectEmail)
        {
            var validator = new SuporteCashBackNaoPagoValidator();
            var result = validator.Validate(viewModel);

            if (!result.IsValid)
            {
                throw new AggregateException(result.Errors.Select(s => new Exception(s.ErrorMessage)));
            }

            viewModel.TipoContato = Util.Enum.TipoContatoEnum.CashbackNaoPago;

            Dictionary<string, string> body = new Dictionary<string, string> {
                { "#Login#", usuario.Login},
                { "#DataCompra#", ((DateTime)viewModel.DataCompra).ToShortDateString() },
                { "#SiteCompra#", viewModel.SiteCompra },
                { "#NumeroPedido#", viewModel.NumeroPedido},
                { "#ValorPedido#", $"R$ {viewModel.ValorPedido}" },
                { "#Observacao#", viewModel.Observacao }
            };

            await new EmailUtilitis().EnviarEmail(body, _appSettings.SuporteEmailTemplate, "", objectEmail);

            var nomeArquivo = "Comp_" + DateTime.UtcNow.HorarioBrasilia().ToString("yyyy-MM-dd HH:mm:ss") + "-" + usuario.IdUsuario;

            var urlImagem = await AzureStorage
                .CreateBlob(
                    viewModel.UrlComprovante,
                    usuario.IdUsuario,
                    _appSettings.StorageAccountConnectionString,
                    "comprovantes",
                    nomeArquivo,
                    true
                );

            viewModel.UrlComprovante = urlImagem;
            viewModel.DataSolicitacao = DateTime.UtcNow.HorarioBrasilia();
            viewModel.IdUsuario = usuario.IdUsuario;
            viewModel.IdStatus = 1;

            this.Insert(viewModel);

            return true;
        }

        public async Task<bool> SolicitarCancelamentoParcela(SuporteViewModel viewModel, UsuarioViewModel usuario, ObjEmailUtilitis objectEmail)
        {
            var pedido = _pedidoNegocio.FirstNoTracking(p => p.IdPedido == Convert.ToInt64(viewModel.NumeroPedido));

            _repositorio.Insert(new Suporte
            {
                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
                DataSolicitacao = DateTime.UtcNow.HorarioBrasilia(),
                DataCompra = pedido.DataPedido,
                IdUsuario = pedido.IdUsuario,
                NumeroPedido = pedido.IdPedido.ToString(),
                ValorPedido = pedido.ValorPedido,
                IdStatus = 1,
                SiteCompra = pedido.Codigo,
                TipoContato = Util.Enum.TipoContatoEnum.CancelamentoParcelas,
                Observacao = string.Empty,
                ObservacaoAdmin = string.Empty,
                UrlComprovante = string.Empty
            });

            _repositorio.SaveChanges();

            return true;
        }

        public async Task<bool> SolicitarCancelamentoParcelaOld(SuporteViewModel viewModel, UsuarioViewModel usuario, ObjEmailUtilitis objectEmail)
        {

            var solicitacao = _mapper.Map<SuporteCancelarParcelamentoViewModel>(viewModel);
            var validator = new SuporteCancelarParcelaViewModel();
            var result = validator.Validate(solicitacao);

            if (!result.IsValid)
            {
                throw new AggregateException(result.Errors.Select(s => new Exception(s.ErrorMessage)));
            }

            Dictionary<string, string> body = new Dictionary<string, string> {
                { "#Login#", usuario.Login},
                { "#DataCompra#", ((DateTime)solicitacao.DataCompra).ToShortDateString() },
                { "#CodigoPedido#", solicitacao.CodigoPedido },
                { "#ValorPedido#", $"R$ {solicitacao.ValorPedido}" },
            };

            await new EmailUtilitis().EnviarEmail(body, _appSettings.SuporteCancelarParcelamentoEmailTemplate, "", objectEmail);

            solicitacao.DataSolicitacao = DateTime.UtcNow.HorarioBrasilia();
            solicitacao.IdUsuario = usuario.IdUsuario;
            solicitacao.IdStatus = 1;

            viewModel = _mapper.Map<SuporteViewModel>(solicitacao);
            viewModel.UrlComprovante = "";

            this.Insert(viewModel);

            return true;
        }
    }
}