using FluentValidation;
using MMN.Util.Enum;
using System;

namespace MMN.Api.ViewModel.Cupom
{
    public class ResgatarCuponViewModel
    {
        public string Token { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime DataOperacao { get; set; }
    }

    public class ResgatarCuponOfflineViewModelValidator : AbstractValidator<ResgatarCuponViewModel>
    {
        public ResgatarCuponOfflineViewModelValidator()
        {
            RuleFor(e => e.Token).NotEmpty().WithMessage("token_venda_requerido");
        }

        private bool TipoPagamentoValido(EnumTipoPagamento tipo)
        {
            return (tipo == EnumTipoPagamento.Saldo || tipo == EnumTipoPagamento.Dinheiro);
        }
    }

    public class CupomCashbackViewModel
    {
        public long? IdPedido { get; set; }
        public string Token { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorCashback { get; set; }
        public decimal PercentualCashback { get; set; }
        public string Documento { get; set; }
        public DateTime DataCompra { get; set; }
        public string Descricao { get; set; }
        public int MeioPagamento { get; set; }
        public string LoginComerciante { get; set; }
        public string NomeComerciante { get; set; }
        public string LogoComerciante { get; set; }
        public int Status { get; set; }
        public string ChaveDeAcessoNF { get; set; }
    }
}
