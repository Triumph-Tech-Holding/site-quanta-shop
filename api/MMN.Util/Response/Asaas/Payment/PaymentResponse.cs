using Newtonsoft.Json;
using System;

namespace MMN.Util.Models.Response.Asaas.Payment
{
    public partial class PaymentResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("subscription")]
        public object Subscription { get; set; }

        [JsonProperty("installment")]
        public object Installment { get; set; }

        [JsonProperty("checkoutSession")]
        public Guid? CheckoutSession { get; set; }

        [JsonProperty("paymentLink")]
        public object PaymentLink { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("netValue")]
        public double NetValue { get; set; }

        [JsonProperty("originalValue")]
        public object OriginalValue { get; set; }

        [JsonProperty("interestValue")]
        public object InterestValue { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("billingType")]
        public string BillingType { get; set; }

        [JsonProperty("creditCard")]
        public CreditCard CreditCard { get; set; }

        [JsonProperty("canBePaidAfterDueDate")]
        public bool CanBePaidAfterDueDate { get; set; }

        [JsonProperty("pixTransaction")]
        public object PixTransaction { get; set; }

        [JsonProperty("pixQrCodeId")]
        public object PixQrCodeId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset DueDate { get; set; }

        [JsonProperty("originalDueDate")]
        public DateTimeOffset OriginalDueDate { get; set; }

        [JsonProperty("paymentDate")]
        public object PaymentDate { get; set; }

        [JsonProperty("clientPaymentDate")]
        public object ClientPaymentDate { get; set; }

        [JsonProperty("installmentNumber")]
        public object InstallmentNumber { get; set; }

        [JsonProperty("invoiceUrl")]
        public string InvoiceUrl { get; set; }

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("externalReference")]
        public string ExternalReference { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("anticipated")]
        public bool Anticipated { get; set; }

        [JsonProperty("anticipable")]
        public bool Anticipable { get; set; }

        [JsonProperty("creditDate")]
        public DateTimeOffset? CreditDate { get; set; }

        [JsonProperty("estimatedCreditDate")]
        public DateTimeOffset? EstimatedCreditDate { get; set; }

        [JsonProperty("transactionReceiptUrl")]
        public object TransactionReceiptUrl { get; set; }

        [JsonProperty("nossoNumero")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long NossoNumero { get; set; }

        [JsonProperty("bankSlipUrl")]
        public string BankSlipUrl { get; set; }

        [JsonProperty("discount")]
        public Discount Discount { get; set; }

        [JsonProperty("fine")]
        public Fine Fine { get; set; }

        [JsonProperty("interest")]
        public Fine Interest { get; set; }

        [JsonProperty("split")]
        public Split[] Split { get; set; }

        [JsonProperty("postalService")]
        public bool PostalService { get; set; }

        [JsonProperty("daysAfterDueDateToRegistrationCancellation")]
        public object DaysAfterDueDateToRegistrationCancellation { get; set; }

        [JsonProperty("chargeback")]
        public Chargeback Chargeback { get; set; }

        [JsonProperty("escrow")]
        public Escrow Escrow { get; set; }

        [JsonProperty("refunds")]
        public Refund[] Refunds { get; set; }
    }

    public partial class Chargeback
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("payment")]
        public string Payment { get; set; }

        [JsonProperty("installment")]
        public string Installment { get; set; }

        [JsonProperty("customerAccount")]
        public string CustomerAccount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("disputeStartDate")]
        public DateTimeOffset DisputeStartDate { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("paymentDate")]
        public DateTimeOffset PaymentDate { get; set; }

        [JsonProperty("creditCard")]
        public ChargebackCreditCard CreditCard { get; set; }

        [JsonProperty("disputeStatus")]
        public string DisputeStatus { get; set; }

        [JsonProperty("deadlineToSendDisputeDocuments")]
        public DateTimeOffset DeadlineToSendDisputeDocuments { get; set; }
    }

    public partial class ChargebackCreditCard
    {
        [JsonProperty("number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Number { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }
    }

    public partial class CreditCard
    {
        [JsonProperty("creditCardNumber")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CreditCardNumber { get; set; }

        [JsonProperty("creditCardBrand")]
        public string CreditCardBrand { get; set; }

        [JsonProperty("creditCardToken")]
        public object CreditCardToken { get; set; }
    }

    public partial class Discount
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("dueDateLimitDays")]
        public long DueDateLimitDays { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Escrow
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonProperty("finishDate")]
        public DateTimeOffset FinishDate { get; set; }

        [JsonProperty("finishReason")]
        public string FinishReason { get; set; }
    }

    public partial class Fine
    {
        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public partial class Refund
    {
        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("endToEndIdentifier")]
        public object EndToEndIdentifier { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTimeOffset EffectiveDate { get; set; }

        [JsonProperty("transactionReceiptUrl")]
        public object TransactionReceiptUrl { get; set; }

        [JsonProperty("refundedSplits")]
        public RefundedSplit[] RefundedSplits { get; set; }
    }

    public partial class RefundedSplit
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }
    }

    public partial class Split
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("walletId")]
        public Guid? WalletId { get; set; }

        [JsonProperty("fixedValue")]
        public double FixedValue { get; set; }

        [JsonProperty("percentualValue")]
        public object PercentualValue { get; set; }

        [JsonProperty("totalValue")]
        public double TotalValue { get; set; }

        [JsonProperty("cancellationReason")]
        public string CancellationReason { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("externalReference")]
        public object ExternalReference { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }
    }

}
