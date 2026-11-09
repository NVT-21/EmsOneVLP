using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class NotifyRepository
    {
        public static ZResponse NotifyList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyListRequest>(data);
            return XHelper.Invoke("POST", "Notify/NotifyList", req);
        }
        public static ZResponse NotifyUpdate(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyUpdateRequest>(data);
            return XHelper.Invoke("POST", "Notify/NotifyUpdate", req);
        }
        public static ZResponse NotifyRemove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyUpdateRequest>(data);
            return XHelper.Invoke("POST", "Notify/NotifyRemove", req);
        }
        public static ZResponse NotifyGetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZNotifyGetByIDRequest>(data);
            return XHelper.Invoke("POST", "Notify/NotifyGetByID", req);
        }
    }
}