using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListCustomerStickerViewModel
    {
        public int ID { get; set; }
        public XBaseModel CustomerSticker { get; set; }
        public string CreatedDate { get; set; }
    }
}