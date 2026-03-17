using FluentValidation;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCadastroPWAViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Celular { get; set; }
        public string Documento { get; set; }
        public string LoginPatrocinador { get; set; }
    }

    public class UsuarioCadastroPWAViewModelValidator : AbstractValidator<UsuarioCadastroPWAViewModel>
    {
        public UsuarioCadastroPWAViewModelValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("email_requerido")
                .EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");
            RuleFor(u => u.Senha).Must(UtilBase.RequisitosSenha).WithMessage("senha_requisitos");
            RuleFor(u => u.LoginPatrocinador).NotNull().WithMessage("patrocinador_requerido").NotEmpty().WithMessage("patrocinador_requerido");
            RuleFor(u => u.Documento).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
        }
    }
}
