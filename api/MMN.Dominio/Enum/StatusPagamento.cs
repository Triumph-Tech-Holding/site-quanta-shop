namespace MMN.Dominio.Enum
{
    public enum StatusPagamento
    {
        Cancelado = 1,
        GerandoPagamentoExterno = 2,
        AguardandoPagamento = 3,
        PagoAguardandoLancamento = 4,
        PagoProcessado = 5,
        Expirado = 6
    }
}
