using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOne
{
    public class EMSOneGetDataForControlFreightRequest
    {
        [Required]
        public string CustomerCode { get; set; }

        public int FromDate { get; set; }

        public int ToDate { get; set; }
    }
}