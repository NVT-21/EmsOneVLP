using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ForgetPasswordSuccessViewModel
    {
        public string Email { get; set; }
        public int OtpID { get; set; }
        public string OtpCode { get; set; }
        public int EmployeeID { get; set; }
        public string MobileNumber { get; set; }
    }
}