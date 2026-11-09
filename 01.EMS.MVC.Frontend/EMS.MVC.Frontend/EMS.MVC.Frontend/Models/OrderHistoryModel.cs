using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class OrderHistoryModel
    {
        public int OrderID { get; set; }
        public string StatusName { get; set; }
        public string StatusChar { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string CreatedDate { get; set; }
        public string StatusTimePartner { get; set; }
        public string PosCode { get; set; }
        public string PosName { get; set; }
        public string PosAddress { get; set; }
        public string PosTel { get; set; }
        public string StatusNote { get; set; }
    }
}