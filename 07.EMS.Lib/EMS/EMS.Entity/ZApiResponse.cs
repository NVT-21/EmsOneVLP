using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Entity
{
    public class ZApiResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public string ListValue { get; set; }
    }

    #region ZohoCRM
    public class ZApiZohoCRMResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string api_domain { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string error { get; set; }
    }
    public class ZApiZohoTokenRefreshResponse
    {
        public string access_token { get; set; }
        public string api_domain { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string error { get; set; }
    }
    public class ZApiZohoCreatResponse
    {
        public List<ZApiZohoCreatLeads> data { get; set; }
    }
    public class ZApiZohoCreatLeads
    {
        public string code { get; set; }
        public ZZohoDetails details { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        
    }
    public class ZZohoDetails
    {
        public string Modified_Time { get; set; }
        public ZAttribute Modified_By { get; set; }
        public string Created_Time { get; set; }
        public string id { get; set; }
        public ZAttribute Created_By { get; set; }
        
    }
    public class ZAttribute
    {
        public string name { get; set; }
        public string id { get; set; }

    }
    public class ZApiZohoGetLeadsResponse
    {
        public List<ZZohoCRMData> data { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
    public class ZZohoCRMData
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
    #endregion

    #region TransportEMS
    // PickupStatus
    public class ZApiInfoStatusPickupOrderResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public List<ZApiInfoStatusPickupOrderDetail> ListValue { get; set; }
    }
    public class ZApiInfoStatusPickupOrderDetail
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string OrderCode { get; set; }
        public string TrackingCode { get; set; }
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public string CustomerCode { get; set; }
        public string Note { get; set; }
        //public DateTime DateTime { get; set; }
        public string DateTime { get; set; }
    }
    // InformationParcelRelease
    public class ZApiTrackingItemInfoResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ZApiTrackingItemInfoDetail Value { get; set; }
        public string ListValue { get; set; }
    }
    public class ZApiTrackingItemInfoDetail
    {
        public string ITEMCODE { get; set; }
        public string DATACODE { get; set; }
        public string CUSTOMERCODE { get; set; }
        public string SENDERFULLNAME { get; set; }
        public string SENDERTEL { get; set; }
        public string SENDERADDRESS { get; set; }
        public string RECEIVERFULLNAME { get; set; }
        public string RECEIVERTEL { get; set; }
        public string RECEIVERADDRESS { get; set; }
        public string ACCEPTANCEPOSCODE { get; set; }
        public string WEIGHT { get; set; }
        public string PAYMENTFREIGHTVAT { get; set; }
        public string VALUEADDEDSERVICECODE { get; set; }
    }
    // TransportStatus
    public class ZApiTrackingTraceItemResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public List<ZListTraceItem> ListValue { get; set; }
    }
    public class ZListTraceItem
    {
        //public DateTime TRACEDATETIME { get; set; }
        public string TRACEDATETIME { get; set; }
        public string TRACEDATE { get; set; }
        public string TIMEDETAIL { get; set; }
        public string STATUS { get; set; }
        public string STATUSTEXT { get; set; }
        public string POSCODE { get; set; }
    }
    #endregion
}