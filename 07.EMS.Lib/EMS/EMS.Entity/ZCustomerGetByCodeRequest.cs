using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerGetByCodeRequest
    {
        public string CustomerCode { get; set; }
    }

    public class ZCustomerGetByCodeResponse
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string GroupName { get; set; }
    }
}
