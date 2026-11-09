using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class DetailRequestSupportViewModel
    {
        public int RequestSupportID { get; set; }
        public string RequestSupportCode { get; set; }
        public string RequestGroup { get; set; }
        public string RequestType { get; set; }
        public string RequestSupportDescription { get; set; }
        public string RequestSupportDocument { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedDate { get; set; }
        public BaseModel RequestSupportStatus { get; set; }
        public string NewUpdateInformation { get; set; }
        public string OldUpdateInformation { get; set; }
        public string PosC { get; set; }

        public string EShippingCode { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string OrderMessage { get; set; }
        public double WeightBefore { get; set; }
        public double WeightAfter { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderFullAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverFullAddress { get; set; }
        public BaseModel OrderStatus { get; set; }
        public long TransportOrderValue { get; set; }
        public long TotalCOD { get; set; }
        public long TransportFee { get; set; }
        public string TransportMainService { get; set; }
        public string TransportExtraService { get; set; }

        public int ShopID { get; set; }
        public string ShopCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string ShopPhone { get; set; }
        public string ShopFullAddress { get; set; }

        public List<RequestSupportSearchHistoryModel> ListSearchHistory { get; set; }
    }
}