using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchAddRequest
    {
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public ZOrderBatchAddSenderInfoRequest SenderInfo { get; set; }
        [DataMember]
        public ZOrderBatchAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public int TransportPayer { get; set; } = 1;
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public string TransportExtraServiceID { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public List<ZOrderBatchAddOrderInfoRequest> ListOrderInfo { get; set; }
        [DataMember]
        public string ActionType { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchAddSenderInfoRequest
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
        public int PickUpAddressID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchAddReceiverInfoRequest
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
        public string IsUpdatedCustomer { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchAddOrderInfoRequest
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public int OrderQuantity { get; set; } = 1;
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public long TotalDiscount { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double ExchangeWeight { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public List<ZOrderBatchAddProductInfoRequest> ListProductInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchAddProductInfoRequest
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Discount { get; set; }
    }
}
