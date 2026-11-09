using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopEditRequest
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }

    }
}
