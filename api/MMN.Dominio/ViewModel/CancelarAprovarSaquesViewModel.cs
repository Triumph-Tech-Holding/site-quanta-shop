using FluentValidation;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class CancelarAprovarSaquesViewModel
    {
        public List<SaqueViewModel> Selecionados { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CancelarAprovarSaquesViewModelValidator : AbstractValidator<CancelarAprovarSaquesViewModel>
    {
        public CancelarAprovarSaquesViewModelValidator()
        {
            RuleFor(c => c.ConfirmPassword).NotEmpty().WithMessage("senha_nao_confere");
            RuleFor(c => c.Selecionados).NotNull().WithMessage("saque_selecao_requerida");
        }
    }
}
