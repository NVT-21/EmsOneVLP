using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserResetPasswordRequest
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int UpdatedBy { get; set; }
    }
}
