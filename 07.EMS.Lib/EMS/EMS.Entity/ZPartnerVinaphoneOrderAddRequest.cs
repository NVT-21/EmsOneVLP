using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPartnerVinaphoneOrderAddRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsTransport { get; set; }
        [DataMember]
        public string IsSendTransport { get; set; }
        //[DataMember]
        //public int ShopID { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        //[DataMember]
        //public int CreatedBy { get; set; }
        [DataMember]
        public ZPartnerOrderAddBuyerInfoRequest BuyerInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddSenderInfoRequest SenderInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerOrderAddTransportInfoRequest TransportInfo { get; set; }
        [DataMember]
        public List<ZPartnerOrderAddProductInfoRequest> ProductInfoConvert { get; set; }
        [DataMember]
        public List<ZPartnerVinaphoneOrderAddProductInfoRequest> ProductInfo { get; set; }


    }

    [Serializable]
    [DataContract]
    public class ZPartnerVinaphoneOrderAddProductInfoRequest
    {
        [DataMember]
        public long ProductID { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
    }
}
