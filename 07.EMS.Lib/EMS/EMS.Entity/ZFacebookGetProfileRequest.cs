using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class ZFacebookGetProfileRequest
    {
        public string AccessToken { get; set; }
    }
    public class ZFacebookGetProfileResponse
    {
        [JsonProperty("id")]
        public string FacebookAccountID { get; set; }
    }
}
