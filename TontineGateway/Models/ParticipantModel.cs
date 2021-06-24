using System;

namespace TontineGateway.Models
{
    class ParticipantModel
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string EffectiveDate { get; set; }
        public Guid MemberId { get; set; }

        public TontineModel Tontine { get; set; }
        public string memberCode { get; set; }
        public string ParticipantCode { get; set; }
        public int Part { get; set; }
    }
}
