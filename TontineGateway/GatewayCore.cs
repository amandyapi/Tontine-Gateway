using IBP.SDKGatewayLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TontineGateway.Enums;
using TontineGateway.Models;

namespace TontineGateway
{
    public class GatewayCore : GatewayCoreBase
    {
        private string url;
        private string _tontineApiBaseUrl;// = "https://tontineapi.azurewebsites.net";
        private string _idToken; // "OTNiMWU1NDMtOTIxYy00MmNlLWIwMzMtMTgzMDEwMDBiMmNiOjl4amxlR0ZOI0RSbDUpJVk2cGp5TkA="
        private List<string> _traces = new List<string>();
        private Dictionary<string, State> _statesMap = new Dictionary<string, State>();

        Guid participantId;
        string action;
        string AmountToReceive;
        string OrderCode;
        string ParticipantCode;
        string PaymentSerial;

        public User User;

        private void FillStates()
        {
            this._statesMap.Add("200", State.AccountExists);
            this._statesMap.Add("401", State.Rejected);
            this._statesMap.Add("500", State.DenialOfService);
            this._statesMap.Add("202", State.Finalized);
            this._statesMap.Add("400", State.Rejected);
            this._statesMap.Add("409", State.Rejected);
        }
        private object GetContextValue(Context context, string key)
        {
            return context[key];
        }

        private object GetFilterValue(Context context, string key)
        {
            return context[key];
        }
        public override void InitGateway(Hashtable settings)
        {

            this.FillStates();
            this.url = (string)settings["Url"];
            this._tontineApiBaseUrl = (string)settings["TontineApiBaseUrl"];
            this._idToken = (string)settings["_idToken"];
            _traces = new List<string>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            this.participantId = new Guid();
            this.action = string.Empty;
            this.AmountToReceive = string.Empty;
            this.OrderCode = string.Empty;
            this.ParticipantCode = string.Empty;
        }
    

        private string GetToken()
        {
            //Logger.Instance.WriteMessage(String.Concat("\r\n\r\nToken: "), 1);
            try
            {
                //Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nToken starting: ", _tontineApiBaseUrl), 1);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{_tontineApiBaseUrl}/security/users/login");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                var x = "";

                var result = "";
                var user = new User();

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"email\":\"ayapi@sk-automate.com\"," +
                                  "\"password\":\"carpeDiem@2021\"}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                x = result;
                User = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(x);
                return User.Token;

            }
            catch (Exception ex)
            {
                //Logger.Instance.WriteMessage(String.Concat("\r\n\r\nToken error: ", ex), 1);

                //Logger.Instance.WriteMessage(ex.Message, 1);
                return "false";
            }
        }

