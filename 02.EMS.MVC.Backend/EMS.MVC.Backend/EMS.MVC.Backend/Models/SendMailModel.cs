using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class SendMailModel
    {
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Password { get; set; }
        public string ToEmail { get; set; }
        public string MobileNumber { get; set; }
    }
    //public class SendMailResponse
    //{
    //    public int IDShop { get; set; }
    //    public string FullName { get; set; }
    //    public string StoreName { get; set; }
    //    public string Password { get; set; }
    //    public string ToEmail { get; set; }
    //    public string MobileNumber { get; set; }
    //}
}