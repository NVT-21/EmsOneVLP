using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class AddReturnOrder3TViewModel
    {
        public CustomerModel Sender { get; set; }
        public CustomerModel Receiver { get; set; }
        public List<Receiver3TModel> ListReceiver3T { get; set; }
        public Receiver3TModel Receiver3T { get; set; }
        public string Channel { get; set; }
        public BaseModel CollectionType { get; set; }
        public string MobileNumber { get; set; }
        public string IsTransport { get; set; }
        public string ProductName { get; set; }
        public List<Product3TItemViewModel> ListProductName { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public double Weight { get; set; }
        public long Quantity { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        [AllowHtml]
        public string Note { get; set; }
        public int VariantID { get; set; }
        public int NumberOfProduct { get; set; }
        public long OrderValue { get; set; }
    }

    public class Receiver3TModel
    {
        public ProvinceModel Province { get; set; }
        public DistrictModel District { get; set; }
        public WardModel Ward { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string CrmOrPayPostCode { get; set; }
        public string ShortAddress { get; set; }
    }
}