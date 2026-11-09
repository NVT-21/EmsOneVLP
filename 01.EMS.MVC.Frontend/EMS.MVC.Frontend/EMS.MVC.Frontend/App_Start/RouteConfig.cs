using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMS.MVC.Frontend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region FREELAYOUT
            routes.MapRoute(name: "guide/delete-account", url: "guide/delete-account", defaults: new { controller = "FreeLayout", action = "DeleteAccount" });
            #endregion

            #region ACCOUNT_CONTROLLER
            routes.MapRoute(name: "dang-nhap", url: "dang-nhap", defaults: new { controller = "Account", action = "LoginView" });
            routes.MapRoute(name: "doi-mat-khau", url: "doi-mat-khau", defaults: new { controller = "Account", action = "ChangePasswordView" });
            routes.MapRoute(name: "cap-nhat-mat-khau", url: "cap-nhat-mat-khau", defaults: new { controller = "Account", action = "UpdatePasswordView" });
            routes.MapRoute(name: "quen-mat-khau", url: "quen-mat-khau", defaults: new { controller = "Account", action = "ForgetPasswordView" });
            #endregion

            #region CUSTOMER_CONTROLLER - KHÁCH HÀNG
            routes.MapRoute(name: "khach-hang/danh-sach-khach-hang", url: "khach-hang/danh-sach-khach-hang", defaults: new { controller = "Customer", action = "CustomerView" });
            routes.MapRoute(name: "khach-hang/chi-tiet-khach-hang/{SearchKey}", url: "khach-hang/chi-tiet-khach-hang/{SearchKey}", defaults: new { controller = "Customer", action = "DetailCustomerView", SearchKey = UrlParameter.Optional });
            routes.MapRoute(name: "khach-hang/import-excel", url: "khach-hang/import-excel", defaults: new { controller = "Customer", action = "ImportExcelView" });
            routes.MapRoute(name: "khach-hang/sua-khach-hang/{SearchKey}", url: "khach-hang/sua-khach-hang/{SearchKey}", defaults: new { controller = "Customer", action = "EditCustomerView", SearchKey = UrlParameter.Optional });
            #endregion

            #region CUSTOMER_GROUP_CONTROLLER - NHÓM KHÁCH HÀNG
            routes.MapRoute(name: "khach-hang/nhom-khach-hang", url: "khach-hang/nhom-khach-hang", defaults: new { controller = "CustomerGroup", action = "CustomerGroupView" });
            routes.MapRoute(name: "khach-hang/them-nhom-khach-hang", url: "khach-hang/them-nhom-khach-hang", defaults: new { controller = "CustomerGroup", action = "AddCustomerGroupView" });

            #endregion

            #region CONFIGURATION
            routes.MapRoute(name: "tinh-trang-xu-ly", url: "cau-hinh/tinh-trang-xu-ly", defaults: new { controller = "Configuration", action = "StatusProcessView" });
            #endregion

            #region HOME_CONTROLLER
            routes.MapRoute(name: "trang-chu", url: "trang-chu", defaults: new { controller = "Home", action = "Index" });
            #endregion

            #region COD_REPORT_CONTROLLER
            routes.MapRoute(name: "giam-sat-don-hang-cod", url: "giam-sat-don-hang-cod", defaults: new { controller = "Report", action = "CODMoneyReportView" });
            #endregion

            #region ERROR_CONTROLLER
            routes.MapRoute(name: "loi/trang-khong-ton-tai", url: "loi/trang-khong-ton-tai", defaults: new { controller = "Error", action = "PageNotFound" });
            routes.MapRoute(name: "loi/truy-cap-khong-thanh-cong", url: "loi/truy-cap-khong-thanh-cong", defaults: new { controller = "Error", action = "InternalServerError" });
            routes.MapRoute(name: "loi/khong-co-quyen-truy-cap", url: "loi/khong-co-quyen-truy-cap", defaults: new { controller = "Error", action = "NotPermission" });
            routes.MapRoute(name: "loi/tai-khoan-da-bi-thay-doi", url: "loi/tai-khoan-da-bi-thay-doi", defaults: new { controller = "Error", action = "StatusCode203" });
            #endregion

            #region ORDER_CONTROLLER - ĐƠN HÀNG
            routes.MapRoute(name: "don-hang/danh-sach-don-hang", url: "don-hang/danh-sach-don-hang", defaults: new { controller = "Order", action = "OrderPaginationExpandView" });
            routes.MapRoute(name: "don-hang/tao-don-hang", url: "don-hang/tao-don-hang", defaults: new { controller = "Order", action = "AddOrderView" });
            routes.MapRoute(name: "don-hang/tao-don-hang-le", url: "don-hang/tao-don-hang-le", defaults: new { controller = "Order", action = "WalkInOrderView" });
            routes.MapRoute(name: "don-hang/tao-don-hang-nhanh", url: "don-hang/tao-don-hang-nhanh", defaults: new { controller = "Order", action = "FastOrderView" });
            routes.MapRoute(name: "don-hang/doi-tra-hang-hm/{OrderCodeEncrypt}", url: "don-hang/doi-tra-hang-hm/{OrderCodeEncrypt}", defaults: new { controller = "Order", action = "ReturnOrderHMView", OrderCodeEncrypt = UrlParameter.Optional });
            routes.MapRoute(name: "don-hang/sua-don-hang/{SearchKey}", url: "don-hang/sua-don-hang/{SearchKey}", defaults: new { controller = "Order", action = "EditOrderView", SearchKey = UrlParameter.Optional });
            routes.MapRoute(name: "don-hang/sao-chep-don-hang/{SearchKey}", url: "don-hang/sao-chep-don-hang/{SearchKey}", defaults: new { controller = "Order", action = "CoppyOrderView", SearchKey = UrlParameter.Optional });
            routes.MapRoute(name: "don-hang/import-excel/{Version}", url: "don-hang/import-excel/{Version}", defaults: new { controller = "Order", action = "ImportExcelView", Version = UrlParameter.Optional });
            routes.MapRoute(name: "don-hang/import-order-excel", url: "don-hang/import-order-excel", defaults: new { controller = "Order", action = "ImportExcelViewNew", Version = UrlParameter.Optional });
            #endregion

            #region ORDER_RETURN_CONTROLLER - ĐƠN HÀNG RETURN
            routes.MapRoute(name: "don-hang-tra-lai/them-don-hang-3T", url: "don-hang-tra-lai/them-don-hang-3t", defaults: new { controller = "ReturnOrder", action = "AddReturnOrder3TView" });
            
            #endregion

            #region ORDER_CONTROLLER - ĐƠN HÀNG LÔ
            routes.MapRoute(name: "don-hang/tao-don-hang-lo", url: "don-hang/tao-don-hang-lo", defaults: new { controller = "OrderBatch", action = "AddOrderBatchView" });
            routes.MapRoute(name: "don-hang/sua-don-hang-lo", url: "don-hang/sua-don-hang-lo", defaults: new { controller = "OrderBatch", action = "HandleEditOrderBatchView" });
            routes.MapRoute(name: "don-hang/sao-chep-don-hang-lo/{SearchKey}", url: "don-hang/sao-chep-don-hang-lo/{SearchKey}", defaults: new { controller = "OrderBatch", action = "CoppyOrderBatchView", SearchKey = UrlParameter.Optional });
            #endregion

            #region Handle
            routes.MapRoute(name: "in-don-hang", url: "in-don-hang", defaults: new { controller = "Handle", action = "PrintOrderView" });
            routes.MapRoute(name: "tra-cuu-buu-gui", url: "tra-cuu-buu-gui", defaults: new { controller = "Handle", action = "DetailShippingView" });
            #endregion

            #region PRODUCT_GROUP_CONTROLLER - NHÓM SẢN PHẨM
            routes.MapRoute(name: "nhom-san-pham/danh-sach", url: "nhom-san-pham/danh-sach-nhom-san-pham", defaults: new { controller = "ProductGroup", action = "ProductGroupView" });
            routes.MapRoute(name: "nhom-san-pham/them-nhom-san-pham", url: "nhom-san-pham/them-nhom-san-pham", defaults: new { controller = "ProductGroup", action = "AddProductGroupView" });
            routes.MapRoute(name: "nhom-san-pham/sua-nhom-san-pham/{SearchKey}", url: "nhom-san-pham/sua-nhom-san-pham/{SearchKey}", defaults: new { controller = "ProductGroup", action = "EditProductGroupView", SearchKey = UrlParameter.Optional });
            routes.MapRoute(name: "nhom-san-pham/chi-tiet/{SearchKey}", url: "nhom-san-pham/chi-tiet/{SearchKey}", defaults: new { controller = "ProductGroup", action = "SearchListProductView", SearchKey = UrlParameter.Optional });
            #endregion

            #region PRODUCT_CONTROLLER - SẢN PHẨM
            routes.MapRoute(name: "san-pham/danh-sach-san-pham", url: "san-pham/danh-sach-san-pham", defaults: new { controller = "Product", action = "ProductView" });
            routes.MapRoute(name: "san-pham/tao-san-pham", url: "san-pham/tao-san-pham", defaults: new { controller = "Product", action = "AddProductView" });
            routes.MapRoute(name: "san-pham/chinh-sua-san-pham", url: "san-pham/chinh-sua-san-pham/{ProductID}", defaults: new { controller = "Product", action = "EditProductView", ProductID = UrlParameter.Optional });
            routes.MapRoute(name: "san-pham/import-excel", url: "san-pham/import-excel", defaults: new { controller = "Product", action = "ImportExcelView" });
            #endregion

            #region WAREHOUSE_CONTROLLER - KHO
            routes.MapRoute(name: "kho/danh-sach-kho", url: "kho/danh-sach-kho", defaults: new { controller = "WareHouse", action = "WareHouseView" });
            #endregion

            #region INVENTORY_CONTROLLER - TỒN KHO
            routes.MapRoute(name: "kho/danh-sach-san-pham-ton-kho", url: "kho/danh-sach-san-pham-ton-kho", defaults: new { controller = "Inventory", action = "InventoryView" });
            #endregion

            #region INVENTORY_OUTPUT_CONTROLLER - PHIẾU XUẤT KHO
            routes.MapRoute(name: "kho/danh-sach-phieu-xuat-kho", url: "kho/danh-sach-phieu-xuat-kho", defaults: new { controller = "InventoryOutput", action = "InventoryOutputView" });
            routes.MapRoute(name: "kho/tao-phieu-xuat-kho", url: "kho/tao-phieu-xuat-kho", defaults: new { controller = "InventoryOutput", action = "AddInventoryOutputView" });
            routes.MapRoute(name: "kho/sua-phieu-xuat-kho", url: "kho/sua-phieu-xuat-kho/{id}", defaults: new { controller = "InventoryOutput", action = "EditInventoryOutputView", id = UrlParameter.Optional });
            routes.MapRoute(name: "kho/chi-tiet-phieu-xuat-kho", url: "kho/chi-tiet-phieu-xuat-kho/{id}", defaults: new { controller = "InventoryOutput", action = "DetailInventoryOutputView", id = UrlParameter.Optional });
            #endregion

            #region PERMISSION_CONTROLLER
            routes.MapRoute(name: "cau-hinh/phan-quyen", url: "cau-hinh/phan-quyen", defaults: new { controller = "Permission", action = "PermissionView" });
            #endregion

            #region RECONCILIATION_CONTROLLER - ĐỐI SOÁT
            routes.MapRoute(name: "doi-soat/ds-cuoc-van-chuyen", url: "doi-soat/ds-cuoc-van-chuyen", defaults: new { controller = "Reconciliation", action = "FreightDailyView" });
            routes.MapRoute(name: "doi-soat/ds-doi-soat-cuoc-van-chuyen", url: "doi-soat/ds-doi-soat-cuoc-van-chuyen", defaults: new { controller = "Reconciliation", action = "FreightView" });
            routes.MapRoute(name: "doi-soat/ds-doi-soat-cuoc-van-chuyen-ctkm", url: "doi-soat/ds-doi-soat-cuoc-van-chuyen-ctkm", defaults: new { controller = "Reconciliation", action = "FreightTradeDiscountsView" });
            routes.MapRoute(name: "doi-soat/ds-tien-cod", url: "doi-soat/ds-tien-cod", defaults: new { controller = "Reconciliation", action = "CODMoneyDailyView" });
            routes.MapRoute(name: "doi-soat/ds-doi-soat-tien-cod", url: "doi-soat/ds-doi-soat-tien-cod", defaults: new { controller = "Reconciliation", action = "CODMoneyView" });
            routes.MapRoute(name: "doi-soat/ds-buu-gui-hoan-cuoc", url: "doi-soat/ds-buu-gui-hoan-cuoc", defaults: new { controller = "Reconciliation", action = "PostageRefundView" });
            routes.MapRoute(name: "doi-soat/giay-bao-cuoc-phi", url: "doi-soat/giay-bao-cuoc-phi", defaults: new { controller = "Reconciliation", action = "DebtNoticesView" });
            #endregion

            #region INVENTORY_IMPORT_CONTROLLER - PHIẾU NHẬP KHO
            routes.MapRoute(name: "kho/danh-sach-phieu-nhap-kho", url: "kho/danh-sach-phieu-nhap-kho", defaults: new { controller = "InventoryImport", action = "InventoryImportView" });
            routes.MapRoute(name: "kho/tao-phieu-nhap-kho", url: "kho/tao-phieu-nhap-kho", defaults: new { controller = "InventoryImport", action = "AddInventoryImportView" });
            routes.MapRoute(name: "kho/chi-tiet-phieu-nhap-kho/{id}", url: "kho/chi-tiet-phieu-nhap-kho/{id}", defaults: new { controller = "InventoryImport", action = "DetailInventoryImportView", id = UrlParameter.Optional });
            routes.MapRoute(name: "kho/sua-phieu-nhap-kho", url: "kho/sua-phieu-nhap-kho/{id}", defaults: new { controller = "InventoryImport", action = "EditInventoryImportView", id = UrlParameter.Optional });
            #endregion

            #region SHOP_CONTROLLER
            routes.MapRoute(name: "thong-tin-tai-khoan", url: "thong-tin-tai-khoan", defaults: new { controller = "Shop", action = "ShopView" });
            #endregion

            #region REQUEST_SUPPORT_CONTROLLER - YÊU CẦU HỖ TRỢ
            routes.MapRoute(name: "ho-tro/danh-sach-yeu-cau-ho-tro", url: "ho-tro/danh-sach-yeu-cau-ho-tro", defaults: new { controller = "RequestSupport", action = "RequestSupportView" });
            routes.MapRoute(name: "ho-tro/chi-tiet-yeu-cau-ho-tro/{SearchKey}", url: "ho-tro/chi-tiet-yeu-cau-ho-tro/{SearchKey}", defaults: new { controller = "RequestSupport", action = "DetailRequestSupportView", SearchKey = UrlParameter.Optional });
            #endregion

            #region REQUEST_ORDER_CONTROLLER - YÊU CẦU ĐẶT HÀNG
            routes.MapRoute(name: "yeu-cau-dat-hang/tao-yeu-cau", url: "yeu-cau-dat-hang/tao-yeu-cau", defaults: new { controller = "RequestOrder", action = "AddRequestOrderView" });
            routes.MapRoute(name: "yeu-cau-dat-hang/xu-ly-yeu-cau", url: "yeu-cau-dat-hang/xu-ly-yeu-cau", defaults: new { controller = "RequestOrder", action = "RequestOrderView" });
            routes.MapRoute(name: "yeu-cau-dat-hang/chia-lai-yeu-cau", url: "yeu-cau-dat-hang/chia-lai-yeu-cau", defaults: new { controller = "RequestOrder", action = "DivisionAgainView" });
            routes.MapRoute(name: "yeu-cau-dat-hang/chia-yeu-cau", url: "yeu-cau-dat-hang/chia-yeu-cau", defaults: new { controller = "RequestOrder", action = "DivisionView" });
            #endregion

            #region PROMOTION_CONTROLLER
            routes.MapRoute(name: "khuyen-mai/danh-sach-khuyen-mai", url: "khuyen-mai/danh-sach-khuyen-mai", defaults: new { controller = "Promotion", action = "PromotionView" });
            routes.MapRoute(name: "khuyen-mai/tao-ma-khuyen-mai", url: "khuyen-mai/tao-ma-khuyen-mai", defaults: new { controller = "Promotion", action = "AddPromotionCode" });
            routes.MapRoute(name: "khuyen-mai/tao-chuong-trinh-khuyen-mai", url: "khuyen-mai/tao-chuong-trinh-khuyen-mai", defaults: new { controller = "Promotion", action = "AddPromotionView" });
            #endregion

            #region INVENTORY_ADJUST - PHIẾU ĐIỀU CHỈNH KHO
            routes.MapRoute(name: "dieu-chinh-kho/danh-sach-dieu-chinh-kho", url: "dieu-chinh-kho/danh-sach-dieu-chinh-kho", defaults: new { controller = "InventoryAdjust", action = "InventoryAdjustView" });
            routes.MapRoute(name: "dieu-chinh-kho/tao-phieu-dieu-chinh-kho", url: "dieu-chinh-kho/tao-phieu-dieu-chinh-kho", defaults: new { controller = "InventoryAdjust", action = "AddInventoryAdjustView" });

            #endregion

            #region STOCK_TAKE_CONTROLLER - PHIẾU KIỂM KHO
            routes.MapRoute(name: "kho/danh-sach-phieu-kiem-kho", url: "kho/danh-sach-phieu-kiem-kho", defaults: new { controller = "StockTake", action = "StockTakeView" });
            routes.MapRoute(name: "kho/tao-phieu-kiem-kho", url: "kho/tao-phieu-kiem-kho", defaults: new { controller = "StockTake", action = "AddStockTakeView" });
            routes.MapRoute(name: "kho/chi-tiet-phieu-kiem-kho", url: "kho/chi-tiet-phieu-kiem-kho", defaults: new { controller = "StockTake", action = "DetailStockTakeView", id = UrlParameter.Optional });
            routes.MapRoute(name: "kho/sua-phieu-kiem-kho", url: "kho/sua-phieu-kiem-kho", defaults: new { controller = "StockTake", action = "EditStockTakeView", id = UrlParameter.Optional });

            #endregion

            #region TRANSPORT_FEE_CONTROLLER - ƯỚC TÍNH CƯỚC PHÍ
            routes.MapRoute(name: "don-hang/uoc-tinh-cuoc-phi", url: "don-hang/uoc-tinh-cuoc-phi", defaults: new { controller = "TransportFee", action = "TransportFeeView" });
            #endregion

            #region TRANSPORT_SERVICE_CONTROLLER - CẤU HÌNH DỊCH VỤ
            routes.MapRoute(name: "cau-hinh/dich-vu", url: "cau-hinh/dich-vu", defaults: new { controller = "TransportService", action = "TransportServiceView" });


            #endregion

            #region GRAB
            routes.MapRoute(name: "don-hang-sieu-toc/tao-don-hang-sieu-toc", url: "don-hang-sieu-toc/tao-don-hang-sieu-toc", defaults: new { controller = "GrabOrder", action = "AddGrabOrderView" });
            routes.MapRoute(name: "don-hang-sieu-toc/danh-sach-don-hang-sieu-toc", url: "don-hang-sieu-toc/danh-sach-don-hang-sieu-toc", defaults: new { controller = "GrabOrder", action = "GrabOrderView" });
            routes.MapRoute(name: "tra-cuu-buu-gui-grab", url: "tra-cuu-buu-gui-grab", defaults: new { controller = "GrabOrder", action = "GrabDetailShippingView" });
            #endregion

                #region NOTIFY
            routes.MapRoute(name: "thong-bao", url: "thong-bao", defaults: new { controller = "Notify", action = "NotifyView" });
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "LoginView", id = UrlParameter.Optional },
                namespaces: new string[] { "EMS.MVC.Frontend.Controllers" }
            );
        }
    }
}
