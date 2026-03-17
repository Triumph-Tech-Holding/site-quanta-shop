using FluentValidation;
using System;

namespace MMN.Dominio.ViewModel
{
    public class FaqViewModel
    {
        public int IdFaq { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public int IdCategoria { get; set; }
    }

    public class FaqViewModelValidator : AbstractValidator<FaqViewModel>
    {
        public FaqViewModelValidator()
        {
            RuleFor(c => c.Pergunta).NotEmpty().WithMessage("PerguntaRequerida");
            RuleFor(c => c.Resposta).NotEmpty().WithMessage("RespostaRequerida");
        }
    }

    public class FiltroFaqAdmin
    {
        public int idFaq { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public bool? IdStatus { get; set; }
    }
}
