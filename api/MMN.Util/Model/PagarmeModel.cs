using System;

namespace MMN.Util.Model
{
    public class PagarmeModel
    {
        //public string _object { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        //public object refuse_reason { get; set; }
        //public string status_reason { get; set; }
        //public object acquirer_response_code { get; set; }
        //public string acquirer_name { get; set; }
        //public string acquirer_id { get; set; }
        //public object authorization_code { get; set; }
        //public object soft_descriptor { get; set; }
        //public int tid { get; set; }
        //public int nsu { get; set; }
        //public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        //public int amount { get; set; }
        //public int authorized_amount { get; set; }
        //public int paid_amount { get; set; }
        //public int refunded_amount { get; set; }
        //public int installments { get; set; }
        //public int id { get; set; }
        //public int cost { get; set; }
        //public object card_holder_name { get; set; }
        //public object card_last_digits { get; set; }
        //public object card_first_digits { get; set; }
        //public object card_brand { get; set; }
        //public object card_pin_mode { get; set; }
        //public bool card_magstripe_fallback { get; set; }
        //public bool cvm_pin { get; set; }
        //public object postback_url { get; set; }
        //public string payment_method { get; set; }
        //public string capture_method { get; set; }
        //public object antifraud_score { get; set; }
        //public string boleto_url { get; set; }
        public string boleto_barcode { get; set; }
        //public DateTime boleto_expiration_date { get; set; }
        //public string referer { get; set; }
        //public string ip { get; set; }
        //public object subscription_id { get; set; }
        //public object phone { get; set; }
        //public object address { get; set; }
        //public PagarmeCustomer customer { get; set; }
        //public PagarmeBilling billing { get; set; }
        //public object shipping { get; set; }
        //public PagarmeItem[] items { get; set; }
        //public object card { get; set; }
        //public object split_rules { get; set; }
        //public PagarmeAntifraud_Metadata antifraud_metadata { get; set; }
        //public object reference_key { get; set; }
        //public object device { get; set; }
        //public object local_transaction_id { get; set; }
        //public object local_time { get; set; }
        //public bool fraud_covered { get; set; }
        //public object fraud_reimbursed { get; set; }
        //public object order_id { get; set; }
        //public string risk_level { get; set; }
        //public object receipt_url { get; set; }
        //public object payment { get; set; }
        //public object addition { get; set; }
        //public object discount { get; set; }
        //public object private_label { get; set; }
        //public object pix_qr_code { get; set; }
        //public object pix_expiration_date { get; set; }
        //public PagarmeMetadata metadata { get; set; }
    }

    //public class PagarmeCustomer
    //{
    //    public string _object { get; set; }
    //    public int id { get; set; }
    //    public string external_id { get; set; }
    //    public string type { get; set; }
    //    public string country { get; set; }
    //    public object document_number { get; set; }
    //    public string document_type { get; set; }
    //    public string name { get; set; }
    //    public string email { get; set; }
    //    public string[] phone_numbers { get; set; }
    //    public object born_at { get; set; }
    //    public object birthday { get; set; }
    //    public object gender { get; set; }
    //    public DateTime date_created { get; set; }
    //    public PagarmeDocument[] documents { get; set; }
    //}

    //public class PagarmeDocument
    //{
    //    public string _object { get; set; }
    //    public string id { get; set; }
    //    public string type { get; set; }
    //    public string number { get; set; }
    //}

    //public class PagarmeBilling
    //{
    //    public string _object { get; set; }
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public PagarmeAddress address { get; set; }
    //}

    //public class PagarmeAddress
    //{
    //    public string _object { get; set; }
    //    public string street { get; set; }
    //    public object complementary { get; set; }
    //    public string street_number { get; set; }
    //    public string neighborhood { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string zipcode { get; set; }
    //    public string country { get; set; }
    //    public int id { get; set; }
    //}

    //public class PagarmeAntifraud_Metadata
    //{
    //}

    //public class PagarmeMetadata
    //{
    //}

    //public class PagarmeItem
    //{
    //    public string _object { get; set; }
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public int unit_price { get; set; }
    //    public int quantity { get; set; }
    //    public object category { get; set; }
    //    public bool tangible { get; set; }
    //    public object venue { get; set; }
    //    public object date { get; set; }
    //}
}