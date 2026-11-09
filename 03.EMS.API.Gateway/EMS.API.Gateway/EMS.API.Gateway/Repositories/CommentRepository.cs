using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class CommentRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCommentSearchListRequest>(data);
            return XHelper.Invoke("POST", "Comment/SearchList", req);
        }
        public static ZResponse DeleteComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCommentDeleteRequest>(data);
            return XHelper.Invoke("POST", "Comment/DeleteComment", req);
        }

        public static ZResponse UpdateLikeComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCommentUpdateReactionRequest>(data);
            return XHelper.Invoke("POST", "Comment/UpdateLikeComment", req);
        }

        public static ZResponse UpdateHiddenComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCommentUpdateReactionRequest>(data);
            return XHelper.Invoke("POST", "Comment/UpdateHiddenComment", req);
        }
    }
}