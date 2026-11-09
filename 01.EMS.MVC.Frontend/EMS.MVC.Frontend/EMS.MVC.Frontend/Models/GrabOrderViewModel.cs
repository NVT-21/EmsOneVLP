using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class GrabOrderViewModel
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string AllDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public List<ZShopGetListCrmOrPaypostCodeResponse> ListCrmOrPaypostCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public List<SelectListItem> ListOrderStatus { get; set; }
        [DataMember]
        public string SearchKey { get; set; } //Mã đơn hàng, mã bưu gửi, sđt người nhận
        [DataMember]
        public string ReloadStatusNumber { get; set; }
    }
}