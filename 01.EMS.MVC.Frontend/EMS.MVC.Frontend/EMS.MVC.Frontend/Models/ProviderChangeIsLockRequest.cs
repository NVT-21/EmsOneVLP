using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProviderChangeIsLockRequest
    {

        public int ID { get; set; }
        public string IsLock { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}