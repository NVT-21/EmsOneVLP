using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddPromotionCodeViewModel
    {
        public int CustomerID { get; set; }
        public List<CustomerModel> ListCustomer { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int CustomerGroupID { get; set; }
        public List<CustomerGroupModel> ListCustomerGroup { get; set; }
        public int ProductGroupID { get; set; }
        public List<ProductGroupModel> ListProductGroup { get; set; }

    }
}