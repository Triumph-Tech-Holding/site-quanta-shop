using System;

namespace MMN.Integracoes.Awin
{
    public class CommisionRule
    {
        public int ruleId { get; set; }
        public int servicePartnerId { get; set; }
        public int publisherId { get; set; }
        public int servicePartnerShare { get; set; }
        public int publisherShare { get; set; }
        public string ruleTitle { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
    }
}