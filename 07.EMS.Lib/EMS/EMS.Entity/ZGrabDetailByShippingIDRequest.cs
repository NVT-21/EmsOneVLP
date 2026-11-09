using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDRequest
    {
        [DataMember]
        public string ShippingID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDResponse
    {
        [DataMember]
        public string ShippingID { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string ServiceType { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public ZGrabDetailByShippingIDSenderInfoResponse SenderInfo { get; set; }
        [DataMember]
        public List<ZGrabDetailByShippingIDReceiverInfoResponse> ListReceiverInfo { get; set; }
        [DataMember]
        public string ShareLink { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public ZGrabDetailByShippingIDDriverInfoResponse DriverInfo { get; set; }
        [DataMember]
        public List<ZGrabDetailByShippingIDHistoryResponse> ListOrderHistory { get; set; }
        [DataMember]
        public double Distance { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long SystemSurcharge { get; set; }
        [DataMember]
        public long TotalFee { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string SendedDate { get; set; }
        [DataMember]
        public string SucceedDate { get; set; }
        [DataMember]
        public string CanceledDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDSenderInfoResponse
    {
        [DataMember]
        public string FullName{ get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string WardName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDReceiverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string WardName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public long MoneyCollect { get; set; }
        [DataMember]
        public ZGrabDetailByShippingIDReceiverInfoEstimatedInfoResponse EstimatedInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDReceiverInfoEstimatedInfoResponse
    {
        [DataMember]
        public string PickUpDate { get; set; }
        [DataMember]
        public string DropOffDate { get; set; }
        [DataMember]
        public string CompletedDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDDriverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public string PhotoURL { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabDetailByShippingIDHistoryResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
