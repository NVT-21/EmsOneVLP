using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class RequestSupportSearchProcessModel
    {
        public string CreatedDate { get; set; }
        public string CreatedName { get; set; }
        public string Description { get; set; }
        public string Result { get; set; }
        public string ResultDocument { get; set; }
        public BaseModel Type { get; set; }
    }
}