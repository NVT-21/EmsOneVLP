using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class UserInCollectionPostOfficeRepository
    {
        //public static ZResponse SearchList(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZUserInCollectionPostOfficeSearchListRequest>(data);
        //    return XHelper.Invoke("POST", "UserInCollectionPostOffice/SearchList", req);
        //}
        public static ZResponse SearchListByUserID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserInCollectionPostOfficeSearchListByUserIDRequest>(data);
            return XHelper.Invoke("POST", "UserInCollectionPostOffice/SearchListByUserID", req);
        }
    }
}