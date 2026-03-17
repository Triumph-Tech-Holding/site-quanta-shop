using FluentValidation;
using MMN.Util.Enum;
using MMN.Util.Util;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace MMN.Api.ViewModel.Cupom
{
    public class CriarCupomViewModel
    {
        [Required]
        public string Token { get; set; }
        /// <summary>
        /// Valor total do pedidoem centavos de real.
        /// </summary>
        [Required]
        public decimal Valor { get; set; }
        /// <summary>
        /// Percentual (0-100). Ex.: 3,17% deve ser representado pelo número 3.17. 15% deve ser representado pelo número 15.
        /// </summary>        
        public decimal? PercentualCashback { get; set; }
        /// <summary>
        /// CPF ou CNPJ(apenas dígitos, sem ponto, traço, barra, etc)
        /// </summary>
        [Required]
        public string Documento { get; set; }
        /// <summary>
        /// Data e hora no formato ISO8601 com timezone UTC.
        /// </summary>
        [Required]
        public DateTime? DataVenda { get; set; }
        [Required]
        public string Descricao { get; set; }
        /// <summary>
        /// Código do meio de pagamento da venda, disponível no endpoint /api/venda/obterTiposPagamento
        /// </summary>
        [Required]
        public EnumTipoPagamento MeioPagamento { get; set; }
        /// <summary>
        /// Compra inserida pelo usuário, que deverá ser aprovada pelo comerciante
        /// </summary>        
        public bool CompraUsuario { get; set; } = false;
        public bool CompraUsuarioAprovada { get; set; } = false;
        public string ComprovanteCompra { get; set; }
        public Guid? IdComerciante { get; set; }
        public string UrlChaveDeAcessoNF { get; set; }
        public bool ChaveManual { get; set; }
    }

    public class CriarCuponViewModelValidator : AbstractValidator<CriarCupomViewModel>
    {
        public CriarCuponViewModelValidator()
        {
            //RuleFor(e => e.Token).NotEmpty().WithMessage("cupon_token_requerido")
            //    .Must(DigitoTokenVendaValidator.ValidarDigitoVerificador).WithMessage("cupon_token_digito_invalido");
            RuleFor(e => e.Valor).NotNull().WithMessage("compra_valor_requerido")
                .GreaterThan(1).WithMessage("venda_valor_minimo_1");
            RuleFor(c => c.Documento).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
            RuleFor(e => e.Descricao).NotNull().WithMessage("compra_data_requerida");
            RuleFor(e => e.Descricao).NotEmpty().WithMessage("compra_descricao_requerida");
        }
    }

    public class DigitoTokenVendaValidator : AbstractValidator<string>
    {
        public DigitoTokenVendaValidator()
        {
            RuleFor(e => e).NotEmpty().WithMessage("token_venda_requerido")
                .Must(ValidarDigitoVerificador).WithMessage("token_venda_digito_invalido");
        }

        public static bool ValidarDigitoVerificador(string token)
        {
            token = token.ToUpper();

            if (token.Length != 14)
            {
                return false;
            }

            var alphabet = "0123456789ABCDEFGHJKMNPQRTUVWXYZ";

            var dv = token[token.Length - 1];
            var raw_token = token.Substring(0, token.Length - 1);
            var ndv = raw_token.Select((e, i) => e * (raw_token.Length - i))
                .Aggregate((total, atual) => total + atual) % 27;

            var cdv = alphabet[ndv];

            return dv == cdv;
        }
    }
}
