using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPermissionListMenuRequest
    {
        public int EmployeeID { get; set; }
    }
    public class ZPermissionListMenuResponse
    {
        public int ParentMenuID { get; set; }
        public string ParentMenuCode { get; set; }
        public string ParentMenuName { get; set; }
        public string ParentMenuIcon { get; set; }
        public string ParentMenuIsSpecial { get; set; }
        public List<ZListSubMenu> listSubMenu { get; set; }
        public List<ZFunctional> listFunctional { get; set; }
    }
    [Serializable]
    public class ZListSubMenu
    {
        public int ParentMenuID { get; set; }
        public int SubMenuID { get; set; }
        public string SubMenuCode { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuIcon { get; set; }
        public int Value { get; set; }
    }
}
