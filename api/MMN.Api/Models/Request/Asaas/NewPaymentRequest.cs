using System;
using System.ComponentModel;
using MMN.Util.Extensions;
using Newtonsoft.Json;

namespace MMN.Api.Models.Request.Asaas
{
    public enum PaymentType
    {
        [Description("Boleto Bancário")]
        BOLETO,
        [Description("PIX")]
        PIX,
        [Description("Cartão de Crédito")]
        CREDIT_CARD
    }

    public class NewPaymentRequest
    {
        [JsonProperty("billingType")]
        public string BillingType { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset DueDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("daysAfterDueDateToRegistrationCancellation")]
        public long DaysAfterDueDateToRegistrationCancellation { get; set; } = 15;

        [JsonProperty("externalReference")]

        public string ExternalReference { get; set; }

        public NewPaymentRequest(PaymentType type)
        {
            BillingType = type.GetEnumValue();
        }
    }
}