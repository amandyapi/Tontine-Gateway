using System;

namespace TontineGateway.Models
{
    public class BillToMemberAccountModel
    {
        public string memberId { get; set; }       
        public string skpaymentId { get; set; }
        public int amount { get; set; }
    }
}
