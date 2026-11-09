using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGetPickupInformationRequest
    {
        public int ID { get; set; }
    }
    public class ZShopGetPickupInformationResponse: XPickupInformationModel
    {
    }
}
