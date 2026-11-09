using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeAddNewRequest
    {
        public string UserName { get; set; }
        //public string PassWord { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string EmployeeLevel { get; set; }
        public int CreatedBy { get; set; }
    }
}
