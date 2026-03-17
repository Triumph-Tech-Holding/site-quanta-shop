using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class TrackingCategories
    {
        public TrackingCategoryItem trackingCategoryItem { get; set; }
    }

    public class TrackingCategoryItem
    {
        public ICollection<trackingCategoryItem> trackingCategoryItem { get; set; }
    }

    public class trackingCategoryItem
    {
        [JsonPropertyName("@id")]
        public string id { get; set; }
        public decimal? salePercent { get; set; }
        public decimal? saleFixed { get; set; }
        public decimal? leadFixed { get; set; }
        public object description { get; set; }
    }

    public class TrackingCategoriesObject
    {
        public TrackingCategoryItemObject trackingCategoryItem { get; set; }
    }

    public class TrackingCategoryItemObject
    {
        public trackingCategoryItem trackingCategoryItem { get; set; }
    }
}
