using FluentValidation;
using MMN.Util.Enum;

namespace MMN.Dominio.ViewModel
{
    public class AdquirirSaldoViewModel
    {
        public decimal Valor { get => Valor10 * 10 + Valor25 * 25 + Valor50 * 50; }
        public int Valor10 { get; set; }
        public int Valor25 { get; set; }
        public int Valor50 { get; set; }
        public decimal SaldoReceber { get; set; }
        public EnumTipoPagamento FormaPagamento { get; set; }
        public bool AceiteTermos { get; set; }
        public decimal? TaxaCompra { get; set; }
    }

    public class AdquirirSaldoViewModelValidator : AbstractValidator<AdquirirSaldoViewModel>
    {
        public AdquirirSaldoViewModelValidator()
        {
            RuleFor(a => a.FormaPagamento).IsInEnum().WithMessage("pagamento_tipo_invalido");
            RuleFor(a => a.FormaPagamento).NotEqual(EnumTipoPagamento.PGPAGARME).WithMessage("pagamanto_tipo_nao_implementado");
            RuleFor(a => a.Valor).GreaterThanOrEqualTo(10).WithMessage("compra_saldo_valor_minimo");
            RuleFor(a => a.Valor10).GreaterThanOrEqualTo(0).WithMessage("compra_saldo_valor_minimo");
            RuleFor(a => a.Valor25).GreaterThanOrEqualTo(0).WithMessage("compra_saldo_valor_minimo");
            RuleFor(a => a.Valor50).GreaterThanOrEqualTo(0).WithMessage("compra_saldo_valor_minimo");
            RuleFor(a => a.AceiteTermos).NotNull().Equal(true).WithMessage("compra_termos_requerido");
        }
    }
}
