using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EMS.API.Core
{
    public class XRedisService
    {
        private readonly IDatabase _db;
        public XRedisService()
        {
            _db = XRedisManager.Db;
        }

        public ZResponse ProcessOrderImportFile(Stream fileStream, string fileName, ZShopGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if(fileStream == null)
                {
                    res.Code = "02";
                    res.Message = "FileStream null";
                }


                if(string.IsNullOrWhiteSpace(fileName))
                {
                    res.Code = "02";
                    res.Message = "FileName rỗng";
                }

                
                string fileHash = XHelper.ComputeSha256(fileStream);

                string ext = Path.GetExtension(fileName).ToLowerInvariant();

                if(fileStream.CanSeek)
                    fileStream.Position = 0;

                IWorkbook workbook = null;
                if(ext == ".xls")
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
                else if(ext == ".xlsx" || ext == ".xlsm")
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    res.Code = "02";
                    res.Message = "File không đúng định dạng Excel";
                }

                ISheet sheet = workbook.GetSheetAt(0);
                if(sheet == null)
                {
                    res.Code = "02";
                    res.Message = "Không tìm thấy sheet";
                }

                var total = sheet.LastRowNum;
                if(total > 1000)
                {
                    res.Code = "02";
                    res.Message = "Hệ thống chỉ hỗ trợ import tối đa 1000 dòng";
                }

                long jobId = DateTime.UtcNow.Ticks;

                string customerId = req.ID.ToString();
                string jobKey = string.Format("import:job:{0}", jobId);
                string rowsKey = string.Format("import:job:{0}:rows", jobId);
                string customerJobsKey = string.Format("import:customer:{0}:jobs", customerId);

                _db.HashSet(jobKey, new HashEntry[]
                {
                    new HashEntry("jobId", jobId),
                    new HashEntry("shopId", req.ID),
                    new HashEntry("fileName", fileName ?? ""),
                    new HashEntry("fileHash", fileHash ?? ""),
                    new HashEntry("status", "Pending"),
                    new HashEntry("totalRows", 0),
                    new HashEntry("successRows", 0),
                    new HashEntry("failedRows", 0),
                    new HashEntry("createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                });


                ZShopGetByIDResponse ShopInformation = new ZShopGetByIDResponse();
                var resShopInformation = ShopRepository.GetByID(req.ID);
                if(resShopInformation.Code == "00")
                {
                    ShopInformation = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(resShopInformation.Data);
                }

                var isService108 = false;
                var isService421 = false;
                List<XTransportServiceModel> ListMainTransportService = new List<XTransportServiceModel>();
                List<XTransportServiceModel> ListExtraTransportService = new List<XTransportServiceModel>();
                List<XTransportServiceModel> ListTransportService = new List<XTransportServiceModel>();
                var resListTransportService = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = req.ID });
                if(resListTransportService.Code == "00")
                {
                    List<XTransportServiceModel> listItem = new List<XTransportServiceModel>();
                    var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resListTransportService.Data);
                    foreach(var item in list)
                    {
                        listItem.Add(new XTransportServiceModel()
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
                    ListTransportService = listItem;
                }
                if(ListTransportService.Count > 0)
                {
                    ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();

                    if(ListExtraTransportService.Where(m => m.ID == 108).ToList().Count > 0)
                    {
                        isService108 = true;
                    }

                    if(ListExtraTransportService.Where(m => m.ID == 421).ToList().Count > 0)
                    {
                        isService421 = true;
                    }
                }

                List<ZProductVariantSearchListResponse> ListProductVariant = new List<ZProductVariantSearchListResponse>();
                var resProductVariant = ProductVariantRepository.SearchList(new ZProductVariantSearchListRequest() { IsLock = "N", ShopID = req.ID });
                if(resProductVariant.Code == "00")
                {
                    ListProductVariant = JsonConvert.DeserializeObject<List<ZProductVariantSearchListResponse>>(resProductVariant.Data);
                }

                

                int totalRows = 0;
                int indexRow = 0;
                for(int row = 4; row <= sheet.LastRowNum; row++)
                {
                    ++indexRow;
                    var ReceiverProvinceID = 0;
                    var ReceiverWardID = 0;

                    var SenderProvinceID = 0;
                    var SenderWardID = 0;
                    long TotalAmount = 0;
                    if(sheet.GetRow(row) != null)
                    {
                        var ReceiverName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        if(!string.IsNullOrEmpty(ReceiverName)) //CHECK HANG NULL
                        {
                            var Product = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                            var OrderCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));

                            var ReceiverMobileNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                            var ReceiverStreet = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                            var ReceiverProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                            //var ReceiverDistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                            var ReceiverWardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                            var Message = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                            var NumberOfProduct = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));
                            var Weight = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(10));
                            var TotalCOD = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(11));
                            var OrderValue = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(12));
                            var Note = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(13));
                            var TransportMainService = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(14));
                            var TransportExtraService = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(15));
                            var SenderName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(16));
                            var SenderMobileNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(17));
                            var SenderStreet = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(18));
                            var SenderProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(19));
                            //var SenderDistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(21));
                            var SenderWardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(20));
                            var CollectionType = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(21));
                            var TransportPayer = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(22));
                            var Long = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(23));
                            var Width = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(24));
                            var Height = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(25));
                            var Channel = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(26));
                            var TotalPHU = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(27));


                            //Số lượng sản phẩm
                            if(string.IsNullOrEmpty(NumberOfProduct))
                            {
                                NumberOfProduct = "1";
                            }

                            //Sản phẩm
                            if(!string.IsNullOrEmpty(Product) && ListProductVariant.Count > 0)
                            {
                                var checkProductVariant = ListProductVariant.Where(m => m.Product.ProductVariant.ProductVariantSKU == Product).ToList();
                                if(checkProductVariant.Count > 0)
                                {
                                    TotalAmount = checkProductVariant.Select(m => m.Product.ProductVariant.ProductVariantPrice).First() * Convert.ToInt32(NumberOfProduct);

                                    //Trọng lượng sản phẩm
                                    if(string.IsNullOrEmpty(Weight))
                                    {
                                        Weight = (checkProductVariant.Select(m => m.Product.ProductVariant.ProductVariantWeight).First() * Convert.ToDouble(NumberOfProduct)).ToString();
                                    }
                                }
                            }

                            //Số điện thoại người nhận
                            if(!string.IsNullOrEmpty(ReceiverMobileNumber))
                            {
                                var ReplaceSpecialChar = Regex.Replace(ReceiverMobileNumber, @"[^\d]", String.Empty);
                                if(ReplaceSpecialChar.Substring(0, 3).Contains("840"))
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar.Substring(0, 3).Replace("840", "0") + ReplaceSpecialChar.Substring(3, ReplaceSpecialChar.Length - 3);
                                }
                                else if(ReplaceSpecialChar.Substring(0, 2).Contains("84"))
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar.Substring(0, 2).Replace("84", "0") + ReplaceSpecialChar.Substring(2, ReplaceSpecialChar.Length - 2);
                                }
                                else if(ReplaceSpecialChar.Substring(0, 1) != "0")
                                {
                                    ReceiverMobileNumber = "0" + ReplaceSpecialChar;
                                }
                                else
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar;
                                }
                            }

                            //Số điện thoại người gửi
                            if(!string.IsNullOrEmpty(SenderMobileNumber))
                            {
                                var ReplaceSpecialChar = Regex.Replace(SenderMobileNumber, @"[^\d]", String.Empty);
                                if(ReplaceSpecialChar.Substring(0, 3).Contains("840"))
                                {
                                    SenderMobileNumber = ReplaceSpecialChar.Substring(0, 3).Replace("840", "0") + ReplaceSpecialChar.Substring(3, ReplaceSpecialChar.Length - 3);
                                }
                                else if(ReplaceSpecialChar.Substring(0, 2).Contains("84"))
                                {
                                    SenderMobileNumber = ReplaceSpecialChar.Substring(0, 2).Replace("84", "0") + ReplaceSpecialChar.Substring(2, ReplaceSpecialChar.Length - 2);
                                }
                                else if(ReplaceSpecialChar.Substring(0, 1) != "0")
                                {
                                    SenderMobileNumber = "0" + ReplaceSpecialChar;
                                }
                                else
                                {
                                    SenderMobileNumber = ReplaceSpecialChar;
                                }
                            }

                            //Kênh bán hàng
                            Channel = string.IsNullOrEmpty(Channel) ? "TT" : XData.GetValueChannel(Channel);

                            //Người trả tiền cước
                            TransportPayer = string.IsNullOrEmpty(TransportPayer) ? "1" : XData.GetValueTransportPayer(TransportPayer);

                            //Dịch vụ chính
                            if(!string.IsNullOrEmpty(TransportMainService))
                            {
                                if(ListMainTransportService.Count > 0)
                                {
                                    var checkMainTransportService = ListMainTransportService.Where(m => m.Name.ToLower() == TransportMainService.ToLower()).ToList();
                                    if(checkMainTransportService.Count > 0)
                                    {
                                        TransportMainService = checkMainTransportService.Select(m => m.ID).First().ToString();
                                    }
                                    else
                                    {
                                        TransportMainService = "";
                                    }
                                }
                                else
                                {
                                    TransportMainService = "";
                                }
                            }

                            //Dịch vụ cộng thêm
                            if(!string.IsNullOrEmpty(TransportExtraService))
                            {
                                if(ListExtraTransportService.Count > 0)
                                {
                                    var tempTransportExtraService = "";
                                    foreach(var item in TransportExtraService.Split(';'))
                                    {
                                        var checkExtraTransportService = ListExtraTransportService.Where(m => m.Name.ToLower() == item.Trim().ToLower()).ToList();
                                        if(checkExtraTransportService.Count > 0)
                                        {
                                            var ExtraTransportServiceID = checkExtraTransportService.Select(m => m.ID).First().ToString();
                                            tempTransportExtraService += ExtraTransportServiceID + ";";
                                            if(ExtraTransportServiceID == "108")
                                            {
                                                if(string.IsNullOrEmpty(TotalCOD))
                                                {
                                                    TotalCOD = TotalAmount.ToString();
                                                }
                                            }
                                        }
                                    }
                                    TransportExtraService = tempTransportExtraService.TrimEnd(';');
                                }
                                else
                                {
                                    TransportExtraService = "";
                                }
                            }

                            if(isService108 == true)
                            {
                                if(!string.IsNullOrEmpty(TotalCOD) && TransportExtraService.Contains("108") == false)
                                {
                                    TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "108" : ";108";
                                }
                            }
                            else
                            {
                                TotalCOD = "";
                            }

                            if(isService421 == true)
                            {
                                if(!string.IsNullOrEmpty(TotalPHU) && TransportExtraService.Contains("421") == false)
                                {
                                    TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "421" : ";421";
                                }
                            }
                            else
                            {
                                TotalPHU = "";
                            }

                            if(TransportPayer == "2")
                            {
                                switch(TransportMainService)
                                {
                                    case "141":
                                    case "360":
                                    case "481":
                                        if(TransportExtraService.Contains("300") == false && TransportExtraService.Contains("221") == false)
                                        {
                                            TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "108" : ";108";
                                        }
                                        break;
                                    default:
                                        if(TransportExtraService.Contains("300") == false && TransportExtraService.Contains("221") == false)
                                        {
                                            TransportPayer = "1";
                                        }
                                        break;
                                }
                            }

                            //Giá trị đơn hàng
                            if(string.IsNullOrEmpty(OrderValue))
                            {
                                OrderValue = TotalAmount.ToString();
                            }

                            CollectionType = string.IsNullOrEmpty(CollectionType) ? ShopInformation.CollectionType.ToString() : XData.GetValueCollectionType(CollectionType);

                            string rowKey = string.Format("import:job:{0}:row:{1}", jobId, indexRow);
                            _db.HashSet(rowKey, new HashEntry[]
                            {
                                new HashEntry("jobId", jobId),
                                new HashEntry("rowNo", indexRow),
                                new HashEntry("Product", Product ?? ""),
                                new HashEntry("OrderCode", OrderCode ?? ""),
                                new HashEntry("ReceiverName", ReceiverName ?? ""),
                                new HashEntry("ReceiverMobileNumber", ReceiverMobileNumber ?? ""),
                                new HashEntry("ReceiverStreet", ReceiverStreet ?? ""),
                                new HashEntry("ReceiverProvinceName", ReceiverProvinceName ?? ""),
                                new HashEntry("ReceiverWardName", ReceiverWardName ?? ""),
                                new HashEntry("Message", Message ?? ""),
                                new HashEntry("NumberOfProduct", NumberOfProduct ?? ""),
                                new HashEntry("Weight", Weight ?? ""),
                                new HashEntry("TotalCOD", TotalCOD ?? ""),
                                new HashEntry("OrderValue", OrderValue ?? ""),
                                new HashEntry("Note", Note ?? ""),
                                new HashEntry("TransportMainService", TransportMainService ?? ""),
                                new HashEntry("TransportExtraService", TransportExtraService ?? ""),
                                new HashEntry("SenderName", SenderName ?? ""),
                                new HashEntry("SenderMobileNumber", SenderMobileNumber ?? ""),
                                new HashEntry("SenderStreet", SenderStreet ?? ""),
                                new HashEntry("SenderProvinceName", SenderProvinceName ?? ""),
                                new HashEntry("SenderWardName", SenderWardName ?? ""),
                                new HashEntry("CollectionType", CollectionType ?? ""),
                                new HashEntry("TransportPayer", TransportPayer ?? ""),
                                new HashEntry("Long", Long ?? ""),
                                new HashEntry("Width", Width ?? ""),
                                new HashEntry("Height", Height ?? ""),
                                new HashEntry("Channel", Channel ?? ""),
                                new HashEntry("TotalPHU", TotalPHU ?? ""),
                                new HashEntry("ShopType",ShopInformation.TypeShop),
                                new HashEntry("ReceiverProvinceID", ReceiverProvinceID),
                                new HashEntry("ReceiverWardID", ReceiverWardID),
                                new HashEntry("SenderProvinceID", SenderProvinceID),
                                new HashEntry("SenderWardID", SenderWardID),

                                new HashEntry("parseStatus", "0"),
                                new HashEntry("parseError", "")
                            }); ;

                            // lưu danh sách row của job
                            _db.SetAdd(rowsKey, rowKey);

                            // TTL 24h cho từng row
                            _db.KeyExpire(rowKey, TimeSpan.FromHours(24));

                            totalRows++;
                        }
                    }
                }

                // cập nhật lại tổng dòng thật sự đã lưu
                _db.HashSet(jobKey, new HashEntry[]
                {
                new HashEntry("totalRows", totalRows)
                });

                // index job theo customer/shop
                _db.SortedSetAdd(
                    customerJobsKey,
                    jobId.ToString(),
                    DateTimeOffset.Now.ToUnixTimeSeconds()
                );

                // TTL
                _db.KeyExpire(jobKey, TimeSpan.FromHours(24));
                _db.KeyExpire(rowsKey, TimeSpan.FromHours(24));
                _db.KeyExpire(customerJobsKey, TimeSpan.FromHours(24));


                _db.StreamAdd("stream:order-import", new NameValueEntry[]
                {
                    new NameValueEntry("jobId", jobId),
                    new NameValueEntry("shopId", req.ID),
                    new NameValueEntry("fileHash", fileHash ?? "")
                });
                res.Code = "00";
                res.Message = "Thành công";
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }

            return res;
               
        }

        public List<XCustomeImportOrderJobInfo> GetImportJobsByCustomer(int customerId)
        {
            var result = new List<XCustomeImportOrderJobInfo>();

            string customerJobsKey = string.Format("import:customer:{0}:jobs", customerId);

            // lấy tất cả, mới nhất trước
            RedisValue[] jobIds = _db.SortedSetRangeByRank(customerJobsKey, 0, -1, Order.Descending);

            foreach(var jobIdValue in jobIds)
            {
                if(!jobIdValue.HasValue)
                    continue;

                string jobKey = string.Format("import:job:{0}", jobIdValue.ToString());
                HashEntry[] entries = _db.HashGetAll(jobKey);

                if(entries == null || entries.Length == 0)
                    continue;

                Func<string, string> get = key =>
                {
                    foreach(var x in entries)
                    {
                        if(x.Name == key) return x.Value;
                    }
                    return "";
                };

                result.Add(new XCustomeImportOrderJobInfo
                {
                    JobId = ToLong(get("jobId")),
                    FileName = get("fileName"),
                    Status = get("status"),
                    TotalRows = ToInt(get("totalRows")),
                    SuccessRows = ToInt(get("successRows")),
                    FailedRows = ToInt(get("failedRows")),
                    CreatedAt = get("createdAt")
                });
            }

            return result;
        }

        public List<ArrayList> GetItems(int shopId, long jobId)
        {
            var result = new List<ArrayList>();

            string jobKey = string.Format("import:job:{0}", jobId);
            string rowsKey = string.Format("import:job:{0}:rows", jobId);

            // 1. kiểm tra job có tồn tại không
            HashEntry[] jobEntries = _db.HashGetAll(jobKey);
            if(jobEntries == null || jobEntries.Length == 0)
                return result;

            Func<string, string> getJob = key =>
            {
                foreach(var x in jobEntries)
                {
                    if(x.Name == key) return x.Value;
                }
                return "";
            };

            // 2. check đúng shop_id
            long redisShopId = 0;
            long.TryParse(getJob("shopId"), out redisShopId);
            if(redisShopId != shopId)
                return result;

            // 3. lấy danh sách row key
            RedisValue[] rowKeys = _db.SetMembers(rowsKey);
            if(rowKeys == null || rowKeys.Length == 0)
                return result;

            var orderedRowKeys = rowKeys
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x =>
                {
                    var parts = x.Split(':');
                    int rowNo = 0;
                    if(parts.Length > 0)
                        int.TryParse(parts[parts.Length - 1], out rowNo);
                    return rowNo;
                })
                .ToList();

            foreach(var rowKey in orderedRowKeys)
            {
                HashEntry[] rowEntries = _db.HashGetAll(rowKey);
                if(rowEntries == null || rowEntries.Length == 0)
                    continue;

                Func<string, string> get = key =>
                {
                    foreach(var x in rowEntries)
                    {
                        if(x.Name == key) return x.Value;
                    }
                    return "";
                };

                ArrayList data = new ArrayList()
                {
                    "",
                    get("Product"),
                    get("OrderCode"),
                    get("ReceiverName"),
                    get("ReceiverMobileNumber"),
                    get("ReceiverStreet"),
                    get("ReceiverProvinceName"),
                    get("ReceiverWardName"),
                    get("Message"),
                    get("NumberOfProduct"),
                    get("Weight"),
                    get("TotalCOD"),
                    get("OrderValue"),
                    get("Note"),
                    get("TransportMainService"),
                    get("TransportExtraService"),
                    get("SenderName"),
                    get("SenderMobileNumber"),
                    get("SenderStreet"),
                    get("SenderProvinceName"),
                    get("SenderWardName"),
                    get("CollectionType"),
                    get("TransportPayer"),
                    get("Long"),
                    get("Width"),
                    get("Height"),
                    get("Channel"),
                    get("TotalPHU"),
                    get("ReceiverProvinceID"),
                    get("ReceiverWardID"),
                    get("SenderProvinceID"),
                    get("SenderWardID")
                };

                result.Add(data);
            }

            return result;
        }

        private int ToInt(string value)
        {
            int rs;
            int.TryParse(value, out rs);
            return rs;
        }

        private long ToLong(string value)
        {
            long rs;
            long.TryParse(value, out rs);
            return rs;
        }
    }
}