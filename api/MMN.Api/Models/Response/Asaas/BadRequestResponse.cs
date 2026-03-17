using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MMN.Api.Models.Response.Asaas
{
    public partial class BadRequestResponse : BaseResponse
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }
    }    
}