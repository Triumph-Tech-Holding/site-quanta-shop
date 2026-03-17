using FluentValidation;
using MMN.Util.Util;
using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioEditarViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Documento { get; set; }
        public string SenhaAntiga { get; set; }
        public string NovaSenha { get; set; }
        public string NovaSenhaConfirma { get; set; }
        public string RG { get; set; }
        public string OrgaoEmissorRG { get; set; }
        public string EstadoRG { get; set; }
        public string Login { get; set; }
        public bool PreCadastro { get; set; }
        public string NomeSocial { get; set; }
        public string Genero { get; set; }
        public bool LoginAlterado { get; set; }
        public DateTime? DataNascimento { get; set; }
    }

    public class UsuarioEditarViewModelValidator : AbstractValidator<UsuarioEditarViewModel>
    {
        public UsuarioEditarViewModelValidator()
        {
            RuleFor(r => r.Nome).NotNull().NotEmpty().WithMessage("nome_requerido");
            RuleFor(r => r.Celular).NotNull().NotEmpty().WithMessage("telefone_requerido");
            RuleFor(r => r.Documento).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
        }
    }
}
