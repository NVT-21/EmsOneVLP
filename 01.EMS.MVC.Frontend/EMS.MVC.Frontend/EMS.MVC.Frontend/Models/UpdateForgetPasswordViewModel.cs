using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class UpdateForgetPasswordViewModel
    {
        public int EmployeeID { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OtpCode { get; set; }
    }
}