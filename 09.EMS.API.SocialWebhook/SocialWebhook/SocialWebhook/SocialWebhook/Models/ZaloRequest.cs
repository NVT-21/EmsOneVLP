using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Models
{
    public class ZaloRequest
    {
        public string event_name { get; set; }
        public object data { get; set; }
    }
}