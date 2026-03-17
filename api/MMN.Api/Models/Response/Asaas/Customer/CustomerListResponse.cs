using System.Collections.Generic;
using Newtonsoft.Json;

namespace MMN.Api.Models.Response.Asaas.Customer
{
    public class CustomerListResponse
    {
        [JsonProperty("data")]
        public List<CustomerResponse> Data { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
