using System.Collections.Generic;
using System;

namespace MMN.Api.Models.Request;

public class GetTransactionsSearchParameters
{
    public long? IdTransacao { get; set; }
    public string Login { get; set; }
    public string Nome { get; set; }
    public string Estabelecimento { get; set; }
    public List<string> Tipos { get; set; } = new List<string>();
    public List<string> Status { get; set; } = new List<string>();
    public string Descricao { get; set; }
    public decimal? MinValorCompra { get; set; }
    public decimal? MaxValorCompra { get; set; }
    public decimal? MinValorCashback { get; set; }
    public decimal? MaxValorCashback { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
