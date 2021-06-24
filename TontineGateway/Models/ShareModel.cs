using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TontineGateway.Models
{
   
    public class ShareModel
    {
        public Guid Id { get; set; }
        public string TontineId { get; set; }
        public int AmountTaked { get; set; }
        public string EffectiveDate { get; set; }
        public int Index { get; set; }
        public bool IsClosed { get; set; }
        public Takebymember TakeByMember { get; set; }
        public string TakedAt { get; set; }
    }

    public class Takebymember
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }


}
