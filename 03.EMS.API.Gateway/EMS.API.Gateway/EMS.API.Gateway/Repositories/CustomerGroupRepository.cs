using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class CustomerGroupRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGroupSearchListRequest>(data);
            return XHelper.Invoke("POST", "CustomerGroup/SearchList", req);
        }

        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGroupAddRequest>(data);
            return XHelper.Invoke("POST", "CustomerGroup/Add", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = Convert.ToInt64(data);
            return XHelper.Invoke("GET", "CustomerGroup/GetById?id=" + req.ToString(), null);
        }

        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGroupEditRequest>(data);
            return XHelper.Invoke("POST", "CustomerGroup/Edit", req);
        }
    }
}