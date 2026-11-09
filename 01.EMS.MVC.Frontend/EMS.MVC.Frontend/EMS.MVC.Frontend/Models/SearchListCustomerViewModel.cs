using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class SearchListCustomerViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string BirthDay { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public CustomerGroupModel CustomerGroup { get; set; }
        [DataMember]
        public CustomerStickerModel CustomerSticker { get; set; }
        [DataMember]
        public string BlackList { get; set; }
        [DataMember]
        public ProvinceModel Province { get; set; }
        [DataMember]
        public DistrictModel District { get; set; }
        [DataMember]
        public WardModel Ward { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string SuccessOrder { get; set; } // Số đơn hàng thành công
        [DataMember]
        public string TotalAmount { get; set; } // Tổng tiền
        [DataMember]
        public string SearchKey { get; set; }

    }
}