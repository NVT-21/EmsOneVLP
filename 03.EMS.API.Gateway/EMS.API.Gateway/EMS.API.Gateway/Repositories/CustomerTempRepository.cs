using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class CustomerTempRepository
    {
        public static ZResponse AddTemp(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerImportAddTempRequest>(data);
            return XHelper.Invoke("POST", "CustomerTemp/AddTemp", req);
        }
        public static ZResponse AddCommit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerImportAddCommitRequest>(data);
            return XHelper.Invoke("POST", "CustomerTemp/AddCommit", req);
        }
    }
}