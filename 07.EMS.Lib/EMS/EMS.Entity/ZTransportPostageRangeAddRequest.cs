using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportPostageRangeAddRequest
    {
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int FromRange { get; set; }
        public int FromRangeValue { get; set; }
        public int ToRange { get; set; }
        public int ToRangeValue { get; set; }
        public string TwoWay { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public List<XPostageWeight> Weight { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZTransportPostageRangeAddResponse
    {
        public int ID { get; set; }
    }

    public class XPostageWeight
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int ParentID { get; set; }
        public int FromWeight { get; set; }
        public int ToWeight { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
    }
}
