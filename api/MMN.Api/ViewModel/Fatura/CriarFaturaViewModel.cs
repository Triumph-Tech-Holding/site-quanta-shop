using MMN.Util.Enum;
using System;

namespace MMN.Api.ViewModel.Fatura
{
    /// <summary>
    /// Parâmetros para criar uma nova fatura
    /// </summary>
    public class CriarFaturaViewModel
    {
        /// <summary>
        /// Pedido de venda mais recente a ser incluído.
        /// </summary>
        public DateTime AteData { get; set; }
        /// <summary>
        /// Tipo do pagamento para a fatura.
        /// </summary>
        public EnumTipoPagamento TipoPagamento { get; set; }
    }
}