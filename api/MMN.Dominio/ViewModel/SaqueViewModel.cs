using FluentValidation;
using System;

namespace MMN.Dominio.ViewModel
{
    public class SaqueViewModel
    {
        public int IdSaque { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public decimal Valor { get; set; }
        public string EnderecoBTC { get; set; }
        public bool Processado { get; set; }
        public DateTime? DataProcessado { get; set; }
        public decimal TaxaSaque { get; set; }
        public decimal? Cotacao { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public int IdStatus { get; set; }
        public StatusViewModel Status { get; set; }
        public int IdTipo { get; set; }
        public TipoViewModel Tipo { get; set; }
        public long IdTransacao { get; set; }
        public TransacaoViewModel Transacao { get; set; }
        public string UrlTransacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public string Aprovador { get; set; }
        public string Historico { get; set; }
        public int IdUsuarioBanco { get; set; }
        public UsuarioBancoViewModel UsuarioBancoViewModel { get; set; }
    }

    public class SaqueViewModelValidator : AbstractValidator<SaqueViewModel>
    {
        public SaqueViewModelValidator()
        {
            RuleFor(s => s.Valor).GreaterThanOrEqualTo(100).WithMessage("saque_valor_minimo");
            RuleFor(s => s.IdUsuarioBanco).NotNull().WithMessage("conta_requerida");
        }
    }
}