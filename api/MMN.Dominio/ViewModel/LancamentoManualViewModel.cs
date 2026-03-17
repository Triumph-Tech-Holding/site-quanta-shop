using FluentValidation;

namespace MMN.Dominio.ViewModel
{
    public class LancamentoManualViewModel
    {
        public decimal Valor { get; set; }
        public string Login { get; set; }
        public string Descricao { get; set; }
        public decimal MaximoDistribuido { get; set; }
        public bool DistribuirRede { get; set; }

    }

    public class LancamentoManualViewModelValidator : AbstractValidator<LancamentoManualViewModel>
    {
        public LancamentoManualViewModelValidator()
        {
            RuleFor(r => r.Valor).NotNull().NotEmpty();
            RuleFor(r => r.Login).NotEmpty().NotNull();
            RuleFor(r => r.Descricao).NotNull().NotEmpty();
        }
    }
}