using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ProviderTempRepository
    {
        public static ZResponse AddTemp(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderImportAddTempRequest>(data);
            return XHelper.Invoke("POST", "ProviderTemp/AddTemp", req);
        }
        public static ZResponse AddCommit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderImportAddCommitRequest>(data);
            return XHelper.Invoke("POST", "ProviderTemp/AddCommit", req);
        }
    }
}