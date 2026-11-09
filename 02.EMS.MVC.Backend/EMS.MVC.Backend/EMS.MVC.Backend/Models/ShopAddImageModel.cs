using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShopAddImageModel
    {
        public string ImageCode { get; set; }
        public string FileName { get; set; }
        public int ShopID { get; set; }
    }
}