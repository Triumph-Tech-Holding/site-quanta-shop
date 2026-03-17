using System.ComponentModel;

namespace MMN.Dominio.Enum
{
    public enum StatusPedido
    {
        [Description("Cancelado")]
        Cancelado = 1,
        [Description("Aguardando pagamento")]
        AguardandoPagamento = 2,
        [Description("Em pagamento")]
        EmPagamento = 3,
        [Description("Pago")]
        Pago = 4,
        [Description("Aguardando cashback")]
        AguardandoCashback = 5,
        [Description("Processado")]
        Processado = 6,
        [Description("Aguardando fatura credenciado")]
        AguardandoFaturaCredenciado = 7,
        [Description("Pago pela Quanta Shop")]
        PagoBigCash = 8,
    }
}
