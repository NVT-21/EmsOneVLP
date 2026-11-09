using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models
{
    public class ResponseEntity
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}