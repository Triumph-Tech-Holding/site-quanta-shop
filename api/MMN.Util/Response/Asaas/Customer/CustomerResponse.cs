using Newtonsoft.Json;
using System;

namespace MMN.Util.Models.Response.Asaas
{
    public partial class CustomerResponse : BaseResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("addressNumber")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long AddressNumber { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("city")]
        public long? City { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("cpfCnpj")]
        public string CpfCnpj { get; set; }

        [JsonProperty("personType")]
        public string PersonType { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("additionalEmails")]
        public string AdditionalEmails { get; set; }

        [JsonProperty("externalReference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ExternalReference { get; set; }

        [JsonProperty("notificationDisabled")]
        public bool NotificationDisabled { get; set; }

        [JsonProperty("observations")]
        public string Observations { get; set; }

        [JsonProperty("foreignCustomer")]
        public bool ForeignCustomer { get; set; }
    }    
}
