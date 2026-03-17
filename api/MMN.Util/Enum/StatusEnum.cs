using System.ComponentModel;

namespace MMN.Util.Enum
{
    public enum StatusTransacaoEnum
    {
        [Description("Em processamento")]
        EmProcessamento = 1,
        [Description("Finalizada")]
        Finalizada = 2,
        [Description("Cancelada")]
        Cancelada = 3,
        [Description("Em aprovação")]
        EmAprovacao = 4,
        [Description("Recusado")]
        Recusado = 5,
        [Description("Aprovado")]
        Aprovado = 6,
        [Description("Aguardando pagamento de fatura")]
        AguardandoPagamentoFatura = 7,
        [Description("Pago pela Quanta Shop")]
        PagoBigCash = 8,
        [Description("Assinatura Ativa")]
        AssinaturaAtiva = 13,

    }
}
