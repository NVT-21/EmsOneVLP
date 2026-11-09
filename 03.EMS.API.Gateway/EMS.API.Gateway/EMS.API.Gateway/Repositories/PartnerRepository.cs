using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class PartnerRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerSearchListRequest>(data);
            return XHelper.Invoke("POST", "Partner/SearchList", req);
        }
    }
}