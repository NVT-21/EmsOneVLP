using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.PartnerGateway.Repositories
{
    public class TransportPostageRepository
    {
        //public static ZResponse ChargedV2(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZTransportPostageChargedV2Request>(data);
        //    return XHelper.Invoke("POST", "Transport/ChargedV2", req);
        //}

        public static ZResponse Charged(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerTransportPostageChargedRequest>(data);
            return XHelper.Invoke("POST", "Partner/TransportPostageCharged", req);
        }
    }
}