using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    [Serializable]
    [DataContract]
    public class TIK_SearchListProductViewModel
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductAvatar { get; set; }
        [DataMember]
        public string SKU { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public string Options { get; set; }
        [DataMember]
        public string TiktokSKUID { get; set; }
        [DataMember]
        public string TiktokProductID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string TiktokStatusUpdatePrice { get; set; }
        [DataMember]
        public string TiktokStatusUpdateStock { get; set; }
    }
}