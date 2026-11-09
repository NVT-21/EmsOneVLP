using Newtonsoft.Json;
using SocialWebhook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Tiktok
{
    public class TIK_AuthExpireRepository
    {
        public static void AuthExpire(object request)
        {
            var dataRequest = JsonConvert.DeserializeObject<TiktokAuthExpireRequest>(JsonConvert.SerializeObject(request));
            XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(dataRequest));
        }
    }
}