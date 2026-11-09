using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class EmployeeLevelRepository
    {
        public static ZResponse Gets()
        {
            return XHelper.Invoke("GET", "EmployeeLevel/Gets", null);
        }
    }
}