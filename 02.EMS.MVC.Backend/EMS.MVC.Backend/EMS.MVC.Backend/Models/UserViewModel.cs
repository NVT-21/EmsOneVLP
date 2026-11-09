using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class UserViewModel
    {
        public string UserGroupCode { get; set; }
        public List<UserGroupModel> ListUserGroup { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }
}