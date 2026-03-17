using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMN.Api.Models.Request.Asaas.Customer
{
    public class CustomerRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cpfCnpj")]
        public string Document { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("addressNumber")]
        public string AddressNumber { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
    }
}