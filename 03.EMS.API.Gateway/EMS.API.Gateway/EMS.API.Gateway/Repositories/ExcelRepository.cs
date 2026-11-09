using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ExcelRepository
    {
        public static ZResponse AddImportExcel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZImportExcelRequest>(data);
            return XHelper.Invoke("POST", "Excel/AddImportExcel", req);
        }
    }
}