using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEMSLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class ZEMSLoginResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
