using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsTransport { get; set; }
        [DataMember]
        public string IsSendTransport { get; set; }
        //[DataMember]
        //public int ShopID { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        //[DataMember]
        //public int CreatedBy { get; set; }
        [DataMember]
        public ZPartnerOrderAddBuyerInfoRequest BuyerInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddSenderInfoRequest SenderInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddTransportInfoRequest TransportInfo { get; set; }
        [DataMember]
        public List<ZPartnerOrderAddProductInfoRequest> ProductInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddBuyerInfoRequest
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
    public class ZPartnerOrderAddSenderInfoRequest
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
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddReceiverInfoRequest
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
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddTransportInfoRequest
    {
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public string TransportExtraServiceID { get; set; } = "";
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long ExtraFee { get; set; }
        [DataMember]
        public long Surcharge { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }

        [DataMember]
        public long MainFeeNotVAT { get; set; }
        [DataMember]
        public long ExtraFeeNotVAT { get; set; }
        [DataMember]
        public long SurchargeNotVAT { get; set; }
        [DataMember]
        public long TotalAmountNotVAT { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public List<ZTransportPostageExtraChargedResponse> TransportPostageExtraChargedDetail { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddProductInfoRequest
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

    [Serializable]
    [DataContract]
    public class ZPartnerOrderAddResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public int TransportFee { get; set; }
        [DataMember]
        public ZTransportPostageChargedResponse TransportFeeDetail { get; set; }
    }
}
