using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class CorporationRepositiry
    {
        public static ZResponse AddNew(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationAddNewRequest>(data);
            return XHelper.Invoke("POST", "Corporation/AddNew", req);
        }

        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationListRequest>(data);
            return XHelper.Invoke("POST", "Corporation/List", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationGetByIDRequest>(data);
            return XHelper.Invoke("GET", "Corporation/GetById?Id=" + req.ID, null);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationEditRequest>(data);
            return XHelper.Invoke("POST", "Corporation/Edit", req);
        }
        public static ZResponse RemoveCorporation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationRemoveRequest>(data);
            return XHelper.Invoke("POST", "Corporation/RemoveCorporation", req);
        }
        public static ZResponse GetListEmployeeByCorporationID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationGetByIDRequest>(data);
            return XHelper.Invoke("GET", "Corporation/GetListEmployeeByCorporationID?CorporationID=" + req.ID, null);
        }
        public static ZResponse RemoveEmployee(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCorporationRemoveEmployeeRequest>(data);
            return XHelper.Invoke("POST", "Corporation/RemoveEmployee", req);
        }
    }
}