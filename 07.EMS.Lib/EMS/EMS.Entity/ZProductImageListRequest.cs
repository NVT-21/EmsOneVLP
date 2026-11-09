using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductImageListRequest
    {
        public int ProductID { get; set; }
    }
    public class ZProductImageListResponse
    {
        public int ID { get; set; }
        public string Image { get; set; }
    }
}
