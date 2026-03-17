using System.ComponentModel;

namespace MMN.Util.Enum
{
    public enum EnumTipoTransacao
    {
        [Description("LM")]
        LançamentoManual = 4,

        [Description("PB")]
        PagamentoBinario = 6,

        [Description("AT")]
        PagamentoAutomaticoTrader = 7,

        [Description("AU")]
        PagamentoAtivacaoUsuario = 8,

        [Description("CPR")]
        Compra = 9,

        [Description("CP")]
        CompraProduto = 10,

        [Description("CV")]
        CompraVoucher = 11,

        [Description("CPCT")]
        CompraPacote = 12,

        [Description("CS")]
        CompraServiço = 13,

        [Description("CCT")]
        CompraConvite = 14,

        [Description("RT")]
        RecebimentoTransferencia = 15,

        [Description("TSIS")]
        TaxaSistema = 16,

        [Description("TSQ")]
        TaxaSaque = 17,

        [Description("EST")]
        Estorno = 18,

        [Description("EB")]
        EstornoBonus = 19,

        [Description("ET")]
        EstornoTrader = 20,

        [Description("EAU")]
        EstornoAtivacaoUsuario = 21,

        [Description("EPAG")]
        EstornoPagamento = 22,

        [Description("IR")]
        ImpostoRenda = 23,

        [Description("INSS")]
        INSS = 24,

        [Description("TCC")]
        TransferenciaContaCorrente = 25,

        [Description("ASNT")]
        Assinatura = 57,

        [Description("DCPLJ")]
        CashbackCredenciado = 52
    }

    public enum EnumStatusLancamento
    {
        [Description("Em Andamento")] AND = 29,
        [Description("Finalizado")] FNL = 30,
        [Description("Cancelado")] CNC = 31,
    }

    public enum EnumTipoSaldo
    {
        [Description("Cashback")] CHBK = 32,
        [Description("SaldoRede")] SLRD = 33
    }

    public enum EnumTipoPagamento
    {
        [Description("Pagar.me")]
        PGPAGARME = 9,
        [Description("Saldo")]
        Saldo = 10,
        [Description("Dinheiro")]
        Dinheiro = 11,
        [Description("Outro")]
        Outro = 12,
        [Description("Cartão de débito")]
        CartaoDebito = 13,
        [Description("Cartão de crédito")]
        CartaoCredito = 14,
        [Description("PIX")]
        Pix = 15,
        [Description("Boleto")]
        Boleto = 18,
        [Description("Não informado")]
        NaoInformado = 19,
		[Description("Asaas")]
		PGASAAS = 20,

	}

    public enum EnumArquivoProduto
    {
        [Description("PDF")] PDF = 39,
        [Description("Video")] MP = 40
    }
}
