using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class CustomerGroupViewModel
    {
        public int CustomerGroupID { get; set; }
        public List<CustomerGroupModel> ListCustomerGroup { get; set; }
        public string SearchKey { get; set; }



        public int Index { get; set; }
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string Sticker { get; set; }
        public string strStickerID { get; set; }
        public string strStickerNew { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int OrderSuccess { get; set; }
        public int PurchaseMoney { get; set; }
        public string BlackList { get; set; }
        public int CreatedBy { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public int CountOrderSuccess { get; set; }
        public long TotalAmountSuccess { get; set; }
    }
}