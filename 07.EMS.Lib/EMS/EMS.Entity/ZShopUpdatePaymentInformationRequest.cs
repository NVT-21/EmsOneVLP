using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopUpdatePaymentInformationRequest
    {
        public int ID { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentType { get; set; }
        public string FullNameOfPayment { get; set; }
        public string MobileNumberOfPayment { get; set; }
        public string NameOfPayment { get; set; }
        public string BranchOfPayment { get; set; }
        public string NumberOfPayment { get; set; }
        public int CollectionType { get; set; }
        public int UpdatedBy { get; set; }
    }
}
