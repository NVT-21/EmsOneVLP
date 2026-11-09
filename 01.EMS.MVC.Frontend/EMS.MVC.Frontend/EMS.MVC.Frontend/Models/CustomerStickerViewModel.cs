using System.Collections.Generic;

namespace EMS.MVC.Frontend.Models
{
    public class CustomerStickerViewModel
    {
        public string StickerCode { get; set; }
        public string StickerName { get; set; }
        public string SearchKey { get; set; }
        public List<CustomerStickerModel> ListCustomerSticker { get; set; }
    }
}