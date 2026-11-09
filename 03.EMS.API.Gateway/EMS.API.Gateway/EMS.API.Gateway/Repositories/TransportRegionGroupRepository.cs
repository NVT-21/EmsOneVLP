using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class TransportRegionGroupRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportRegionGroupSearchListRequest>(data);
            return XHelper.Invoke("POST", "TransportRegionGroup/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportRegionGroupAddRequest>(data);
            return XHelper.Invoke("POST", "TransportRegionGroup/Add", req);
        }
    }
}