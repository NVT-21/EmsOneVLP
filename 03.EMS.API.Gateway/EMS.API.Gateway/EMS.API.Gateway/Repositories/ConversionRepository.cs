using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ConversionRepository
    {
        public static ZResponse SearchListConversion(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversionSearchListRequest>(data);
            return XHelper.Invoke("POST", "Conversion/SearchList", req);
        }
    }
}