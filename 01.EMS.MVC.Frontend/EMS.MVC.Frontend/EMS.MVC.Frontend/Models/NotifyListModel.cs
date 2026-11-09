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
    public class NotifyListModel
    {
        [DataMember]
        public int RowLimit { get; set; }
        [DataMember]
        public string AllDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public List<SelectListItem> ListNotifyType { get; set; }
        [DataMember]
        public string IsCheck { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyListViewModel
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public string NotifyTypeName { get; set; }
        [DataMember]
        public string NotifyName { get; set; }
        [DataMember]
        public string NotifyContent { get; set; }
        [DataMember]
        public string NotifyRedirectUrl { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string IsCheck { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyUpdateModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string StrID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyGetByIDModel
    {
        [DataMember]
        public int ID { get; set; }
    }
}