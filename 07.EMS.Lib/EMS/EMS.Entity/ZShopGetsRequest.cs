using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGetsRequest
    {
        public int CreateBy { get; set; }
    }
    public class ZShopGetsResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
