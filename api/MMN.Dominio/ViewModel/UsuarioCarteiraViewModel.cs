using FluentValidation;
using MMN.Dominio.Model;
using MMN.Util.Util;
using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCarteiraViewModel
    {
        public long IdUsuarioCarteira { get; set; }
        public string Endereco { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public bool Aprovado { get; set; }
        public DateTime? DataAprovacao { get; set; }

        public Guid IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
    public class UsuarioCarteiraViewModelCrypto
    {
        public string IdUsuarioCarteira { get; set; }
    }

    public class UsuarioCarteiraViewModelValidator : AbstractValidator<UsuarioCarteiraViewModel>
    {
        public UsuarioCarteiraViewModelValidator()
        {
            RuleFor(r => r.Endereco).NotNull().NotEmpty().WithMessage("EnderecoCadastroCarteiraInvalido");
            RuleFor(r => r.Descricao).NotNull().NotEmpty().WithMessage("DescricaoCadastroCarteiraInvalido");
        }
    }
}
