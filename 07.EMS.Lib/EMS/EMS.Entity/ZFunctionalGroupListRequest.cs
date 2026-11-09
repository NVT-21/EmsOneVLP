using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFunctionalGroupListRequest
    {
        public int ShopID { get; set; }
        public int GroupPermissionID { get; set; }
        public string IsShop { get; set; }
        public string IsLock { get; set; }

    }
    public class ZFunctionalGroupListResponse
    {
        public int FunctionalGroupID { get; set; }
        public string FunctionalGroupCode { get; set; }
        public string FunctionalGroupName { get; set; }
        public int Value { get; set; }
        public List<ZFunctionalGroupListResponse> lstFunctionalGroup { get; set; }
        public List<ZFunctional> listFunctional { get; set; }
    }
    [Serializable]
    public class ZFunctional
    {
        public int FunctionalGroupID { get; set; }
        public int FunctionalID { get; set; }
        public string FunctionalCode { get; set; }
        public string FunctionalName { get; set; }
        public int Value { get; set; }
        public string IsSubFunctional { get; set; }
        public List<ZFunctional> ListFunctional { get; set; }
    }
    //public class ZFunctionalGroupListResponse
    //{
    //    public int ID { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public FunctionalGroup FunctionalGroup { get; set; }
    //    public int Value { get; set; }
    //}
    //Temp
    public class FunctionalGroup
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
