using IBP.SDKGatewayLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TontineGateway;

namespace TontineConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = 2;
            //var tt = GeneratePassword();
            //var xx = Convert.ToBase64String(Encoding.UTF8.GetBytes("alibabba:abuoi125358oiuby"));
            var gatewayCore = new GatewayCore();

            var settigs = new Hashtable();
            settigs["TontineApiBaseUrl"] = "https://tontineapi.azurewebsites.net";
            settigs["IdToken"] = "OTNiMWU1NDMtOTIxYy00MmNlLWIwMzMtMTgzMDEwMDBiMmNiOjl4amxlR0ZOI0RSbDUpJVk2cGp5TkA=";//"YTQ1MzE4NTgtNmZjYi00M2JiLTllYTgtYTFmMDBjYzgwMjVjOjkoaXM0SGoqalXCoyNiQW40VnlldmZA";//YTQ1MzE4NTgtNmZjYi00M2JiLTllYTgtYTFmMDBjYzgwMjVjOjkoaXM0SGoqalXCoyNiQW40VnlldmZA
            gatewayCore.InitGateway(settigs);

            var context = new Context();
            context["PaymentContext.Payment.Account"] = "MF83YI314";// Participant Code
            //context["paymentType"] = "1";
            //context["PaymentContext.Payment.Value"] = "50";
            context["PaymentContext.Payment.Number"] = "99328-288f-4249-b00b-2615c300008";//SKPaymentID
            context["Action"] = "2";//1 = Billing 2 = Refund

            context["PaymentType"] = 1;//1=accountType-Contribution ** 2=accountType-Solidarity ** 3=accountType-MembershipFee ** 4=accountType-Sequestre
            //context["OrderCode"] = 1;
            context["ParticipantId"] = "288d0c36-6ef9-4fe3-97e6-ee3e410ab5a8";
            //context["SchoolId"] = context["SchoolId1"];
            context["PaymentContext.Payment.Value"] = "1000";
            context["OrderCode"] = "XVF4871";
            gatewayCore.CheckAccount(ref context);

            if(context["Action"].ToString() == "1")
            {
                gatewayCore.Process(ref context);
            }
            //gatewayCore.Process(ref context);
        }

        public static string GeneratePassword(bool useLowercase = true, bool useUppercase = true, bool useNumbers = true, bool useSpecial = true, int passwordSize = 20)
        {
            string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string NUMBERS = "123456789";
            string SPECIALS = @"!@£$%^&*()#€";
            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }
          
            return new string(_password);
            //return String.Join(null, _password);
        }
    }
}
