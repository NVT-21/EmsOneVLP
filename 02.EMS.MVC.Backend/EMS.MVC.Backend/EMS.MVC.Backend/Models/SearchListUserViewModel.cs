using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class SearchListUserViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public UserGroupModel UserGroup { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string IsLock { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string SearchKey { get; set; }
        public string LastLogin { get; set; }
    }
}