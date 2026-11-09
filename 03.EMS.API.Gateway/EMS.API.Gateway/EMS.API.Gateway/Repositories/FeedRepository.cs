using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class FeedRepository
    {
        public static ZResponse AddSyncFeed(string data)
        {
            var req = JsonConvert.DeserializeObject<List<ZFeedAddSyncRequest>>(data);
            return XHelper.Invoke("POST", "Feed/AddSyncFeed", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFeedSearchListRequest>(data);
            return XHelper.Invoke("POST", "Feed/SearchList", req);
        }
        public static ZResponse AddSyncComment(string data)
        {
            var req = JsonConvert.DeserializeObject<List<ZCommentAddSyncRequest>>(data);
            return XHelper.Invoke("POST", "Comment/AddSyncComment", req);
        }
        public static ZResponse DeleteFeed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFeedDeleteRequest>(data);
            return XHelper.Invoke("POST", "Feed/DeleteFeed", req);
        }
    }
}