        public override void CheckAccount(ref Context context)
        {
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n-----------------------------------------------------------------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n------ SK - WELCOME TO SK TONTINE GATEWAY check Account -------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n-----------------------------------------------------------------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\nStarting Check Account !!"), 1);

            Logger.Instance.WriteMessage(String.Concat("\r\n\r\nClear context Vars"), 1);
            //this.ResetId(ref context);
            //this.ResetData(ref context);

            Logger.Instance.WriteMessage(String.Concat("\r\n\r\nCheck Params start: "), 1);

            //this.participantId = new Guid(context["ParticipantId"].ToString());
            //this.AmountToReceive = context["AmountToReceive"].ToString();

            //Logger.Instance.WriteMessage(String.Concat("\r\n\r\nAction: ", this.action), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\nParticipantId: ", this.participantId), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\nAmountToReceive: ", this.AmountToReceive), 1);

            //Logger.Instance.WriteMessage(String.Concat("\r\n\r\nContext ParticipantId: ", context["ParticipantId"].ToString()), 1);
            //Logger.Instance.WriteMessage(String.Concat("\r\n\r\ncontext AmountToReceive: ", context["AmountToReceive"].ToString()), 1);

            this.action = this.GetContextValue(context, "Action").ToString();

            if (this.participantId == new Guid())
            {
                try
                {
                    Logger.Instance.WriteMessage(String.Concat("\r\n\r\nCheckAccount step 0001: Participant info"), 1);

                    var token = GetToken();
                    //this.ParticipantCode = $"{context["PaymentContext.Payment.Account"]}";
                    this.ParticipantCode = this.GetContextValue(context, "PaymentContext.Payment.Account").ToString();

                    if(String.IsNullOrEmpty(this.ParticipantCode))
                    {
                        Logger.Instance.WriteMessage(String.Concat("\r\n\r\nParticipant code is null or empty"), 1);
                        Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nParticipant code: ", this.ParticipantCode), 1);

                        context.Status = State.AccountNotExists;
                        context.Description = "Code participant incorrect";
                    }
                    else
                    {
                        Logger.Instance.WriteMessage(String.Concat("\r\n\r\nParticipant code is not null"), 1);

                        Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nParticipant code: ", this.ParticipantCode), 1);

                        //Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nTontine get member request: ", token + "participantcode: " + this.ParticipantCode + "url: " + _tontineApiBaseUrl + "/members/tontine-participant/" + this.ParticipantCode), 1);

                        WebRequest request = WebRequest.Create($"{_tontineApiBaseUrl}/members/tontine-participant/{this.ParticipantCode}");

                        request.Headers.Add("Authorization", $"Bearer {token}");

                        var response = request.Execute<MemberModel>(null, "GET");
                        //Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nParticipant info Response: ", response), 1);

                        if (response.Data.ParticipantId == new Guid() || response.Data.ParticipantId == null)
                        {
                            this.ResetData(ref context);

                            context.Status = State.AccountNotExists;
                            context.Description = "Code participant incorrect";
                        }
                        else
                        {
                            //Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nTontine get member details: ", response.ToString()), 1);

                            var txt = response.Status.Code;

                            context["ParticipantId"] = response.Data.ParticipantId;
                            this.participantId = response.Data.ParticipantId;

                            context["MemberId"] = response.Data.Id;
                            context["ParticipantFullName"] = $"{response.Data.Firstname} {response.Data.Lastname}";
                            context["TontineName"] = response.Data.Tontine.Name[0].Text;
                            context["TontineId"] = response.Data.Tontine.Id;
                            context["MembershipTicketFee"] = response.Data.Tontine.MembershipTicketFee;
                            context["MemberShipFeesBillingFeesToPay"] = response.Data.Tontine.MemberShipFeesBilling.FeesToPay;
                            context["MemberShipFeesBillingFeesPaid"] = response.Data.Tontine.MemberShipFeesBilling.feesPaid;
                            context["ContributionsAmount"] = response.Data.Tontine.ContributionsAmount;

                            Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nTontine Name: ", context["TontineName"]), 1);
                            Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nParticipant Full Name: ", context["ParticipantFullName"]), 1);
                            Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nContributions Amount: ", context["ContributionsAmount"]), 1);

                            for (int i = 0; i < response.Data.Tontine.ContributionBilling.Count; i++)
                            {
                                if (i < 3)
                                {
                                    context["ContributionBillingShareIndex" + (i + 1)] = response.Data.Tontine.ContributionBilling[i].ShareIndex;
                                    context["ContributionBillingFeesToPay" + (i + 1)] = response.Data.Tontine.ContributionBilling[i].FeesToPay;
                                    context["ContributionBillingFeesPaid" + (i + 1)] = response.Data.Tontine.ContributionBilling[i].FeesPaid;
                                }

                            }

                            context.Status = State.AccountExists;
                            context.Description = "Code participant valide";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\n Check account step 0001 error : ", ex), 1);

                    this.ResetData(ref context);

                    context.Status = State.AccountNotExists;
                }
            }
            else if (this.participantId != new Guid())
            {
                if(String.IsNullOrEmpty(this.AmountToReceive) && this.action == "2")
                {
                    Logger.Instance.WriteMessage(String.Concat("\r\n\r\nCheckAccount step 0002: Refund"), 1);
                    Logger.Instance.WriteMessage(String.Concat("\r\n\r\nCheckAccount step 0002: Refund Info"), 1);
                    try
                    {
                        Logger.Instance.WriteMessage(String.Concat("\r\n\r\nCheckAccount step 2: Participant Refund Info"), 1);
                        this.OrderCode = this.GetContextValue(context, "OrderCode").ToString();
                        Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nOrder code: ", this.OrderCode), 1);

                        var body = new RefundExecuteModel
                        {
                            orderCode = this.OrderCode,
                            participantId = $"{context["ParticipantId"]}"
                        };

                        var token = GetToken();

                        WebRequest request = WebRequest.Create($"{_tontineApiBaseUrl}/shares/orders/excecute-refund-process");

                        request.Headers.Add("Authorization", $"Bearer {token}");

                        var response = request.Execute<RefundModel>(body);
                        Logger.Instance.WriteMessage(response.Data.ToString(), 1);
                        Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\n Refund Response : ", response.Data.ToString()), 1);

                        context["AmountToReceive"] = response.Data.AmountToReceive;
                        this.AmountToReceive = response.Data.AmountToReceive.ToString();

                        Logger.Instance.WriteMessage(String.Concat("\r\n\r\nOrder Code:", this.OrderCode), 1);
                        Logger.Instance.WriteMessage(String.Concat("\r\n\r\nAmount To Receive:", context["AmountToReceive"]), 1);

                        //ParticipantInfo = true;
                        this.ResetData(ref context);

                        context.Status = State.AccountExists;
                        context.Description = "Code de retrait valide";
                    }
                    catch (Exception ex)
                    {

                        this.ResetData(ref context);

                        context.Status = State.AccountNotExists;
                        context.Description = "Vérifier la validité du code de retrait";

                        Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\n Check account step 0002 error : ", ex), 1);
                        
                        foreach (var trace in _traces)
                        {
                            Logger.Instance.WriteMessage(trace, 1);
                        }
                        throw;
                    }
                }

                /*if(context.Description == "Code de retrait valide")
                {
                    Process(ref context);
                    context.Status = State.Finalized;
                    Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\n State Is y : ", context.Status), 1);
                }*/
            }     

            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n CheckAccount End !!!"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n------------------------------------------------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n------------ GOOD BY SK TONTINE GATEWAY --------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n------------------------------------------------"), 1);
            Logger.Instance.WriteMessage(String.Concat("\r\n\r\n"), 1);

        }

