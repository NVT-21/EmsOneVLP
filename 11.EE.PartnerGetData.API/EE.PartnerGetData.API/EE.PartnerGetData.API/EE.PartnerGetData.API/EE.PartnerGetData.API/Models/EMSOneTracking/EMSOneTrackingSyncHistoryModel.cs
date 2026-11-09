using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingSyncHistoryModel
    {
        public int TrackingID { get; set; }
        public string TrackingCode { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int Amount { get; set; }
        public string ActionName { get; set; }
        public List<EMSOneTrackingItemInfoModel> ListDataItemInfo { get; set; }
        public List<EMSOneTrackingValueAddedServiceCodeModel> ListDataValueSerrviceCode { get; set; }
        public List<EMSOneTrackingItemStatusModel> ListDataItemStatus { get; set; }
        public List<EMSOneTrackingItemStatusPickupModel> ListDataItemStatusPickup { get; set; }
        public List<EMSOneTrackingDeliveryModel> ListDataDelivery { get; set; }
        public List<EMSOneTrackingCODModel> ListDataCOD { get; set; }
    }
}