using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportPostageWeightAddRequest
    {
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int ParentID { get; set; }
        public int FromWeight { get; set; }
        public int ToWeight { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZTransportPostageWeightAddResponse
    {
        public int ID { get; set; }
    }

}
