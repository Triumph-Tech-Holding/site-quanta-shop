using FluentValidation;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class MensagemViewModel
    {
        public int IdMensagem { get; set; }
        public Guid? IdUsuarioDestino { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public TipoMensagem TipoMensagem { get; set; }
        public string UrlArquivo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Base64Arquivo { get; set; }
        public DateTime? DataLeitura { get; set; }
        public virtual UsuarioViewModel UsuarioDestino { get; set; }

        public ICollection<MensagemGraduacaoViewModel> MensagemGraduacao { get; set; }
    }
    public class MensagemViewModelValidator : AbstractValidator<MensagemViewModel>
    {
        public MensagemViewModelValidator()
        {
            RuleFor(c => c.Titulo).NotEmpty().WithMessage("titulo_requerido");
            RuleFor(c => c.Texto).NotEmpty().When(c => string.IsNullOrEmpty(c.Base64Arquivo)).WithMessage("mensagem_imagem_nao_preenchida");
            RuleFor(c => c.MensagemGraduacao).Must(MinimoGraduacao).WithMessage("graduacao_requerida");
            RuleFor(c => c.DataInicio).NotNull().WithMessage("data_inicio_requerida");
            RuleFor(c => c.DataFim).NotNull().WithMessage("data_fim_requerida");
            RuleFor(c => c.DataFim).GreaterThan(c => c.DataInicio).WithMessage("data_inicial_final");
        }

        private bool MinimoGraduacao(ICollection<MensagemGraduacaoViewModel> graduacoes)
        {
            if (graduacoes == null || graduacoes.Count == 0) return false;
            return true;
        }
    }
    public class MensagemViewModelUpdateValidator : AbstractValidator<MensagemViewModel>
    {
        public MensagemViewModelUpdateValidator()
        {
            RuleFor(c => c.IdMensagem).GreaterThan(0).WithMessage("idmensagem_requerida");
            RuleFor(c => c.Titulo).NotEmpty().WithMessage("titulo_requerido");
            RuleFor(c => c.MensagemGraduacao).Must(MinimoGraduacao).WithMessage("graduacao_requerida");
            RuleFor(c => c.DataInicio).NotNull().WithMessage("data_inicio_requerida");
            RuleFor(c => c.DataFim).NotNull().WithMessage("data_fim_requerida");
            RuleFor(c => c.DataFim).GreaterThan(c => c.DataInicio).WithMessage("valor_inicial_final");
        }

        private bool MinimoGraduacao(ICollection<MensagemGraduacaoViewModel> graduacoes)
        {
            if (graduacoes == null || graduacoes.Count == 0) return false;
            return true;
        }
    }
}
