using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ZaloConversionRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloConversionSearchListRequest>(data);
            return XHelper.Invoke("POST", "ZaloConversion/SearchList", req);
        }
    }
}