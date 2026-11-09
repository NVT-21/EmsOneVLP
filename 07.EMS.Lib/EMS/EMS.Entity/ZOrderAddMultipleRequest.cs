using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderAddMultipleRequest
    {
        public string Products { get; set; }
        public ShipmentDetailsModel Pickup { get; set; }
        public ShipmentDetailsModel Deliver { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string Transport { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public string CollectionType { get; set; }
        public string TransportMainService { get; set; }
        public string TransportExtraService { get; set; }
        public string TransportPayer { get; set; }
        public string Long { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string NumberOfProduct { get; set; }
        public string TotalCOD { get; set; }
        public string OrderValue { get; set; }
        public int WareHouseID { get; set; }
        public int Type { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
        public string DeviceName { get; set; }
    }
    public class ShipmentDetailsModel
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
    }
}
