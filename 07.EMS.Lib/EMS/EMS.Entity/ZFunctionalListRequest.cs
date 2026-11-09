using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFunctionalListRequest
    {
        public int ShopID { get; set; }
        public int GroupPermissionID { get; set; }
    }
    public class ZFunctionalListResponse
    {
        public int ID { get; set; }
        public string FunctionalCode { get; set; }
        public string FunctionalName { get; set; }
        public int FunctionalLevel { get; set; }
        public int FunctionalParentID { get; set; }
        public int IsValue { get; set; }
        public string MenuIcon { get; set; }
        public int IsType { get; set; }
        public List<ZFunctionalListResponse> ListFunctional { get; set; }
    }
}
