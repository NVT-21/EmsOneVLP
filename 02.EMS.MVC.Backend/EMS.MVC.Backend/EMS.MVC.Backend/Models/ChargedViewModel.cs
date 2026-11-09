using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ChargedViewModel
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public long MainFee { get; set; }
        public long ExtraFee { get; set; }
        public long Surcharge { get; set; }
        public long TotalAmount { get; set; }
    }
}