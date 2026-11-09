using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    #region WEB
    public class ZStatisticalDeliveryTransportWeb
    {
        public string CreatedDate { get; set; }
        public string StatusName_G { get; set; }
        public string StatusName_F { get; set; }
        public string Status_G { get; set; }
        public string Status_F { get; set; }
        public long TotalOrder_G { get; set; }
        public long TotalOrder_F { get; set; }
        public long TotalCOD_G { get; set; }
        public long TotalCOD_F { get; set; }
    }
    #endregion

    #region ANDROID
    public class ZStatisticalDeliveryTransportAndroid
    {
        public string CreatedDate { get; set; }
        public string StatusName_G { get; set; }
        public string StatusName_F { get; set; }
        public string Status_G { get; set; }
        public string Status_F { get; set; }
        public long TotalOrder_G { get; set; }
        public long TotalOrder_F { get; set; }
        public long TotalCOD_G { get; set; }
        public long TotalCOD_F { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalDeliveryTransportIOS
    {
        public string CreatedDate { get; set; }
        public string StatusName_G { get; set; }
        public string StatusName_F { get; set; }
        public string Status_G { get; set; }
        public string Status_F { get; set; }
        public long TotalOrder_G { get; set; }
        public long TotalOrder_F { get; set; }
        public long TotalCOD_G { get; set; }
        public long TotalCOD_F { get; set; }
    }
    #endregion
}
