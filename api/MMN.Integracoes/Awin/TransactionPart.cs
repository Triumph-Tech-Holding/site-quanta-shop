namespace MMN.Integracoes.Awin
{
    public class TransactionPart
    {
        public int commissionGroupId { get; set; }
        public double amount { get; set; }
        public double commissionAmount { get; set; }
        public string commissionGroupCode { get; set; }
        public string commissionGroupName { get; set; }

    }
}