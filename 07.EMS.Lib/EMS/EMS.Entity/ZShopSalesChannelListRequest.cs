using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopSalesChannelListRequest
    {
        public int ID { get; set; }
    }
    public class ZShopSalesChannelListResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Channel { get; set; }
        public string AccountID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string CreatedDate { get; set; }
        public string EmployeeName { get; set; }
    }
}
