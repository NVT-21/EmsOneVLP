using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XPickupInformationModel
    {
        public int ID { get; set; }
        public int PickupAddressID { get; set; }
        public string PickupFullName { get; set; }
        public string PickupName { get; set; }
        public string PickupMobileNumber { get; set; }
        public int PickupProvinceID { get; set; }
        public string PickupProvinceName { get; set; }
        public int PickupDistrictID { get; set; }
        public string PickupDistrictName { get; set; }
        public int PickupWardID { get; set; }
        public string PickupWardName { get; set; }
        public string PickupStreet { get; set; }
        public string PickupFullAddress { get; set; }
    }
}
