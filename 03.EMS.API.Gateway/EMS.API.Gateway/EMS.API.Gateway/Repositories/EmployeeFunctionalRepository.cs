using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class EmployeeFunctionalRepository
    {
        public static ZResponse ListFunctional(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalListRequest>(data);
            return XHelper.Invoke("POST", "EmployeeFunctional/ListFunctional", req);
        }
        public static ZResponse ChangeFunctional(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalChangeRequest>(data);
            return XHelper.Invoke("POST", "EmployeeFunctional/ChangeFunctional", req);
        }
        public static ZResponse CheckFunctional(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalCheckRequest>(data);
            return XHelper.Invoke("POST", "EmployeeFunctional/CheckFunctional", req);
        }
        public static ZResponse ListMenu(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalListMenuRequest>(data);
            return XHelper.Invoke("POST", "EmployeeFunctional/ListMenu", req);
        }
    }
}