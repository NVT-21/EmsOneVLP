using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddWorkingTimeDetailRequest
    {
        public int IDWorkingTime { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ZFacebookEditWorkingTimeDetailRequest
    {
        public int ID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZFacebookDeleteWorkingTimeDetailRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZFacebookSearchWorkingTimeDetailRequest
    {
        public int ID { get; set; }
        public string Day { get; set; }
    }

    public class ZFacebookSearchWorkingTimeDetailResponse
    {
        public int ID { get; set; }
        public int WorkingTimeID { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

}
