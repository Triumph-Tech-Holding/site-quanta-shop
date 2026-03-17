using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace MMN.Api.Models.Response;

public class Advertiser
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("joined")]
    public bool Joined { get; set; }
}

public class Data
{
    [JsonProperty("promotionId")]
    public int PromotionId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("advertiser")]
    public Advertiser Advertiser { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("terms")]
    public string Terms { get; set; }

    [JsonProperty("startDate")]
    public DateTime StartDate { get; set; }

    [JsonProperty("endDate")]
    public DateTime EndDate { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("urlTracking")]
    public string UrlTracking { get; set; }

    [JsonProperty("dateAdded")]
    public DateTime DateAdded { get; set; }

    [JsonProperty("campaign")]
    public string Campaign { get; set; }

    [JsonProperty("regions")]
    public Regions Regions { get; set; }

    [JsonProperty("voucher")]
    public Voucher Voucher { get; set; }
}

public class Pagination
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}

public class Regions
{
    [JsonProperty("all")]
    public bool All { get; set; }
}

public class FiltroAwinCuponOfertaResponse
{
    [JsonProperty("data")]
    public List<Data> Data { get; set; }

    [JsonProperty("pagination")]
    public Pagination Pagination { get; set; }
}

public class Voucher
{
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("exclusive")]
    public bool Exclusive { get; set; }

    [JsonProperty("attributable")]
    public bool Attributable { get; set; }
}

