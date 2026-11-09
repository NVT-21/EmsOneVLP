using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class CustomerStickerModel
    {
        public int ID { get; set; }        
        public string Code { get; set; }
        public string Name { get; set; }
        public string CreatedDate { get; set; }
    }
}