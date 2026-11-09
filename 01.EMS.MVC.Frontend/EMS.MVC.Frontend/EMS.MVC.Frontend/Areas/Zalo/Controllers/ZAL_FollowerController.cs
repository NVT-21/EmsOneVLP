using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Areas.Zalo.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using EMS.Entity;
namespace EMS.MVC.Frontend.Areas.Zalo.Controllers
{
    public class ZAL_FollowerController : ZAL_BaseController
    {
        public ActionResult ZAL_FollowerView()
        {
            return View();
        }
        public ActionResult ZAL_SearchListFollowerView()
        {
            List<ZAL_SearchListFollowerViewModel> model = new List<ZAL_SearchListFollowerViewModel>();
            var res = UserChannelRepository.SearchListUserChannel(new ZUserChannelSearchListRequest()
            {
                PageID = XHelper.Session.ZaloProfile.PageID,
                Channel = "ZA"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZUserChannelSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ZAL_SearchListFollowerViewModel()
                    {
                        ID = item.UserChannelID,
                        ChannelUserID = item.ChannelUserID,
                        ChannelUserIDByApp = item.ChannelUserIDByApp,
                        FullName = item.FullName,
                        UserGender = item.UserGender,
                        MobileNumber = item.MobileNumber,
                        Avatar = item.Avatar,
                        BirthDay = item.BirthDay,
                        DisplayName = item.DisplayName,
                        ProvinceName = item.ProvinceName,
                        DistrictName = item.DistrictName,
                        WardName = item.WardName,
                        Street = item.Street,
                        FullAddress = item.FullAddress,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return PartialView(model);
        }
    }
}