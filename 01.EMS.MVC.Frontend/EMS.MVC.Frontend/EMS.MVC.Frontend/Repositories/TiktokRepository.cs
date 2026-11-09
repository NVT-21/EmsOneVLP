using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Tiktok.Models;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Repositories
{
    public class TiktokRepository
    {
        #region GetTimeStamp
        public static string GetTimeStamp()
        {
            string _timeStamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
            return _timeStamp;
        }
        #endregion

        #region PageConnect
        public static List<SelectListItem> ListPageConnect(string Channel)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            var res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "TIK",
                IsActive = "Y"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    if (item.Channel == Channel && item.IsActiveCheck == "Y")
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.PageName,
                            Value = item.PageID
                        });
                    }
                }
            }
            return listItem;
        }
        #endregion

        #region SearchListProduct
        public static ZResponse SearchListProduct(ZTikProductListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.SearchListProduct: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchListProductProcess(ZTikProductProcessListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_PROCESS_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.SearchListProductProcess: " + ex.Message);
            }
            return res;
        }
        public static ZResponse ListProductNotLinkedYet(ZTikProductNotLinkedYetListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_NOT_LINKED_YET_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.ListProductNotLinkedYet: " + ex.Message);
            }
            return res;
        }
        public static List<ProductModel> SearchListProductNotLinkedYet(ZTikProductNotLinkedYetListRequest req)
        {
            List<ProductModel> listItem = new List<ProductModel>();
            try
            {
                var res = ListProductNotLinkedYet(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTikProductNotLinkedYetListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductModel()
                        {
                            ProductName = item.ProductName,
                            ProductVariant = new ProductVariantModel() { ID = item.ProductVariantID, Options = item.Options },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.SearchListProductNotLinkedYet: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region ProductUpdateConnection
        public static ZResponse ProductUpdateConnection(ZTikProductUpdateConnectionRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_UPDATE_CONNECTION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.ProductUpdateConnection: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region ConfigProduct
        public static ZTikConfigProductResponse ConfigProductByPageID()
        {
            ZTikConfigProductResponse model = null;
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_CONFIG",
                    Data = JsonConvert.SerializeObject(new ZTikConfigProductRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.TiktokProfile.PageID
                    })
                });
                if (res.Code == "00")
                {
                    model = JsonConvert.DeserializeObject<ZTikConfigProductResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.ConfigProductByPageID: " + ex.Message);
            }
            return model;
        }
        public static ZResponse ConfigProductChange(TIK_ConfigProductChangeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TIK_PRODUCT_CONFIG_CHANGE",
                    Data = JsonConvert.SerializeObject(new ZTikConfigProductChangeRequest()
                    {
                        ID = req.ID,
                        Status = req.Status,
                        Type = req.Type,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokRepository.ConfigProductChange: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}