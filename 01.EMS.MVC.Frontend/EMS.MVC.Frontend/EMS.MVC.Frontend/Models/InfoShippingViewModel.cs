using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class InfoShippingViewModel
    {
        public int OrderID { get; set; }
        public string ShippingCode { get; set; }
        public string[] ListShippingCode { get; set; }
        public string OrderCode { get; set; }
        public BaseModel MainTransportService { get; set; }
        public BaseModel ExtraTransportService { get; set; }
        public string CreatedDate { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string TransportWeight { get; set; }
        public string ExchangeTransportWeight { get; set; }
        public string TotalCOD { get; set; }
        public string TotalPHU { get; set; }
        public string Message { get; set; }
        public string PosCodeAccept { get; set; }
        public string PosNameAccept { get; set; }
        public string PosTelAccept { get; set; }
        public string PosCodeDelivery { get; set; }
        public string PosNameDelivery { get; set; }
        public string PosTelDelivery { get; set; }
        public CustomerModel Sender { get; set; }
        public CustomerModel Receiver { get; set; }
        public BaseModel OrderStatus { get; set; }
        public List<OrderHistoryModel> ListSearchHistory { get; set; }
        public List<RequestSupportModel> ListUpdateInformation { get; set; }
        public List<ZOrderSearchHistoryDeliveryResponse> ListSearchHistoryDelivery { get; set; }
        public List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo { get; set; }
        public List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo { get; set; }
    }
}