namespace MMN.Integracoes.Awin
{
    public class Programme
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayUrl { get; set; }
        public string clickThroughUrl { get; set; }
        public string logoUrl { get; set; }
        public PrimaryRegion primaryRegion { get; set; }
        public string currencyCode { get; set; }
        public ProgrammeDetail Detail { get; set; }
    }
}