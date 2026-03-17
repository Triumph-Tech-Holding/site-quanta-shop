namespace MMN.Integracoes.Awin
{
    public class CommissionGroup
    {
        public int groupId { get; set; }
        public string groupCode { get; set; }
        public string groupName { get; set; }
        public string type { get; set; }
        public int percentage { get; set; }
        public int? amount { get; set; }
        public string currency { get; set; }
    }
}