using System.ComponentModel;

namespace MMN.Util.Enum
{
    public enum TipoContatoEnum
    {
        [Description("Contato")]
        Contato = 1,
        [Description("Cashback não pago")]
        CashbackNaoPago = 2,
        [Description("Cancelamento do parcelamento")]
        CancelamentoParcelas = 3,
        [Description("Reabertura do parcelamento")]
        ReaberturaParcelas = 4,
    }
}
