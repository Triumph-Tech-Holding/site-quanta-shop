using System.Collections.Generic;

namespace MMN.Api.Models.Response;

public class GetTransactionsResponse
{
    public IEnumerable<object> Transactions { get; set; }
    public int TotalCount { get; set; }
}