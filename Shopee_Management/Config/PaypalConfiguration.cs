using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Management.Config
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AWZx2NxWwr0N9NXr50qm1Nbe2XQAu7o2wDwn9R6HtDGd0RR3PyoLf1s6u4nl-bwCJ62n6DPJn5Zt3ZL9";
            clientSecret = "ELastgemG8opJcB0QbxqnTb_4dJxtACPeiDMmdAojKMegSPpfkGMJwiZkRxWE4ob0fWxzwmEKoguJYBr";
        }

        private static Dictionary<string, string> getconfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}