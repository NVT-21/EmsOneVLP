using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPartnerWebhookAddPartnerRegisterRequest
    {
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerURL { get; set; }
        public string PartnerKey { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ZPartnerWebhookEditPartnerRegisterRequest
    {
        public int ID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerURL { get; set; }
        public string PartnerKey { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZPartnerWebhookDeletePartnerRegisterRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZPartnerWebhookListPartnerRegisterRequest
    {
        public string PartnerCode { get; set; }
    }

    public class ZPartnerWebhookListPartnerRegisterResponse
    {
        public int ID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerURL { get; set; }
        public string PartnerKey { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
    }

    public class ZPartnerWebhookGetPartnerRegisterByIDRequest
    {
        public int ID { get; set; }
    }
}
