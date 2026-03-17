using System.Collections.Generic;

namespace MMN.Integracoes.Awin
{
    public class Commission
    {
        public int advertiser { get; set; }
        public int publisher { get; set; }
        public List<CommissionGroup> commissionGroups { get; set; }
    }
}