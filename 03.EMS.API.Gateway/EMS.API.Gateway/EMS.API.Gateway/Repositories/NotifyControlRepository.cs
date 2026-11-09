using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class NotifyControlRepository
    {
        public static ZResponse NotifyAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlAddRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyAdd", req);
        }
        public static ZResponse NotifyList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlListRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyList", req);
        }
        public static ZResponse NotifyChangeLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlChangeLockRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyChangeLock", req);
        }
        public static ZResponse NotifyRemove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlRemoveRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyRemove", req);
        }
        public static ZResponse NotifyGetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlGetByIDRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyGetByID", req);
        }
        public static ZResponse NotifyEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyControlEditRequest>(data);
            return XHelper.Invoke("POST", "NotifyControl/NotifyEdit", req);
        }
    }
}
