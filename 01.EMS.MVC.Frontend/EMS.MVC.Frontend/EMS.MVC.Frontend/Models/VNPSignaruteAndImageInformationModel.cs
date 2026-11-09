using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class VNPSignaruteAndImageInformationModel
    {
        public List<string> ImagesSignature { get; set; } // Link chữ ký báo phát
        public List<string> ImagesDelivery { get; set; } // Link hình ảnh báo phát
        public List<string> Images { get; set; } // Link hình ảnh báo phát dịch vụ “Báo phát hình ảnh” (HA)
        public List<string> ImagesPlus { get; set; } // Link hình ảnh báo phát cập nhật bổ sung
    }
}