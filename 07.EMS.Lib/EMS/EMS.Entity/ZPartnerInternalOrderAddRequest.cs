using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    /// <summary>
    /// Request received from Partner/API customer.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ZPartnerInternalOrderAddRequest
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
        public decimal OrderValue { get; set; }

        [DataMember]
        public int OrderQuantity { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        [DataMember]
        public string GoodsType { get; set; }

        [DataMember]
        public ZPartnerInternalSenderInfoRequest SenderInfo { get; set; }

        [DataMember]
        public ZPartnerInternalReceiverInfoRequest ReceiverInfo { get; set; }

        [DataMember]
        public ZPartnerInternalTransportInfoRequest TransportInfo { get; set; }

        [DataMember]
        public ZPartnerInternalCustomsInfoRequest CustomsInfo { get; set; }

        [DataMember]
        public List<ZPartnerInternalProductItemRequest> ProductList { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ZPartnerInternalSenderInfoRequest
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
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ZPartnerInternalReceiverInfoRequest
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string ProvinceName { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Street { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ZPartnerInternalTransportInfoRequest
    {
        [DataMember]
        public double TransportWeight { get; set; }

        [DataMember]
        public double TransportSizeLength { get; set; }

        [DataMember]
        public double TransportSizeWidth { get; set; }

        [DataMember]
        public double TransportSizeHeight { get; set; }

        /// <summary>
        /// Customer only sends Service ID.
        /// ServiceCode and ServiceName are resolved internally.
        /// </summary>
        [DataMember]
        public int TransportMainServiceID { get; set; }

        [DataMember]
        public string TransportExtraServiceID { get; set; }

        [DataMember]
        public int CollectionType { get; set; }

        [DataMember]
        public int TransportPayer { get; set; }

        [DataMember]
        public long TransportFee { get; set; }

        [DataMember]
        public long MainFee { get; set; }

        [DataMember]
        public long ExtraFee { get; set; }

        [DataMember]
        public long SurchargeFee { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ZPartnerInternalCustomsInfoRequest
    {
        [DataMember]
        public decimal DeclareValue { get; set; }

        [DataMember]
        public string DeclareCurrency { get; set; }

        [DataMember]
        public string CustomsGoodsType { get; set; }

        [DataMember]
        public string OriginCountryCode { get; set; }

        [DataMember]
        public string OriginCountryName { get; set; }

        [DataMember]
        public string IsAgreeCustoms { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ZPartnerInternalProductItemRequest
    {
        [DataMember]
        public string DescriptionVI { get; set; }

        [DataMember]
        public string DescriptionEN { get; set; }

        [DataMember]
        public string HSCode { get; set; }

        [DataMember]
        public decimal DeclareValue { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        [DataMember]
        public double Weight { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string OriginCountryCode { get; set; }
    }
}
