using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeEditTimeActiveRequest
    {
        public int EmployeeID { get; set; }
        public List<Time> ListMonday { get; set; }
        public List<Time> ListTuesday { get; set; }
        public List<Time> ListWednesday { get; set; }
        public List<Time> ListThursday { get; set; }
        public List<Time> ListFriday { get; set; }
        public List<Time> ListSaturday { get; set; }
        public List<Time> ListSunday { get; set; }
    }
}
