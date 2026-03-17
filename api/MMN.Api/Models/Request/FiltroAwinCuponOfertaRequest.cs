using MMN.Api.Models.Response;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MMN.Api.Models.Request;

public class FiltroAwinCuponOfertaRequest
{
    [JsonProperty("advertiserIds")]
    public List<int> AdvertiserIds { get; set; }
    
    [JsonProperty("exclusiveOnly")]
    public bool ExclusiveOnly { get; set; }
    
    [JsonProperty("membership")]
    public string Membership { get; set; }
    
    [JsonProperty("regionCodes")]
    public List<string> RegionCodes { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("updatedSince")]
    public string UpdatedSince { get; set; }
    
    [JsonProperty("pagination")]
    public Pagination Pagination { get; set; }
}

public class Pagination
{
    [JsonProperty("page")]
    public int Page { get; set; }
    
    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    public Pagination(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}