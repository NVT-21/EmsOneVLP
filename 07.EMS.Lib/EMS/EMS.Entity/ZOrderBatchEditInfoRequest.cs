using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoRequest
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoResponse
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public ZOrderBatchEditInfoBuyerInfoResponse BuyerInfo { get; set; }
        [DataMember]
        public ZOrderBatchEditInfoSenderInfoResponse SenderInfo { get; set; }
        [DataMember]
        public ZOrderBatchEditInfoReceiverInfoResponse ReceiverInfo { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public int TransportPayer { get; set; } = 1;
        [DataMember]
        public int MainTransportServiceID { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public List<ZOrderBatchEditInfoOrderInfoResponse> ListOrderInfo { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoBuyerInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoSenderInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int PickUpAddressID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoReceiverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string IsUpdatedCustomer { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoOrderInfoResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public int OrderQuantity { get; set; } = 1;
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public List<ZOrderBatchEditInfoOrderInfoProductInfoResponse> ListProductInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchEditInfoOrderInfoProductInfoResponse
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public string VariantName { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public long Discount { get; set; }
    }
}
