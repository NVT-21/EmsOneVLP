using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class TemplatePrintDataViewModel
    {
        public int TemplatePrintID { get; set; }
        public string SenderName { get; set; } = "N";
        public string SenderPhone { get; set; } = "N";
        public string SenderProvince { get; set; } = "N";
        public string SenderDistrict { get; set; } = "N";
        public string SenderAddress { get; set; } = "N";
        public string ReceiverName { get; set; } = "N";
        public string ReceiverPhone { get; set; } = "N";
        public string ReceiverProvince { get; set; } = "N";
        public string ReceiverDistrict { get; set; } = "N";
        public string ReceiverAddress { get; set; } = "N";
        public string OrderCode { get; set; } = "N";
        public string Message { get; set; } = "N";
        public string Note { get; set; } = "N";
        public string Weight { get; set; } = "N";
        public string Quantity { get; set; } = "N";
        public string TotalCOD { get; set; } = "N";
        public string TransportFee { get; set; } = "N";
        public string Title { get; set; } = "N";
        public string PostOffice { get; set; } = "N";
        public string TotalAmount { get; set; } = "N";
        public string PrintType { get; set; } = "N";
        public string TransportMainServiceCode { get; set; } = "N";
        public string TransportMainServiceName { get; set; } = "N";
        public string TransportExtraServiceCode { get; set; } = "N";
    }
}