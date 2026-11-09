using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class LoginViewModel
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Message { get; set; }
    }
}