using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MMN.Dominio.WebHook.WebHookSubscription;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMN.Dominio.WebHook
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Account
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class AntifraudResponse
    {
        public string status { get; set; }
        public string score { get; set; }
        public string provider_name { get; set; }
    }

    public class BillingAddress
    {
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string zip_code { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string line_1 { get; set; }
        public string line_2 { get; set; }
    }

    public class Card
    {
        public string id { get; set; }
        public string first_six_digits { get; set; }
        public string last_four_digits { get; set; }
        public string brand { get; set; }
        public string holder_name { get; set; }
        public int exp_month { get; set; }
        public int exp_year { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public BillingAddress billing_address { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string code { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }
        public string type { get; set; }
        public bool delinquent { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Phones phones { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string code { get; set; }
        public string gateway_id { get; set; }
        public int amount { get; set; }
        public int paid_amount { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public string payment_method { get; set; }
        public DateTime due_at { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Invoice invoice { get; set; }
        public Customer customer { get; set; }
        public LastTransaction last_transaction { get; set; }
        public string recurrence_cycle { get; set; }
    }

    public class GatewayResponse
    {
        public string code { get; set; }
        public List<Error> errors { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
    }

    public class Invoice
    {
        public string id { get; set; }
        public string code { get; set; }
        public string url { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public string payment_method { get; set; }
        public DateTime due_at { get; set; }
        public DateTime created_at { get; set; }
        public string subscriptionId { get; set; }
        public Metadata metadata { get; set; }
    }

    public class LastTransaction
    {
        public string id { get; set; }
        public string transaction_type { get; set; }
        public string gateway_id { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public bool success { get; set; }
        public int installments { get; set; }
        public string statement_descriptor { get; set; }
        public string acquirer_name { get; set; }
        public string acquirer_tid { get; set; }
        public string acquirer_nsu { get; set; }
        public string acquirer_auth_code { get; set; }
        public string acquirer_message { get; set; }
        public string acquirer_return_code { get; set; }
        public string operation_type { get; set; }
        public Card card { get; set; }
        public string funding_source { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public GatewayResponse gateway_response { get; set; }
        public AntifraudResponse antifraud_response { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
    }

    public class MobilePhone
    {
        public string country_code { get; set; }
        public string number { get; set; }
        public string area_code { get; set; }
    }

    public class Phones
    {
        public MobilePhone mobile_phone { get; set; }
    }

    public class WebHookSubscription
    {
        public string id { get; set; }
        public Account account { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
        public Data data { get; set; }
    }

}