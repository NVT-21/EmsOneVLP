using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.PayPost.Core.Models
{
    public class EditCODRequest
    {
        public string ParcelCode { get; set; }
        public double Amount { get; set; }
        public string PoCode { get; set; }
    }
}