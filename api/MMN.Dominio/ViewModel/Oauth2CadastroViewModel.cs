using FluentValidation;
using MMN.Util.Util;

namespace MMN.Dominio.ViewModel
{
    public class Oauth2CadastroViewModel
    {
        public string Code { get; set; }
        public string RedirectUri { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Celular { get; set; }
        public string LoginPatrocinador { get; set; }
        public string Documento { get; set; }
        public string Senha { get; set; }
    }

    public class OauthCadastroViewModelValidator : AbstractValidator<Oauth2CadastroViewModel>
    {
        public OauthCadastroViewModelValidator()
        {
            RuleFor(u => u.Login).MinimumLength(5).WithMessage("login_tamanho_minimo")
                .Must(RequisitosLogin).WithMessage("login_nao_permitido");
            RuleFor(u => u.LoginPatrocinador).NotNull().NotEmpty();
            RuleFor(u => u.Senha).Must(m => string.IsNullOrEmpty(m) || UtilBase.RequisitosSenha(m))
                .WithMessage("senha_requisitos");
        }

        public bool RequisitosLogin(string login)
        {
            return !login.ToLower().Contains("big")
                && !login.ToLower().Contains("cash")
                && !login.ToLower().Contains("admin");
        }
    }
}
