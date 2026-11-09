using EMS.Entity;
using Newtonsoft.Json;
namespace EMS.API.Gateway.Repositories
{
    public class TransportRegionRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportRegionSearchListRequest>(data);
            return XHelper.Invoke("POST", "TransportRegion/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportRegionAddRequest>(data);
            return XHelper.Invoke("POST", "TransportRegion/Add", req);
        }
        public static ZResponse GetByParentID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportRegionGetByParentIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("TransportRegion/GetByParentID?ParentID={0}", req.ParentID), null);
        }
    }
}