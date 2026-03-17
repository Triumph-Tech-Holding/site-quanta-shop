using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentValidation;
using MMN.Util.Util;

namespace MMN.Dominio.ViewModel
{
    public class AdminDadosUsuarioViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string SenhaConfirma { get; set; }
        public string LoginPatrocinador { get; set; }
        public string Celular { get; set; }
        public string Nome { get; set; }
    }

    public class AdminDadosUsuarioViewModelValidator  : AbstractValidator<AdminDadosUsuarioViewModel>
    {
        public AdminDadosUsuarioViewModelValidator()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("email_invalido");
            RuleFor(u => u.Senha).Must(RequisitosSenha).When(u => !string.IsNullOrWhiteSpace(u.Senha)).WithMessage("senha_requisitos");
            RuleFor(u => u.Login).MinimumLength(5).WithMessage("login_tamanho_minimo");
            RuleFor(u => u.Nome).NotNull().NotEmpty().WithMessage("nome_requerido");
            RuleFor(u => u.Celular).NotNull().NotEmpty().WithMessage("celular_requerido");
            RuleFor(u => u.LoginPatrocinador).NotNull().NotEmpty().WithMessage("patrocinador_requerido");
            RuleFor(u => u.Senha).Equal(u => u.SenhaConfirma).When(u => !string.IsNullOrWhiteSpace(u.Senha))
                .WithMessage("senha_nao_confere");
        }

        public bool RequisitosSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return false;

            if (senha.Length < 8 || !UtilBase.HasDigit(senha) || !UtilBase.HasLetter(senha)) return false;

            return true;
        }

    }
}
