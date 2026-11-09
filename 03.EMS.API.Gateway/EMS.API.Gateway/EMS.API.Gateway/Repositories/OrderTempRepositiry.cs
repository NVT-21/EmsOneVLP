using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class OrderTempRepositiry
    {
        public static ZResponse AddTemp(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderImportAddTempRequest>(data);
            return XHelper.Invoke("POST", "OrderTemp/AddTemp", req);
        }
        public static ZResponse AddCommit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderImportAddCommitRequest>(data);
            return XHelper.Invoke("POST", "OrderTemp/AddCommit", req);
        }
    }
}