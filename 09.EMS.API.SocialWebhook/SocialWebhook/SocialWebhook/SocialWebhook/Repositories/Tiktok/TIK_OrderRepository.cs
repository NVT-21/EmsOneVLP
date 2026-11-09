using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using SocialWebhook.Models;

namespace SocialWebhook.Repositories.Tiktok
{
    public class TIK_OrderRepository
    {
        public static void ReceiveStatusOrder(object request)
        {
            var dataRequest = JsonConvert.DeserializeObject<TiktokReceiveStatusRequest>(JsonConvert.SerializeObject(request));
            switch (dataRequest.Data.OrderStatus)
            {
                case "AWAITING_SHIPMENT":
                case "CANCEL":
                    TIK_WebhookDataLogRepository.AddDataLog(dataRequest);
                    break;
                case "UNPAID":
                case "AWAITING_COLLECTION":
                case "PARTIALLY_SHIPPING":
                    break;
                case "IN_TRANSIT":
                case "DELIVERED":
                case "COMPLETED":
                    TIK_ReceiveStatusRepository.ReceiveStatus(dataRequest);
                    break;
                default:
                    XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(dataRequest));
                    break;
            }
        }
        public static void ReverseStatusOrder(object request)
        {
            var dataRequest = JsonConvert.DeserializeObject<TiktokReverseStatusRequest>(JsonConvert.SerializeObject(request));
            switch (dataRequest.Data.ReverseOrderStatus)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 50:
                case 51:
                case 99:
                case 100:
                    TIK_ReverseStatusRepository.ReverseStatus(dataRequest);
                    break;
                default:
                    XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(dataRequest));
                    break;
            }
        }
    }
}