using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_SearchListFollowerViewModel
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string AuthorizationCode { get; set; }
        public string CreatedDate { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string UserAccessToken { get; set; }
        public string ProvinceName { get; set; }
        public string BirthDay { get; set; }
        public string Avatar { get; set; }
        public string MobileNumber { get; set; }
        public string UserGender { get; set; }
        public string FullName { get; set; }
        public string ChannelUserIDByApp { get; set; }
        public string ChannelUserID { get; set; }
        public int UserChannelID { get; set; }
        public string DisplayName { get; set; }
        public string PageAccessToken { get; set; }
    }
}