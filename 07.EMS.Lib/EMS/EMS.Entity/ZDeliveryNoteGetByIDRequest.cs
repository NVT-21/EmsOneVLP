using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteGetByIDRequest
    {
        public int ID { get; set; }
    }
    public class ZDeliveryNoteGetByIDResponse
    {
        public string WareHouseName { get; set; }
        public string WareHouseFullAddress { get; set; }
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public int NumberOfProduct { get; set; }
    }
}
