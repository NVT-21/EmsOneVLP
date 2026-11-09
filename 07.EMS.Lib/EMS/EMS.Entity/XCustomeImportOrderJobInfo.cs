using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XCustomeImportOrderJobInfo
    {
        public long JobId { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public int TotalRows { get; set; }
        public int SuccessRows { get; set; }
        public int FailedRows { get; set; }
        public string CreatedAt { get; set; }
    }
}
