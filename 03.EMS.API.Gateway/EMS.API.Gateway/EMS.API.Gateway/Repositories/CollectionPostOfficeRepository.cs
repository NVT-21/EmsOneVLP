using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class CollectionPostOfficeRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeSearchListRequest>(data);
            return XHelper.Invoke("POST", "CollectionPostOffice/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeAddRequest>(data);
            return XHelper.Invoke("POST", "CollectionPostOffice/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeEditRequest>(data);
            return XHelper.Invoke("POST", "CollectionPostOffice/Edit", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("CollectionPostOffice/GetByID?ID={0}", req.ID), null);
        }
        public static ZResponse SearchListByParentID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeSearchListByParentIDRequest>(data);
            return XHelper.Invoke("POST", "CollectionPostOffice/SearchListByParentID", req);
        }
        public static ZResponse SearchListRecent(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCollectionPostOfficeSearchListRecentRequest>(data);
            return XHelper.Invoke("POST", "CollectionPostOffice/SearchListRecent", req);
        }
        
    }
}