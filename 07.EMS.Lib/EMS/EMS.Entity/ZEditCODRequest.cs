using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEditCODRequest
    {
        public string ParcelCode { get; set; }
        public double Amount { get; set; }
        public string PoCode { get; set; }
    }
}
