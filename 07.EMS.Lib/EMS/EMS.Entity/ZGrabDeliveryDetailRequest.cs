using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailRequest
    {
        [DataMember]
        public string id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailResponse
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public ZGrabDeliveryDetailDataResponse data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataResponse
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string customer_code { get; set; }
        [DataMember]
        public string serviceType { get; set; }
        [DataMember]
        public string serviceName { get; set; }
        [DataMember]
        public string itemcode { get; set; }
        [DataMember]
        public string ordercode { get; set; }
        [DataMember]
        public ZGrabDeliveryDetailDataOriginResponse origin { get; set; }
        [DataMember]
        public List<ZGrabDeliveryDetailDataDestinationResponse> destination { get; set; }
        [DataMember]
        public string shareLink { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string statusName { get; set; }
        [DataMember]
        public ZGrabDeliveryDetailDataDriverResponse driver { get; set; }
        [DataMember]
        public List<ZGrabDeliveryDetailDataHistoryResponse> history { get; set; }
        [DataMember]
        public long distance { get; set; }
        [DataMember]
        public long total_fee { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string sended_at { get; set; }
        [DataMember]
        public string succeed_at { get; set; }
        [DataMember]
        public string canceled_at { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataOriginResponse
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string district { get; set; }
        [DataMember]
        public string ward { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataDriverResponse
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string licensePlate { get; set; }
        [DataMember]
        public string photoURL { get; set; }
        
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataHistoryResponse
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string statusName { get; set; }
        [DataMember]
        public string created_at { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataDestinationResponse
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string province { get; set; }
        [DataMember]
        public string district { get; set; }
        [DataMember]
        public string ward { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string product_name { get; set; }
        [DataMember]
        public long quantity { get; set; }
        [DataMember]
        public long total_amount { get; set; }
        [DataMember]
        public double weight { get; set; }
        [DataMember]
        public double height { get; set; }
        [DataMember]
        public double width { get; set; }
        [DataMember]
        public double length { get; set; }
        [DataMember]
        public long money_collect { get; set; }
        [DataMember]
        public string extra { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string statusName { get; set; }
        [DataMember]
        public ZGrabDeliveryDetailDataDestinationEstimatedResponse estimated { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDeliveryDetailDataDestinationEstimatedResponse
    {
        [DataMember]
        public string pickup { get; set; }
        [DataMember]
        public string dropoff { get; set; }
        [DataMember]
        public string completed { get; set; }
    }
}
