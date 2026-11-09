using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSubcribedRequest
    {
        public string PageID { get; set; }
        public string AccessToken { get; set; }
    }
    public class ZFacebookSubcribedResponse
    {
        [JsonProperty("success")]
        public Boolean Success { get; set; }
    }
}
