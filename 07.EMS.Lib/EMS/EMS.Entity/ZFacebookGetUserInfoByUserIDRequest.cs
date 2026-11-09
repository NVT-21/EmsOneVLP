using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class ZFacebookGetUserInfoByUserIDRequest
    {
        public string ChannelUserID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookGetUserInfoByUserIDResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("birthday")]
        public DateTime BirthDay { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("picture")]
        public ZFacebookGetUserInfoPictureByUserID Picture { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
    }
    public class ZFacebookGetUserInfoPictureByUserID
    {
        [JsonProperty("data")]
        public ZFacebookGetUserInfoPictureDataByUserID Data { get; set; }
    }
    public class ZFacebookGetUserInfoPictureDataByUserID
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
