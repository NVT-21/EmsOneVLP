using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderImportAddTempRequest
    {
        public string ImportCode { get; set; }
        public string Product { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string Transport { get; set; }
        public string OrderCode { get; set; }
        public string TransportOrderName { get; set; }
        public int CollectionType { get; set; }
        public int TransportMainService { get; set; }
        public string TransportExtraService { get; set; }
        public long TransportFee { get; set; }
        public double TransportLength { get; set; }
        public int TransportPayer { get; set; }
        public double TransportWidth { get; set; }
        public double TransportHeight { get; set; }
        public double TransportWeight { get; set; }
        public int NumberOfProduct { get; set; }
        public long TotalCOD { get; set; }
        public long OrderValue { get; set; }
    }
}
