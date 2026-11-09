using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class PermissionViewModel
    {
        public int ShopLevel { get; set; }
        public List<SelectListItem> ListShopLevel { get; set; }
        public int GroupPermissionID { get; set; }
        public List<ZShopGroupPermissionListResponse> ListGroupPermission { get; set; }
        public string strFunctionalID { get; set; }
    }
}