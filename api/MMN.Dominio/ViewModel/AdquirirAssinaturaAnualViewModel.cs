using FluentValidation;
using MMN.Util.Enum;
using System;


namespace MMN.Dominio.ViewModel
{
    public class AdquirirAssinaturaAnualViewModel
    {
        public int IdProduto { get; set; }
        public EnumTipoPagamento FormaPagamento { get; set; }
        public bool AceiteTermos { get; set; }
        public int NumParcelas { get; set; }
        public CartaoViewModel Card { get; set; }
    }

    public class AdquirirAssinaturaAnualViewModelValidator : AbstractValidator<AdquirirAssinaturaAnualViewModel>
    {
        public AdquirirAssinaturaAnualViewModelValidator()
        {
            //RuleFor(a => a.FormaPagamento).IsInEnum().WithMessage("pagamento_tipo_invalido");
            //RuleFor(a => a.FormaPagamento).NotEqual(EnumTipoPagamento.PGPAGARME).WithMessage("pagamanto_tipo_nao_implementado");
            RuleFor(a => a.IdProduto).NotNull().WithMessage("produto_requerido");
            RuleFor(a => a.AceiteTermos).NotNull().Equal(true).WithMessage("compra_termos_requerido");
            RuleFor(a => a.Card).SetValidator(new CartaoViewModelValidator());

        }
    }
}
