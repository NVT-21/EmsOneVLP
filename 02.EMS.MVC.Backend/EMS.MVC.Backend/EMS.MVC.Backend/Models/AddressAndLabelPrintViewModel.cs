using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class AddressAndLabelPrintViewModel
    {
        public string StoreName { get; set; }
        public int ShopID { get; set; }
        public int LabelPrintID { get; set; }
        public int PickUpAddressID { get; set; }
        public int CollectionPostOfficeID { get; set; }
        public string IsDisplay { get; set; }
    }
}