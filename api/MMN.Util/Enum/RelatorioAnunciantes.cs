using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Util.Enum
{
    public enum EnumOrdenacaoAnuncitantes
    {
        Nome,
        Status,
        CashbackMin,
        CashbackMax,
        TipoCashback,
        Moeda
    }

    public enum EnumStatusAnunciante
    {
        Ativo,
        Inativo,
        Todos
    }

    public enum EnumConexaoAnunciante
    {
        Awin,
        Afilio
    }

    public enum EnumTipoCashbackAnunciante
    {
        Percentual,
        Valor
    }
}
