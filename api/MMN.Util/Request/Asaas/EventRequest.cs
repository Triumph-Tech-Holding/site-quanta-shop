using MMN.Util.Models.Response.Asaas.Payment;

namespace MMN.Util.Models.Request.Asaas
{
    public class EventRequest
    {
        public string Id { get; set; }
        public string Event { get; set; }
        public PaymentResponse Payment { get; set; }
    }
}