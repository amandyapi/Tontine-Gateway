using System;

namespace TontineGateway.Models
{
    public class BillingModel
    {
        public int AmountTotalPaid { get; set; }
        public int AmountToPaid { get; set; }
        public int? ShareNumber { get; set; }
        public int? AmountToRefund { get; set; }
    }
}
