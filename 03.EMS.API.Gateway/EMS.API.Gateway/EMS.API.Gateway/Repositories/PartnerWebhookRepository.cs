using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class PartnerWebhookRepository
    {
        public static ZResponse AddPartnerRegister(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookAddPartnerRegisterRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/AddPartnerRegister", req);
        }
        public static ZResponse EditPartnerRegister(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookEditPartnerRegisterRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/EditPartnerRegister", req);
        }
        public static ZResponse DeletePartnerRegister(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookDeletePartnerRegisterRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/DeletePartnerRegister", req);
        }
        public static ZResponse ListPartnerRegister(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookListPartnerRegisterRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/ListPartnerRegister", req);
        }
        public static ZResponse GetPartnerRegisterByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookGetPartnerRegisterByIDRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/GetPartnerRegisterByID", req);
        }

        public static ZResponse ListHistoryStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookListHistoryStatusRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/ListHistoryStatus", req);
        }
        public static ZResponse UpdateHistoryStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWebhookUpdateHistoryStatusRequest>(data);
            return XHelper.Invoke("POST", "PartnerWebhook/UpdateHistoryStatus", req);
        }
    }
}