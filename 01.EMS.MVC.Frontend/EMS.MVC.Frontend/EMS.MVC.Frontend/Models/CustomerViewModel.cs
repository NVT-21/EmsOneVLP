using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class CustomerViewModel
    {
        public string AllDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchKey { get; set; }
        public int ProvinceID { get; set; }        
        public List<ProvinceModel> ListProvince { get; set; }
        public string CustomerGroup { get; set; }
        public List<CustomerGroupModel> ListCustomerGroup { get; set; }
        public int CustomerSticker { get; set; }
        public List<CustomerStickerModel> ListCustomerSticker { get; set; }
    }
}