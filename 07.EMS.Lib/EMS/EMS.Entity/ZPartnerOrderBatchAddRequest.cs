using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
        [DataMember]
        public string ActionType { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public ZPartnerOrderBatchAddBuyerInfoRequest BuyerInfo { get; set; }
        [DataMember]
        public ZPartnerOrderBatchAddSenderInfoRequest SenderInfo { get; set; }
        [DataMember]
        public ZPartnerOrderBatchAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerOrderBatchAddTransportInfoRequest TransportInfo { get; set; }
        [DataMember]
        public List<ZPartnerOrderBatchAddOrderInfoRequest> ListOrderInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddBuyerInfoRequest
    {
        [DataMember]
        public int CustomerID { get; set; }
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
        public string IsUpdate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddSenderInfoRequest
    {
        [DataMember]
        public int PickUpAddressID { get; set; }
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
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddReceiverInfoRequest
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
    public class ZPartnerOrderBatchAddTransportInfoRequest
    {
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public string TransportExtraServiceID { get; set; } = "";
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddOrderInfoRequest
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
        public List<ZPartnerOrderBatchAddProductInfoRequest> ListProductInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderBatchAddProductInfoRequest
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
    }
}
