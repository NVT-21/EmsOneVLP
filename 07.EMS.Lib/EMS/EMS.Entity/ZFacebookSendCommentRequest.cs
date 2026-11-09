using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSendCommentRequest
    {
        public string ID { get; set; }
        public ZFacebookReplyCommentRequest RequestData { get; set; }
    }
    public class ZFacebookLikeCommentRequest
    {
        public string CommentID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookLikeCommentResponse
    {
        [JsonProperty("success")]
        public Boolean Success { get; set; }
    }
    public class ZFacebookDeleteLikeCommentRequest
    {
        public string CommentID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookDeleteLikeCommentResponse
    {
        [JsonProperty("success")]
        public Boolean Success { get; set; }
    }
    public class ZFacebookDeleteCommentRequest
    {
        public string CommentID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookDeleteCommentResponse
    {
        
    }
    public class ZFacebookHiddenCommentRequest
    {
        public string CommentID { get; set; }
        public string Type { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookHiddenCommentResponse
    {
        [JsonProperty("success")]
        public Boolean Success { get; set; }
    }
}
