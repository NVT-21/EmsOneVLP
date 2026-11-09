using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class EditFreightChargesViewModel
    {
        public int TransportPostageID { get; set; }
        public string TransportPostageCode { get; set; }
        public string TransportServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public int ChargingConditionsID { get; set; }
        public List<SelectListItem> ListChargingConditions { get; set; }
        public double ChargingConditionsValue { get; set; }
        public long ChargingConditionsMinValue { get; set; }
        public BaseModel VAT { get; set; }
        public string ServicesUsed { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string AppliesToShop { get; set; }
        public int SubjectOfApplication { get; set; }
    }
}