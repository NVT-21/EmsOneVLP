using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class DetailEditedItemCodeViewModel
    {
        public int NumRow { get; set; }
        public string ItemCode { get; set; }
        public List<DetailEditedItemCode> ItemCodeDetail { get; set; }
        public long AdjBalanceProvisionalVolume { get; set; }
        public long AdjBalanceChargeableVolume { get; set; }
        public long AdjBalanceMainFee { get; set; }
        public long AdjBalanceServiceFee { get; set; }
        public long AdjBalanceTransportFeePublic { get; set; }
        public long AdjBalanceFeePublic { get; set; }
    }
}