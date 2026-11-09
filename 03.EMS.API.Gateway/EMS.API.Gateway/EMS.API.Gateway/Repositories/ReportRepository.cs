using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ReportRepository
    {
        public static ZResponse CODMoney(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReportCODMoneyRequest>(data);
            return XHelper.Invoke("POST", "Report/CODMoney", req);
        }
    }
}