using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderGetByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZRequestOrderGetByIDResponse
    {
        public int ID { get; set; }
        public int WareHouseID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Channel { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Product { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string Transport { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public int Status { get; set; }
        public string BlackList { get; set; }
        public int CustomerID { get; set; }
        public string TransportOrderName { get; set; }
        public int TransportWeight { get; set; }
        public string TransportSize { get; set; }
        public int TransportQuantity { get; set; }
        public int TransportOrderValue { get; set; }
        public int TransportMainService { get; set; }
        public string TransportExtraService { get; set; }
    }
}
