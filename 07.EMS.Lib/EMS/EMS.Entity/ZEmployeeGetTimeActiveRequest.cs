using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGetTimeActiveRequest
    {
        public int EmployeeID { get; set; }
    }
    public class ZEmployeeGetTimeActiveResponse
    {
        public List<DayTimeActive> ListMonday { get; set; }
        public List<DayTimeActive> ListTuesday { get; set; }
        public List<DayTimeActive> ListWednesday { get; set; }
        public List<DayTimeActive> ListThursday { get; set; }
        public List<DayTimeActive> ListFriday { get; set; }
        public List<DayTimeActive> ListSaturday { get; set; }
        public List<DayTimeActive> ListSunday { get; set; }
    }
    public class DayTimeActive
    {
        public int ID { get; set; }
        public int Day { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string IsChoose { get; set; }
    }
}
