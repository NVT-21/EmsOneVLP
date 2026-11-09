using System.Collections.Generic;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EMS.Entity;
using System;
using EMS.Library;
using System.Text.RegularExpressions;
using System.Web;
using EMS.MVC.Frontend.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Collections;

namespace EMS.MVC.Frontend
{
    public class XCommon
    {
        #region Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã

        #region Danh sách Tỉnh/ Thành phố
        public static List<ProvinceModel> SearchListProvince(ZProvinceListRequest req)
        {
            List<ProvinceModel> ListItem = new List<ProvinceModel>();
            try
            {
                var res = PWDRepository.SearchListProvince(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProvinceListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new ProvinceModel()
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
            return ListItem;
        }
        #endregion

        #region Danh sách Quận/ Huyện
        public static List<DistrictModel> SearchListDistrictByProvinceID(ZDistrictGetByProvinceIDRequest req)
        {
            List<DistrictModel> ListItem = new List<DistrictModel>();
            try
            {
                var res = PWDRepository.SearchListDistrictByProvinceID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZDistrictGetByProvinceIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new DistrictModel()
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
            return ListItem;
        }
        #endregion

        #region Danh sách Phường/ Xã
        public static List<WardModel> SearchListWardByDistrictID(ZWardGetByDistrictIDRequest req)
        {
            List<WardModel> ListItem = new List<WardModel>();
            try
            {
                var res = PWDRepository.SearchListWardByDistrictID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWardGetByDistrictIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new WardModel()
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
            return ListItem;
        }

        public static List<WardModel> SearchListWardByProvinceID(ZWardGetByDistrictIDRequest req)
        {
            List<WardModel> ListItem = new List<WardModel>();
            try
            {
                var res = PWDRepository.SearchListWardByDistrictID(req);
                if(res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWardGetByDistrictIDResponse>>(res.Data);
                    foreach(var item in list)
                    {
                        ListItem.Add(new WardModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListWardByDistrictID: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Autocomplete) của VNP)

        public static ZResponse SearchPWDByVNPMapsAutocomplete(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resVNPMapsAutocomplete = VNPRepository.VNPMapsAutocomplete(new ZVNPMapsAutocompleteRequest() { Size = 1, Text = Address });
                if (resVNPMapsAutocomplete.Code == "00")
                {
                    var DataAutoComple = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteDataResponse>(resVNPMapsAutocomplete.Data);
                    var regionGID = Convert.ToInt32(DataAutoComple.features[0].properties.region_gid);
                    var countyGID = Convert.ToInt32(DataAutoComple.features[0].properties.county_gid);
                    var localityGID = Convert.ToInt32(DataAutoComple.features[0].properties.locality_gid);
                    res = PWDRepository.SearchPWDByVMapCode(new ZMapsGetByVMapCodeRequest()
                    {
                        ProvinceVMapCode = regionGID == 0 ? 0 : regionGID - 100000000, //VMapCode của Tỉnh
                        DistrictVMapCode = countyGID == 0 ? 0 : countyGID - 100000000, //VMapCode của Quận
                        WardVMapCode = localityGID == 0 ? 0 : localityGID - 100000000, //VMapCode của Phường
                        StreetVMapCode = DataAutoComple.features[0].properties.name // Số nhà + đường
                    });
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Lỗi phân tích địa chỉ.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByVNPMapsAutocomplete: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Search) của VNP)

        public static ZResponse SearchPWDByVNPMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resVNPMapsAutocomplete = VNPRepository.VNPMapsSearch(new ZVNPMapsSearchRequest() { Size = 1, Text = Address });
                if (resVNPMapsAutocomplete.Code == "00")
                {
                    var DataSearch = JsonConvert.DeserializeObject<ZVNPMapsSearchDataResponse>(resVNPMapsAutocomplete.Data);
                    var region_gid = Convert.ToInt32(DataSearch.features[0].properties.region_gid); //VMapCode của Tỉnh
                    var county_gid = Convert.ToInt32(DataSearch.features[0].properties.county_gid); //VMapCode của Quận
                    var locality_gid = Convert.ToInt32(DataSearch.features[0].properties.locality_gid); //VMapCode của Phường
                    var Street = DataSearch.features[0].properties.name; // Số nhà + đường
                    res = PWDRepository.SearchPWDByVMapCode(new ZMapsGetByVMapCodeRequest()
                    {
                        ProvinceVMapCode = region_gid == 0 ? 0 : region_gid - 100000000, //mã bưu chính = gid - 100000000
                        DistrictVMapCode = county_gid == 0 ? 0 : county_gid - 100000000, //mã bưu chính = gid - 100000000
                        WardVMapCode = locality_gid == 0 ? 0 : locality_gid - 100000000, //mã bưu chính = gid - 100000000
                        StreetVMapCode = Street
                    });
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Lỗi phân tích địa chỉ.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByVNPMapsSearch: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Search) của EMS)
        public static ZResponse SearchPWDByEMSMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resEMSMapSearch = EMSRepository.EMSMapSearch(new ZEMSMapSearchRequest() { AddressFull = Address });
                if (resEMSMapSearch.Code == "00")
                {
                    var dataEMSMapSearch = JsonConvert.DeserializeObject<ZEMSMapSearchDataResponse>(resEMSMapSearch.Data);
                    if (dataEMSMapSearch.Province_Code == "0")
                    {
                        res.Code = "01";
                        res.Message = "Không tìm thấy địa chỉ";
                    }
                    else
                    {
                        res = PWDRepository.SearchPWDByMBC2017Code(new ZMapsGetByMBC2017CodeRequest()
                        {
                            ProvinceMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Province_Code_2017) ? "0" : dataEMSMapSearch.Province_Code_2017,
                            DistrictMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.District_Code_2017) ? "0" : dataEMSMapSearch.District_Code_2017,
                            WardMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Ward_Code_2017) ? "0" : dataEMSMapSearch.Ward_Code_2017,
                            Street = dataEMSMapSearch.Label.Split(',')[0]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByEMSMapSearch: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchPWDByEMSMapsSearchV2(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resEMSMapSearch = EMSRepository.EMSVMapSearch(new ZEMSMapSearchRequest() { AddressFull = Address });
                if(resEMSMapSearch.Code == "00")
                {
                    var dataEMSMapSearch = JsonConvert.DeserializeObject<ZEMSMapSearchDataResponse>(resEMSMapSearch.Data);
                    if(dataEMSMapSearch.Province_Code == "0")
                    {
                        res.Code = "01";
                        res.Message = "Không tìm thấy địa chỉ";
                    }
                    else
                    {
                        res = PWDRepository.SearchPWDByMBCCode(new ZMapsGetByMBC2017CodeRequest()
                        {
                            ProvinceMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Province_Code) ? "0" : dataEMSMapSearch.Province_Code,
                            DistrictMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.District_Code) ? "0" : dataEMSMapSearch.District_Code,
                            WardMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Ward_Code) ? "0" : dataEMSMapSearch.Ward_Code,
                            Street = dataEMSMapSearch.Label.Split(',')[0]
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByEMSMapsSearchV2: " + ex.Message);
            }
            return res;
        }
        #endregion

        #endregion

        #region Cửa hàng (Shop)

        #region Danh sách mã CRM/PP
        public static List<ZShopGetListCrmOrPaypostCodeResponse> SearchListCrmOrPaypostCode(ZShopGetListCrmOrPaypostCodeRequest req) //Mã cửa hàng và mã cửa hàng con
        {
            List<ZShopGetListCrmOrPaypostCodeResponse> ListItem = new List<ZShopGetListCrmOrPaypostCodeResponse>();
            try
            {
                var res = ShopRepository.SearchListCrmOrPaypostCode(req);
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZShopGetListCrmOrPaypostCodeResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon: SearchListCrmOrPaypostCode: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #endregion

        #region Nhân viên (Employee)
        public static List<ZEmployeeListResponse> SearchListEmployee(ZEmployeeListRequest req)
        {
            List<ZEmployeeListResponse> ListItem = new List<ZEmployeeListResponse>();
            try
            {
                var res = EmployeeRepository.SearchListEmployee(req);
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZEmployeeListResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon: SearchListEmployee: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #region Nhóm nhân viên (EmployeeGroup)
        public static List<EmployeeGroupModel> SearchListEmployeeGroup(ZEmployeeGroupListRequest req)
        {
            List<EmployeeGroupModel> ListItem = new List<EmployeeGroupModel>();
            try
            {
                var res = EmployeeGroupRepository.SearchListEmployeeGroup(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZEmployeeGroupListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new EmployeeGroupModel()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon: SearchListEmployeeGroup: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #region Kho (WareHouse)

        #region Danh sách kho
        public static List<WareHouseModel> SearchListWareHouse(ZWareHouseSearchListRequest req)
        {
            List<WareHouseModel> ListItem = new List<WareHouseModel>();
            try
            {
                var res = WareHouseRepository.SearchListWareHouse(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new WareHouseModel()
                        {
                            ID = item.ID,
                            WareHouseCode = item.WareHouseCode,
                            WareHouseName = item.WareHouseName,
                            FullName = item.FullName,
                            MobileNumber = item.MobileNumber,
                            FullAddress = item.FullAddress,
                            IsDefault = item.IsDefault,
                            IsLock = item.IsLock,
                            CreatedName = item.EmployeeName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListWareHouse: " + ex.Message);
            }
            return ListItem;
        }

        //public static List<ZWareHouseSearchListResponse> SearchListWareHouseSetDefault(string IsWareHouse, string IsLock, int ShopID)
        //{
        //    List<ZWareHouseListResponse> listItem = new List<ZWareHouseListResponse>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "WAREHOUSE_LIST",
        //            Data = JsonConvert.SerializeObject(new ZWareHouseListRequest()
        //            {
        //                IsWareHouse = IsWareHouse,
        //                IsLock = IsLock,
        //                ShopID = ShopID,
        //            })
        //        });
        //        if (res.Code == "00")
        //        {
        //            var list = JsonConvert.DeserializeObject<List<ZWareHouseListResponse>>(res.Data);
        //            foreach (var item in list)
        //            {
        //                listItem.Add(new ZWareHouseListResponse()
        //                {
        //                    ID = item.ID,
        //                    WareHouseName = item.WareHouseName,
        //                    IsDefault = item.IsDefault,
        //                    FullAddress = item.FullAddress
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListWareHouseSetDefault: " + ex.Message);
        //        listItem = null;
        //    }
        //    return listItem;
        //}
        #endregion

        #region Lấy thông tin kho mặc định

        public static ZWarehouseGetDefaultWarehouseResponse DetailWareHouseDefault(ZWarehouseGetDefaultWarehouseRequest req)
        {
            ZWarehouseGetDefaultWarehouseResponse Item = null;
            try
            {
                var res = WareHouseRepository.DetailWareHouseDefault(req);
                if (res.Code == "00")
                {
                    Item = JsonConvert.DeserializeObject<ZWarehouseGetDefaultWarehouseResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon: DetailWareHouseDefault: " + ex.Message);
            }
            return Item;
        }

        #endregion

        #region Chỉnh sửa thông tin phiếu Xuất - Nhập - Điều chỉnh
        public static ZResponse InventoryEdit(ZInventoryEditRequest req) // Chỉnh sửa thông tin phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryEdit: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #endregion

        #region Nhà cung cấp (Provider)

        #region Danh sách nhà cung cấp
        public static List<ProviderModel> SearchListProvider(ZProviderSearchListRequest req)
        {
            List<ProviderModel> ListItem = new List<ProviderModel>();
            try
            {
                var res = ProviderRepotiory.SearchListProvider(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProviderSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new ProviderModel()
                        {
                            ID = item.ID,
                            ProviderCode = item.Code,
                            ProviderName = item.Name,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProvider: " + ex.Message);
            }
            return ListItem;
        }
        #endregion
        #endregion

        #region Dịch vụ vận chuyển (TransportService)

        #region Danh sách dịch vụ vận chuyển
        public static List<TransportServiceModel> SearchListTransportService(ZTransportServiceSearchListByShopIDRequest req)
        {
            List<TransportServiceModel> listItem = new List<TransportServiceModel>();
            try
            {
                var res = TransportServiceRepository.SearchListTransportServiceByShopID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new TransportServiceModel()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            SpecialService = item.SpecialService,
                            IsLock = item.IsLock,
                            IsDefault = item.IsDefault,
                            Sort = item.Sort,
                            Description = item.Description,
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

        #region Dịch vụ vận chuyển Grab
        public static List<GrabServiceModel> GrabListService()
        {
            List<GrabServiceModel> listItem = new List<GrabServiceModel>();
            try
            {
                var res = GrabRepository.GrabListService();
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZGrabDeliveryServiceData>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new GrabServiceModel()
                        {
                            Code = item.code,
                            Name = item.name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GrabListService: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        //#region Chi tiết dịch vụ vận chuyển
        //public static ZTransportServiceGetByIDResponse DetailTransportServiceByTransportServiceID(ZTransportServiceGetByIDRequest req)
        //{
        //    ZTransportServiceGetByIDResponse Item = null;
        //    try
        //    {
        //        var res = TransportServiceRepository.DetailTransportServiceByTransportServiceID(req);
        //        if (res.Code == "00")
        //        {
        //            Item = JsonConvert.DeserializeObject<ZTransportServiceGetByIDResponse>(res.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.DetailTransportServiceByTransportServiceID: " + ex.Message);
        //    }
        //    return Item;
        //}
        //#endregion

        #endregion

        #region Khách hàng (Customer)

        #region Danh sách khách hàng
        public static List<CustomerModel> SearchListCustomer(ZCustomerSearchListRequest req)
        {
            List<CustomerModel> ListItem = new List<CustomerModel>();
            try
            {
                var res = CustomerRepository.SearchListV2(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new CustomerModel()
                        {
                            ID = item.ID,
                            MobileNumber = item.MobileNumber,
                            FullName = item.FullName,
                            Email = item.Email
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListCustomer: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #endregion

        #region Nhãn khách hàng (CustomerSticker)

        #region Danh sách nhãn khách hàng
        public static List<CustomerStickerModel> SearchListCustomerSticker(ZCustomerStickerListRequest req)
        {
            List<CustomerStickerModel> ListItem = new List<CustomerStickerModel>();
            try
            {
                var res = CustomerStickerRepository.SearchListCustomerSticker(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerStickerListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new CustomerStickerModel()
                        {
                            ID = item.ID,
                            Code = item.StickerCode,
                            Name = item.StickerName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListCustomerSticker: " + ex.Message);
            }
            return ListItem;
        }

        public static List<SelectListItem> GetListCustomerStickerName()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CUSTOMER_STICKER_LIST",
                    Data = JsonConvert.SerializeObject(new ZCustomerStickerListRequest()
                    {
                        StickerCode = "",
                        StickerName = "",
                        IsLock = "N",
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerStickerListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.StickerName,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetListCustomerStickerName: " + ex.Message);
            }
            return listItem;
        }

        #endregion

        #endregion

        #region Nhóm khách hàng (CustomerGroup)

        #region Danh sách nhóm khách hàng
        public static List<CustomerGroupModel> SearchListCustomerGroup(ZCustomerSearchGroupRequest req)
        {
            List<CustomerGroupModel> ListItem = new List<CustomerGroupModel>();
            try
            {
                var res = CustomerGroupRepository.SearchListCustomerGroup(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerSearchGroupResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new CustomerGroupModel()
                        {
                            ID = item.ID,
                            Code = item.GroupCode,
                            Name = item.GroupName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListCustomerGroup: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #endregion

        #region Nhóm/ Loại yêu cầu hỗ trợ (RequestSupportGroup/ Type)

        #region Danh sách nhóm/ loại yêu cầu hỗ trợ
        public static List<ZRequestSupportGroupListResponse> SearchListRequestSupportGroup(ZRequestSupportGroupListRequest req)
        {
            List<ZRequestSupportGroupListResponse> ListItem = new List<ZRequestSupportGroupListResponse>();
            try
            {
                var res = RequestSupportRepository.SearchListRequestSupportGroup(req);
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZRequestSupportGroupListResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListRequestSupportGroup: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #endregion

        #region Đơn hàng (Order)

        #region Lấy chi tiết đơn hàng bằng ShippingCode

        public static ZResponse GetByShippingCode(ZOrderGetByShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.GetByShippingCode(req);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetByShippingCode: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region In đơn hàng
        public static List<PrintOrderTypeDataModel> ListPrintOrder(PrintOrderViewModel req)
        {
            List<PrintOrderTypeDataModel> oList = new List<PrintOrderTypeDataModel>();
            var ListOrderPrint = JsonConvert.DeserializeObject<List<ZOrderPrintRequest>>(req.ListDataOrderPrint);
            foreach (var itemOrderPrint in ListOrderPrint)
            {
                var res = OrderRepository.PrintOrder(new ZOrderPrintRequest() { EMSOneCode = itemOrderPrint.EMSOneCode, ShopID = itemOrderPrint.ShopID, PrintType = req.PrintType });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZOrderPrintResponse>(res.Data);
                    oList.Add(new PrintOrderTypeDataModel()
                    {
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        ShippingCode = item.ShippingCode,
                        PickUpProvinceName = item.PickUpProvinceName,
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

        public static PrintOrderTypeDataModel SinglePrintOrder(PrintSingleOrderViewModel req)
        {

            var res = OrderRepository.PrintSingleOrder(new ZOrderSinglePrintRequest() { CrmOrPaypostCode = req.CrmOrPayPostCode, ShippingCde = req.ShippingCodes, PrintType = req.PrintType });
            if(res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZOrderPrintResponse>(res.Data);
                var model = new PrintOrderTypeDataModel()
                {
                    OrderID = item.OrderID,
                    OrderCode = item.OrderCode,
                    BatchCode = item.BatchCode,
                    ShippingCode = item.ShippingCode,
                    PickUpProvinceName = item.PickUpProvinceName,
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
                };
                return model;
            }
            else
            {
                var model = new PrintOrderTypeDataModel();
                return model;
            }
        }
        #endregion

        #endregion

        #region Biến thể (ProductVariant)
        public static List<ZProductVariantGetByWareHouseIDResponse> SearchListProductVariantByWareHouseID(ZProductVariantGetByWareHouseIDRequest req)
        {
            List<ZProductVariantGetByWareHouseIDResponse> ListItem = new List<ZProductVariantGetByWareHouseIDResponse>();
            try
            {
                var res = ProductVariantRepository.GetByWareHouseID(req);
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZProductVariantGetByWareHouseIDResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon: SearchListProductVariantByWareHouseID: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #region Thư viện hoặc hàm hỗ trợ 

        #region Convert thành địa chỉ chi tiết
        public static string ConvertAddress(string no, string street, string ward, string district, string province)
        {
            string address = string.Empty;
            if (string.IsNullOrEmpty(address))
                address = no;
            else
                address = address + ", " + no;

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
        #endregion

        #region Thay thế các non-unicode
        public static string ReplaceNonUnicodeCharacter(string Value)
        {
            return Regex.Replace(Value, @"[\u200c]", "");
            //return Regex.Replace(Value, @"[\u0000-\u0008\u000A-\u001F\u0100-\uFFFF]", "");
            //return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Value));
            //return Regex.Replace(Value, @"[^\u0000-\u007F]+", string.Empty);
        }
        #endregion

        #region Kiểm tra kiểu dữ liệu của cột trong file Excel 
        public static string CheckStyleColumnExcel(ICell cell)
        {
            string value = "";
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        if (HSSFDateUtil.IsCellDateFormatted(cell))
                        {
                            //DateFormat df = new SimpleDateFormat("dd/MM/yyyy");
                            //return df.Format(cell.getDateCellValue());
                            value = cell.DateCellValue.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            //DecimalFormat df = new DecimalFormat("##.###############");
                            //return df.Format(cell.NumericCellValue());
                            value = cell.NumericCellValue.ToString();
                        }
                        break;
                    case CellType.String:
                        value = Convert.ToString(cell.StringCellValue);
                        break;
                    case CellType.Formula:
                        if (cell.CachedFormulaResultType == CellType.String)
                        {
                            value = Convert.ToString(cell.StringCellValue);
                        }
                        else
                        {
                            value = Convert.ToString(cell.NumericCellValue);
                        }
                        break;
                    default:
                        value = Convert.ToString(cell.StringCellValue);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(value))
            {
                value = ReplaceNonUnicodeCharacter(value.Trim());
            }
            return value;
        }
        #endregion

        #region Xóa file
        public static void DeleteFile(string FileName)
        {
            var FullPath = HttpContext.Current.Server.MapPath("~/Assets/temp/" + FileName);
            try
            {
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                }
                else
                {
                    //DirectoryInfo folder = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Assets/temp"));
                    //FileInfo[] files = folder.GetFiles();
                    //foreach (FileInfo file in files)
                    //{
                    //    file.Delete();
                    //}
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.DeleteFile: " + ex.Message);
            }
        }
        #endregion

        #region Thay thế các ký tự . , trong format number
        public static string ReplaceFormatNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf(",") > 0)
                {
                    value = value.Replace(",", "");
                }
                else if (value.IndexOf(".") > 0)
                {
                    value = value.Replace(".", "");
                }
            }
            return value;
        }
        #endregion

        #region Upload image từ excel
        public static ZResponse ExcelUploadImage(string Data)
        {
            ZResponse res = new ZResponse();
            try
            {
                HttpClient _clientDataFile = null;
                Uri uriResult;
                bool checkUri = Uri.TryCreate(Data, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (_clientDataFile == null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Uri BaseAddress = new Uri(XHelper.URLImage);
                    _clientDataFile = new HttpClient();
                    _clientDataFile.BaseAddress = BaseAddress;
                    _clientDataFile.DefaultRequestHeaders.Accept.Clear();
                    _clientDataFile.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    ServicePointManager.FindServicePoint(BaseAddress).ConnectionLeaseTimeout = 5 * 60 * 1000;
                }

                byte[] bytes;
                var type = "";
                if (checkUri)
                {
                    using (var webClient = new WebClient())
                    {
                        bytes = webClient.DownloadData(Data);
                    }
                    int index = Data.LastIndexOf('.') + 1;
                    type = Data.Substring(index);
                }
                else// Base64
                {
                    var arr = Data.Split(';');
                    bytes = Convert.FromBase64String(arr[1].Split(',')[1]);
                    type = arr[0].Split('/')[1];
                }

                if (type != "jpg" && type != "gif" && type != "png" && type != "jpeg")
                {
                    res.Code = "01";
                    res.Message = "Vui lòng tải lên hình ảnh thuộc loại: jpg, jpeg, gif, png.";
                    return res;
                }

                var fileName = Guid.NewGuid().ToString() + "." + type;
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StreamContent(new MemoryStream(bytes));
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName
                };
                form.Add(content);
                HttpResponseMessage response = _clientDataFile.PostAsync("Handle/UploadImage", form).Result;
                res = response.Content.ReadAsAsync<ZResponse>().Result;
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ExcelUploadImage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #endregion

        #region Tính cước (TransportPostage)
        public static ZResponse TransportPostageCharged(ZTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.TransportPostageCharged(req);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.TransportPostageCharged: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Địa chỉ lấy hàng (PICK_UP_ADDRESS)

        #region Danh sách địa chỉ lấy hàng
        public static List<PickUpAddressModel> SearchListPickUpAddress(ZPickUpAddressSearchListRequest req)
        {
            List<PickUpAddressModel> ListItem = new List<PickUpAddressModel>();
            try
            {
                var res = PickUpAddressRepository.SearchListPickUpAddress(new ZPickUpAddressSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (res.Code == "00")
                {
                    var listItem = JsonConvert.DeserializeObject<List<ZPickUpAddressSearchListResponse>>(res.Data);
                    foreach(var item in listItem)
                    {
                        ListItem.Add(new PickUpAddressModel() 
                        {
                            PickUpAddressID = item.PickUpAddressID,
                            PickUpName = item.PickUpName,
                            FullName = item.FullName,
                            MobileNumber = item.MobileNumber,
                            Province = new ProvinceModel() 
                            {
                                ID = item.ProvinceID,
                                Name = item.ProvinceName,
                            }, 
                            Ward = new WardModel() 
                            {
                                ID = item.WardID,
                                Name = item.WardName,
                            },
                            Street = item.Street,
                            FullAddress = item.FullAddress,
                            IsDefault = item.IsDefault,
                            QuantityLabelPrint = item.QuantityLabelPrint,
                            CollectionPostOfficeID = item.CollectionPostOfficeID
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListPickUpAddress: " + ex.Message);
            }
            return ListItem;
        }
        #endregion

        #endregion

        #region Nhóm quyền (PERMISSION)

        #region Danh sách nhóm quyền
        public static List<ZShopGroupPermissionListResponse> SearchListGroupPermission(ZShopGroupPermissionListRequest req)
        {
            List<ZShopGroupPermissionListResponse> ListItem = new List<ZShopGroupPermissionListResponse>();
            try
            {
                var res = PermissionRepository.SearchListGroupPermission(req);
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZShopGroupPermissionListResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListGroupPermission: " + ex.Message);
            }
            return ListItem;
        }

        #endregion

        #endregion

        #region Sản phẩm (Product)

        #region Danh sách đơn vị tính của sản phẩm
        public static List<ProductUnitModel> SearchListProductUnit(ZProductUnitSearchListRequest req)
        {
            List<ProductUnitModel> listItem = new List<ProductUnitModel>();
            try
            {
                var res = ProductUnitRepository.SearchListProductUnit(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductUnitSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductUnitModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProductUnit: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #endregion

        #region Nhóm sản phẩm (ProductGroup)

        #region Danh sách nhóm sản phẩm
        public static List<ProductGroupModel> SearchListProductGroup(ZProductGroupListRequest req)
        {
            List<ProductGroupModel> listItem = new List<ProductGroupModel>();
            try
            {
                var res = ProductGroupRepository.SearchListProductGroup(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductGroupListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductGroupModel()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            CodeAndName = item.Code + " - " + item.Name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProductGroup: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #endregion

        #region Bưu cục thu gom

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
                            CodeAndName = CodeAndName,
                            FullAddress = item.FullAddress,
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

        public static List<CollectionPostOfficeModel> SearchListRecentCollectionPostOffice(ZCollectionPostOfficeSearchListRecentRequest req)
        {
            List<CollectionPostOfficeModel> listItem = new List<CollectionPostOfficeModel>();
            try
            {
                var res = CollectionPostOfficeRepository.SearchListRecent(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCollectionPostOfficeSearchListRecentResponse>>(res.Data);
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
                            CodeAndName = CodeAndName,
                            FullAddress = item.FullAddress,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListRecentCollectionPostOffice: " + ex.Message);
            }
            return listItem;
        }

        #endregion

        #endregion

        #region Danh sách mẫu in
        public static List<LabelPrintModel> SearchListLabelPrint(ZLabelPrintSearchListRequest req)
        {
            List<LabelPrintModel> listItem = new List<LabelPrintModel>();
            try
            {
                var res = LabelPrintRepository.SearchListLabelPrint(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZLabelPrintSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new LabelPrintModel()
                        {
                            LabelPrintID = item.LabelPrintID,
                            LabelName = item.LabelName,
                            FullName = item.FullName,
                            MobileNumber = item.MobileNumber,
                            ProvinceName = item.ProvinceName,
                            DistrictName = item.DistrictName,
                            WardName = item.WardName,
                            FullAddress = item.FullAddress,
                            IsDefault = item.IsDefault,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListLabelPrint: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        public static List<ProductModel> SearchListProductVariantByShopID(ZProductVariantGetByShopIDRequest req)
        {
            List<ProductModel> listItem = new List<ProductModel>();
            try
            {
                var res = ProductVariantRepository.GetByShopID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByShopIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductModel()
                        {
                            ProductName = item.ProductName,
                            ProductVariant = new ProductVariantModel() { ID = item.ID, Options = item.Options },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProductVariantByShopID: " + ex.Message);
            }
            return listItem;
        }

        public static List<ProductVariantModel> SearchListProductVariantByProductID(ZProductVariantGetByProductIDRequest req)
        {
            List<ProductVariantModel> listItem = new List<ProductVariantModel>();
            try
            {
                var res = ProductVariantRepository.GetByProductID(req);
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByProductIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductVariantModel()
                        {
                            ID = item.ID,
                            Avatar = item.Avatar,
                            Options = item.Options,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProductVariantByProductID: " + ex.Message);
            }
            return listItem;
        }

        //public static List<SelectListItem> SearchListProductVariant()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PRODUCT_VARIANT_GET_BY_SHOP_ID",
        //            Data = JsonConvert.SerializeObject(new ZProductVariantGetByShopIDRequest()
        //            {
        //                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //            })
        //        });
        //        if (res.Code == "00")
        //        {
        //            var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByShopIDResponse>>(res.Data);
        //            foreach (var item in list)
        //            {
        //                listItem.Add(new SelectListItem()
        //                {
        //                    Text = item.ProductName + "( " + item.Options + ")",
        //                    Value = item.ID.ToString()
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListProductVariant: " + ex.Message);
        //        listItem = null;
        //    }
        //    return listItem;
        //}

        public static ZResponse SearchListPermissionMenu(ZPermissionMenuListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERMISSION_LIST_MENU",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListPermissionMenu: " + ex.Message);
            }
            return res;
        }

        public static bool CheckRegex(string Data, string Type)
        {
            var Regular = "";
            switch (Type)
            {
                case "Date":
                    Regular = @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$";
                    break;
                case "Number":
                    Regular = @"^[0-9]+$";
                    break;
                case "MobileNumber":
                    if (Data.Length <= 10)
                    {
                        Regular = @"\b((03|05|07|08|09)+([0-9]{8}))\b";
                    }
                    else
                    {
                        Regular = @"\b((02)+([0-9]{9}))\b";
                    }
                    break;
                default:
                    //Cho phép nhập: 
                    // + tiếng việt
                    // + 0-9
                    // + -'’()\/.,|
                    // + khoảng trắng
                    Regular = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ0-9\-–'’()\/.,|\s]+$";
                    break;
            }
            Regex regex = new Regex(Regular, RegexOptions.CultureInvariant | RegexOptions.Singleline);
            return regex.IsMatch(Data);
        }

        public static ZResponse CheckPermission(string ControllerName, string ActionName)
        {
            ZResponse res = new ZResponse();
            HttpContext ctx = HttpContext.Current;
            if ((ctx.Session[XHelper.SessionEmployee] as EmployeeModel) != null)
            {
                if ((ctx.Session[XHelper.SessionEmployee] as EmployeeModel).StoreOwner == "Y")
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    return res;
                }
                //var PositionListMenu = XHelper.Session.ListMenu.Count() - 1;
                //var ListMenu = XHelper.Session.ListMenu[PositionListMenu];
                res = SearchListPermissionAllow(new ZPermissionAllowListRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                List<string> ListPermission = new List<string>();
                if (res.Code == "00")
                {
                    var ListItem = JsonConvert.DeserializeObject<List<ZPermissionAllowListResponse>>(res.Data);
                    foreach (var item in ListItem)
                    {
                        ListPermission.Add(item.FunctionalCode);
                    }
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Không lấy được thông tin quyền.";
                }
                //if (ListMenu.listFunctional != null)
                //{
                //    foreach (var itemFunctional in ListMenu.listFunctional)
                //    {
                //        listpermission.Add(itemFunctional.FunctionalCode);
                //    }
                //}

                string Function = ControllerName + "_" + ActionName;
                if (!ListPermission.Contains(Function))
                {
                    res.Code = "403";
                    res.Message = "Không có quyền truy cập chức năng này";
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                }
            }
            else
            {
                res.Code = "408";
                res.Message = "Thời gian đăng nhập đã hết, vui lòng đăng nhập lại.";
            }
            return res;
        }

        public static ZResponse SearchListPermissionAllow(ZPermissionAllowListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERMISSION_LIST_ALLOW",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchListPermissionAllow: " + ex.Message);
            }
            return res;
        }

        public static List<SelectListItem> GetListGroupPermission(int ShopLevel)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GROUP_PERMISSION_LIST",
                    Data = JsonConvert.SerializeObject(new ZShopGroupPermissionListRequest()
                    {
                        ShopLevel = ShopLevel,
                        IsShop = "Y",
                        IsLock = "N"
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZShopGroupPermissionListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.GroupPermissionName,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "Helpers.GetListGroupPermission: " + ex.Message);
                listItem = null;
            }
            return listItem;
        }

        public static ZResponse InventorySearch(ZInventorySearchListRequest req) // Tìm phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_SEARCH_BY_SHOP_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventorySearch: " + ex.Message);
            }
            return res;
        }

        //public static ZInventoryGetDataByIDResponse InventoryGetDataByID(ZInventoryGetDataByIDRequest req) // Lấy thông tin phiếu Xuất - Nhập - Điều chỉnh
        //{
        //    ZInventoryGetDataByIDResponse model = new ZInventoryGetDataByIDResponse();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "INVENTORY_GET_DATA_BY_ID",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //        if (res.Code == "00")
        //        {
        //            model = JsonConvert.DeserializeObject<ZInventoryGetDataByIDResponse>(res.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryGetDataByID: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
        //    }
        //    return model;
        //}

        public static List<ZWareHouseSearchListResponse> GetListWareHouseSetDefault(string IsWareHouse, string IsLock, int ShopID)
        {
            List<ZWareHouseSearchListResponse> listItem = new List<ZWareHouseSearchListResponse>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_LIST",
                    Data = JsonConvert.SerializeObject(new ZWareHouseSearchListRequest()
                    {
                        IsLock = IsLock,
                        ShopID = ShopID,
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ZWareHouseSearchListResponse()
                        {
                            ID = item.ID,
                            WareHouseName = item.WareHouseName,
                            IsDefault = item.IsDefault,
                            FullAddress = item.FullAddress
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "Helpers.GetListWareHouseSetDefault: " + ex.Message);
                listItem = null;
            }
            return listItem;
        }

        public static List<SelectListItem> GetListProductVariant()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_GET_BY_SHOP_ID",
                    Data = JsonConvert.SerializeObject(new ZProductVariantGetByShopIDRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByShopIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.ProductName + "( " + item.Options + ")",
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetListProductVariant: " + ex.Message);
                listItem = null;
            }
            return listItem;
        }

        public static List<ProductVariantGetByWareHouseIDModel> GetListProductVariantByWareHouseID_V2(int WareHouseID)
        {
            List<ProductVariantGetByWareHouseIDModel> listItem = new List<ProductVariantGetByWareHouseIDModel>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_GET_BY_WAREHOUSE_ID",
                    Data = JsonConvert.SerializeObject(new ZProductVariantGetByWareHouseIDRequest()
                    {
                        WareHouseID = WareHouseID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByWareHouseIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new ProductVariantGetByWareHouseIDModel()
                        {
                            DetailID = item.ProductID + "_" + item.ProductVariantID,
                            DetailName = item.ProductName + " (" + item.ProductVariantName + ")",
                            ProductVariantSKU = item.ProductVariantSKU,
                            ProductVariantPrice = item.ProductVariantPrice,
                            NumberOfInventory = item.NumberOfInventory
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetListProductVariantByWareHouseID_V2: " + ex.Message);
            }
            return listItem;
        }

        public static ZResponse InventoryAdd(ZInventoryAddRequest req) // Tạo phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryAdd: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static List<SelectListItem> GetListWarehouse(string IsWareHouse, string IsLock, int ShopID)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_LIST",
                    Data = JsonConvert.SerializeObject(new ZWareHouseSearchListRequest()
                    {
                        IsLock = IsLock,
                        ShopID = ShopID,
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.WareHouseName,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "Helpers.GetListWareHouse: " + ex.Message);
                listItem = null;
            }
            return listItem;
        }

        public static ZResponse InventoryDetailAdd(ZInventoryDetailAddRequest req) // Thêm chi tiêt phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryAdd: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static List<ZInventoryDetailGetByInventoryIDResponse> InventoryDetailGetDataByInventoryID(ZInventoryDetailGetByInventoryIDRequest req) // Lấy chi tiết phiếu Xuất - Nhập - Điều chỉnh
        {
            List<ZInventoryDetailGetByInventoryIDResponse> model = new List<ZInventoryDetailGetByInventoryIDResponse>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_GET_BY_INVENTORY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code == "00")
                {
                    model = JsonConvert.DeserializeObject<List<ZInventoryDetailGetByInventoryIDResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryDetailGetDataByInventoryID: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
            }
            return model;
        }

        public static ZResponse InventoryDetailEdit(ZInventoryDetailEditRequest req) // Chỉnh sửa chi tiết phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryDetailEdit: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse InventoryDetailRemove(ZInventoryDetailRemoveRequest req) // Xoá chi tiết phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryDetailRemove: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse InventoryUpdateStatus(ZInventoryUpdateStatusRequest req) // Cập nhật trạng thái phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_UPDATE_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryUpdateStatus: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse InventoryCancel(ZInventoryCancelRequest req) // Huỷ phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_CANCEL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.InventoryCancel: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        //public static ZResponse ListConversion(ZConversionSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "CONVERSION_SEARCH_LIST",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ListConversion: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse MessageOfConversion(ZMessageSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "MESSAGE_SEARCH_LIST",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.MessageOfConversion: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse ConnectPageSearchList(ZConnectPageSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "CONNECT_PAGE_SEARCH_LIST",
        //            Data = JsonConvert.SerializeObject(new ZConnectPageSearchListRequest()
        //            {
        //                ShopID = req.ShopID,
        //                PageID = req.PageID,
        //                Channel = req.Channel,
        //                IsActive = req.IsActive
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ConnectPageSearchList: " + ex.Message);
        //    }
        //    return res;
        //}

        //public static ZResponse ZaloSendMessage(ZZaloSendMessageRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.InvokeZaloV1(new ZRequest()
        //        {
        //            Code = "https://openapi.zalo.me/v2.0/oa/message",
        //            Data = JsonConvert.SerializeObject(req)
        //        }, "POST", req.AccessToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ZaloSendMessage: " + ex.Message);
        //    }
        //    return res;
        //}

        public static List<EmployeeListEmployeeInPermissionModel> GetListEmployeeInPermission()
        {
            List<EmployeeListEmployeeInPermissionModel> oListItem = new List<EmployeeListEmployeeInPermissionModel>();

            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "EMPLOYEE_LIST_EMPLOYEE_IN_PERMISSION",
                Data = JsonConvert.SerializeObject(new ZEmployeeListEmployeeInPermissionRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ShopLevel = XHelper.Session.EmployeeProfile.ShopInformation.Level,
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZEmployeeListEmployeeInPermissionResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new EmployeeListEmployeeInPermissionModel()
                    {
                        EmployeeGroupPermissionID = item.EmployeeGroupPermissionID,
                        EmployeeGroupPermissionName = item.EmployeeGroupPermissionName,
                        ListEmployee = item.ListEmployee
                    });
                }
            }
            return oListItem;
        }

        public static ZShopGetPickupInformationResponse GetPickupInformation()
        {
            ZShopGetPickupInformationResponse Info = new ZShopGetPickupInformationResponse();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GET_PICKUP_INFORMATION",
                    Data = JsonConvert.SerializeObject(new ZShopGetPickupInformationRequest()
                    {
                        ID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    Info = JsonConvert.DeserializeObject<ZShopGetPickupInformationResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetPickupInformation: " + ex.Message);
            }
            return Info;
        }

        public static List<SelectListItem> GetListProductName()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_LIST",
                    Data = JsonConvert.SerializeObject(new ZProductListRequest()
                    {
                        Code = "",
                        Name = "",
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.ProductName,
                            Value = item.ID.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetListProductName: " + ex.Message);
                listItem = null;
            }
            return listItem;
        }

        #region Translate 
        public static string TranslateAPI(string inputLanguage, string outputLanguage, string inputText)
        {
            var outputText = "";
            HttpClient client = new HttpClient();
            var rs = client.GetAsync(string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", inputLanguage, outputLanguage, inputText)).Result;
            if (rs.IsSuccessStatusCode)
            {
                var response = rs.Content.ReadAsStringAsync().Result;
                var convertResponse = JsonConvert.DeserializeObject<ArrayList>(response);
                var getFirstArray = convertResponse[0];
                var convertFirstArray = (JArray)getFirstArray;
                outputText = convertFirstArray.First.First.ToString();
            }
            return outputText;
        }
        #endregion

        #region Ngày dự kiến giao hàng
        public static ZResponse SearchDateExpectedDelivery(ZExpectedDeliverySearchDateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ExpectedDeliveryRepository.SearchDate(req);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchDateExpectedDelivery: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin khách hàng bằng số điện thoại
        public static ZResponse SearchCustomerByMobileNumber(ZCustomerGetByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerRepository.SearchCustomerByMobileNumber(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchCustomerByMobileNumber: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}