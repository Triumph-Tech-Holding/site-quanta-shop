using FluentValidation;
using MMN.Util.Util;
using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCadastroViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Celular { get; set; }
        public string Documento { get; set; }
        public string LoginPatrocinador { get; set; }
    }

    public class UsuarioCadastroViewModelValidator : AbstractValidator<UsuarioCadastroViewModel>
    {
        public UsuarioCadastroViewModelValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("email_requerido")
                .EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");
            RuleFor(u => u.Senha).Must(UtilBase.RequisitosSenha).WithMessage("senha_requisitos");
            RuleFor(u => u.Login).MinimumLength(5).WithMessage("login_tamanho_minimo")
                .Must(UtilBase.RequisitosLogin).WithMessage("login_nao_permitido");            
        }
    }
}
