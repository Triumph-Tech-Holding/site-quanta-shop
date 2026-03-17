using FluentValidation;
using System;

namespace MMN.Dominio.ViewModel
{
    public class MaterialApoioViewModel
    {
        public int IdMaterial { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string URLMaterial { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
    public class MaterialApoioViewModelValidator : AbstractValidator<MaterialApoioViewModel>
    {
        public MaterialApoioViewModelValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("NomeRequerido");
            RuleFor(c => c.Descricao).NotEmpty().WithMessage("DescricaoRequerida");
        }
    }
}
