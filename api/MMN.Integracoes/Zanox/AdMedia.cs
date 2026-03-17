using System.Collections.Generic;

namespace MMN.Integracoes.Zanox
{
    public class AdMedia
    {
        public AdMediaItems admediumItems { get; set; }
    }

    public class AdMediaItems
    {
        public AdMediumItem admediumItem { get; set; }
    }

    public class AdMediumItem
    {
        public TrackingLinks trackingLinks { get; set; }
    }

    public class TrackingLinks
    {
        public ICollection<TrackingLink> trackingLink { get; set; }
    }

    public class TrackingLink
    {
        public string ppv { get; set; }
        public string ppc { get; set; }
    }
}
