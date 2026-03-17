using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MMN.Api.Models.Response.Asaas.Payment;

namespace MMN.Api.Models.Request.Asaas
{
    public class EventRequest
    {
        public string Id { get; set; }
        public string Event { get; set; }
        public PaymentResponse Payment { get; set; }
    }
}