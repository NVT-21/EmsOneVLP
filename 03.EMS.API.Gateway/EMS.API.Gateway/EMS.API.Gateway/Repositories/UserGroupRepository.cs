using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class UserGroupRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserGroupSearchListRequest>(data);
            return XHelper.Invoke("POST", "UserGroup/SearchList", req);
        }
    }
}