using System.Collections.Generic;

namespace MMN.Integracoes.Awin
{
    public class ProgrammeInfo
    {
        public string clickThroughUrl { get; set; }
        public string logoUrl { get; set; }
        public string displayUrl { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string currencyCode { get; set; }
        public PrimaryRegion primaryRegion { get; set; }
        public List<ValidDomain> validDomains { get; set; }
    }
}