using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingItemInfoModel
    {
        public string ItemCode { get; set; }
        public string DataCode { get; set; }
        public string CustomerCode { get; set; }
        public string SenderFullName { get; set; }
        public string SenderMobileNumber { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public string ReceiverAddress { get; set; }
        public string AcceptancePosCode { get; set; }
        public int Weight { get; set; }
        public int PaymentFreight { get; set; }
        public string CreateTime { get; set; }
        public string LastUpdatedTime { get; set; }

        public string SenderAddressCode { get; set; }
        public int SenderDistrictCode { get; set; }
        public int ReceiverProvinceCode { get; set; }
        public int ReceiverDistrictCode { get; set; }
        public int ReceiverCommuneCode { get; set; }
        public string ReceiverAddressCode { get; set; }
        public int CODValue { get; set; }
        public int TransportPayer { get; set; }
        public string BatchCode { get; set; }
    }
}