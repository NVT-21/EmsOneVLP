using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SocialWebhook.Models;
using SocialWebhook.Repositories;
using EMS.Entity;
using EMS.Library;

namespace SocialWebhook.Repositories.Tiktok
{
    public class TIK_EventRepository
    {
        public static void Event(object request)
        {
            var checkType = JsonConvert.DeserializeObject<TiktokRequest>(JsonConvert.SerializeObject(request));
            switch (checkType.Type)
            {
                case 1:
                    TIK_OrderRepository.ReceiveStatusOrder(request);
                    break;
                case 2:
                    TIK_OrderRepository.ReverseStatusOrder(request);
                    break;
                case 6:
                    TIK_SellerDeauthorizationRepository.SellerDeauthorization(request);
                    break;
                case 7:
                    TIK_AuthExpireRepository.AuthExpire(request);
                    break;
                default:
                    XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(request));
                    break;
            }
        }
    }
}