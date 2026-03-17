using FluentValidation;

namespace MMN.Dominio.ViewModel
{
    public class AssinaturaEletronicaViewModel
    {
        public string AssinaturaEletronica { get; set; }
        public string RepitaAssinatura { get; set; }
        public string AssinaturaAtual { get; set; }
    }

    public class AssinaturaEletronicaViewModelValidator : AbstractValidator<AssinaturaEletronicaViewModel>
    {
        public AssinaturaEletronicaViewModelValidator()
        {
            RuleFor(r => r.AssinaturaEletronica).NotNull().NotEmpty();
            RuleFor(r => r.RepitaAssinatura).NotNull().NotEmpty().Equal(e => e.AssinaturaEletronica);
            RuleFor(r => r.AssinaturaAtual).NotNull().NotEmpty();
        }
    }
}
