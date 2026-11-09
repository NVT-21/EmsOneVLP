using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ZaloFollowRepository
    {
        public static ZResponse Follow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloFollowOrUnFollowRequest>(data);
            return XHelper.Invoke("POST", "ZaloFollow/Follow", req);
        }
        public static ZResponse UnFollow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloFollowOrUnFollowRequest>(data);
            return XHelper.Invoke("POST", "ZaloFollow/UnFollow", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloFollowSearchListRequest>(data);
            return XHelper.Invoke("POST", "ZaloFollow/SearchList", req);
        }
        public static ZResponse UserSubmitInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloUserSubmitInfoRequest>(data);
            return XHelper.Invoke("POST", "ZaloFollow/UserSubmitInfo", req);
        }
    }
}