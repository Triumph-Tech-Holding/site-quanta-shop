using FluentValidation;

namespace MMN.Dominio.ViewModel
{
    public class ContatoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Mensagem { get; set; }
    }

    public class ContatoViewModelValidator : AbstractValidator<ContatoViewModel>
    {
        public ContatoViewModelValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("nome_requerido");
            RuleFor(c => c.Email).EmailAddress().WithMessage("email_invalido");
            RuleFor(c => c.Mensagem).NotEmpty().WithMessage("mensagem_requerida");
        }
    }
}
