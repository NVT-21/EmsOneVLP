using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class GrabInfoShippingViewModel
    {
        [DataMember]
        public string SendedDate { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long TotalFee { get; set; }
        [DataMember]
        public long SystemSurcharge { get; set; }
        [DataMember]
        public double Distance { get; set; }
        [DataMember]
        public BaseModel Status { get; set; }
        [DataMember]
        public string ShareLink { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public BaseModel Service { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string ShippingID { get; set; }
        [DataMember]
        public string SucceedDate { get; set; }
        [DataMember]
        public string CanceledDate { get; set; }
        [DataMember]
        public GrabInfoShippingSenderInfoModel SenderInfo { get; set; }
        [DataMember]
        public List<GrabInfoShippingReceiverInfoModel> ListReceiverInfo { get; set; }
        [DataMember]
        public GrabInfoShippingDriverInfoModel DriverInfo { get; set; }
        [DataMember]
        public List<OrderHistoryModel> ListOrderHistory { get; set; }

    }

    [Serializable]
    [DataContract]
    public class GrabInfoShippingSenderInfoModel
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
        public string Description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GrabInfoShippingReceiverInfoModel
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
        public string Description { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public long MoneyCollect { get; set; }
        [DataMember]
        public GrabInfoShippingEstimatedInfoModel EstimatedInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GrabInfoShippingEstimatedInfoModel
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
    public class GrabInfoShippingDriverInfoModel
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
}