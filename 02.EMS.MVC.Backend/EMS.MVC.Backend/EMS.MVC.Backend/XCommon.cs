using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Backend.Repository;

namespace EMS.MVC.Backend
{
    public class XCommon
    {
        #region Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã

        #region Danh sách Tỉnh/ Thành phố
        public static List<ProvinceModel> SearchListProvince(ZProvinceListRequest req)
        {
            List<ProvinceModel> listItem = new List<ProvinceModel>();
            try
            {
                var res = PWDRepository.SearchListProvince(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProvinceListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProvinceModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProvince: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region Danh sách Quận/ Huyện
        public static List<DistrictModel> SearchListDistrictByProvinceID(ZDistrictGetByProvinceIDRequest req)
        {
            List<DistrictModel> listItem = new List<DistrictModel>();
            try
            {
                var res = PWDRepository.SearchListDistrictByProvinceID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZDistrictGetByProvinceIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new DistrictModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListDistrictByProvinceID: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region Danh sách Phường/ Xã
        public static List<WardModel> SearchListWardByDistrictID(ZWardGetByDistrictIDRequest req)
        {
            List<WardModel> listItem = new List<WardModel>();
            try
            {
                var res = PWDRepository.SearchListWardByDistrictID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWardGetByDistrictIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new WardModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListWardByDistrictID: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #endregion

        #region Danh sách bưu cục thu gom
        public static List<CollectionPostOfficeModel> SearchListCollectionPostOffice(ZCollectionPostOfficeSearchListRequest req)
        {
            List<CollectionPostOfficeModel> listItem = new List<CollectionPostOfficeModel>();
            try
            {
                var res = CollectionPostOfficeRepository.SearchList(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCollectionPostOfficeSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        var CodeAndName = "";
                        if (!string.IsNullOrEmpty(item.CollectionPostOfficeCode))
                        {
                            CodeAndName = string.Format("{0} - {1}", item.CollectionPostOfficeCode, item.CollectionPostOfficeName);
                        }
                        listItem.Add(new CollectionPostOfficeModel()
                        {
                            ID = item.ID,
                            Code = item.CollectionPostOfficeCode,
                            Name = item.CollectionPostOfficeName,
                            CodeAndName = CodeAndName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListCollectionPostOffice: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region Danh sách nhóm người dùng
        public static List<UserGroupModel> SearchListUserGroup(ZUserGroupSearchListRequest req)
        {
            List<UserGroupModel> listItem = new List<UserGroupModel>();
            try
            {
                var res = UserGroupRepository.SearchList(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZUserGroupSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new UserGroupModel()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListUserGroup: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region Dịch vụ vận chuyển (TransportService)

        #region Danh sách dịch vụ vận chuyển
        public static List<TransportServiceModel> SearchListTransportService(ZTransportServiceSearchListRequest req)
        {
            List<TransportServiceModel> listItem = new List<TransportServiceModel>();
            try
            {
                var res = TransportServiceRepository.SearchList(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new TransportServiceModel()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            SpecialService = new BaseModel()
                            {
                                Code = item.SpecialService.Code,
                                Name = item.SpecialService.Name
                            },
                            IsLock = item.IsLock,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListTransportService: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #endregion

        #region Yêu cầu hỗ trợ

        #region Danh sách nhóm yêu cầu hỗ trợ
        public static List<RequestSupportGroupModel> SearchListRequestSupportGroup(int ParentID)
        {
            List<RequestSupportGroupModel> listItem = new List<RequestSupportGroupModel>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_GROUP_LIST",
                    Data = JsonConvert.SerializeObject(new ZRequestSupportGroupListRequest()
                    {
                        ParentID = ParentID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZRequestSupportGroupListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new RequestSupportGroupModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            CmsReasonForComplaint = item.CmsReasonForComplaint
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListRequestSupportGroup: " + ex.Message);
            }
            return listItem;
        }

        #endregion

        #endregion

        #region Danh sách cửa hàng
        public static List<ShopModel> SearchListShop(ZShopSearchListRequest req)
        {
            List<ShopModel> listItem = new List<ShopModel>();
            try
            {
                var res = ShopRepository.SearchListShop(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZShopSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ShopModel()
                        {
                           ID = item.ID,
                           Code = item.Code
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListShop: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        public static List<SelectListItem> TransportServiceList(int ServiceGroupID) // Danh sách dịch vụ
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest() 
                {
                    ParentID = ServiceGroupID,
                    Channel = "EMSONE"
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        if (item.IsLock == "N")
                        {
                            listItem.Add(new SelectListItem()
                            {
                                Text = item.Name,
                                Value = item.ID.ToString()
                            });
                        }
                    }
                }
                else
                {
                    throw new Exception("Tải nhóm danh sách dịch vụ thất bại.");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.TransportServiceList: " + ex.Message);
                throw new Exception("Tải nhóm danh sách dịch vụ thất bại!");
            }
            return listItem;
        }

        public static List<SelectListItem> TransportRegionGroupList() // Danh sách nhóm vùng
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = TransportRegionGroupRepository.SearchList(new ZTransportRegionGroupSearchListRequest());
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTransportRegionGroupSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.ID.ToString()
                        });
                    }
                }
                else
                {
                    throw new Exception("Tải danh sách nhóm vùng thất bại.");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.TransportRegionGroupList: " + ex.Message);
                throw new Exception("Tải danh sách nhóm vùng thất bại!");
            }
            return listItem;
        }

        public static List<SelectListItem> TransportRegionByParentID(int ParentID) // Danh sách vùng theo ParentID
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = TransportRegionRepository.GetByParentID(new ZTransportRegionGetByParentIDRequest() 
                {
                    ParentID = ParentID
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTransportRegionGetByParentIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.ID.ToString()
                        });
                    }
                }
                else
                {
                    throw new Exception("Tải danh sách vùng thất bại.");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.TransportRegionGroupList: " + ex.Message);
                throw new Exception("Tải danh sách vùng thất bại!");
            }
            return listItem;
        }

        public static List<SelectListItem> ProvincesList()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PROVINCE_LIST",
                    Data = ""
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProvinceListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ProvincesList: " + ex.Message);
            }
            return listItem;
        }

        public static List<SelectListItem> GetDistrictByProvinceID(int ProvinceID)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "DISTRICT_GET_BY_PROVINCE_ID",
                    Data = JsonConvert.SerializeObject(new ZDistrictListRequest()
                    {
                        ProvinceID = ProvinceID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZDistrictListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetDistrictByProvinceID: " + ex.Message);
            }
            return listItem;
        }

        public static List<SelectListItem> GetWardByDistrictID(int DistrictID)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WARD_GET_BY_DISTRICT_ID",
                    Data = JsonConvert.SerializeObject(new ZWardListRequest()
                    {
                        DistrictID = DistrictID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWardListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetWardByDistrictID: " + ex.Message);
            }
            return listItem;
        }

        public static List<SelectListItem> ApprovedShopList()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = ShopRepository.SearchListShop(new ZShopSearchListRequest() { Status = "A" });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZShopSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Code,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ApprovedShopList: " + ex.Message);
            }
            return listItem;
        }

        //public static List<SelectListItem> UserGroupList()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "USER_GROUP_SEARCH_LIST",
        //            Data = ""
        //        });
        //        if (res.Code == "00")
        //        {
        //            var list = JsonConvert.DeserializeObject<List<ZUserGroupSearchListResponse>>(res.Data);
        //            foreach (var item in list)
        //            {
        //                listItem.Add(new SelectListItem()
        //                {
        //                    Text = item.Name,
        //                    Value = item.Code
        //                });
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Tải danh sách nhóm người dùng thất bại.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.UserGroupList: " + ex.Message);
        //        throw new Exception("Tải danh sách nhóm người dùng thất bại!");
        //    }
        //    return listItem;
        //}

        public static string ConvertAddress(/*string no, */string street, string ward, string district, string province)
        {
            string address = string.Empty;
            //if (string.IsNullOrEmpty(address))
            //    address = no;
            //else
            //    address = address + ", " + no;

            if (string.IsNullOrEmpty(address))
                address = street;
            else
                address = address + ", " + street;

            if (string.IsNullOrEmpty(address))
                address = ward;
            else
                address = address + ", " + ward;

            if (string.IsNullOrEmpty(address))
                address = district;
            else
                address = address + ", " + district;

            if (string.IsNullOrEmpty(address))
                address = province;
            else
                address = address + ", " + province;

            return address;
        }

        public static ZResponse NotifyListRequestSupport(int MaxRequestSupportID = 0)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (XHelper.Session.UserProfile == null)
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                }
                else
                {
                    res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "REQUEST_SUPPORT_NOTIFY_LIST",
                        Data = JsonConvert.SerializeObject(new ZRequestSupportNotifyListRequest()
                        {
                            MaxRequestSupportID = MaxRequestSupportID,
                            UserID = XHelper.Session.UserProfile.ID
                        })
                    });
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.NotifyListRequestSupport: " + ex.Message);
            }
            return res;
        }

        public static List<PrintOrderTypeDataModel> ListOrderPrint(OrderPrintViewModel req)
        {
            List<PrintOrderTypeDataModel> oList = new List<PrintOrderTypeDataModel>();
            var ListOrderPrint = JsonConvert.DeserializeObject<List<ZOrderPrintRequest>>(req.ListDataOrderPrint);
            foreach (var itemOrderPrint in ListOrderPrint)
            {
                var res = OrderRepository.PrintOrder(new ZOrderPrintRequest() 
                {
                    EMSOneCode = itemOrderPrint.EMSOneCode,
                    ShopID = itemOrderPrint.ShopID,
                    PrintType = req.PrintType
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZOrderPrintResponse>(res.Data);
                    oList.Add(new PrintOrderTypeDataModel()
                    {
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        ShippingCode = item.ShippingCode,
                        SenderFullName = item.SenderFullName,
                        SenderMobileNumber = item.SenderMobileNumber,
                        SenderProvinceName = item.SenderProvinceName,
                        SenderDistrictName = item.SenderDistrictName,
                        SenderFullAddress = item.SenderFullAddress,
                        SenderSentDate = item.SenderSentDate,
                        RecieverFullName = item.ReceiverFullName,
                        RecieverMobileNumber = item.ReceiverMobileNumber,
                        RecieverProvinceName = item.ReceiverProvinceName,
                        RecieverDistrictName = item.ReceiverDistrictName,
                        RecieverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        Note = item.Note,
                        TransportWeight = item.TransportWeight,
                        TransportQuantity = item.TransportQuantity,
                        TransportMainServiceCode = item.TransportMainServiceCode,
                        TransportMainServiceName = item.TransportMainServiceName,
                        TransportExtraServiceCode = item.TransportExtraServiceCode,
                        TransportExtraServiceName = item.TransportExtraServiceName,
                        TotalCOD = item.TotalCOD,
                        TotalPHU = item.TotalPHU,
                        QRCode = item.QRCode,
                        ShippingBarCode = item.ShippingBarCode,
                        OrderBarCode = item.OrderBarCode,
                        PaymentFreightVat = item.PaymentFreightVat,
                        TransportPayer = item.TransportPayer,
                        TransportFee = item.TransportFee,
                        PostOffice = item.PostOffice,
                    });
                }
            }
            return oList;
        }

        //public static ZResponse SendRequestSupport(List<ListRequestSupportDataViewModel> data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        List<XShowListErrorMultipleSelectModel> ListItem = new List<XShowListErrorMultipleSelectModel>();
        //        foreach (var item in data)
        //        {
        //            var resSendRequestSupport = XHelper.Invoke(new ZRequest()
        //            {
        //                Code = "REQUEST_SUPPORT_SEND",
        //                Data = JsonConvert.SerializeObject(new ZRequestSupportSendRequest()
        //                {
        //                    ID = item.ID,
        //                    RequestCode = item.RequestCode,
        //                    ShopID = item.ShopID,
        //                    UserID = XHelper.Session.UserProfile.ID,
        //                })
        //            });
        //            if (resSendRequestSupport.Code != "00")
        //            {
        //                var ItemError = JsonConvert.DeserializeObject<XShowListErrorMultipleSelectModel>(resSendRequestSupport.Data);
        //                ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                {
        //                    RequestSupportCode = ItemError.RequestSupportCode,
        //                    Description = ItemError.Description

        //                });
        //            }
        //        }
        //        if (ListItem.Count > 0)
        //        {
        //            var TotalSuccess = data.Count - ListItem.Count;
        //            res.Code = "10";
        //            res.Message = "Duyệt yêu cầu thành công " + TotalSuccess + "/" + data.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
        //            res.Data = JsonConvert.SerializeObject(ListItem);
        //        }
        //        else
        //        {
        //            res.Code = "00";
        //            res.Message = "Thành công.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse SendRequestSupport(List<ListRequestSupportDataViewModel> data)
        //{
        //    ZResponse res = new ZResponse();
        //    List<XShowListErrorMultipleSelectModel> ListItem = new List<XShowListErrorMultipleSelectModel>();
        //    try
        //    {
        //        foreach (var item in data)
        //        {
        //            var resSearchInfoSendRequestSupport = SearchInfoSendRequestSupport(item.RequestCode, item.ShopID, XHelper.Session.UserProfile.ID);
        //            if (resSearchInfoSendRequestSupport.Code == "00")
        //            {
        //                var dataSearchInfoSendRequestSupport = JsonConvert.DeserializeObject<ZSearchInfoSendRequestSupportResponse>(resSearchInfoSendRequestSupport.Data);
        //                if (dataSearchInfoSendRequestSupport.Status != "W")
        //                {
        //                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                    {
        //                        RequestSupportCode = item.RequestCode,
        //                        Description = "Trạng thái đã bị thay đổi."
        //                    });
        //                }
        //                else if (dataSearchInfoSendRequestSupport.RequestSupportGroupID == 1 || dataSearchInfoSendRequestSupport.RequestSupportGroupID == 30)
        //                {
        //                    //Sửa thông tin đơn hàng hoặc Rút bưu gửi
        //                    //var resAcceptedStatus = AcceptedStatusRequestSupport(item.ID, item.ShopID);
        //                    //if (resAcceptedStatus.Code != "00")
        //                    //{
        //                    //    ListItem.Add(new ShowListErrorMultipleSelectModel()
        //                    //    {
        //                    //        RequestSupportCode = item.RequestCode,
        //                    //        Description = resAcceptedStatus.Message
        //                    //    });
        //                    //}

        //                    if (dataSearchInfoSendRequestSupport.RequestSupportTypeID == 7)
        //                    {
        //                        //Sửa tiền đơn hàng (tiền COD)
        //                        var resLockAcceptedStatus = LockAcceptedStatusRequestSupport(new ZRequestSupportLockAcceptedStatusRequest()
        //                        {
        //                            RequestSupportID = item.ID,
        //                            ShopID = item.ShopID,
        //                            UpdatedBy = XHelper.Session.UserProfile.ID
        //                        });
        //                        if (resLockAcceptedStatus.Code == "00")
        //                        {
        //                            var resCallApiPaypostCore = XHelper.InvokeApiPaypostCore(new ZRequest()
        //                            {
        //                                Code = "CODMoney/EditCOD",
        //                                Data = JsonConvert.SerializeObject(new ZRequestSupportEditCODRequest()
        //                                {
        //                                    ParcelCode = dataSearchInfoSendRequestSupport.ShippingCode,
        //                                    Amount = long.Parse(dataSearchInfoSendRequestSupport.UpdateInformation),
        //                                    PoCode = dataSearchInfoSendRequestSupport.AcceptancePosCode
        //                                })
        //                            });
        //                            if (resCallApiPaypostCore.Code == "00")
        //                            {
        //                                var resUnLockAcceptedStatus = UnLockAcceptedStatusRequestSupport(new ZRequestSupportUnLockAcceptedStatusRequest()
        //                                {
        //                                    RequestSupportID = item.ID,
        //                                    ShopID = item.ShopID,
        //                                    UpdatedBy = XHelper.Session.UserProfile.ID,
        //                                    Type = 1
        //                                });
        //                                if (resUnLockAcceptedStatus.Code != "00")
        //                                {
        //                                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                    {
        //                                        RequestSupportCode = item.RequestCode,
        //                                        Description = resUnLockAcceptedStatus.Message
        //                                    });
        //                                }
        //                            }
        //                            else
        //                            {
        //                                var resUnLockAcceptedStatus = UnLockAcceptedStatusRequestSupport(new ZRequestSupportUnLockAcceptedStatusRequest()
        //                                {
        //                                    RequestSupportID = item.ID,
        //                                    ShopID = item.ShopID,
        //                                    UpdatedBy = XHelper.Session.UserProfile.ID,
        //                                    Type = 2
        //                                });
        //                                if (resUnLockAcceptedStatus.Code == "00")
        //                                {
        //                                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                    {
        //                                        RequestSupportCode = item.RequestCode,
        //                                        Description = resCallApiPaypostCore.Message
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                    {
        //                                        RequestSupportCode = item.RequestCode,
        //                                        Description = resUnLockAcceptedStatus.Message
        //                                    });
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                            {
        //                                RequestSupportCode = item.RequestCode,
        //                                Description = resLockAcceptedStatus.Message
        //                            });
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var resAcceptedStatus = AcceptedStatusRequestSupport(item.ID, item.ShopID);
        //                        if (resAcceptedStatus.Code != "00")
        //                        {
        //                            ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                            {
        //                                RequestSupportCode = item.RequestCode,
        //                                Description = resAcceptedStatus.Message
        //                            });
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    List<XCallApiRequestSupportModel> ListRequestSupport = new List<XCallApiRequestSupportModel>();
        //                    ListRequestSupport.Add(new XCallApiRequestSupportModel()
        //                    {
        //                        Id = dataSearchInfoSendRequestSupport.RequestSupportCode,
        //                        AcountID = dataSearchInfoSendRequestSupport.CrmOrPayPostCode,
        //                        Employee__c = "", //dataSearchInfoSendRequestSupport.EmployeeID.ToString(),
        //                        Nhan_vien_CSKH__c = dataSearchInfoSendRequestSupport.UserCMS,
        //                        Tai_Lieu_Dinh_Kem__c = dataSearchInfoSendRequestSupport.Document == "" ? "" : XHelper.pathRequestSupport + dataSearchInfoSendRequestSupport.Document,
        //                        CaseNumber = "",
        //                        Description = dataSearchInfoSendRequestSupport.Description,
        //                        So_phieu_khieu_nai__c = "",
        //                        Li_do_khieu_nai__c = dataSearchInfoSendRequestSupport.ReasonForComplaint,
        //                        Priority = dataSearchInfoSendRequestSupport.Priority,
        //                        Origin = dataSearchInfoSendRequestSupport.Origin,
        //                        Nhom_dich_vu__c = "Dịch vụ bưu chính chuyển phát",//dataSearchInfoSendRequestSupport.ServiceGroup,
        //                        Location_Type__c = "Yêu cầu khách hàng",//dataSearchInfoSendRequestSupport.Scope,
        //                        Service__c = dataSearchInfoSendRequestSupport.TransportServiceCode,
        //                        Status = "Chưa xử lý",
        //                        So_Ho_So__c = "",
        //                        POS__c = dataSearchInfoSendRequestSupport.PosC,
        //                        Ng_y_ti_p_nh_n__c = dataSearchInfoSendRequestSupport.CreatedDate,
        //                        Sales_Order__c = dataSearchInfoSendRequestSupport.ShippingCode,
        //                        SuppliedName = dataSearchInfoSendRequestSupport.EmployeeName,
        //                        SuppliedPhone = dataSearchInfoSendRequestSupport.EmployeePhone,
        //                        SuppliedEmail = dataSearchInfoSendRequestSupport.EmployeeEmail,
        //                        Contact_Address__c = dataSearchInfoSendRequestSupport.EmployeeAddress,
        //                        Contact_province__c = dataSearchInfoSendRequestSupport.EmployeeProvince,
        //                        Contact_district__c = dataSearchInfoSendRequestSupport.EmployeeDistrict,
        //                        Contact_commune__c = dataSearchInfoSendRequestSupport.EmployeeWard,
        //                        CreatedDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"),
        //                        LastModifiedDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"),
        //                        Files = null,
        //                        Attachments = null
        //                    });

        //                    var resSendRequestSupport = XHelper.InvokeSyncGateway(new ZRequest()
        //                    {
        //                        Code = "CMS_REQUEST_SUPPORT",
        //                        Data = JsonConvert.SerializeObject(new ZCallApiRequestSupportRequest()
        //                        {
        //                            items = ListRequestSupport
        //                        })
        //                    });

        //                    if (resSendRequestSupport.Code == "00")
        //                    {
        //                        var resAcceptedStatus = AcceptedStatusRequestSupport(item.ID, item.ShopID);
        //                        if (resAcceptedStatus.Code != "00")
        //                        {
        //                            ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                            {
        //                                RequestSupportCode = item.RequestCode,
        //                                Description = resAcceptedStatus.Message
        //                            });
        //                        }
        //                    }
        //                    else if (resSendRequestSupport.Code == "01")
        //                    {
        //                        XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.SendRequestSupport: " + JsonConvert.SerializeObject(resSendRequestSupport));
        //                        var checkSuccessData = JsonConvert.DeserializeObject<ZCallApiRequestSupportResponse>(resSendRequestSupport.Data);
        //                        var checkRequestSupportCodeError = checkSuccessData.rows.Where(m => m.Id == item.RequestCode).ToList();
        //                        if (checkRequestSupportCodeError.Count > 0)
        //                        {
        //                            if (checkRequestSupportCodeError[0].error.Trim() == "Yêu cầu đã tồn tại trong hệ thống HTKH.")
        //                            {
        //                                var resExistStatus = ExistStatusRequestSupport(item.ID, item.ShopID); // Trạng thái đã tồn tại
        //                                if (resExistStatus.Code == "00")
        //                                {
        //                                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                    {
        //                                        RequestSupportCode = checkRequestSupportCodeError[0].Id,
        //                                        Description = checkRequestSupportCodeError[0].error
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                    {
        //                                        RequestSupportCode = checkRequestSupportCodeError[0].Id,
        //                                        Description = resExistStatus.Message
        //                                    });
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                                {
        //                                    RequestSupportCode = checkRequestSupportCodeError[0].Id,
        //                                    Description = checkRequestSupportCodeError[0].error
        //                                });
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                        {
        //                            RequestSupportCode = item.RequestCode,
        //                            Description = resSendRequestSupport.Message
        //                        });
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ListItem.Add(new XShowListErrorMultipleSelectModel()
        //                {
        //                    RequestSupportCode = item.RequestCode,
        //                    Description = resSearchInfoSendRequestSupport.Message
        //                });
        //            }
        //        }
        //        if (ListItem.Count > 0)
        //        {
        //            var TotalSuccess = data.Count - ListItem.Count;
        //            res.Code = "10";
        //            res.Message = "Duyệt yêu cầu thành công " + TotalSuccess + "/" + data.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
        //            res.Data = JsonConvert.SerializeObject(ListItem);
        //        }
        //        else
        //        {
        //            res.Code = "00";
        //            res.Message = "Thành công.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse SearchInfoSendRequestSupport(string RequestSupportCode, int ShopID, int UserID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT",
        //            Data = JsonConvert.SerializeObject(new ZSearchInfoSendRequestSupportRequest()
        //            {
        //                RequestSupportCode = RequestSupportCode,
        //                ShopID = ShopID,
        //                UserID = UserID
        //            })
        //        });
        //        if (res.Code != "00")
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.SearchInfoSendRequestSupport: " + JsonConvert.SerializeObject(res));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchInfoSendRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse AcceptedStatusRequestSupport(int RequestSupportID, int ShopID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_ACCEPTED_STATUS",
        //            Data = JsonConvert.SerializeObject(new ZRequestSupportAcceptedStatusRequest()
        //            {
        //                RequestSupportID = RequestSupportID,
        //                ShopID = ShopID,
        //                UpdatedBy = XHelper.Session.UserProfile.ID,
        //            })
        //        });
        //        if (res.Code != "00")
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.AcceptedStatusRequestSupport: " + JsonConvert.SerializeObject(res));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.AcceptedStatusRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse ExistStatusRequestSupport(int RequestSupportID, int ShopID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_EXIST_STATUS",
        //            Data = JsonConvert.SerializeObject(new ZRequestSupportExistStatusRequest()
        //            {
        //                RequestSupportID = RequestSupportID,
        //                ShopID = ShopID,
        //                UpdatedBy = XHelper.Session.UserProfile.ID,
        //            })
        //        });
        //        if (res.Code != "00")
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.ExistStatusRequestSupport: " + JsonConvert.SerializeObject(res));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ExistStatusRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse LockAcceptedStatusRequestSupport(ZRequestSupportLockAcceptedStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_LOCK_ACCEPTED_STATUS",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //        if (res.Code != "00")
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.LockAcceptedStatusRequestSupport: request= " + JsonConvert.SerializeObject(req) + "response=" + JsonConvert.SerializeObject(res));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.LockAcceptedStatusRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse UnLockAcceptedStatusRequestSupport(ZRequestSupportUnLockAcceptedStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_UNLOCK_ACCEPTED_STATUS",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //        if (res.Code != "00")
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.UnLockAcceptedStatusRequestSupport: request= " + JsonConvert.SerializeObject(req) + "response=" + JsonConvert.SerializeObject(res));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.UnLockAcceptedStatusRequestSupport: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse SendConfirmationEmail(SendMailModel req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var link = Security.MD5Encrypt(JsonConvert.SerializeObject(new
        //        {
        //            MobileNumber = req.MobileNumber,
        //            Password = req.Password
        //        }), Security.PASSWORD_SALT);

        //        var stringBody = "";
        //        stringBody = "<div>Chào " + req.FullName + ",";
        //        stringBody += "<p>Bạn đã được cấp tài khoản trên hệ thống EMS One. Thông tin đăng nhập như sau:</p>";
        //        stringBody += "<p>Số điện thoại đăng nhập: " + req.MobileNumber + "</p>";
        //        stringBody += "<p>Truy cập vào đây để tạo mật khẩu và sử dụng: <a href=" + XHelper.Ems247Url + "Account/UpdatePasswordView?data=" + link + ">tại đây</a> !</p>";
        //        stringBody += "<p>Trân trọng cảm ơn!</p></div>";

        //        //JObject parsedContent = JObject.Parse("{'bounce_address':'dev@ems.ems.com.vn','from': { 'address': 'transmail@ems.com.vn'},'to': [{'email_address': {'address': '" + req.ToEmail + "','name': 'ktcntt'}}],'subject':'EMS-ONE - KÍCH HOẠT TÀI KHOẢN','htmlbody':'" + stringBody + "'}");

        //        var to = new List<ZZohoSendMailToRequest>();
        //        to.Add(new ZZohoSendMailToRequest()
        //        {
        //            email_address = new ZZohoSendMailFromRequest()
        //            {
        //                address = req.ToEmail,
        //                name = req.FullName
        //            }
        //        });
        //        res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "ZOHO_SEND_MAIL",
        //            Data = JsonConvert.SerializeObject(new ZZohoSendMailRequest()
        //            {
        //                //to = new List<XApiZohoSendMailToModel>()
        //                //{
        //                //    new XApiZohoSendMailToModel(){ email_address = new XApiZohoSendMailFromModel(){ address = req.ToEmail, name = req.FullName } }
        //                //},
        //                to = to,
        //                subject = "EMS-ONE - KÍCH HOẠT TÀI KHOẢN",
        //                htmlbody = stringBody
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendConfirmationEmail: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse UpdateConfirmationEmail(int ShopID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "SHOP_UPDATE_CONFIRMATION_EMAIL",
        //            Data = JsonConvert.SerializeObject(new ZShopUpdateConfirmationEmailRequest()
        //            {
        //                ID = ShopID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.UpdateConfirmationEmail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        //public static ZResponse SearchListLabelPrint(ZLabelPrintSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "LABEL_PRINT_SEARCH_LIST",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListLabelPrint: " + ex.Message);
        //    }
        //    return res;
        //}

        public static List<PickUpAddressModel> SearchListPickUpAddress(ZPickUpAddressSearchListRequest req)
        {
            List<PickUpAddressModel> listItem = new List<PickUpAddressModel>();
            var res = PickUpAddressRepository.SearchList(req);
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPickUpAddressSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listItem.Add(new PickUpAddressModel()
                    {
                        ID = item.PickUpAddressID,
                        PickUpName = item.PickUpName,
                    });
                }
            }
            return listItem;
        }

        #region Danh sách đối tác
        public static List<PartnerModel> SearchListPartner(ZPartnerSearchListRequest req)
        {
            List<PartnerModel> listItem = new List<PartnerModel>();
            try
            {
                var res = PartnerRepository.SearchListPartner(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZPartnerSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new PartnerModel()
                        {
                            ID = item.ID,
                            PartnerCode = item.PartnerCode,
                            PartnerName = item.PartnerName,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListPartner: " + ex.Message);
            }
            return listItem;
        }
        #endregion
    }
}