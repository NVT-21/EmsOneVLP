using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class CustomerModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Channel { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
        public CustomerStickerModel CustomerSticker { get; set; }
        public CustomerGroupModel CustomerGroup { get; set; }
        public ProvinceModel Province { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public DistrictModel District { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public WardModel Ward { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string BlackList { get; set; }
        public string SuccessOrder { get; set; } // Số đơn hàng thành công
        public string TotalOrder { get; set; } // Tổng đơn hàng đã đặt
        public string TotalAmount { get; set; } // Tổng tiền
        public int PickUpAddressID { get; set; } // Địa chỉ lấy hàng  
    }
}