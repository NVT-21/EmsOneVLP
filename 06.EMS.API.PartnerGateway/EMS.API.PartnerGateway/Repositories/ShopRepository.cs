using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.PartnerGateway.Repositories
{
    public class ShopRepository
    {
        public static ZResponse SearchShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSearchListRequest>(data);
            return XHelper.Invoke("POST", "Shop/SearchList", req);
        }
        public static ZResponse GetByCrmOrPaypostCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeRequest>(data);
            return XHelper.Invoke("GET", string.Format("Shop/GetByCrmOrPaypostCode?CrmOrPaypostCode={0}", req.CrmOrPaypostCode), null);
        }

        public static ZResponse SearchByToken(string token)
        {
            return XHelper.Invoke("GET", $"Shop/GetByToken?Token={token}",null);
        }

        public static ZResponse SearchByAccessToken(string token)
        {
            return XHelper.Invoke("GET", $"Shop/GetByAccessToken?Token={token}", null);
        }
    }
}