using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFunctionalListMenuRequest
    {
        public int ShopID { get; set; }
        public int EmployeeID { get; set; }
    }
    public class ZFunctionalListMenuResponse
    {
        public int ID { get; set; }
        public string FunctionalCode { get; set; }
        public string FunctionalName { get; set; }
        public int FunctionalLevel { get; set; }
        public int FunctionalParentID { get; set; }
        public string FunctionalTitle { get; set; }
        public int IsValue { get; set; }
        public string MenuIcon { get; set; }
        public int IsType { get; set; }
        public List<ZFunctionalListMenuResponse> ListFunctional { get; set; }
    }
}
