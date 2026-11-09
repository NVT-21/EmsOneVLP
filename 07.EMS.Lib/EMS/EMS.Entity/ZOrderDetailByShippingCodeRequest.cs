using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderDetailByShippingCodeRequest
    {
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public string AuthorizationCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderDetailByShippingCodeResponse
    {
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string TransportMainServiceName { get; set; }
        [DataMember]
        public string TransportExtraServiceCode { get; set; }
        [DataMember]
        public string TransportExtraServiceName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double ExchangeTransportWeight { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string PosCodeAccept { get; set; }
        [DataMember]
        public string PosNameAccept { get; set; }
        [DataMember]
        public string PosTelAccept { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public string PosCodeDelivery { get; set; }
        [DataMember]
        public string PosNameDelivery { get; set; }
        [DataMember]
        public string PosTelDelivery { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string StatusChar { get; set; }
        [DataMember]
        public List<ZOrderSearchHistoryResponse> ListSearchHistory { get; set; }
        [DataMember]
        public List<ZRequestSupportHistorySearchListUpdateInformationResponse> ListUpdateInformation { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ParentListID { get; set; }
        [DataMember]
        public int IsSercure { get; set; }
        [DataMember]
        public List<ZOrderSearchHistoryDeliveryResponse> ListSearchHistoryDelivery { get; set; }
        [DataMember]
        public List<ZEMSSignaruteAndImageInformationResponse> ListEMSSignaruteAndImageInfo { get; set; }
        [DataMember]
        public List<ZVNPSignaruteAndImageInformationResponse> ListVNPSignaruteAndImageInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderDetailByShippingCodeResponse
    {
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string TransportMainServiceName { get; set; }
        [DataMember]
        public string TransportExtraServiceCode { get; set; }
        [DataMember]
        public string TransportExtraServiceName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double ExchangeTransportWeight { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string PosCodeAccept { get; set; }
        [DataMember]
        public string PosNameAccept { get; set; }
        [DataMember]
        public string PosTelAccept { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public string PosCodeDelivery { get; set; }
        [DataMember]
        public string PosNameDelivery { get; set; }
        [DataMember]
        public string PosTelDelivery { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string StatusChar { get; set; }
        [DataMember]
        public List<ZOrderSearchHistoryResponse> ListSearchHistory { get; set; }
        [DataMember]
        public List<ZRequestSupportHistorySearchListUpdateInformationResponse> ListUpdateInformation { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ParentListID { get; set; }
        [DataMember]
        public List<ZOrderSearchHistoryDeliveryResponse> ListSearchHistoryDelivery { get; set; }
        [DataMember]
        public List<ZEMSSignaruteAndImageInformationResponse> ListEMSSignaruteAndImageInfo { get; set; }
        [DataMember]
        public List<ZVNPSignaruteAndImageInformationResponse> ListVNPSignaruteAndImageInfo { get; set; }
    }
}
