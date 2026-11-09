using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOptionAddRequest
    {
        public int ShopID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Values { get; set; } /*Chuỗi giá trị*/
        public int Sort { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZOptionAddResponse
    {
        public int ID { get; set; }
    }
}
