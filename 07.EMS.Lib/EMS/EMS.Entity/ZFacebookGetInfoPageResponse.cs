using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookGetInfoPageResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public ZFacebookInfoPagePicture Picture { get; set; }

        [JsonProperty("cover")]
        public ZFacebookInfoPageCover Cover { get; set; }
    }
    public class ZFacebookInfoPagePicture
    {
        [JsonProperty("data")]
        public ZFacebookInfoPagePictureData Data { get; set; }
    }
    public class ZFacebookInfoPagePictureData
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class ZFacebookInfoPageCover
    {
        [JsonProperty("cover_id")]
        public string CoverID { get; set; }
        [JsonProperty("offset_x")]
        public int OffsetX { get; set; }
        [JsonProperty("offset_y")]
        public int OffsetY { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
