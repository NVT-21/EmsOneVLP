using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportServiceModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public BaseModel SpecialService { get; set; }
        public string IsLock { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}