using FluentValidation;
using MMN.Dominio.Model;
using MMN.Util.Util;
using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioBancoViewModel
    {
        public int IdUsuarioBanco { get; set; }
        public Guid? IdUsuario { get; set; }
        public int? IdBanco { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpfcnpj { get; set; }
        public string NomeConta { get; set; }
        public bool Ativo { get; set; }
        public int IdTipo { get; set; }
        public virtual BancoViewModel Banco { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual TipoViewModel Tipo { get; set; }
    }

    public class UsuarioBancoViewModelValidator : AbstractValidator<UsuarioBancoViewModel>
    {
        public UsuarioBancoViewModelValidator()
        {
            RuleFor(r => r.IdBanco).NotNull().GreaterThan(0);
            RuleFor(r => r.Agencia).NotEmpty().NotNull().Length(4);
            RuleFor(r => r.DigitoAgencia).Must(DigitoValido).WithMessage("agencia_digito_requisito");
            RuleFor(r => r.Conta).NotEmpty().NotNull().MaximumLength(11);
            RuleFor(r => r.DigitoConta).NotEmpty().NotNull().Length(1);
            RuleFor(r => r.Cpfcnpj).NotEmpty().NotNull().Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
            RuleFor(r => r.IdTipo).NotNull().GreaterThan(0);
            RuleFor(r => r.NomeConta).NotEmpty();
        }

        private bool DigitoValido(string digito)
        {
            if (string.IsNullOrEmpty(digito)) return true;
            else if (digito.Length == 1) return true;
            else return false;
        }
    }
}
