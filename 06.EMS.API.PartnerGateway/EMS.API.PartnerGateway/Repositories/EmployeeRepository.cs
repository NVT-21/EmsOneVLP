using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.PartnerGateway.Repositories
{
    public class EmployeeRepository
    {
        public static ZResponse EmployeeLogin(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeLoginRequest>(data);
            return XHelper.Invoke("POST", "Employee/LoginAPI", req);
        }
    }
}