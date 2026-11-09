using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class DetailOrderBatchViewModel
    {
        public string BatchCode { get; set; }
        public CustomerModel BuyerInfo { get; set; }
        public CustomerModel ReceiverInfo { get; set; }
        public string WareHouseName { get; set; }
        public BaseModel CollectionType { get; set; }
        public BaseModel TransportPayer { get; set; }
        public BaseModel MainTransportServiceID { get; set; }
        public BaseModel ExtraTransportServiceID { get; set; }
        public long ProvisionalTotalCOD { get; set; }
        public long ActualTotalCOD { get; set; }
        public string Note { get; set; }
        public BaseModel Channel { get; set; }
        public string PlaceOfCreation { get; set; }

        public List<DetailOrderBatchOrderInfoModel> ListOrderInfo { get; set; }
    }

    public class DetailOrderBatchOrderInfoModel
    {
        public string ShippingCode { get; set; }
        public string EMSOneCode { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public BaseModel OrderStatus { get; set; }
        public long OrderQuantity { get; set; }
        public long OrderValue { get; set; }
        public long ProvisionalTransportFee { get; set; }
        public long ActualTransportFee { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double ProvisionalTransportWeight { get; set; }
        public double ActualTransportWeight { get; set; }
        public double ExchangeTransportWeight { get; set; }
        public long TotalPHU { get; set; }
        public string Message { get; set; }
        public string AcceptancePosCode { get; set; }
        public string ReasonCancel { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public long TotalAmount { get; set; }
        public long TotalProductQuantity { get; set; }
        public long TotalProductValue { get; set; }
        public long TotalOrderValue { get; set; }
        public long TotalDiscount { get; set; }

        public List<DetailOrderBatchProductInfoModel> ListProductInfo { get; set; }
        public List<RequestSupportModel> ListRequestSupportInfo { get; set; }
        public List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo { get; set; }
        public List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo { get; set; }
    }
    public class DetailOrderBatchProductInfoModel
    {
        public string ProductName { get; set; }
        public string VariantSKU { get; set; }
        public string VariantName { get; set; }
        public int NumberOfProduct { get; set; }
        public long Price { get; set; }
        public double Weight { get; set; }
        public double Discount { get; set; }
        public long TotalPrice { get; set; }
    }
}