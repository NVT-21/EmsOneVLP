using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ForgetPasswordViewModel
    {
        public string Email { get; set; }
        public int OtpID { get; set; }
    }
}