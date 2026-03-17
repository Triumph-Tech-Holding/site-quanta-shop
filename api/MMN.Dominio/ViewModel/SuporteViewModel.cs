using FluentValidation;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class SuporteViewModel
    {
        public int IdSuporte { get; set; }
        public DateTime? DataCompra { get; set; }
        public string SiteCompra { get; set; }
        public string NumeroPedido { get; set; }
        public decimal? ValorPedido { get; set; }
        public string Observacao { get; set; }
        public string UrlComprovante { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string ObservacaoAdmin { get; set; }
        public TipoContatoEnum TipoContato { get; set; }
        public Guid? IdUsuario { get; set; }
        public int? IdStatus { get; set; }
        public Guid? IdUsuarioAcao { get; set; }

        public UsuarioViewModel Usuario { get; set; }
        public StatusViewModel Status { get; set; }
        public UsuarioViewModel UsuarioAcao { get; set; }

        public virtual ICollection<SuporteLogViewModel> SuporteLog { get; set; }
    }

    public class SuporteCashBackNaoPagoValidator : AbstractValidator<SuporteViewModel>
    {
        public SuporteCashBackNaoPagoValidator()
        {
            RuleFor(c => c.NumeroPedido).NotEmpty().When(c => c.TipoContato == TipoContatoEnum.CashbackNaoPago || c.TipoContato == TipoContatoEnum.CancelamentoParcelas || c.TipoContato == TipoContatoEnum.ReaberturaParcelas).WithMessage("compra_numero_requerido");
            RuleFor(c => c.UrlComprovante).NotNull().NotEmpty().When(c => c.TipoContato == TipoContatoEnum.CashbackNaoPago).WithMessage("nota_fiscal_requerida");
            RuleFor(c => c.SiteCompra).NotEmpty().When(c => c.TipoContato == TipoContatoEnum.CashbackNaoPago).WithMessage("compra_site_requerido");
            RuleFor(c => c.ValorPedido).NotEmpty().When(c => c.TipoContato == TipoContatoEnum.CashbackNaoPago).WithMessage("compra_valor_requerido");
            RuleFor(c => c.Observacao).NotEmpty().WithMessage("observacao_requerida");
            RuleFor(c => c.TipoContato).NotNull().IsInEnum().WithMessage("contato_tipo_requerido");
        }
    }
}
