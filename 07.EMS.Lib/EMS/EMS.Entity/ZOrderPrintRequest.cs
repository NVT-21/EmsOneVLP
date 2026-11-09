using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderPrintRequest
    {
        [DataMember]
        public string EMSOneCode { get; set; }
        //[DataMember]
        //public string OrderCode { get; set; }
        //[DataMember]
        //public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PrintType { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderSinglePrintRequest
    {
        [DataMember]
        public string ShippingCde { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string PrintType { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderPrintResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string PickUpProvinceName { get; set; }
        [DataMember]
        public string SenderFullName { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public string SenderDistrictName { get; set; }
        [DataMember]
        public string SenderProvinceName { get; set; }
        [DataMember]
        public string SenderMobileNumber { get; set; }
        [DataMember]
        public string SenderSentDate { get; set; }
        [DataMember]
        public string ReceiverFullName { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public string ReceiverDistrictName { get; set; }
        [DataMember]
        public string ReceiverProvinceName { get; set; }
        [DataMember]
        public string ReceiverMobileNumber { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string TransportWeight { get; set; }
        [DataMember]
        public string TransportQuantity { get; set; }
        [DataMember]
        public string TransportMainServiceCode { get; set; }
        [DataMember]
        public string TransportMainServiceName { get; set; }
        [DataMember]
        public string TransportExtraServiceCode { get; set; }
        [DataMember]
        public string TransportExtraServiceName { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public long PaymentFreightVat { get; set; }
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public byte[] QRCode { get; set; }
        [DataMember]
        public byte[] ShippingBarCode { get; set; }
        [DataMember]
        public byte[] OrderBarCode { get; set; }
        [DataMember]
        public string PostOfficeCode { get; set; }
        [DataMember]
        public string PostOfficeName { get; set; }
        [DataMember]
        public string PostOffice { get; set; }
    }
}
