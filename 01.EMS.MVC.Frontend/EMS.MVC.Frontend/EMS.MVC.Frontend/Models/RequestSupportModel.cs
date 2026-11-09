using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class RequestSupportModel
    {
        public BaseModel RequestSupportType { get; set; }
        public string OldUpdateInformation { get; set; }
        public string NewUpdateInformation { get; set; }
        public string CreatedDate { get; set; }
    }
}