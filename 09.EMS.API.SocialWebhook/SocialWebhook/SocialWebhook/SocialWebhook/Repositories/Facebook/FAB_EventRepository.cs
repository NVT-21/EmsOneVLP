using SocialWebhook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Facebook
{
    public class FAB_EventRepository
    {
        public static void Event(string event_name, FacebookRequest req)
        {
            switch (event_name)
            {
                case "PageSendMessage":
                    FAB_PageRepository.SendMessage(req);
                    break;
                case "UserSendMessage":
                    FAB_UserRepository.SendMessage(req);
                    break;
                case "RecievedMessage":   
                    FAB_UserRepository.ReceivedMessage(req);
                    break;
                case "SeenMessage":       
                    FAB_UserRepository.SeenMessage(req);
                    break;
            }
        }
    }
}