using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class RequestOrderTempRepository
    {
        public static ZResponse AddTemp(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderImportAddTempRequest>(data);
            return XHelper.Invoke("POST", "RequestOrderTemp/AddTemp", req);
        }
        public static ZResponse AddCommit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderImportAddCommitRequest>(data);
            return XHelper.Invoke("POST", "RequestOrderTemp/AddCommit", req);
        }
    }
}