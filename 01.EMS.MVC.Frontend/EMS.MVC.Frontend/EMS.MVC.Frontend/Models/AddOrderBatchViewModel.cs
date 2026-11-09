using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class AddOrderBatchViewModel
    {
        public string MobileNumber { get; set; }
        public SearchCustomerViewModel ShipmentInfo { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public int CollectionType { get; set; }
        public int TransportPayer { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public long TotalCOD { get; set; }
        public string Note { get; set; }
        public string Channel { get; set; }
        public List<SelectListItem> ListChannel { get; set; }
        public List<AddOrderBatchOrderInfoViewModel> ListOrderInfo { get; set; }

        public string ActionType { get; set; }

        public int NumberOfDetail { get; set; }
    }
}