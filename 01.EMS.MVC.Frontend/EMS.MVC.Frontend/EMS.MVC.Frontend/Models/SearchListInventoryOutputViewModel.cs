using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListInventoryOutputViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public OrderModel Order { get; set; }
        public BaseModel Status { get; set; }
        public string ReceiverName { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdaterName { get; set; }
        public string UpdatedDate { get; set; }
        public int ShopID { get; set; }
        public string SearchKey { get; set; }
        public string ListDataInventoryOutputPrint { get; set; }
    }
}