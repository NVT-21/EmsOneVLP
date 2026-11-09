using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderListRequest
    {
        public int ShopID { get; set; }
        public int RecieverID { get; set; }
        public string StoreOwner { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string ProcessDivision { get; set; }
        public string MobileNumber { get; set; }
    }
    public class ZRequestOrderListResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Channel { get; set; }
        public string Address { get; set; }
        public List<string> ProductName { get; set; }
        public string Product { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string Transport { get; set; }
        public int PaymentMethod { get; set; }
        public int RecieverID { get; set; }
        public string RecieverName { get; set; }
        public string ProcessDivisionRequest { get; set; }
        public string Status { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Reason { get; set; }
    }
}
