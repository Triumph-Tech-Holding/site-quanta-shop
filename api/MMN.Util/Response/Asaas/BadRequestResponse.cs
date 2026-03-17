using Newtonsoft.Json;

namespace MMN.Util.Models.Response.Asaas
{
    public partial class BadRequestResponse : BaseResponse
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }
    }    
}