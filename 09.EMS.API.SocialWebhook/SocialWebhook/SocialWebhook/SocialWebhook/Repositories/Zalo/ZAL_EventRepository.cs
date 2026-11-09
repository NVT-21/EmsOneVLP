using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SocialWebhook.Models;
using SocialWebhook.Repositories;
using EMS.Entity;
using EMS.Library;

namespace SocialWebhook.Repositories.Zalo
{
    public class ZAL_EventRepository
    {
        public static void Event(ZaloRequest req)
        {
            switch (req.event_name)
            {
                case "user_send_text":          //Sự kiện người dùng gửi tin nhắn
                case "user_send_image":         //Sự kiện người dùng gửi tin nhắn hình ảnh
                case "user_send_link":          //Sự kiện người dùng gửi tin nhắn liên kết
                case "user_send_sticker":       //Sự kiện người dùng gửi tin nhắn sticker
                case "user_send_gif":           //Sự kiện người dùng gửi tin nhắn gif
                case "user_send_location":      //Sự kiện người dùng gửi vị trí
                case "user_send_audio":         //Sự kiện người dùng gửi tin nhắn voice
                case "user_reacted_message":    //Sự kiện người dùng thả biểu tượng cảm xúc lên tin nhắn
                    ZAL_UserRepository.SendMessage(req.data);
                    break;
                case "oa_send_text":            //Sự kiện shop gửi tin nhắn
                    ZAL_PageRepository.SendMessage(req.data);
                    break;
                case "user_received_message":   //Sự kiện tin nhắn đã được nhận
                    ZAL_UserRepository.ReceivedMessage(req.data);
                    break;
                case "user_seen_message":       //Sự kiện người dùng đã đọc tin nhắn
                    ZAL_UserRepository.SeenMessage(req.data);
                    break;
                case "follow":                  //Sự kiện người dùng quan tâm Official Account
                    ZAL_UserRepository.Follow(req.data);
                    break;
                case "unfollow":                //Sự kiện người dùng bỏ quan tâm Official Account
                    ZAL_UserRepository.UnFollow(req.data);
                    break;
                case "user_submit_info":        //Sự kiện người dùng chia sẻ thông tin
                    ZAL_UserRepository.SubmitInfo(req.data);
                    break;
                case "user_authentication":     //Sự kiện user access wifi authenticate by zalo
                    ZAL_UserRepository.Authentication(req.data);
                    break;
                case "NEKO":
                    XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(req));
                    break;
            }
        }
    }
}