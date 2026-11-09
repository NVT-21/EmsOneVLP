using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class StatisticaltRepository
    {
        #region Statiscal
        public static ZResponse StatisticalDashboard(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalDashboardRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalDashboard", req);
        }

        public static ZResponse StatisticalTransport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalSearchTransportRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalTransport", req);
        }
        public static ZResponse StatisticalOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalSearchOrderRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalOrder", req);
        }
        public static ZResponse StatisticalCustomer(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalSearchCustomerRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalCustomer", req);
        }
        public static ZResponse StatisticalRequestOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderDashboardRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalRequestOrder", req);
        }
        public static ZResponse StatisticalCashflow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalSearchCashflowRequest>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalCashflow", req);
        }
        #endregion

        #region StatisticalV2
        public static ZResponse StatisticalDashboardV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatisticalDashboardV2Request>(data);
            return XHelper.Invoke("POST", "Statistical/StatisticalDashboardV2", req);
        }
        #endregion
    }
}