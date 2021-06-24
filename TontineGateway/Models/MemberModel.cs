using System;
using System.Collections.Generic;

namespace TontineGateway.Models
{
    public class MemberModel
    {
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public string MemberCode { get; set; }
        public TontineModel Tontine { get; set; }
        public bool IsActive { get; set; }
        public string EffectiveDate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }

    }

}
