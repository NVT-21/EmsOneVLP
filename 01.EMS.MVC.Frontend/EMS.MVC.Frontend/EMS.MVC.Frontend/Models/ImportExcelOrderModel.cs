using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelOrderModel
    {
        public string Product { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string Transport { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public string CollectionType { get; set; }
        public string TransportMainService { get; set; }
        public string TransportExtraService { get; set; }
        public int PickUpAddressID { get; set; }
        public string SenderName { get; set; }
        public string SenderMobileNumber { get; set; }
        public int SenderProvinceID { get; set; }
        public string SenderProvinceName { get; set; }
        public int SenderDistrictID { get; set; }
        public string SenderDistrictName { get; set; }
        public int SenderWardID { get; set; }
        public string SenderWardName { get; set; }
        public string SenderStreet { get; set; }
        public string SenderFullAddress { get; set; }
        public string TransportPayer { get; set; }
        public string Long { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string NumberOfProduct { get; set; }
        public string TotalCOD { get; set; }
        public string TotalPHU { get; set; }
        public string OrderValue { get; set; }
    }
}