using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    [Serializable]
    [DataContract]
    public class TIK_ProductUpdateConnectionModel
    {
        [DataMember]
        public string TypeUpdate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public List<ZTikProductUpdateConnectionListItemRequest> ListItem { get; set; }
    }
}