namespace MMN.Integracoes.Awin
{
    public class ReportAggregatedByCreative
    {
        public int advertiserId { get; set; }
        public string advertiserName { get; set; }
        public int publisherId { get; set; }
        public string publisherName { get; set; }
        public string region { get; set; }
        public string currency { get; set; }
        public int impressions { get; set; }
        public int clicks { get; set; }
        public int creativeId { get; set; }
        public string creativeName { get; set; }
        public string tagName { get; set; }
        public int pendingNo { get; set; }
        public int pendingValue { get; set; }
        public int pendingComm { get; set; }
        public int confirmedNo { get; set; }
        public int confirmedValue { get; set; }
        public int confirmedComm { get; set; }
        public int bonusNo { get; set; }
        public int bonusValue { get; set; }
        public int bonusComm { get; set; }
        public int totalNo { get; set; }
        public int totalValue { get; set; }
        public int totalComm { get; set; }
        public int declinedNo { get; set; }
        public int declinedValue { get; set; }
        public int declinedComm { get; set; }

    }
}