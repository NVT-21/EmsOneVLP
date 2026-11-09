using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeRequest
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeResponse
    {
        [DataMember]
        public ZOrderBatchDetailByBatchCodeBuyerInfoResponse BuyerInfo { get; set; }
        [DataMember]
        public ZOrderBatchDetailByBatchCodeReceiverInfoResponse ReceiverInfo { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public ZBaseResponse CollectionType { get; set; }
        [DataMember]
        public ZBaseResponse TransportPayer { get; set; }
        [DataMember]
        public ZBaseResponse MainTransportServiceID { get; set; }
        [DataMember]
        public ZBaseResponse ExtraTransportServiceID { get; set; }
        [DataMember]
        public long ProvisionalTotalCOD { get; set; }
        [DataMember]
        public long ActualTotalCOD { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public ZBaseResponse Channel { get; set; }
        [DataMember]
        public string PlaceOfCreation { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public List<ZOrderBatchDetailByBatchCodeOrderInfoResponse> ListOrderInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeBuyerInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeReceiverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeOrderInfoResponse
    {
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public ZBaseResponse OrderStatus { get; set; }
        [DataMember]
        public int OrderQuantity { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long ProvisionalTransportFee { get; set; }
        [DataMember]
        public long ActualTransportFee { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public double ProvisionalTransportWeight { get; set; }
        [DataMember]
        public double ActualTransportWeight { get; set; }
        [DataMember]
        public double ExchangeTransportWeight { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string AcceptancePosCode { get; set; }
        [DataMember]
        public string ReasonCancel { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }

        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public long TotalProductQuantity { get; set; }
        [DataMember]
        public long TotalProductValue { get; set; }
        [DataMember]
        public long TotalOrderValue { get; set; }
        [DataMember]
        public long TotalDiscount { get; set; }
        [DataMember]
        public List<ZOrderBatchDetailByBatchCodeOrderInfoProductInfoResponse> ListProductInfo { get; set; }

        [DataMember]
        public List<ZRequestSupportHistorySearchListUpdateInformationResponse> ListRequestSupportInfo { get; set; }

        [DataMember]
        public List<ZEMSSignaruteAndImageInformationResponse> ListEMSSignaruteAndImageInfo { get; set; }
        [DataMember]
        public List<ZVNPSignaruteAndImageInformationResponse> ListVNPSignaruteAndImageInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchDetailByBatchCodeOrderInfoProductInfoResponse
    {
        //[DataMember]
        //public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        //[DataMember]
        //public int VariantID { get; set; }
        [DataMember]
        public string VariantName { get; set; }
        [DataMember]
        public string VariantSKU { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public long Discount { get; set; }
        [DataMember]
        public long TotalPrice { get; set; }
    }

}
