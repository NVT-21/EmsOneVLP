using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InformationSignaruteAndImageOrderViewModel
    {
        public string Title { get; set; }
        public string ShippingCode { get; set; }
        public string SearchKey { get; set; }
        public List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInformation { get; set; } // API trả trạng thái phát kèm thông tin chữ ký , hình ảnh báo phát EMS 
        public List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInformation { get; set; } //  API trả trạng thái phát kèm thông tin chữ ký , hình ảnh báo phát VNPOST
    }

    
}