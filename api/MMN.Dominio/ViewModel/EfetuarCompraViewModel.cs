using FluentValidation;
using MMN.Util.Enum;
using System;

namespace MMN.Dominio.ViewModel
{
    public class EfetuarCompraViewModel
    {
        public Guid? IdComerciante { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public EnumTipoPagamento TipoPagamento { get; set; }
    }

    public class EfetuarCompraViewModelValidator : AbstractValidator<EfetuarCompraViewModel>
    {
        public EfetuarCompraViewModelValidator()
        {
            RuleFor(e => e.TipoPagamento).IsInEnum().WithMessage("pagamento_tipo_requirido")
                .Must(TipoPagamentoValido).WithMessage("pagamento_tipo_invalido");
            RuleFor(e => e.IdComerciante).NotNull().WithMessage("comerciante_requerido");
            RuleFor(e => e.Descricao).NotEmpty().WithMessage("compra_descricao_requerida");
            RuleFor(e => e.Valor).NotNull().WithMessage("compra_valor_requerido").GreaterThan(0).WithMessage("compra_valor_minimo");
        }

        private bool TipoPagamentoValido(EnumTipoPagamento tipo)
        {
            return (tipo == EnumTipoPagamento.Dinheiro);
        }
    }
}