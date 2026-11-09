using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class EMSSignaruteAndImageInformationModel
    {
        public string Signature { get; set; } // Link chữ ký báo phát
        public List<string> FileNames { get; set; } // Link hình ảnh báo phát
        public List<string> FileNamesAuth { get; set; } // Link hình ảnh báo phát dịch vụ “Báo phát hình ảnh” (HA)

    }
}