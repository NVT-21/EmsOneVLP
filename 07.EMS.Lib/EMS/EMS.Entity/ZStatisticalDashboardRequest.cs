using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalDashboardRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZStatisticalDashboardV2Request
    {
        public string DeviceName { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    #region Web
    public class ZStatisticalDashboardWebRequest
    {
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZStatisticalDashboardWebResponse
    {
        public long TotalTurnover { get; set; }
        public List<ZStatisticalSearchCashflowWeb> Cashflow { get; set; }
        public List<ZStatisticalSearchOrderWeb> Order { get; set; }
        public List<ZStatisticalDeliveryTransportWeb> DeliveryTransport { get; set; }
        public ZStatisticalProductWeb Product { get; set; }
        public ZStatisticalCustomerWeb Customer { get; set; }
    }
    #endregion

    #region Android
    public class ZStatisticalDashboardAndroidRequest
    {
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ZStatisticalDashboardAndroidResponse
    {
        public long TotalTurnover { get; set; }
        public List<ZStatisticalSearchCashflowAndroid> Cashflow { get; set; }
        public List<ZStatisticalSearchOrderAndroid> Order { get; set; }
        public List<ZStatisticalDeliveryTransportAndroid> DeliveryTransport { get; set; }
        public ZStatisticalProductAndroid Product { get; set; }
        public ZStatisticalCustomerAndroid Customer { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalDashboardIOSRequest
    {
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZStatisticalDashboardIOSResponse
    {
        public long TotalTurnover { get; set; }
        public List<ZStatisticalSearchCashflowIOS> Cashflow { get; set; }
        public List<ZStatisticalSearchOrderIOS> Order { get; set; }
        public List<ZStatisticalDeliveryTransportIOS> DeliveryTransport { get; set; }
        public ZStatisticalProductIOS Product { get; set; }
        public ZStatisticalCustomerIOS Customer { get; set; }
    }
    #endregion
}
