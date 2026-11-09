using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    /// <summary>
    /// Internal model used to create an international order in database.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ZInternationalOrderAddRequest
    {
        #region Order

        [DataMember]
        public string OrderCode { get; set; }

        [DataMember]
        public string OrderName { get; set; }

        [DataMember]
        public string Channel { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public string Message { get; set; }

        #endregion


        #region Customer

        [DataMember]
        public int CustomerID { get; set; }

        #endregion


        #region Receiver

        [DataMember]
        public string ReceiverName { get; set; }

        [DataMember]
        public string ReceiverPhone { get; set; }

        [DataMember]
        public string ReceiverAddress { get; set; }

        [DataMember]
        public string ReceiverCountryCode { get; set; }

        [DataMember]
        public string ReceiverCountryName { get; set; }

        [DataMember]
        public string ReceiverCity { get; set; }

        [DataMember]
        public string ReceiverZipCode { get; set; }

        #endregion


        #region Sender

        [DataMember]
        public string SenderName { get; set; }

        [DataMember]
        public string SenderPhone { get; set; }

        [DataMember]
        public int SenderProvinceID { get; set; }

        [DataMember]
        public int SenderWardID { get; set; }

        [DataMember]
        public string SenderStreet { get; set; }

        #endregion


        #region Service

        [DataMember]
        public int ServiceID { get; set; }

        [DataMember]
        public string GoodsType { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        #endregion


        #region Package

        [DataMember]
        public double TotalWeight { get; set; }

        [DataMember]
        public double Length { get; set; }

        [DataMember]
        public double Width { get; set; }

        [DataMember]
        public double Height { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        #endregion


        #region Customs

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

        #endregion


        #region Fee

        [DataMember]
        public long TransportFee { get; set; }

        [DataMember]
        public long MainFee { get; set; }

        [DataMember]
        public long ExtraFee { get; set; }

        [DataMember]
        public long SurchargeFee { get; set; }

        #endregion


        #region Products

        [DataMember]
        public List<ZInternationalOrderProductRequest> ProductList { get; set; }

        #endregion


        #region System

        [DataMember]
        public int ShopID { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string DeviceName { get; set; }

        #endregion
    }


    [Serializable]
    [DataContract]
    public class ZInternationalOrderProductRequest
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


    [Serializable]
    [DataContract]
    public class ZInternationalOrderAddResponse
    {
        [DataMember]
        public int OrderID { get; set; }

        [DataMember]
        public string OrderCode { get; set; }

        [DataMember]
        public string EMSOneCode { get; set; }

        [DataMember]
        public string EMSBarcode { get; set; }
    }
}
