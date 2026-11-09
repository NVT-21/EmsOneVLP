using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZImportExcelRequest
    {
        public string IsReplace { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
        public string FileName { get; set; }
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public string FunctionName { get; set; }
        public string ImportVersion { get; set; }
    }
    public class ZImportExcelResponse
    {
        public string ImportCode { get; set; }
    }
}
