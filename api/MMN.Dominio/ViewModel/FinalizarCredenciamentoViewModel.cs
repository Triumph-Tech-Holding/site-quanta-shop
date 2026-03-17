using FluentValidation;
using MMN.Util.Util;

namespace MMN.Dominio.ViewModel
{
    public class FinalizarCredenciamentoViewModel
    {
        public string IdCredenciamento { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmEmail { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }

    public class FinalizarCredenciamentoViewModelValidator : AbstractValidator<FinalizarCredenciamentoViewModel>
    {
        public FinalizarCredenciamentoViewModelValidator()
        {
            RuleFor(c => c.IdCredenciamento).NotEmpty().WithMessage("credenciamento_id_requerido");
            RuleFor(c => c.Login).NotEmpty().WithMessage("login_requerido");
            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido").EmailAddress().WithMessage("email_invalido");
            RuleFor(c => c.ConfirmEmail).NotEmpty().WithMessage("email_confirmacao_requerida").Equal(c => c.Email).WithMessage("email_nao_confere");
            RuleFor(c => c.Nome).NotEmpty().WithMessage("nome_requerido");
            RuleFor(c => c.Telefone).NotEmpty().WithMessage("telefone_requerido");
            RuleFor(c => c.Password).Must(RequisitosSenha).WithMessage("senha_requisitos");
        }

        public bool RequisitosSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return false;

            if (senha.Length < 8 || !UtilBase.HasDigit(senha) || !UtilBase.HasLetter(senha)) return false;

            return true;
        }
    }
}
