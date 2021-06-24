using System;
using System.Collections.Generic;

namespace TontineGateway.Models
{
    public class TontineModel
    {        
        public Guid Id { get; set; }
        public List<MultilingualLabelModel> Name { get; set; }
        public string Code { get; set; }
        public int MembershipTicketFee { get; set; }
        public MemberShipFeesBillingModel MemberShipFeesBilling { get; set; }
        public List<ContributionBillingModel> ContributionBilling { get; set; }
        public List<MultilingualLabelModel> Description { get; set; }
        public int ContributionsAmount { get; set; }
        public EnumModel Frequency { get; set; }
        public EnumModel Type { get; set; }
        public bool IsActive { get; set; }
        public string EffectiveDate { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
    }


    public class MemberShipFeesBillingModel
    {
        public int FeesToPay { get; set; }
        public int feesPaid { get; set; }
    }

    public class ContributionBillingModel
    {
        public int FeesToPay { get; set; }
        public int FeesPaid { get; set; }
        public int ShareIndex { get; set; }
        public string ShareId { get; set; }
    }
}
