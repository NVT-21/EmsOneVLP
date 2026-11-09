using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderSendTransportInfoRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public string AmndState { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderSendTransportInfoResponse
    {
        [DataMember]
        public string CrmOrPayPostCode { get; set; }
        [DataMember]
        public string ShopPhone { get; set; }
        [DataMember]
        public string ShopStoreName { get; set; }
        [DataMember]
        public string ShopFullName { get; set; }
        [DataMember]
        public string ShopEmail { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }


        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderCode { get; set; }
        [DataMember]
        public string SenderStoreName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderStreet { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public int SenderProvinceID { get; set; }
        [DataMember]
        public int SenderDistrictID { get; set; }
        [DataMember]
        public int SenderWardID { get; set; }
        [DataMember]
        public string SenderEmail { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public int ReceiverProvinceID { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public int ReceiverDistrictID { get; set; }
        [DataMember]
        public int ReceiverWardID { get; set; }
        [DataMember]
        public string ReceiverStreet { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public long ProductValue { get; set; }
        [DataMember]
        public string ProductItem { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public string ProductNote { get; set; }
        [DataMember]
        public int ProductQuantity { get; set; }
        [DataMember]
        public int Weight  { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public long ServiceFee { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public string Transport { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string EspeciallyServiceCode { get; set; }
        [DataMember]
        public int EspeciallyServiceType { get; set; }
        [DataMember]
        public ZBaseResponse Classification { get; set; }

        [DataMember]
        public string AdditionService { get; set; }

        [DataMember]
        public string POCode { get; set; }

        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public ZBaseResponse Consignment { get; set; }

        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public int TransportSizeLength { get; set; }
        [DataMember]
        public int TransportSizeWidth { get; set; }
        [DataMember]
        public int TransportSizeHeight { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int TransportPayer { get; set; }
    }
}
