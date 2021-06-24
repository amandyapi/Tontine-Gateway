using System;

namespace TontineGateway.Models
{
    public class BillToParticipantAccountModel
    {
        public string participantId { get; set; }
        public string skpaymentId { get; set; }
        public int amount { get; set; }
        public int? ShareNumber { get; set; }
    }
}