        public override void CheckProcessStatus(ref Context context)
        {
            context.Status = State.Rejected;
            context.Description = "Check status is NOT Supported by Service Provider";
        }

        public override void CheckRecallStatus(ref Context context)
        {
            throw new NotSupportedException("Recall payment is NOT  Supported");
        }

        public override void Dispose()
        {
        }


        public override void Process(ref Context context)
        {
            Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nProcessing... "), 1);

            //Billing
            try
            {
                var paymentType = context["PaymentType"];
                //accountType -- Pay a tontine bill. *** accountType-Contribution = 1 accountType-MembershipFee = 3
                var tontineId = context["TontineId"];
                // var tontineId = context["TontineId"].ToString();
                var amount = int.Parse(this.GetContextValue(context, "PaymentContext.Payment.Value").ToString());

                var account = (context["PaymentContext.Payment.Account"]);

                var body = new BillToParticipantAccountModel
                {
                    participantId = context["ParticipantId"].ToString(),
                    amount = amount,
                    skpaymentId = $"{context["PaymentContext.Payment.Number"]}"
                };

                var token = GetToken();

                WebRequest request = WebRequest.Create($"{_tontineApiBaseUrl}/tontines/{tontineId}/pay-participant-bill?accountType={paymentType}");

                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = request.Execute<BillingModel>(body);

                context["ShareNumber"] = response.Data.ShareNumber;
                context["ContributionBillingFeesToPay"] = response.Data.AmountToPaid;
                context["ContributionBillingFeesPaid"] = response.Data.AmountTotalPaid;
                context["ContributionBillingFeesToRefund"] = response.Data.AmountToRefund;

                this.ResetData(ref context);//context.clear()

                context.Status = State.AccountExists;
                context.Description = response.Status.Description;
            }
            catch (Exception e)
            {
                //e.Message
                Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\n Processing error : ", e), 1);
                foreach (var trace in _traces)
                {
                    Logger.Instance.WriteMessage(trace, 1);
                }
                throw;
            }
            Logger.Instance.WriteMessage(System.String.Concat("\r\n\r\nEnd Processing... "), 1);
            context.Clear();
        }

        public override void RecallPayment(ref Context context)
        {
            throw new NotSupportedException("Recall payment is NOT  Supported");
        }

        public override Hashtable SaveSettings()
        {
            return null;
        }

        public override bool SendRoll(Context[] payments, Hashtable filterContext)
        {
            return true;
        }

        private void ResetParticipantId(ref Context context)
        {
            context["ParticipantId"] = new Guid();
        }

        private void ResetId(ref Context context)
        {
            /*context["ParticipantId"] = new Guid();
            context["AmountToReceive"] = String.Empty;
            context["OrderCode"] = String.Empty;*/

            /*this.participantId = new Guid();
            this.AmountToReceive = String.Empty;
            this.OrderCode = String.Empty;*/

            //context.Clear();
        }

        private void ResetData(ref Context context)
        {
            //context.Clear();

            
            this.participantId = new Guid();
            this.action = String.Empty;
            this.AmountToReceive = String.Empty;
            this.ParticipantCode = String.Empty;

            /*
            context["ParticipantId"] = new Guid();
            context["MemberId"] = String.Empty;
            context["ParticipantFullName"] = String.Empty;
            context["TontineName"] = String.Empty;
            context["TontineId"] = String.Empty;
            context["MembershipTicketFee"] = String.Empty;
            context["MemberShipFeesBillingFeesToPay"] = String.Empty;
            context["MemberShipFeesBillingFeesPaid"] = String.Empty;
            context["ContributionsAmount"] = String.Empty;

            context["ContributionBillingShareIndex1"] = String.Empty;
            context["ContributionBillingFeesToPay1"] = String.Empty;
            context["ContributionBillingFeesPaid1"] = String.Empty;

            context["ContributionBillingShareIndex2"] = String.Empty;
            context["ContributionBillingFeesToPay2"] = String.Empty;
            context["ContributionBillingFeesPaid2"] = String.Empty;

            context["ContributionBillingShareIndex3"] = String.Empty;
            context["ContributionBillingFeesToPay3"] = String.Empty;
            context["ContributionBillingFeesPaid3"] = String.Empty;

            context["AmountToReceive"] = String.Empty;

            context["ShareNumber"] = String.Empty;
            context["ContributionBillingFeesToPay"] = String.Empty;
            context["ContributionBillingFeesPaid"] = String.Empty;
            context["ContributionBillingFeesToRefund"] = String.Empty;*/
        }
    }
}
