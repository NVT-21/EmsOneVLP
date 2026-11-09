using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteListRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InventoryOutputCode { get; set; }
        public string OrderCode { get; set; }
        public int ShopID { get; set; }
    }
    public class ZDeliveryNoteListResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Transport { get; set; }
        //public List<ZOrderItemModel> listProduct { get; set; }
    }
}
