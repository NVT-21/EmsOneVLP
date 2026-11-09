using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class PermissionRepository
    {
        public static ZResponse ListPermission(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPermissionListRequest>(data);
            return XHelper.Invoke("POST", "Permission/List", req);
        }
        //public static ZResponse ListFunctionalGroup(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZFunctionalGroupListRequest>(data);
        //    return XHelper.Invoke("POST", "Permission/ListFunctionalGroup", req);
        //}
        public static ZResponse ChangePermission(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPermissionChangeRequest>(data);
            return XHelper.Invoke("POST", "Permission/ChangePermission", req);
        }
        public static ZResponse ListFunctional(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalListRequest>(data);
            return XHelper.Invoke("POST", "Permission/ListFunctional", req);
        }
        //public static ZResponse MenuLeft(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZPermissionListMenuRequest>(data);
        //    return XHelper.Invoke("POST", "Permission/ListMenu", req);
        //}
        public static ZResponse ListMenu(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPermissionMenuListRequest>(data);
            return XHelper.Invoke("POST", "Permission/ListMenu", req);
        }
        public static ZResponse ListAllow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPermissionAllowListRequest>(data);
            return XHelper.Invoke("POST", "Permission/ListAllow", req);
        }
    }
}