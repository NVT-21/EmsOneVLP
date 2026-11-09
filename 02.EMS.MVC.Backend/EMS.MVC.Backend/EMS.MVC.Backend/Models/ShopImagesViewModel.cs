using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShopImagesViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ZImageModel> Images { get; set; }
        public int ShopID { get; set; }
    }

}