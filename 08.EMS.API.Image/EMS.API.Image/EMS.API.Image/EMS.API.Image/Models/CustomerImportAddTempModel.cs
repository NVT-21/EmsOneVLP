using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Image.Models
{
    public class CustomerImportAddTempModel
    {
        public string ImportCode { get; set; }
        public List<CustomerImportAddTempDetailModel> ListCustomerImportAddTempDetail { get; set; }
        public int Type { get; set; }
        public string FileName { get; set; }
    }
    public class CustomerImportAddTempDetailModel
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string Sticker { get; set; }
        public string InformationError { get; set; }
        public string IsSuccess { get; set; }
    }
}