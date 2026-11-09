using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class ZFacebookReplyCommentRequest
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
