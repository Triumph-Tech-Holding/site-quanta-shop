using System.Collections.Generic;
using Newtonsoft.Json;

namespace MMN.Api.Models.Response.Asaas.Payment
{
    public class PaymentListResponse
    {
        [JsonProperty("data")]
        public List<PaymentResponse> Data { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
