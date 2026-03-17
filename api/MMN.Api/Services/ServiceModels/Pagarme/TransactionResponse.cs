using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Util
{
    public class TransactionResponse
    {
        public long Id { get; set; }
        public TransactionStatus Status;
        public DateTime Date_created { get; set; }
        public DateTime? Date_updated { get; set; }
    }

    public enum TransactionStatus
    {
        Waiting_Payment,
        Paid
    }
}
