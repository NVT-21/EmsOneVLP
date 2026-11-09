using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Backend.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string WareHouseName { get; set; }
        public string Note { get; set; }
        public string Message { get; set; }
        public BaseModel CollectionType { get; set; }
        public BaseModel Transport { get; set; }
        public string OrderName { get; set; }
        public BaseModel MainTransportService { get; set; }
        public BaseModel ExtraTransportService { get; set; }
        public BaseModel TransportPayer { get; set; }
        public string TotalProductQuantity { get; set; }
        public string TotalProductValue { get; set; }
        public string TotalDiscount { get; set; }
        public string ProvisionalTransportFee { get; set; }
        public string ActualTransportFee { get; set; }
        public string TotalOrderValue { get; set; }
        public string TotalCOD { get; set; }
        public string ActualTotalCOD { get; set; }
        public string TotalPHU { get; set; }
        public string OrderQuantity { get; set; }
        public string OrderValue { get; set; }
        public string ProvisionalTransportWeight { get; set; }
        public string ActualTransportWeight { get; set; }
        public string ExchangeTransportWeight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public CustomerModel Buyer { get; set; }
        public CustomerModel Receiver { get; set; }
        public BaseModel OrderStatus { get; set; }
        public string ReasonCancel { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string ShippingCode { get; set; }
        public string AcceptancePosCode { get; set; }
        public List<RequestSupportModel> ListUpdateInformation { get; set; }
        public List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo { get; set; }
        public List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo { get; set; }
        public string PlaceOfCreation { get; set; }
        public string PostalType { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string CollectionPostOfficeRecentFullAddress { get; set; }
    }
}