using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Entity
{
    public class ZApiRequest
    {
        public string Code { get; set; }
        public string Data { get; set; }
    }

    #region ZohoCRM
    public class ZApiZohoCRMRequest
    {
        public string Data { get; set; }
    }

    public class ZApiZohoCRMLeadsRequest
    {
        
        public string zohoToken { get; set; }
        public string phone { get; set; }
    }
    public class ZZohoCRMCreateLeadsRequest
    {
        public List<ZZohoCRMDetailLeadsRequest> data { get; set; }
        public List<string> trigger { get; set; }
        public string zohoToken { get; set; }
    }

    public class ZZohoCRMCreateLeads02Request
    {
        public List<ZZohoCRMDetailLeadsRequest> data { get; set; }
        public List<string> trigger { get; set; }
    }

    public class ZZohoCRMDetailLeadsRequest
    {
        public string Company { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    #endregion

    #region TransportEMS
    // Create Pickup
    public class ZTransportInfoShippingApiRequest
    {
        public string PICKUP_CODE_API { get; set; }
        public string PO_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_PHONE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_EMAIL { get; set; }
        public string STOREID { get; set; }
        public int PICKUP_PROVINCE_ID { get; set; }
        public int PICKUP_DISTRICT_ID { get; set; }
        public int PICKUP_WARD_ID { get; set; }
        public string PICKUP_STREET { get; set; }
        public string PICKUP_DATE { get; set; }
        public string PICKUP_HOURS { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string PICKUP_POSTMAN { get; set; }
        public string PICKUP_POCODE { get; set; }
        public string PICKUP_CONFIG { get; set; }
        public string SignatureCode { get; set; }
        public List<ZListItemInfoShippingRequest> LIST_ITEM { get; set; }
    }
    public class ZListItemInfoShippingRequest
    {
        public string TRACKING_CODE { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_CODE { get; set; }
        public string SENDER_PHONE { get; set; }
        public string SENDER_STREET { get; set; }
        public string SENDER_ADDRESS { get; set; }
        public string SENDER_PROVINCE_ID { get; set; }
        public string SENDER_DISTRICT_ID { get; set; }
        public string SENDER_WARD_ID { get; set; }
        public string SENDER_EMAIL { get; set; }
        public string RECEIVER_NAME { get; set; }
        public string RECEIVER_PROVINCE_ID { get; set; }
        public string RECEIVER_PHONE { get; set; }
        public string RECEIVER_DISTRICT_ID { get; set; }
        public string RECEIVER_WARD_ID { get; set; }
        public string RECEIVER_EMAIL { get; set; }
        public string RECEIVER_STREET { get; set; }
        public string RECEIVER_ADDRESS { get; set; }
        public string PRODUCT_NAME { get; set; }
        public int PRODUCT_VALUE { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_DESCRIPTION { get; set; }
        public int PRODUCT_QUANTITY { get; set; }
        public int WEIGHT { get; set; }
        public string ADDITION_SERVICE { get; set; }
        public string SPECIAL_SERVICE { get; set; }
        public long TOTAL_AMOUNT { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string TO_COUNTRY { get; set; }
        public string PO_CREATE { get; set; }
        public int COMPLETE { get; set; }
        public string POSTAL_PROVINCE_CODE { get; set; }
        public string POSTAL_DISTRICT_CODE { get; set; }
        public string POSTAL_COMMUNE_CODE { get; set; }
        public string POSTAL_CODE { get; set; }
        public int SERVICE_TYPE { get; set; }
        public long SERVICE_FEE { get; set; }
        public string CLASSIFY { get; set; }
        public int IS_CONSIGNMENT { get; set; }
        public string CHANNEL { get; set; }
    }
    // Cancel Pickup
    public class ZTransportShippingCancelRequest
    {
        public string TrackingCode { get; set; }
        public string CustomerCode { get; set; }
        public string SignatureCode { get; set; }
    }
    // PickupStatus
    public class ZTransportGetStatusPickupOrderRequest
    {
        public string OrderCode { get; set; }
        public string OrderNumber { get; set; }
    }
    // InformationParcelRelease
    public class ZTransportTrackingItemInfoRequest
    {
        public string ItemCode { get; set; }
        public string SignatureCode { get; set; }
    }
    public class ZTransportTrackingTraceItemRequest
    {
        public string ItemCode { get; set; }
        public string SignatureCode { get; set; }
    }
    #endregion
}