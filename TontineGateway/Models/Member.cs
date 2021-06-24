using System;

namespace TontineGateway.Models
{

    public class Member
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phoneNumber { get; set; }
        public string birthDate { get; set; }
        public bool isActive { get; set; }
        public string effectiveDate { get; set; }
        public string id { get; set; }
        public string memberCode { get; set; }
        public string participantId { get; set; }
        public Tontine tontine { get; set; }
    }

    public class Tontine
    {
        public string id { get; set; }
        public Name[] name { get; set; }
        public string code { get; set; }
        public int membershipTicketFee { get; set; }
        public Description[] description { get; set; }
        public int contributionsAmount { get; set; }
        public Frequency frequency { get; set; }
        public Type type { get; set; }
        public bool isActive { get; set; }
        public string effectiveDate { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
        public int part { get; set; }
        public Membershipfeesbilling memberShipFeesBilling { get; set; }
        public Contributionbilling[] contributionBilling { get; set; }
    }

    public class Frequency
    {
        public int value { get; set; }
        public string displayName { get; set; }
    }

    public class Type
    {
        public int value { get; set; }
        public string displayName { get; set; }
    }

    public class Membershipfeesbilling
    {
        public int feesToPay { get; set; }
        public int feesPaid { get; set; }
    }

    public class Name
    {
        public string cultureName { get; set; }
        public string text { get; set; }
    }

    public class Description
    {
        public string cultureName { get; set; }
        public string text { get; set; }
    }

    public class Contributionbilling
    {
        public int shareIndex { get; set; }
        public string shareId { get; set; }
        public int feesToPay { get; set; }
        public int feesPaid { get; set; }
    }
}
