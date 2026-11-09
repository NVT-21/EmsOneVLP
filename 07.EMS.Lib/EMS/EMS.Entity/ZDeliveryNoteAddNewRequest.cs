using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteAddNewRequest
    {
        public int ShopID { get; set; }
        public int OrderID { get; set; }
        public int WareHouseID { get; set; }
        public string DeliveryNoteCode { get; set; }
        public string OrderCode { get; set; }
        public int CreatedBy { get; set; }
        public List<ZDeliveryNoteAddNew> listAddDetail { get; set; }
        public string BeforeStatus { get; set; }
        public string AfterStatus { get; set; }
    }
    public class ZDeliveryNoteAddNew
    {
        public int ProductID { get; set; }
        public int VariantID { get; set; }
        public int NumberOfProduct { get; set; }
    }
}
