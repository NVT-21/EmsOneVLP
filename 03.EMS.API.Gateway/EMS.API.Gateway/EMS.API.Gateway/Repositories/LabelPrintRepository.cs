using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class LabelPrintRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintSearchListRequest>(data);
            return XHelper.Invoke("POST", "LabelPrint/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintAddRequest>(data);
            return XHelper.Invoke("POST", "LabelPrint/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("LabelPrint/GetByID?ShopID={0}&LabelPrintID={1}", req.ShopID, req.LabelPrintID), null);
        }
        public static ZResponse Update(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintUpdateRequest>(data);
            return XHelper.Invoke("POST", "LabelPrint/Update", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintRemoveRequest>(data);
            return XHelper.Invoke("POST", "LabelPrint/Remove", req);
        }
        public static ZResponse DefaultAccordingToPickUpAddress(string data)
        {
            var req = JsonConvert.DeserializeObject<ZLabelPrintDefaultAccordingToPickUpAddressRequest>(data);
            return XHelper.Invoke("POST", "LabelPrint/DefaultAccordingToPickUpAddress", req);
        }
    }
}