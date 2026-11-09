using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationRequest
    {
        [DataMember]
        public string merchant_token { get; set; }
        [DataMember]
        public string serviceType { get; set; }
        [DataMember]
        public List<ZGrabDeliveryQuotationOriginRequest> origin { get; set; }
        [DataMember]
        public List<ZGrabDeliveryQuotationDestinationRequest> destination { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationV2Request
    {
        [DataMember]
        public string merchant_token { get; set; }
        [DataMember]
        public string serviceType { get; set; }
        [DataMember]
        public List<ZGrabDeliveryQuotationOriginRequest> origin { get; set; }
        [DataMember]
        public List<ZGrabDeliveryQuotationDestinationRequest> destination { get; set; }
        [DataMember]
        public string promo_code { get; set; }
        [DataMember]
        public string paymentMethod { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationOriginRequest
    {
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string district { get; set; }
        [DataMember]
        public string ward { get; set; }
        [DataMember]
        public string address { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationDestinationRequest
    {
        [DataMember]
        public string product_name { get; set; }
        [DataMember]
        public int quantity { get; set; }
        [DataMember]
        public int total_amount { get; set; }
        [DataMember]
        public int weight { get; set; }
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public int length { get; set; }
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string district { get; set; }
        [DataMember]
        public string ward { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public int money_collect { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationResponse
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public ZGrabDeliveryQuotationDataResponse data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationDataResponse
    {
        [DataMember]
        public int total_freight { get; set; }
        [DataMember]
        public int distance { get; set; }
        [DataMember]
        public List<ZGrabDeliveryQuotationDataEstimatedTimeLineResponse> estimatedTimeLine { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryQuotationDataEstimatedTimeLineResponse
    {
        [DataMember]
        public string pickup { get; set; }
        [DataMember]
        public string dropoff { get; set; }
        [DataMember]
        public string completed { get; set; }
    }
}
