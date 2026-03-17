using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioEnderecoViewModel
    {
        public int IdEndereco { get; set; }
        public int IdCidade { get; set; }
        public Guid IdUsuario { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual CidadeViewModel Cidade { get; set; }
    }

    public class UsuarioEnderecoViewModelValidator : AbstractValidator<UsuarioEnderecoViewModel>
    {
        public UsuarioEnderecoViewModelValidator()
        {
            RuleFor(r => r.IdEndereco).NotNull().GreaterThan(0);
            RuleFor(r => r.IdCidade).NotNull().GreaterThan(0);
            RuleFor(r => r.IdUsuario).NotNull().NotEmpty();
            RuleFor(r => r.Rua).NotNull().NotEmpty();
            RuleFor(r => r.Numero).NotNull().NotEmpty();
            RuleFor(r => r.Bairro).NotNull().NotEmpty();
            RuleFor(r => r.Cep).NotNull().NotEmpty();
        }
    }
}
