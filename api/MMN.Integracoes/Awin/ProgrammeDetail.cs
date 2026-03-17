using System.Collections.Generic;

namespace MMN.Integracoes.Awin
{
    public class ProgrammeDetail
    {
        public ProgrammeInfo programmeInfo { get; set; }
        public Kpi kpi { get; set; }
        public List<CommissionRange> commissionRange { get; set; }
    }
}