using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Text.RegularExpressions;
namespace EMS.Library
{
    public static class XData
    {
        #region Danh sách loại tài khoản
        public static List<SelectListItem> ListAccountType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "-1", Text = "Dùng thử (7 ngày)" },
            new SelectListItem() { Value = "1", Text = "1 năm" },
            new SelectListItem() { Value = "2", Text = "2 năm" },
            new SelectListItem() { Value = "3", Text = "3 năm" },
            new SelectListItem() { Value = "4", Text = "4 năm" },
            new SelectListItem() { Value = "5", Text = "5 năm" },
            new SelectListItem() { Value = "99", Text = "Vĩnh viễn" }
        };

        public static List<SelectListItem> GetListAccountType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListAccountType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameAccountType(string Value)
        {
            foreach (var item in ListAccountType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static List<SelectListItem> ListTypeBusiness = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Cá thể", Text = "Cá thể" },
            new SelectListItem() { Value = "Công ty", Text = "Công ty" }
        };

        #region Danh sách trạng thái tài khoản
        public static List<SelectListItem> ListAccountStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Chờ phê duyệt" },
            new SelectListItem() { Value = "A", Text = "Đã chấp nhận" },
            new SelectListItem() { Value = "R", Text = "Đã từ chối" },
            new SelectListItem() { Value = "D", Text = "Đã xóa" }
        };

        public static List<SelectListItem> GetListAccountStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListAccountStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameAccountStatus(string Status)
        {
            foreach (var item in ListAccountStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static List<SelectListItem> ListNumberOfEmployee = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "9", Text = "< 10" },
            new SelectListItem() { Value = "30", Text = "10 - 30" },
            new SelectListItem() { Value = "50", Text = "31 - 50" },
            new SelectListItem() { Value = "100", Text = "51 - 100" },
            new SelectListItem() { Value = "500", Text = "101 - 500" },
            new SelectListItem() { Value = "501", Text = "> 500" },
        };
        public static List<SelectListItem> ListShopLevel = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Trung tâm" },
            //new SelectListItem() { Value = "2", Text = "Chi nhánh" },
            //new SelectListItem() { Value = "3", Text = "Cửa hàng" }
        };
        public static Dictionary<int, string> AccountTypeDictionary = new Dictionary<int, string>()
        {
            { -1, "Dùng thử" },   //0
            { 1, "1 năm"},        //1
            { 2, "2 năm"},        //2
            { 3, "3 năm"},        //3
            { 4, "4 năm"},        //4
            { 5, "5 năm"},        //5
            { 99, "Vĩnh viễn"},   //6
            { 0, "" },            //7
        };
        //public static List<SelectListItem> ListPaymentMethod = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Value = "1", Text = "Thanh toán khi nhận hàng (COD)" },
        //    new SelectListItem() { Value = "2", Text = "Chuyển khoản qua ngân hàng" }
        //};

        #region Danh sách trạng thái Mở/ Khóa
        public static List<SelectListItem> ListIsLock = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Mở" },
            new SelectListItem() { Value = "Y", Text = "Khoá" }
        };

        public static List<SelectListItem> GetListIsLock()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListIsLock)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameIsLock(string Value)
        {
            foreach (var item in ListIsLock)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static List<SelectListItem> ListIsActive = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Hoạt động" },
            new SelectListItem() { Value = "Y", Text = "Không hoạt động" }
        };
        public static List<SelectListItem> ListTransport = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Không giao hàng" },
            new SelectListItem() { Value = "Y", Text = "Có giao hàng" }
        };
        public static List<SelectListItem> ListIsSercurity = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Hiện thông tin" },
            new SelectListItem() { Value = "Y", Text = "Ẩn thông tin" }
        };
        public static List<SelectListItem> ListPromotionCodeType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Giảm theo số tiền" },
            new SelectListItem() { Value = "2", Text = "Giảm theo %" },
            new SelectListItem() { Value = "3", Text = "Miễn phí vận chuyển" },
            new SelectListItem() { Value = "4", Text = "Đồng giá" }
        };

        #region Danh sách kênh bán hàng

        public static List<SelectListItem> ListChannel = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "WEB", Text = "Website" },
            new SelectListItem() { Value = "LAZ", Text = "Lazada" },
            new SelectListItem() { Value = "FB", Text = "Facebook" },
            new SelectListItem() { Value = "ZA", Text = "Zalo" },
            new SelectListItem() { Value = "SP", Text = "Shopee" },
            new SelectListItem() { Value = "TT", Text = "Trực tiếp"},
            new SelectListItem() { Value = "TIK", Text = "TiktokShop" },
            new SelectListItem() { Value = "PAN", Text = "Partner" },
            //new SelectListItem() { Value = "GRA", Text = "GRAB" },
        };

        public static List<SelectListItem> GetListChannel()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListChannel)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameChannel(string Channel)
        {
            foreach (var item in ListChannel)
            {
                if (item.Value == Channel)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static string GetValueChannel(string Channel)
        {
            foreach (var item in ListChannel)
            {
                if (item.Text.ToLower() == Channel.ToLower())
                {
                    return item.Value;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách loại đơn hàng
        public static List<SelectListItem> ListTypeOrder = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "ALL", Text = "Đơn hàng thường" },
            new SelectListItem() { Value = "GRA", Text = "Đơn hàng siêu tốc" },
        };

        public static List<SelectListItem> GetListTypeOrder()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTypeOrder)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        #region Danh sách loại bưu gửi
        public static List<SelectListItem> ListPostalType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Đơn lẻ" },
            new SelectListItem() { Value = "2", Text = "Đơn lô" },
            new SelectListItem() { Value = "3", Text = "Tất cả" },
        };

        public static List<SelectListItem> GetListPostalType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPostalType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNamePostalType(string Value)
        {
            foreach (var item in ListPostalType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách nơi tạo đơn hàng
        public static List<SelectListItem> ListPlaceOfCreateOrder = new List<SelectListItem>()
        {
            new SelectListItem(){ Value = "EMSONE", Text = "EMSOne" },
            new SelectListItem(){ Value = "EMSENTERPRISE", Text = "EMSEnterprise"},
            new SelectListItem(){ Value = "PARTNER", Text = "API Đối tác"},
        };

        public static List<SelectListItem> GetListPlaceOfCreateOrder()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPlaceOfCreateOrder)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        public static List<SelectListItem> ListPromotionCodeApplyForOption = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Tất cả đơn hàng" },
            new SelectListItem() { Value = "2", Text = "Trị giá đơn hàng từ" },
            new SelectListItem() { Value = "3", Text = "Nhóm sản phẩm" },
            new SelectListItem() { Value = "4", Text = "Sản phẩm" },
            new SelectListItem() { Value = "5", Text = "Nhóm khách hàng" },
            new SelectListItem() { Value = "6", Text = "Khách hàng" },
            new SelectListItem() { Value = "7", Text = "Biến thể" }
        };
        public static List<SelectListItem> ListPromotionCustomer = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Không giới hạn" },
            new SelectListItem() { Value = "2", Text = "Tất cả đơn hàng" },
            new SelectListItem() { Value = "3", Text = "Nhận email quảng cáo" },
            new SelectListItem() { Value = "4", Text = "Khách hàng thân thiết" },
            new SelectListItem() { Value = "5", Text = "Khách hàng tiềm năng" },
            new SelectListItem() { Value = "6", Text = "Trong nước" }
        };
        public static List<SelectListItem> PromotionListNumberOfUse = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Không giới hạn"},
            new SelectListItem() { Value = "2", Text = "Số lần sử dụng theo khách hàng"},
            new SelectListItem() { Value = "3", Text = "Số lần sử dụng theo mã khuyến mãi"}
        };
        public static Dictionary<string, string> ChannelDictionary = new Dictionary<string, string>()
        {
            {"WEB", "Website" },
            {"LAZ", "Lazada" },
            {"FB", "Facebook" },
            {"ZA", "Zalo" },
            {"SP", "Shopee"},
            {"TT", "Trực tiếp"}
        };
        public static List<SelectListItem> ListPromotionName = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Giảm theo số tiền" },
            new SelectListItem() { Value = "2", Text = "Giảm theo %" },
            new SelectListItem() { Value = "3", Text = "Đồng giá" }
        };
        public static List<SelectListItem> ListPromotionCoditionUse = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Số lượng sản phẩm áp dụng" },
            new SelectListItem() { Value = "2", Text = "Tổng giá trị đơn hàng" }
        };
        public static List<SelectListItem> ListPromotionScopeOfUse = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "trên cả đơn hàng" },
            new SelectListItem() { Value = "2", Text = "từng sản phẩm trong đơn hàng" }
        };
        public static List<SelectListItem> ListPromotionApplyForOption = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "3", Text = "Nhóm sản phẩm" },
            new SelectListItem() { Value = "4", Text = "Sản phẩm" },
            new SelectListItem() { Value = "7", Text = "Biến thể" }
        };
        public static Dictionary<int, string> PaymentMethodsDictionary = new Dictionary<int, string>()
        {
            { 1, "Thanh toán khi nhận hàng (COD)"},        //1
            { 2, "Chuyển khoản qua ngân hàng"},            //2
        };


        #region Trạng thái phiếu nhập kho
        public static List<SelectListItem> ListInventoryImportStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "W", Text = "Chờ xác nhận" },
            new SelectListItem() { Value = "F", Text = "Hoàn thành" },
            new SelectListItem() { Value = "R", Text = "Từ chối" },
            new SelectListItem() { Value = "C", Text = "Đã huỷ" },
        };

        public static List<SelectListItem> GetListInventoryImportStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListInventoryImportStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameInventoryImportStatus(string Status)
        {
            foreach (var item in ListInventoryImportStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Trạng thái phiếu điều chỉnh kho
        public static List<SelectListItem> ListInventoryAdjustStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "W", Text = "Chờ xác nhận" },
            new SelectListItem() { Value = "F", Text = "Hoàn thành" },
            new SelectListItem() { Value = "R", Text = "Từ chối" },
            new SelectListItem() { Value = "C", Text = "Đã huỷ" },
        };

        public static List<SelectListItem> GetListInventoryAdjustStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListInventoryAdjustStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameInventoryAdjustStatus(string Status)
        {
            foreach (var item in ListInventoryAdjustStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Trạng thái phiếu xuất kho
        public static List<SelectListItem> ListInventoryOutputStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Tạo mới" },
            new SelectListItem() { Value = "P", Text = "Chờ xử lý" },
            new SelectListItem() { Value = "O", Text = "Hoàn thành" },
            new SelectListItem() { Value = "C", Text = "Đã từ chối" }
        };

        public static List<SelectListItem> GetListInventoryOutputStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListInventoryOutputStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameInventoryOutputStatus(string Status)
        {
            foreach (var item in ListInventoryOutputStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Trạng thái phiếu kiểm kho
        public static List<SelectListItem> ListStockTakeStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "D", Text = "Nháp" },
            new SelectListItem() { Value = "N", Text = "Chờ xác nhận" },
            new SelectListItem() { Value = "A", Text = "Hoàn thành" },
            new SelectListItem() { Value = "R", Text = "Từ chối"},
        };

        public static List<SelectListItem> GetListStockTakeStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListStockTakeStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameStockTakeStatus(string Status)
        {
            foreach (var item in ListStockTakeStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static Dictionary<string, string> OrderStatusDictionary = new Dictionary<string, string>()
        {
            { "N", "Tạo mới"},
            { "T", "Đang giao dịch" },
            { "F", "Hoàn thành" },
            { "C", "Huỷ bỏ" }
        };

        #region Trạng thái yêu cầu đặt hàng
        public static List<SelectListItem> ListRequestOrderStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Tạo mới" },
            new SelectListItem() { Value = "1", Text = "Đã tạo đơn hàng" },
            new SelectListItem() { Value = "2", Text = "Đang xử lý" },
        };

        public static List<SelectListItem> GetListRequestOrderStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestOrderStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameRequestOrderStatus(string Status)
        {
            foreach (var item in ListRequestOrderStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static List<SelectListItem> ListPaymentStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Chờ thanh toán" },
            new SelectListItem() { Value = "1", Text = "Đã thanh toán" },
        };

        //public static List<SelectListItem> ListTransportStatus = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Value = "W", Text = "Chờ lấy hàng" },
        //    new SelectListItem() { Value = "P", Text = "Đã lấy hàng" }, //Pull off
        //    new SelectListItem() { Value = "N", Text = "Đang phát hàng" }, //Un pull off
        //    new SelectListItem() { Value = "D", Text = "Phát không thành công" }, //Down
        //    new SelectListItem() { Value = "S", Text = "Phát thành công" }, //Ship
        //    new SelectListItem() { Value = "T", Text = "Chuyển hoàn" }, //Tranfer
        //};

        #region Trạng thái đơn hàng

        public static List<SelectListItem> ListOrderStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "D", Text = "Đơn nháp" },
            new SelectListItem() { Value = "N", Text = "Đã tạo" },
            new SelectListItem() { Value = "L", Text = "Đơn tại cửa hàng" },
            new SelectListItem() { Value = "W", Text = "Chờ lấy hàng" },
            new SelectListItem() { Value = "P", Text = "Đã lấy hàng" },
            new SelectListItem() { Value = "S", Text = "Đang vận chuyển" },
            new SelectListItem() { Value = "A", Text = "Xác nhận đến BCP" },
            new SelectListItem() { Value = "T", Text = "Đang phát hàng" },
            new SelectListItem() { Value = "F", Text = "Phát không thành công" },
            new SelectListItem() { Value = "G", Text = "Phát thành công" },
            new SelectListItem() { Value = "H", Text = "Đã thu tiền" },
            new SelectListItem() { Value = "I", Text = "Đã trả tiền" },
            new SelectListItem() { Value = "U", Text = "Chờ chuyển hoàn" },
            new SelectListItem() { Value = "V", Text = "Đang chuyển hoàn" },
            new SelectListItem() { Value = "Z", Text = "Chuyển hoàn thành công" },
            new SelectListItem() { Value = "C", Text = "Đã huỷ" },
            new SelectListItem() { Value = "Q", Text = "[PHU]Đã thu tiền khách hàng" }
        };

        public static List<SelectListItem> GetListOrderStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListOrderStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameOrderStatus(string Status)
        {
            foreach (var item in ListOrderStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static List<SelectListItem> ListRequestSupportServiceGroup = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Bưu chính chuyển phát", Text = "Bưu chính chuyển phát" },
            new SelectListItem() { Value = "Tài chính bưu chính", Text = "Tài chính bưu chính" },
            new SelectListItem() { Value = "Phân phối truyền thông", Text = "Phân phối truyền thông" },
        };

        public static List<SelectListItem> ListRequestSupportScope = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Trong nước", Text = "Trong nước" },
            new SelectListItem() { Value = "Quốc tế", Text = "Quốc tế" },
        };

        #region Trạng thái yêu cầu hỗ trợ
        public static List<SelectListItem> ListRequestSupportStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Tạo mới" },
            new SelectListItem() { Value = "W", Text = "Chưa xử lý" },
            new SelectListItem() { Value = "R", Text = "Không duyệt" },
            new SelectListItem() { Value = "P", Text = "Đang xử lý" },
            new SelectListItem() { Value = "T", Text = "Đã xử lý" },
            new SelectListItem() { Value = "F", Text = "Đã hoàn thành" },
            new SelectListItem() { Value = "O", Text = "Mở lại" },
            //new SelectListItem() { Value = "C", Text = "Huỷ" },
            new SelectListItem() { Value = "E", Text = "Yêu cầu đã tồn tại" },
        };

        public static List<SelectListItem> GetListRequestSupportStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestSupportStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameRequestSupportStatus(string Status)
        {
            foreach (var item in ListRequestSupportStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách phân loại bưu gửi căn cứ vào dịch vụ chính của bưu gửi
        public static List<SelectListItem> ListTransportServiceType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Ems thường" },
            new SelectListItem() { Value = "1", Text = "Ems sự vụ" },
            new SelectListItem() { Value = "3", Text = "Ems thỏa thuận" },
            new SelectListItem() { Value = "4", Text = "Chuyển tiếp" },
            new SelectListItem() { Value = "5", Text = "Ems TMĐT" },
        };

        public static List<SelectListItem> GetListTransportServiceType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTransportServiceType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameListTransportServiceType(string Value)
        {
            foreach (var item in ListTransportServiceType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách phân loại hàng hóa căn cứ vào dịch vụ chính của bưu gửi
        public static List<SelectListItem> ListTransportServiceClassification = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "D", Text = "Tài liệu" },
            new SelectListItem() { Value = "M", Text = "Hàng hóa" },
        };

        public static List<SelectListItem> GetListTransportServiceClassification()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTransportServiceClassification)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameListTransportServiceClassification(string Value)
        {
            foreach (var item in ListTransportServiceClassification)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Phân loại hàng hóa
        public static List<SelectListItem> ListConsignment = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Hàng lẻ" },
            new SelectListItem() { Value = "1", Text = "Hàng lô" },
        };

        public static string GetNameConsignment(string Value)
        {
            foreach (var item in ListConsignment)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        //public static List<dynamic> ListTransportService = new List<dynamic>()
        //{
        //    new { Value = "21", Level = 1, Type = 5, Code = "ETN031", Text = "CPN TMĐT Nhanh", Price = 24000 },
        //    new { Value = "22", Level = 1, Type = 5, Code = "ETN032", Text = "CPN TMĐT Nhanh tiết kiệm", Price = 25000},
        //    new { Value = "24", Level = 2, Type = 0, Code = "", Text = "Dịch vụ đồng kiểm", Price = 16500 },
        //    new { Value = "25", Level = 2, Type = 0, Code = "", Text = "Dịch vụ phát tận tay", Price = 5500 },
        //    new { Value = "27", Level = 2, Type = 0, Code = "", Text = "Dịch vụ chuyển phát hóa đơn", Price = 11000 },
        //    new { Value = "102", Level = 1, Type = 0, Code = "ETN013", Text = "Phát hỏa tốc", Price = 70000 },
        //    new { Value = "103", Level = 2, Type = 0, Code = "", Text = "Dịch vụ khai giá", Price = 16500 },
        //    new { Value = "104", Level = 2, Type = 0, Code = "", Text = "Hàng nhạy cảm EMS - VUN", Price = 7700 },
        //    new { Value = "105", Level = 2, Type = 0, Code = "", Text = "Dịch vụ EMS cây cảnh", Price = 7700 },
        //    new { Value = "106", Level = 2, Type = 0, Code = "", Text = "Dịch vụ EMS thực phẩm", Price = 7700 },
        //    new { Value = "107", Level = 2, Type = 0, Code = "", Text = "Dịch vụ phát ngoài giờ", Price = 25000 },
        //    new { Value = "108", Level = 2, Type = 0, Code = "", Text = "Dịch vụ thu hộ - chi hộ", Price = 18000 },
        //    new { Value = "109", Level = 2, Type = 0, Code = "", Text = "Dịch vụ PPA", Price = 0 },
        //    new { Value = "110", Level = 2, Type = 0, Code = "", Text = "EMSC - Người nhận trả tiền cước", Price = 0 },
        //    new { Value = "122", Level = 1, Type = 0, Code = "ETN011", Text = "CPN EMS cơ bản", Price = 16500},
        //    new { Value = "141", Level = 1, Type = 5, Code = "ETN037", Text = "CPN TMĐT đồng giá TMĐ", Price = 22000 },
        //};

        public static List<SelectListItem> ListMessageStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Đã gửi" },
            new SelectListItem() { Value = "R", Text = "Đã nhận" },
            new SelectListItem() { Value = "S", Text = "Đã xem" },
        };

        public static List<SelectListItem> ListZaloMessageType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "STICKER", Text = "<i class='fa fa-icon-outline-sticker'></i> Sticker" },
            new SelectListItem() { Value = "IMAGE", Text = "<i class='fa fa-picture-o'></i> Hình ảnh" },
            new SelectListItem() { Value = "GIF", Text = "<i class='fa fa-combined-shape'></i> GIF" },
            new SelectListItem() { Value = "LINK", Text = "<i class='fa fa-link'></i> Link" },
            new SelectListItem() { Value = "VIDEO", Text = "<i class='fa fa-file-video-o'></i> Video" },
            new SelectListItem() { Value = "AUDIO", Text = "<i class='fa fa-file-audio-o'></i> Audio" },
            new SelectListItem() { Value = "LOCATION", Text = "<i class='fa fa-map-marker'></i> Vị trí" },
        };

        public static List<SelectListItem> ListZaloAutoSendMessageStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Chưa gửi" },
            new SelectListItem() { Value = "S", Text = "Đã gửi" },
            new SelectListItem() { Value = "F", Text = "Gửi thất bại" },
        };

        public static List<SelectListItem> ListZaloAutoSendMessageType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "TEXT", Text = "Dạng văn bản" },
            new SelectListItem() { Value = "MEDIA", Text = "Đính kèm ảnh" },
            new SelectListItem() { Value = "LIST", Text = "Đính kèm danh sách" },
            new SelectListItem() { Value = "FILE", Text = "Đính kèm file" },
            new SelectListItem() { Value = "REQUEST_USER_INFO", Text = "Yêu cầu thông tin người dùng" },
        };

        public static List<SelectListItem> ListGender = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Nam" },
            new SelectListItem() { Value = "2", Text = "Nữ" },
        };

        #region Loại khách hàng 

        public static List<SelectListItem> ListTypeCustomer = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "KH thường" },
            new SelectListItem() { Value = "2", Text = "Đại lý" },
        };

        public static string GetNameTypeCustomer(string Value)
        {
            foreach (var item in ListTypeCustomer)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách loại bưu cục chấp nhận
        public static List<SelectListItem> ListCollectionPostOfficeType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Giao dịch" },
        };

        public static List<SelectListItem> GetListCollectionPostOfficeType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListCollectionPostOfficeType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameCollectionPostOfficeType(string Value)
        {
            foreach (var item in ListCollectionPostOfficeType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Ngưởi trả tiền cước
        public static List<SelectListItem> ListTransportPayer = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Shop trả" },
            new SelectListItem() { Value = "2", Text = "Người nhận" },
        };

        public static List<SelectListItem> GetListTransportPayer()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTransportPayer)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameTransportPayer(string Value)
        {
            foreach (var item in ListTransportPayer)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static string GetValueTransportPayer(string Name)
        {
            foreach (var item in ListTransportPayer)
            {
                if (item.Text.ToLower() == Name.ToLower())
                {
                    return item.Value;
                }
            }
            return "";
        }
        #endregion

        #region Loại trạng thái yêu cầu hỗ trợ
        public static List<SelectListItem> ListRequestSupportTypeStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Tạo mới" },
            new SelectListItem() { Value = "2", Text = "Chuyển xử lý" },
            new SelectListItem() { Value = "3", Text = "Xử lý khiếu nại" },
            new SelectListItem() { Value = "4", Text = "Nhập kết quả cuối cùng" },
            new SelectListItem() { Value = "5", Text = "Mở lại khiếu nại" },
            new SelectListItem() { Value = "6", Text = "Hủy khiếu nại" },
        };

        public static List<SelectListItem> GetListRequestSupportTypeStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestSupportTypeStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameRequestSupportTypeStatus(string Value)
        {
            foreach (var item in ListRequestSupportTypeStatus)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        public static string GetNameGender(string Value)
        {
            foreach (var item in ListGender)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static List<SelectListItem> GetListZaloAutoSendMessageType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListZaloAutoSendMessageType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameZaloAutoSendMessageStatus(string Status)
        {
            foreach (var item in ListZaloAutoSendMessageStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static string GetNameMessageStatus(string Status)
        {
            foreach (var item in ListMessageStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static string GetNameZaloMessageType(string Type)
        {
            foreach (var item in ListZaloMessageType)
            {
                if (item.Value == Type)
                {
                    return item.Text;
                }
            }
            return "";
        }

        public static string GetNameIsSercurityType(string Type)
        {
            foreach(var item in ListIsSercurity)
            {
                if(item.Value == Type)
                {
                    return item.Text;
                }
            }
            return "";
        }

        #region Danh sách kết quả yêu cầu hỗ trợ
        public static List<SelectListItem> ListRequestSupportResult = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Bưu gửi bị mất một phần", Text = "Bưu gửi bị mất một phần" },
            new SelectListItem() { Value = "Chậm phát hành tiền COD", Text = "Chậm phát hành tiền COD" },
            new SelectListItem() { Value = "Đã nộp tiền COD", Text = "Đã nộp tiền COD" },
        };

        public static List<SelectListItem> GetListRequestSupportResult()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestSupportResult)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        public static List<SelectListItem> GetListRequestSupportServiceGroup()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestSupportServiceGroup)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetListIsSercurity()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var item in ListIsSercurity)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListRequestSupportScope()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListRequestSupportScope)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        //public static List<SelectListItem> GetListTransportService(int Level)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in ListTransportService)
        //    {
        //        if (item.Level == Level)
        //        {
        //            list.Add(new SelectListItem()
        //            {
        //                Text = item.Text,
        //                Value = item.Value.ToString()
        //            });
        //        }
        //    }
        //    return list;
        //}


        public static List<SelectListItem> GetListShopLevel()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListShopLevel)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetListTypeBusiness()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTypeBusiness)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetListNumberOfEmployee()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListNumberOfEmployee)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetListIsActive()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListIsActive)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetListPromotionCodeType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionCodeType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionCodeApplyForOption()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionCodeApplyForOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> PromotionGetListNumberOfUse()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in PromotionListNumberOfUse)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionCustomer()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionCustomer)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionCoditionUse()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionCoditionUse)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionScopeOfUse()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionScopeOfUse)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionName)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetListPromotionApplyForOption()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPromotionApplyForOption)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        //public static List<SelectListItem> GetListPaymentMethod()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in ListPaymentMethod)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.Text,
        //            Value = item.Value.ToString()
        //        });
        //    }
        //    return list;
        //}
        public static List<SelectListItem> GetListPaymentStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPaymentStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }


        //public static List<SelectListItem> GetListTransportStatus()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in ListTransportStatus)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.Text,
        //            Value = item.Value.ToString()
        //        });
        //    }
        //    return list;
        //}
        //public static long GetTransportServicePrice(int ID)
        //{
        //    foreach (var item in ListTransportService)
        //    {
        //        if (Convert.ToInt32(item.Value) == ID)
        //        {
        //            return item.Price;
        //        }
        //    }
        //    return 0;
        //}
        //public static dynamic GetTransportServiceByText(string Name)
        //{
        //    foreach (var item in ListTransportService)
        //    {
        //        if (item.Text == Name)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}

        public static string GetNameNumberOfEmployeeByValue(string Value)
        {
            foreach (var item in ListNumberOfEmployee)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }


        public static string GetNameTransportByText(string Transport)
        {
            foreach (var item in ListTransport)
            {
                if (item.Text == Transport)
                {
                    return item.Value;
                }
            }
            return "";
        }
        //public static string GetValuePaymentMethodByText(string PaymentMethod)
        //{
        //    foreach (var item in ListPaymentMethod)
        //    {
        //        if (item.Text == PaymentMethod)
        //        {
        //            return item.Value;
        //        }
        //    }
        //    return "";
        //}
        public static string GetValuePaymentStatusByText(string PaymentStatus)
        {
            foreach (var item in ListPaymentStatus)
            {
                if (item.Text == PaymentStatus)
                {
                    return item.Value;
                }
            }
            return "";
        }
        public static string GetNamePromotionCodeType(string ID)
        {
            foreach (var item in GetListPromotionCodeType())
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionCodeApplyForOption(string ID)
        {
            foreach (var item in ListPromotionCodeApplyForOption)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionCustomer(string ID)
        {
            foreach (var item in ListPromotionCustomer)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionCoditionUse(string ID)
        {
            foreach (var item in ListPromotionCoditionUse)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionScopeOfUse(string ID)
        {
            foreach (var item in ListPromotionScopeOfUse)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionName(string ID)
        {
            foreach (var item in ListPromotionName)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetNamePromotionApplyForOption(string ID)
        {
            foreach (var item in ListPromotionApplyForOption)
            {
                if (item.Value == ID)
                {
                    return item.Text;
                }
            }
            return "";
        }

        //public static string GetNameTransportStatus(string Status)
        //{
        //    foreach (var item in ListTransportStatus)
        //    {
        //        if (item.Value == Status)
        //        {
        //            return item.Text;
        //        }
        //    }
        //    return "";
        //}


        public static string GetNamePaymentStatus(string Status)
        {
            foreach (var item in ListPaymentStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string PromotionGetNameNumberOfUse(string Status)
        {
            foreach (var item in PromotionListNumberOfUse)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }



        // Promotion
        public static string GetPromotionCodeFirstName(int PromotionTypeOption, int Reduction, int MaxReduction)
        {
            if (PromotionTypeOption == 1) //Giảm theo số tiền
            {
                return "Giảm " + Reduction.ToString("N0") + " cho";
            }
            else if (PromotionTypeOption == 2) //Giảm theo %
            {
                if (MaxReduction != 0)
                {
                    return "Giảm " + Reduction + "% (tối đa " + MaxReduction.ToString("N0") + " đồng) cho";
                }
                else
                {
                    return "Giảm " + Reduction.ToString("N0") + " cho";
                }
            }
            else if (PromotionTypeOption == 3)//Miễn phí vận chuyển
            {
                return "Miễn phí vận chuyển về";
            }
            else if (PromotionTypeOption == 4)//Đồng giá
            {
                return "Đồng giá " + Reduction.ToString("N0") + " cho";
            }
            return "";
        }
        public static string GetPromotionCodeEndName(int ScopeOfUse, int AfoCoditionsApply, int AfoCaValue)
        {
            if (AfoCoditionsApply != 0)
            {
                if (ScopeOfUse == 1)
                {
                    if (AfoCoditionsApply == 1)
                    {
                        return "trên cả đơn hàng (khi mua " + AfoCaValue.ToString("N0") + " sản phẩm trở lên)";
                    }
                    else if (AfoCoditionsApply == 2)
                    {
                        return "trên cả đơn hàng (khi có Tổng giá trị đơn hàng từ " + AfoCaValue.ToString("N0") + " đồng trở lên)";
                    }
                }
                else if (ScopeOfUse == 2)
                {
                    if (AfoCoditionsApply == 1)
                    {
                        return "từng sản phẩm trong đơn hàng (khi mua " + AfoCaValue.ToString("N0") + " sản phẩm trở lên)";
                    }
                    else if (AfoCoditionsApply == 2)
                    {
                        return "từng sản phẩm trong đơn hàng (khi có Tổng giá trị đơn hàng từ " + AfoCaValue.ToString("N0") + " đồng trở lên)";
                    }
                }
                else if (ScopeOfUse == 0)
                {
                    if (AfoCoditionsApply == 1)
                    {
                        return "từng sản phẩm trong đơn hàng (khi mua " + AfoCaValue.ToString("N0") + " sản phẩm trở lên)";
                    }
                    else if (AfoCoditionsApply == 2)
                    {
                        return "từng sản phẩm trong đơn hàng (khi có Tổng giá trị đơn hàng từ " + AfoCaValue.ToString("N0") + " đồng trở lên)";
                    }
                }
            }
            return "";
        }
        public static string GetNameNumberOfCustomerUse(int NumberOfCustomerUse)
        {
            if (NumberOfCustomerUse != 0)
            {
                return NumberOfCustomerUse.ToString("N0") + " lần";
            }
            else
            {
                return "không giới hạn";
            }
        }
        public static string GetNameNumberOfUse(int NumberOfUse)
        {
            if (NumberOfUse != 0)
            {
                return NumberOfUse.ToString("N0") + " lần sử dụng";
            }
            else
            {
                return "không giới hạn";
            }
        }

        #region Reconciliation
        public static List<SelectListItem> GetListReconciliationStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ReconciliationStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameFreightReconciliationStatus(string Status)
        {
            foreach (var item in ReconciliationStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static List<SelectListItem> ListTypeItemFreight = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Phát hành mới" },
            new SelectListItem() { Value = "R", Text = "Chuyển hoàn" },
        };
        public static string GetNameListTypeItemFreight(string Text)
        {
            foreach (var item in ListTypeItemFreight)
            {
                if (item.Value == Text)
                {
                    return item.Text;
                }
            }
            return "";
        }

        #region Status ItemCode From Paypost
        public static List<SelectListItem> ListCODTransType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Phát hành" },
            new SelectListItem() { Value = "3", Text = "Chuyển hoàn" },
            new SelectListItem() { Value = "4", Text = "Đang trên đường phát" },
            new SelectListItem() { Value = "5", Text = "Đã thu tiền" },
            new SelectListItem() { Value = "6", Text = "Đã trả tiền" }
        };
        public static List<SelectListItem> GetListCODTransType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListCODTransType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameCODTransType(string Text)
        {
            foreach (var item in ListCODTransType)
            {
                if (item.Value == Text)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region DebtNotices Report Type
        public static List<SelectListItem> ListDebtNoticesReportType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "O", Text = "Giấy báo tổng hợp" },
            new SelectListItem() { Value = "D", Text = "Giấy báo chi tiết" }
        };
        public static List<SelectListItem> GetListDebtNoticesReportType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListDebtNoticesReportType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        #endregion

        //Transport Service - Data
        public static List<SelectListItem> TransportServiceGroup = new List<SelectListItem>() //Nhóm dịch vụ vận chuyển
        {
            new SelectListItem() { Value = "1", Text = "Dịch vụ chính" },
            new SelectListItem() { Value = "2", Text = "Dịch vụ cộng thêm" }
        };
        public static List<SelectListItem> TransportCharacteristic = new List<SelectListItem>() //Đặc tính
        {
            new SelectListItem() { Value = "1", Text = "Trung tâm" },
            new SelectListItem() { Value = "2", Text = "Dịch vụ cộng thêm" }
        };
        public static List<SelectListItem> TransportSurcharge = new List<SelectListItem>() //Phụ phí
        {
            new SelectListItem() { Value = "1", Text = "Xăng dầu" },
            new SelectListItem() { Value = "2", Text = "Vùng xa" },
            new SelectListItem() { Value = "3", Text = "Hải đảo" },
            new SelectListItem() { Value = "4", Text = "Hàng cồng kềnh" },
        };
        public static List<SelectListItem> TransportCommoditiesType = new List<SelectListItem>() //Loại hàng hoá
        {
            new SelectListItem() { Value = "1", Text = "Hàng nhẹ" },
            new SelectListItem() { Value = "2", Text = "Hàng cồng kềnh" },
        };

        //Transport Service - Get List
        public static List<SelectListItem> TransportGetServiceGroup()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in TransportServiceGroup)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> TransportGetCharacteristic()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in TransportCharacteristic)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> TransportGetSurcharge()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in TransportSurcharge)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> TransportGetCommoditiesType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in TransportCommoditiesType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        //Transport Service - Get Name
        public static string TransportGetServiceGroupByValue(string Value)
        {
            foreach (var item in TransportServiceGroup)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string TransportGetCharacteristicByValue(string Value)
        {
            foreach (var item in TransportCharacteristic)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string TransportGetSurchargeByValue(string Value)
        {
            foreach (var item in TransportSurcharge)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string TransportGetCommoditiesTypeByValue(string Value)
        {
            foreach (var item in TransportCommoditiesType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }

        #region Transport Postage

        #region Trạng thái bảng cước dịch vụ
        public static List<SelectListItem> ListTransportPostageStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "U", Text = "Chưa xác nhận" }, //Unconfirmed
            new SelectListItem() { Value = "C", Text = "Đã xác nhận" },  //Confirmed
            new SelectListItem() { Value = "E", Text = "Đã hết hạn" },  //Expired
            new SelectListItem() { Value = "P", Text = "Tạm ngừng" }   //Pause
        };

        public static string GetNameTransportPostageStatus(string Value)
        {
            foreach (var item in ListTransportPostageStatus)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        //#region Loại bảng cước
        //public static List<SelectListItem> ListTransportPostageType = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Value = "0", Text = "Cước công bố" },
        //    new SelectListItem() { Value = "1", Text = "Đặc thù theo KH" },
        //};

        //public static List<SelectListItem> GetListTransportPostageType()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in ListTransportPostageType)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.Text,
        //            Value = item.Value.ToString()
        //        });
        //    }
        //    return list;
        //}
        //#endregion

        public static string TransportPostageGetNameByFromToRange(int Value)
        {
            if (Value == 1)
            {
                return "Vùng";
            }
            else if (Value == 2)
            {
                return "Tỉnh/Thành phố";
            }

            return "";
        }

        #region Danh sách đối tượng áp dụng
        public static List<SelectListItem> ListSubjectsOfApplication = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Không" },
            //new SelectListItem() { Value = "2", Text = "Nhóm khách hàng" },
            new SelectListItem() { Value = "3", Text = "Khách hàng" },
            new SelectListItem() { Value = "4", Text = "Nơi xuất phát" },
            new SelectListItem() { Value = "5", Text = "Cả 2 điều kiện" }
        };

        public static List<SelectListItem> GetListSubjectsOfApplication()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListSubjectsOfApplication)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameSubjectsOfApplication(string Value)
        {
            foreach (var item in ListSubjectsOfApplication)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách tỉnh áp dụng
        public static List<SelectListItem> ListOptionProvinceApply = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Không giới hạn" },
            new SelectListItem() { Value = "1", Text = "Nội tỉnh" },
            new SelectListItem() { Value = "2", Text = "Liên tỉnh" },
        };

        public static List<SelectListItem> GetListOptionProvinceApply()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListOptionProvinceApply)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        #region Danh sách vùng áp dụng
        public static List<SelectListItem> ListOptionAreaApply = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Không giới hạn" },
            new SelectListItem() { Value = "1", Text = "Nội vùng" },
            new SelectListItem() { Value = "2", Text = "Liên vùng" },
        };

        public static List<SelectListItem> GetListOptionAreaApply()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListOptionAreaApply)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        #region Danh sách phạm vi
        public static List<SelectListItem> ListTransportRange = new List<SelectListItem>() //Phạm vi
        {
            new SelectListItem() { Value = "1", Text = "Tỉnh/Thành phố" },
            new SelectListItem() { Value = "2", Text = "Khu vực" },
            new SelectListItem() { Value = "3", Text = "Vùng" },
            new SelectListItem() { Value = "4", Text = "Khu vực/Vùng" },
            new SelectListItem() { Value = "5", Text = "Vùng/ Tỉnh" },
            new SelectListItem() { Value = "6", Text = "Last Mile" },
        };

        public static List<SelectListItem> GetListTransportRange()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListTransportRange)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameTransportRange(string Value)
        {
            foreach (var item in ListTransportRange)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách căn cứ tính cước
        public static List<SelectListItem> ListChargingConditions = new List<SelectListItem>() //Danh sách điều kiện tính cước DV cộng thêm
        {
            new SelectListItem() { Value = "1", Text = "Bưu gửi" },
            new SelectListItem() { Value = "2", Text = "Số lượng" },
            new SelectListItem() { Value = "3", Text = "Giá trị" },
            new SelectListItem() { Value = "4", Text = "Cước chính" },
            new SelectListItem() { Value = "5", Text = "Khối lượng và phạm vi vùng" },
            new SelectListItem() { Value = "6", Text = "Khối lượng và phạm vi tỉnh" }
        };
        public static List<SelectListItem> GetListChargingConditions()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListChargingConditions)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameChargingConditions(string Value)
        {
            foreach (var item in ListChargingConditions)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion


        #endregion

        #region Shop_Image
        public static List<SelectListItem> ShopImages = new List<SelectListItem>() //Nhóm dịch vụ
        {
            new SelectListItem() { Value = "1", Text = "Giấy phép ĐKKD" },
            new SelectListItem() { Value = "2", Text = "Giấy tờ tùy thân" }
        };
        #endregion

        #region Reconciliation
        public static List<SelectListItem> ReconciliationStatus = new List<SelectListItem>() //Trạng thái đối soát
        {
            new SelectListItem() { Value = "W", Text = "Chờ xác nhận" },
            new SelectListItem() { Value = "Y", Text = "Đã đồng ý" },
            new SelectListItem() { Value = "N", Text = "Chờ xử lý" },
            new SelectListItem() { Value = "C", Text = "Đã điều chỉnh" },
            new SelectListItem() { Value = "D", Text = "Chưa đến kỳ đối soát" },
            new SelectListItem() { Value = "R", Text = "Chờ đối soát lại" }
        };

        public static List<SelectListItem> ListMonth = new List<SelectListItem>() //Tháng
        {
            new SelectListItem() { Value = "01", Text = "Tháng 01" },
            new SelectListItem() { Value = "02", Text = "Tháng 02" },
            new SelectListItem() { Value = "03", Text = "Tháng 03" },
            new SelectListItem() { Value = "04", Text = "Tháng 04" },
            new SelectListItem() { Value = "05", Text = "Tháng 05" },
            new SelectListItem() { Value = "06", Text = "Tháng 06" },
            new SelectListItem() { Value = "07", Text = "Tháng 07" },
            new SelectListItem() { Value = "08", Text = "Tháng 08" },
            new SelectListItem() { Value = "09", Text = "Tháng 09" },
            new SelectListItem() { Value = "10", Text = "Tháng 10" },
            new SelectListItem() { Value = "11", Text = "Tháng 11" },
            new SelectListItem() { Value = "12", Text = "Tháng 12" }
        };

        public static int y = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

        public static List<SelectListItem> ListYear = new List<SelectListItem>() //Năm
        {
            new SelectListItem() { Value = (y - 3).ToString(), Text = (y - 3).ToString() },
            new SelectListItem() { Value = (y - 2).ToString(), Text = (y - 2).ToString() },
            new SelectListItem() { Value = (y - 1).ToString(), Text = (y - 1).ToString() },
            new SelectListItem() { Value = (y).ToString(), Text = (y).ToString() }
        };

        public static List<SelectListItem> ReconciliationTypeDate = new List<SelectListItem>() //Loại ngày đối soát
        {
            new SelectListItem() { Value = "1", Text = "Ngày tạo đơn" },
            new SelectListItem() { Value = "2", Text = "Ngày phát hành" },
            //new SelectListItem() { Value = "3", Text = "Ngày đối soát" }
        };

        public static List<SelectListItem> GetMonth()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListMonth)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetYear()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListYear)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static List<SelectListItem> GetReconciliationTypeDate()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ReconciliationTypeDate)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        #region PrepaidFreightPayment
        public static List<SelectListItem> PrepaidFreightPaymentStatus = new List<SelectListItem>() //Trạng thái thanh toán cước trả trước
        {
            new SelectListItem() { Value = "", Text = "Tất cả" },
            new SelectListItem() { Value = "Y", Text = "Đã thanh toán" },
            new SelectListItem() { Value = "N", Text = "Chờ xử lý" }
        };
        #endregion

        #region WarehouseImport
        public static List<SelectListItem> ListInventoryStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Tạo mới" },
            new SelectListItem() { Value = "W", Text = "Chờ xác nhận" },
            new SelectListItem() { Value = "P", Text = "Chờ xử lý" },
            new SelectListItem() { Value = "F", Text = "Hoàn thành" },
            new SelectListItem() { Value = "O", Text = "Đã phê duyệt" },
            new SelectListItem() { Value = "R", Text = "Từ chối" },
            new SelectListItem() { Value = "C", Text = "Đã huỷ" },
        };
        public static List<SelectListItem> GetListInventoryStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListInventoryStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameInventoryStatus(string Value)
        {
            foreach (var item in ListInventoryStatus)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Transport Postage Extra Service

        #endregion

        #region Period
        public static List<SelectListItem> PeriodType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Tuần" },
            new SelectListItem() { Value = "2", Text = "Tháng" }
        };

        public static List<SelectListItem> AllDays = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Ngày 1" },
            new SelectListItem() { Value = "2", Text = "Ngày 2" },
            new SelectListItem() { Value = "3", Text = "Ngày 3" },
            new SelectListItem() { Value = "4", Text = "Ngày 4" },
            new SelectListItem() { Value = "5", Text = "Ngày 5" },
            new SelectListItem() { Value = "6", Text = "Ngày 6" },
            new SelectListItem() { Value = "7", Text = "Ngày 7" },
            new SelectListItem() { Value = "8", Text = "Ngày 8" },
            new SelectListItem() { Value = "9", Text = "Ngày 9" },
            new SelectListItem() { Value = "10", Text = "Ngày 10" },
            new SelectListItem() { Value = "11", Text = "Ngày 11" },
            new SelectListItem() { Value = "12", Text = "Ngày 12" },
            new SelectListItem() { Value = "13", Text = "Ngày 13" },
            new SelectListItem() { Value = "14", Text = "Ngày 14" },
            new SelectListItem() { Value = "15", Text = "Ngày 15" },
            new SelectListItem() { Value = "16", Text = "Ngày 16" },
            new SelectListItem() { Value = "17", Text = "Ngày 17" },
            new SelectListItem() { Value = "18", Text = "Ngày 18" },
            new SelectListItem() { Value = "19", Text = "Ngày 19" },
            new SelectListItem() { Value = "20", Text = "Ngày 20" },
            new SelectListItem() { Value = "21", Text = "Ngày 21" },
            new SelectListItem() { Value = "22", Text = "Ngày 22" },
            new SelectListItem() { Value = "23", Text = "Ngày 23" },
            new SelectListItem() { Value = "24", Text = "Ngày 24" },
            new SelectListItem() { Value = "25", Text = "Ngày 25" },
            new SelectListItem() { Value = "26", Text = "Ngày 26" },
            new SelectListItem() { Value = "27", Text = "Ngày 27" },
            new SelectListItem() { Value = "28", Text = "Ngày 28" },
            new SelectListItem() { Value = "29", Text = "Ngày 29" },
            new SelectListItem() { Value = "30", Text = "Ngày 30" },
            new SelectListItem() { Value = "31", Text = "Ngày 31" }
        };
        public static List<SelectListItem> Week = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Chủ nhật" },
            new SelectListItem() { Value = "2", Text = "Thứ 2" },
            new SelectListItem() { Value = "3", Text = "Thứ 3" },
            new SelectListItem() { Value = "4", Text = "Thứ 4" },
            new SelectListItem() { Value = "5", Text = "Thứ 5" },
            new SelectListItem() { Value = "6", Text = "Thứ 6" },
            new SelectListItem() { Value = "7", Text = "Thứ 7" }
        };
        public static List<SelectListItem> GetPeriodType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in PeriodType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetAllDays()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in AllDays)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static List<SelectListItem> GetWeek()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in Week)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string PeriodTypeByValue(string Value)
        {
            foreach (var item in PeriodType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string AllDaysByValue(string Value)
        {
            foreach (var item in AllDays)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string WeekByValue(string Value)
        {
            foreach (var item in Week)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách hình thức thanh toán
        public static List<SelectListItem> ListPaymentMethod = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Tài khoản ngân hàng" },
            new SelectListItem() { Value = "2", Text = "Rút tiền tại bưu cục" },
            //new SelectListItem() { Value = "3", Text = "Ví điện tử" }
        };

        public static List<SelectListItem> GetListPaymentMethod()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPaymentMethod)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNamePaymentMethod(string Value)
        {
            foreach (var item in ListPaymentMethod)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }

        #endregion

        #region Danh sách hình thức trả cước
        public static List<SelectListItem> ListPaymentType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Cước trả trước" },
            new SelectListItem() { Value = "2", Text = "Cước trả sau" }
        };

        public static List<SelectListItem> GetListPaymentType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPaymentType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNamePaymentType(string Value)
        {
            foreach (var item in ListPaymentType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Danh sách hình thức thu gom
        public static List<SelectListItem> ListCollectionType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Thu gom tận nơi" },
            new SelectListItem() { Value = "2", Text = "Gửi hàng tại bưu cục" }
        };

        public static List<SelectListItem> GetListCollectionType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListCollectionType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameCollectionType(string Value)
        {
            foreach (var item in ListCollectionType)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        public static string GetValueCollectionType(string Name)
        {
            foreach (var item in ListCollectionType)
            {
                if (item.Text.ToLower() == Name.ToLower())
                {
                    return item.Value;
                }
            }
            return "";
        }
        #endregion

        #region Common
        public static List<SelectListItem> ListYesNoStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Y", Text = "Có" },
            new SelectListItem() { Value = "N", Text = "Không" }
        };

        public static string GetNameYesNoStatus(string Value)
        {
            foreach (var item in ListYesNoStatus)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }

        //public static string YesNoGetNameByValue(string Value)
        //{
        //    foreach (var item in YesNo)
        //    {
        //        if (item.Value == Value)
        //        {
        //            return item.Text;
        //        }
        //    }
        //    return "";
        //}

        // Get Name Yes No
        //public static string GetNameYesOrNo(string Value)
        //{
        //    if (Value == "Y")
        //    {
        //        return "Có";
        //    }
        //    else if (Value == "N")
        //    {
        //        return "Không";
        //    }
        //    return "";
        //}
        #endregion

        #region Facebook

        #region List Data
        public static List<SelectListItem> ListDay = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "MONDAY", Text = "Thứ 2" },
            new SelectListItem() { Value = "TUESDAY", Text = "Thứ 3" },
            new SelectListItem() { Value = "WEDNESDAY", Text = "Thứ 4" },
            new SelectListItem() { Value = "THURSDAY", Text = "Thứ 5" },
            new SelectListItem() { Value = "FRIDAY", Text = "Thứ 6" },
            new SelectListItem() { Value = "SATURDAY", Text = "Thứ 7" },
            new SelectListItem() { Value = "SUNDAY", Text = "Chủ nhật" }
        };
        public static List<SelectListItem> ListSelectTransport = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "--- Chọn ---" },
            new SelectListItem() { Value = "Y", Text = "Có giao hàng" },
            new SelectListItem() { Value = "N", Text = "Không giao hàng" }
        };
        public static List<SelectListItem> ListSelectPay = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "--- Chọn ---" },
            new SelectListItem() { Value = "1", Text = "Thanh toán COD" },
            new SelectListItem() { Value = "2", Text = "Chuyển khoản" },
            new SelectListItem() { Value = "3", Text = "Trực tiếp" }
        };
        public static List<SelectListItem> ListSelTimeResponse = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "5", Text = "Sau 5 phút" },
            new SelectListItem() { Value = "30", Text = "Sau 30 phút" },
            new SelectListItem() { Value = "60", Text = "Sau 60 phút" },
        };
        public static List<SelectListItem> ListSelTimeExcute = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Toàn thời gian" },
            //new SelectListItem() { Value = "1", Text = "Trong thời gian làm việc" },
            //new SelectListItem() { Value = "2", Text = "Ngoài thời gian làm việc" },
            new SelectListItem() { Value = "3", Text = "Tự thiết lập" }
        };
        public static List<SelectListItem> ListSelSendTime = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "Ngay lập tức" },
            new SelectListItem() { Value = "2", Text = "Theo thời gian" }
        };
        public static List<SelectListItem> ListSelAvailableDate = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "1", Text = "1 Ngày" },
            new SelectListItem() { Value = "2", Text = "5 Ngày" },
            new SelectListItem() { Value = "3", Text = "10 Ngày" },
            new SelectListItem() { Value = "4", Text = "15 Ngày" }
        };
        #endregion

        #region List Data Search
        public static List<SelectListItem> ListSelDaySearch = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Chọn" },
            new SelectListItem() { Value = "1", Text = "Hôm nay" },
            new SelectListItem() { Value = "2", Text = "Hôm qua" }
        };
        public static List<SelectListItem> ListSelWeekSearch = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Chọn" },
            new SelectListItem() { Value = "1", Text = "Tuần này" },
            new SelectListItem() { Value = "2", Text = "Tuần trước" }
        };
        public static List<SelectListItem> ListSelMonthSearch = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0", Text = "Chọn" },
            new SelectListItem() { Value = "1", Text = "Tháng này" },
            new SelectListItem() { Value = "2", Text = "Tháng trước" }
        };
        #endregion

        #region Get List From Data
        public static List<SelectListItem> GetListDay()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListDay)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelectTransport()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelectTransport)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelectPay()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelectPay)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelTimeResponse()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelTimeResponse)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelTimeExcute()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelTimeExcute)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelSendTime()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelSendTime)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelAvailableDate()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelAvailableDate)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        #endregion

        #region Get List Select Search
        public static List<SelectListItem> GetListSelDaySearch()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelDaySearch)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelWeekSearch()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelWeekSearch)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        public static List<SelectListItem> GetListSelMonthSearch()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in ListSelMonthSearch)
            {
                listItem.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return listItem;
        }
        #endregion


        #endregion

        #region StatisticalColor
        public static List<string> StatisticalBorderColor = new List<string>()
        {
            "#008FE5" ,//0
            "#FFADBF",//1
            "#FFE4AB",//2
            "#96D0F3",//3
            "#99FF99",//4
            "#BDA3FF",//5
            "#69B1C5",//6
            "#ABCEEE" ,//7
            "#94C4C1" ,//8
            "#F68FB4" ,//9
            "#98527F" ,//10
            "#F7941E",//11
        };
        public static List<string> GetStatisticalBorderColor()
        {
            List<string> data = new List<string>();
            data = StatisticalBorderColor;
            return data;
        }
        public static List<string> StatisticalBackgroundColor = new List<string>()
        {
            "#008FE5" ,//0
            "#FFADBF",//1
            "#FFE4AB",//2
            "#96D0F3",//3
            "#99FF99",//4
            "#BDA3FF",//5
            "#69B1C5",//6
            "#ABCEEE" ,//7
            "#94C4C1" ,//8
            "#F68FB4" ,//9
            "#98527F" ,//10
            "#F7941E",//11
        };
        public static List<string> GetStatisticalBackgroundColor()
        {
            List<string> data = new List<string>();
            data = StatisticalBackgroundColor;
            return data;
        }
        #endregion

        #region StatisticalReqOrderColor
        public static List<string> StatisticalReqOrderBorderColor = new List<string>()
        {
            "#F5821F",//0
            "#44B7C2",//1
            "#0054A6",//2
        };
        public static List<string> GetStatisticalReqOrderBorderColor()
        {
            List<string> data = new List<string>();
            data = StatisticalReqOrderBorderColor;
            return data;
        }
        public static List<string> StatisticalReqOrderBackgroundColor = new List<string>()
        {
            "#F5821F",//0
            "#44B7C2",//1
            "#0054A6",//2
        };
        public static List<string> GetStatisticalReqOrderBackgroundColor()
        {
            List<string> data = new List<string>();
            data = StatisticalReqOrderBackgroundColor;
            return data;
        }
        #endregion

        #region StatisticalColorV2
        public static List<string> StatisticalBorderColorV2 = new List<string>()
        {
            //"#44B7C2",//0
            //"#0054A6",//1
            //"#F5821F",//2
            //"#5AC8FA",//3
            //"#FFCC00",//4

            "#0054A6",//0
            "#44B7C2",//1
            "#5AC8FA",//2
            "#F5821F",//3
            "#FFCC00",//4
        };
        public static List<string> GetStatisticalBorderColorV2()
        {
            List<string> data = new List<string>();
            data = StatisticalBorderColorV2;
            return data;
        }
        public static List<string> StatisticalBackgroundColorV2 = new List<string>()
        {
            //"#44B7C2",//0
            //"#0054A6",//1
            //"#F5821F",//2
            //"#5AC8FA",//3
            //"#FFCC00",//4

            "#0054A6",//0
            "#44B7C2",//1
            "#5AC8FA",//2
            "#F5821F",//3
            "#FFCC00",//4
        };
        public static List<string> GetStatisticalBackgroundColorV2()
        {
            List<string> data = new List<string>();
            data = StatisticalBackgroundColorV2;
            return data;
        }
        #endregion

        public static List<SelectListItem> ListOrderStatusCashflow = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "T", Text = "Đang vận chuyển" },
            new SelectListItem() { Value = "F", Text = "Phát chưa TC" },
            new SelectListItem() { Value = "G", Text = "Phát TC chưa nộp COD" },
            new SelectListItem() { Value = "H", Text = "Phát TC đã nộp COD" },
            new SelectListItem() { Value = "I", Text = "Đã trả COD" },
            new SelectListItem() { Value = "U", Text = "Chuyển hoàn" },
        };
        public static string GetNameOrderStatusCashflow(string Status)
        {
            foreach (var item in ListOrderStatusCashflow)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }

        #region Danh sách kiểu in đơn hàng
        public static List<SelectListItem> ListPrintType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "A4", Text = "Kiểu A4" },
            new SelectListItem() { Value = "A6", Text = "Kiểu A6" },
            new SelectListItem() { Value = "B1", Text = "Kiểu B1" },
            new SelectListItem() { Value = "B2", Text = "Kiểu B2" },
            new SelectListItem() { Value = "Label", Text = "Kiểu nhãn" },
        };

        public static List<SelectListItem> GetListPrintType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPrintType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        #endregion

        //#region Product 
        //public static List<SelectListItem> ProductUnit = new List<SelectListItem>()
        //{
        //    new SelectListItem() { Value = "1", Text = "Cái" },
        //    new SelectListItem() { Value = "2", Text = "Hộp" },
        //    new SelectListItem() { Value = "3", Text = "Đơn vị tính" }
        //};

        //public static string ProductUnitGetByValue(string Value)
        //{
        //    foreach (var item in ProductUnit)
        //    {
        //        if (item.Value == Value)
        //        {
        //            return item.Text;
        //        }
        //    }
        //    return "";
        //}

        //public static List<SelectListItem> ProductUnitGetAll()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    foreach (var item in ProductUnit)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.Text,
        //            Value = item.Value.ToString()
        //        });
        //    }
        //    return list;
        //}
        //#endregion

        #region Trạng thái in đơn hàng
        public static List<SelectListItem> ListPrintStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Chưa in" },
            new SelectListItem() { Value = "A", Text = "Đã in" },
        };

        public static List<SelectListItem> GetListPrintStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListPrintStatus)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNamePrintStatus(string Value)
        {
            foreach (var item in ListPrintStatus)
            {
                if (item.Value == Value)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Thư viện và hàm hỗ trợ
        public static DateTime TimezoneToDateLocal(string timezone)
        {
            DateTimeOffset _timeZone = DateTimeOffset.Parse(timezone, CultureInfo.InvariantCulture);
            var _targetTime = _timeZone.LocalDateTime;
            return _targetTime;
        }

        public static string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng chẵn" : "");
        }

        public static string ConvertLongTypeDateTime(long DateTime)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(DateTime).ToLocalTime();
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string TimeStampSecondToDateTimeString(long timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp);
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string ChangeEnglishDayToVietNamDay(string day)
        {
            string dayVN = string.Empty;
            switch (day)
            {
                case "Monday":
                    dayVN = "Thứ 2";
                    break;
                case "Tuesday":
                    dayVN = "Thứ 3";
                    break;
                case "Wednesday":
                    dayVN = "Thứ 4";
                    break;
                case "Thursday":
                    dayVN = "Thứ 5";
                    break;
                case "Friday":
                    dayVN = "Thứ 6";
                    break;
                case "Saturday":
                    dayVN = "Thứ 7";
                    break;
                case "Sunday":
                    dayVN = "Chủ nhật";
                    break;

            }
            return dayVN;
        }

        public static string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} giây trước", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    string.Format("{0} phút trước", timeSpan.Minutes) :
                    "1 phút trước";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    string.Format("{0} tiếng trước", timeSpan.Hours) :
                    "1 tiếng trước";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    string.Format("{0} ngày trước", timeSpan.Days) :
                    "Hôm qua";
            }
            //else if (timeSpan < TimeSpan.FromDays(30))
            //{
            //    Calendar cal = new CultureInfo("en-US").Calendar;
            //    int week = cal.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //    result = week + " tuần trước";
            //}
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    string.Format("{0} tháng trước", timeSpan.Days / 30) :
                    "1 tháng trước";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    string.Format("{0} năm trước", timeSpan.Days / 365) :
                    "1 năm trước";
            }

            return result;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion

        #region Tiktok

        #region Product Process Status
        public static List<SelectListItem> ListTikProductProcessStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "A", Text = "SKU Seller trên TiktokShop không trùng với EMSOne" },
            new SelectListItem() { Value = "D", Text = "SKU Seller trên TiktokShop trùng nhau" },
            new SelectListItem() { Value = "N", Text = "SKU Seller trên TiktokShop không có" }
        };
        public static string GetNameTikProductProcessStatus(string Status)
        {
            foreach (var item in ListTikProductProcessStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region Trạng thái đơn hàng kênh TiktokShop

        public static List<SelectListItem> ListOrderStatusTiktokShop = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "W", Text = "Chờ lấy hàng" },
            new SelectListItem() { Value = "S", Text = "Đang vận chuyển" },
            new SelectListItem() { Value = "G", Text = "Phát thành công" },
            new SelectListItem() { Value = "H", Text = "Đã thu tiền" },
            //new SelectListItem() { Value = "U", Text = "Chờ chuyển hoàn" },
            //new SelectListItem() { Value = "V", Text = "Đang chuyển hoàn" },
            //new SelectListItem() { Value = "Z", Text = "Chuyển hoàn thành công" },
            new SelectListItem() { Value = "C", Text = "Đã huỷ" }
        };

        public static List<SelectListItem> GetListOrderStatusTiktokShop()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListOrderStatusTiktokShop)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }

        public static string GetNameOrderStatusTiktokShop(string Status)
        {
            foreach (var item in ListOrderStatusTiktokShop)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #region TiktokOrderInventoryStatus
        public static List<SelectListItem> ListTiktokOrderInventoryStatus = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "N", Text = "Không thành công. Sản phẩm chưa liên kết" },
            new SelectListItem() { Value = "S", Text = "Xuất kho thành công" },
            new SelectListItem() { Value = "P", Text = "Không thành công. Sản phẩm không có trong kho mặc định" },
            new SelectListItem() { Value = "Q", Text = "Không thành công. Sản phẩm trong kho mặc định không đủ" },
        };
        public static string GetNameTiktokOrderInventoryStatus(string Status)
        {
            foreach (var item in ListTiktokOrderInventoryStatus)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion

        #endregion

        #region Thông báo
        public static List<SelectListItem> ListNotifyType = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "ORDER_STATUS", Text = "Trạng thái đơn hàng" },
            new SelectListItem() { Value = "EMS", Text = "Gửi từ EMS Vietnam" }
        };
        public static List<SelectListItem> GetListNotifyType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in ListNotifyType)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }
            return list;
        }
        public static string GetNameNotifyType(string Status)
        {
            foreach (var item in ListNotifyType)
            {
                if (item.Value == Status)
                {
                    return item.Text;
                }
            }
            return "";
        }
        #endregion
    }
}
