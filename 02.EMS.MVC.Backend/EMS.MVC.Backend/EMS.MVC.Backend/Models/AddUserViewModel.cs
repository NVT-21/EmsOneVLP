using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class AddUserViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UserGroupCode { get; set; }
        public List<UserGroupModel> ListUserGroup { get; set; }
        public string UserCMS { get; set; }
        public string ServesCustomer { get; set; }
        public List<int> ListCollectionPostOfficeID { get; set; }
    }
}