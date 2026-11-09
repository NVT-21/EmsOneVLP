using Newtonsoft.Json;
using SocialWebhook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Tiktok
{
    public class TIK_SellerDeauthorizationRepository
    {
        public static void SellerDeauthorization(object request)
        {
            var dataRequest = JsonConvert.DeserializeObject<TiktokSellerDeauthorizationRequest>(JsonConvert.SerializeObject(request));
            XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(dataRequest));
        }
    }
}