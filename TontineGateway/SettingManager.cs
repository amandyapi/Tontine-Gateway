using IBP.SDKGatewayLibrary;
using System;

namespace TontineGateway
{
    public class SettingManager : SettingManagerBase
    {
        public SettingManager()
        {
        }   
        public override string[] GetFilterContextKeys()
        {
            var strArrays = new string[] {
                "Filter.Roll.Serial",
                "Filter.Roll.Total",
                "Filter.Roll.Comission",
                "Filter.Roll.Value",
                "Filter.Roll.Count",
                "Filter.Roll.CreationTime",
                "Filter.Roll.StartTime",
                "Filter.Roll.EndTime",
                "Filter.Roll",
                "Filter.Roll.Recipient.DealNumber",
                "Filter.Roll.Recipient.Email"
            };
            return strArrays;
        }

        public override string[] GetPaymentContextKeys(Operation operation)
        {
            if (!Enum.IsDefined(typeof(Operation), operation))
            {
                return new string[0];
            }

            return new string[]
            {
                 // Payment data
                 "PaymentContext.Payment.Account", // Contribution = 1, MembershipFee = 3 //ParticipantCode?
                 "PaymentContext.Payment.Value", //amount to pay
                 "PaymentContext.Payment.Serial",
                 "PaymentContext.Payment.InputDate",
                 "PaymentContext.Payment.Number",
                 "PaymentType",//accountType -- Pay a tontine bill. *** accountType-Contribution = 1 accountType-Solidarity = 2 accountType-MembershipFee = 3 accountType-Sequestre = 4
                 "Action", //1 = Billing 2 = Refund,
                 "OrderCode",//Refund Order Code
                 //// Member Data
                  //"Member.MemberCode",
                  //"Member.MemberId",
                  //"Member.SelectedTontineId",
                  //"Member.FullName",
                  //"Member.Tontines",
                  //"SelectedPaymentType",

            };
        }

        public override string[] GetSettingsKey()
        {
              return new string[]
              {
                    "TontineApiBaseUrl", // "https://tontineapi.azurewebsites.net",
                    "IdToken",//  "OTNiMWU1NDMtOTIxYy00MmNlLWIwMzMtMTgzMDEwMDBiMmNiOjl4amxlR0ZOI0RSbDUpJVk2cGp5TkA="
              };
        }

        public override void InitSettingManadger(string initString)
        {
        }

        public override string[] SaveSettingKey()
        {
            return new string[0];
        }

        public override string[] SetPaymentContextKeys(Operation operation)
        {
            switch (operation)
            {
                case Operation.Process:
                    return null;
                case Operation.CheckAccount:
                    return new string[]
                    {
                       "ParticipantId",
                       "MemberId",
                       "ParticipantFullName",
                       "TontineName",
                       "TontineId",
                       "MembershipTicketFee",
                       "MemberShipFeesBillingFeesToPay",
                       "MemberShipFeesBillingFeesPaid",
                       "ContributionsAmount",
                       "ShareNumber",
                       "ContributionBillingShareIndex",
                       "ContributionBillingShareIndex1",
                       "ContributionBillingShareIndex2",
                       "ContributionBillingShareIndex3",
                       "ContributionBillingFeesToPay",
                       "ContributionBillingFeesToPay1",
                       "ContributionBillingFeesToPay2",
                       "ContributionBillingFeesToPay3",
                       "ContributionBillingFeesPaid",
                       "ContributionBillingFeesPaid1",
                       "ContributionBillingFeesPaid2",
                       "ContributionBillingFeesPaid3",
                       "ContributionBillingFeesToRefund",
                       "AmountToReceive"
                    };
                case Operation.CheckProcessStatus:
                    return null;
                case Operation.RecallPayment:
                    return null;
            }
            return null;
        }
    }
}
