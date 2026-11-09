using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingDeliveryModel
    {
        public int DeliveryIndex { get; set; }
        public string ItemCode { get; set; }
        public string CustomerCode { get; set; }
        public string CauseCode { get; set; }
        public string ReasonDeliveryFailure { get; set; }
        public string SolutionCode { get; set; }
        public string SolutionName { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryNote { get; set; }
        public string DeliveryUser { get; set; }
        public string Poscode { get; set; }
        public string TransferDate { get; set; }
        public string PostmanCode { get; set; }
        public string PostmanName { get; set; }
        public string CreateTime { get; set; }
        public string LastUpdatedTime { get; set; }
        public string InputDate { get; set; }
        public int DeliveryReturn { get; set; }
        public int IsDeliverable { get; set; }
        public string RealReciverName { get; set; }
    }
}