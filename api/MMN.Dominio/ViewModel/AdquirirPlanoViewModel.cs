using FluentValidation;
using MMN.Util.Enum;
using System;


namespace MMN.Dominio.ViewModel
{
    public class AdquirirPlanoViewModel
    {
        public int IdProduto { get; set; }
        public EnumTipoPagamento FormaPagamento { get; set; }
        public bool AceiteTermos { get; set; }
        public int NumParcelas { get; set; }
        public CartaoViewModel Card { get; set; }
    }

    public class AdquirirPlanoViewModelValidator : AbstractValidator<AdquirirPlanoViewModel>
    {
        public AdquirirPlanoViewModelValidator()
        {
            //RuleFor(a => a.FormaPagamento).IsInEnum().WithMessage("pagamento_tipo_invalido");
            //RuleFor(a => a.FormaPagamento).NotEqual(EnumTipoPagamento.PGPAGARME).WithMessage("pagamanto_tipo_nao_implementado");
            RuleFor(a => a.IdProduto).NotNull().WithMessage("produto_requerido");
            RuleFor(a => a.AceiteTermos).NotNull().Equal(true).WithMessage("compra_termos_requerido");
            RuleFor(a => a.Card).SetValidator(new CartaoViewModelValidator());

        }
    }

    public class CartaoViewModelValidator : AbstractValidator<CartaoViewModel>
    {
        public CartaoViewModelValidator()
        {
            RuleFor(c => c.Card_expiration_year)
            .NotNull().WithMessage("ano_deve_ser_informado")
            .Must((card, expirationYear) => IsValidExpirationYear(expirationYear))
            .WithMessage("data_expiracao_invalido");

            RuleFor(c => c.Card_expiration_month)
                .NotNull().WithMessage("mes_invalido")
                .Must((card, expirationMonth) => IsValidExpirationMonth(card.Card_expiration_year, expirationMonth.ToString()))
                .WithMessage("data_expiracao_invalido");
 
            RuleFor(c => c.Card_cvv)
                .NotNull().WithMessage("card_cvv")
                .NotEmpty().WithMessage("card_cvv");

            RuleFor(c => c.Card_holder_name)
                .NotNull().WithMessage("card_holder_name")
                .NotEmpty().WithMessage("card_holder_name");

            RuleFor(c => c.Card_number)
                .NotNull().WithMessage("card_number");
        }

        private bool IsValidExpirationYear(string expirationYear)
        {
            if (!int.TryParse(expirationYear, out int year))
                return false;

            int currentYear = DateTime.Now.Year;
            return year >= currentYear;
        }

        private bool IsValidExpirationMonth(string expirationYear, string expirationMonth)
        {
            if (!int.TryParse(expirationYear, out int year) || !int.TryParse(expirationMonth, out int month))
                return false;

            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            if (year > currentYear) 
            {
                return true;
            }else if (year == currentYear)
            {
                if(month <= currentMonth)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }             

            return month < currentMonth;
        }
    }
}
