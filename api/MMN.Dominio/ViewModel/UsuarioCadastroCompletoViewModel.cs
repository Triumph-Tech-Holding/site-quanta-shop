using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioCadastroCompletoViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string SenhaAntiga { get; set; }
        public string NovaSenha { get; set; }
        public string NovaSenhaConfirma { get; set; }
        public string Celular { get; set; }
        public string AssinaturaEletronica { get; set; }
        public string Imagem { get; set; }
        public int IdCidade { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string NomeSocial { get; set; }
        public string Genero { get; set; }
        public bool LoginAlterado { get; set; }
        public string Login { get; set; }
        public DateTime? DataNascimento { get; set; }
    }

    public class UsuarioCadastroCompletoViewModelValidator : AbstractValidator<UsuarioCadastroCompletoViewModel>
    {
        public UsuarioCadastroCompletoViewModelValidator()
        {
            RuleFor(r => r.Nome).NotNull().NotEmpty();
            RuleFor(r => r.Celular).NotNull().NotEmpty();
            RuleFor(r => r.IdCidade).NotNull().GreaterThan(0);
            RuleFor(r => r.Rua).NotNull().NotEmpty();
            RuleFor(r => r.Numero).NotNull().NotEmpty();
            RuleFor(r => r.Bairro).NotNull().NotEmpty();
            RuleFor(r => r.Cep).NotNull().NotEmpty();
        }
    }
}
