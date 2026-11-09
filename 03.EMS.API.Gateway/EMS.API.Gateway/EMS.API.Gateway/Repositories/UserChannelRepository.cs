using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class UserChannelRepository
    {
        public static ZResponse SearchByChannelUserID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChannelGetByChannelUserIDRequest>(data);
            return XHelper.Invoke("POST", "UserChannel/SearchByChannelUserID", req);
        }
        public static ZResponse WebUpdateInformation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChannelWebUpdateInformationRequest>(data);
            return XHelper.Invoke("POST", "UserChannel/WebUpdateInformation", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChannelSearchListRequest>(data);
            return XHelper.Invoke("POST", "UserChannel/SearchList", req);
        }
    }
}