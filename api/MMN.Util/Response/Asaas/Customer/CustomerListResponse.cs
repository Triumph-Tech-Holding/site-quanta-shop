using Newtonsoft.Json;
using System.Collections.Generic;

namespace MMN.Util.Models.Response.Asaas.Customer
{
    public class CustomerListResponse
    {
        [JsonProperty("data")]
        public List<CustomerResponse> Data { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
