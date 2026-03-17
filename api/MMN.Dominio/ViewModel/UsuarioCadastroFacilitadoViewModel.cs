using FluentValidation;
using MMN.Util.Util;
using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCadastroFacilitadoViewModel
    {
        public string CPF { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public decimal ValorCompra { get; set; }
        public string ComprovanteCompra { get; set; }
        public string CNPJ { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string LoginIndicacao { get; set; }
    }

    public class UsuarioCadastroFacilitadoViewModelValidator : AbstractValidator<UsuarioCadastroFacilitadoViewModel>
    {
        public UsuarioCadastroFacilitadoViewModelValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("email_requerido")
                .EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");            
            RuleFor(u => u.Celular).NotNull().NotEmpty().WithMessage("telefone_invalido");
            //RuleFor(u => u.ValorCompra).NotNull().NotEmpty().NotEqual(0.00m);
            //RuleFor(u => u.ComprovanteCompra).NotNull().NotEmpty();
            RuleFor(u => u.CPF).Must(UtilBase.IsCpf).WithMessage("cpf_cnpj_invalido");
        }
    }
}
