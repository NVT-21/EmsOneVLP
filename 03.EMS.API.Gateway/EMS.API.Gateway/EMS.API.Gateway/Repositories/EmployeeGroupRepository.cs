using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class EmployeeGroupRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupListRequest>(data);
            return XHelper.Invoke("POST", "EmployeeGroup/List", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupAddRequest>(data);
            return XHelper.Invoke("POST", "EmployeeGroup/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupGetByIDRequest>(data);
            return XHelper.Invoke("GET", "EmployeeGroup/GetByID?ID=" + req.ID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupEditRequest>(data);
            return XHelper.Invoke("POST", "EmployeeGroup/Edit", req);
        }
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "EmployeeGroup/ChangeIsLock", req);
        }
    }
}