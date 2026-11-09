using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteChangeStatusRequest
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
        public string BeforeStatus { get; set; }
        public string AfterStatus { get; set; }
        public string ReasonCancel { get; set; }
        public int UpdatedBy { get; set; }
        public int PickupProvinceID { get; set; }
        public int PickupDistrictID { get; set; }
        public int PickupWardID { get; set; }
        public string PickupStreet { get; set; }
    }
}