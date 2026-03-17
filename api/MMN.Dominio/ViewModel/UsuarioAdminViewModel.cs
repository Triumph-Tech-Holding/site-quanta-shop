using FluentValidation;
using MMN.Util.Util;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioAdminViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Celular { get; set; }
        public int IdGrupo { get; set; }
    }

    public class UsuarioAdminViewModelValidator : AbstractValidator<UsuarioAdminViewModel>
    {
        public UsuarioAdminViewModelValidator()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("EmailInvalido");
            RuleFor(u => u.Senha).Must(RequisitosSenha).WithMessage("RequisitosSenha");
            RuleFor(u => u.Login).MinimumLength(5).WithMessage("TamanhoMinimoLogin");
        }

        public bool RequisitosSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return false;

            if (senha.Length < 8 || !UtilBase.HasDigit(senha) || !UtilBase.HasLetter(senha)) return false;

            return true;
        }
    }
}
