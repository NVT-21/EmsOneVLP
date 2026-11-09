using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMS.MVC.Backend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region COLLECTION_POST_OFFICE_CONTROLLER
            routes.MapRoute(name: "quan-ly-buu-cuc-thu-gom/danh-sach", url: "quan-ly-buu-cuc-thu-gom/danh-sach", defaults: new { controller = "CollectionPostOffice", action = "CollectionPostOfficeView" });

            #endregion

            #region LOGIN_CONTROLLER
            routes.MapRoute(name: "dang-nhap", url: "dang-nhap", defaults: new { controller = "Login", action = "Index" });
            #endregion

            #region SHOP_CONTROLLER
            routes.MapRoute(name: "tai-khoan-khach-hang/danh-sach", url: "tai-khoan-khach-hang/danh-sach", defaults: new { controller = "Shop", action = "ShopView" });
            routes.MapRoute(name: "tai-khoan-khach-hang/them-tai-khoan-khach-hang", url: "tai-khoan-khach-hang/them-tai-khoan-khach-hang", defaults: new { controller = "Shop", action = "AddShopView" });
            routes.MapRoute(name: "tai-khoan-khach-hang/sua-tai-khoan-khach-hang/{ID}", url: "tai-khoan-khach-hang/sua-tai-khoan-khach-hang/{ID}", defaults: new { controller = "Shop", action = "EditShopView", ID = UrlParameter.Optional });
            routes.MapRoute(name: "tai-khoan-khach-hang/xem-tai-khoan-khach-hang/{ShopID}", url: "tai-khoan-khach-hang/xem-tai-khoan-khach-hang/{ShopID}", defaults: new { controller = "Shop", action = "ApprovedShopView", ShopID = UrlParameter.Optional });
            #endregion

            #region ORDER_CONTROLLER
            routes.MapRoute(name: "quan-ly-don-hang/danh-sach", url: "quan-ly-don-hang/danh-sach", defaults: new { controller = "Order", action = "OrderView" });
            routes.MapRoute(name: "quan-ly-don-hang/in-don-hang", url: "quan-ly-don-hang/in-don-hang", defaults: new { controller = "Order", action = "OrderPrintView" });
            #endregion

            #region REQUEST_SUPPORT_CONTROLLER
            routes.MapRoute(name: "yeu-cau-ho-tro/danh-sach-yeu-cau-ho-tro", url: "yeu-cau-ho-tro/danh-sach-yeu-cau-ho-tro", defaults: new { controller = "RequestSupport", action = "RequestSupportView" });
            routes.MapRoute(name: "yeu-cau-ho-tro/chi-tiet-yeu-cau-ho-tro/{SearchKey}", url: "yeu-cau-ho-tro/chi-tiet-yeu-cau-ho-tro/{SearchKey}", defaults: new { controller = "RequestSupport", action = "DetailRequestSupportView", SearchKey = UrlParameter.Optional });
            #endregion

            #region USER_CONTROLLER
            routes.MapRoute(name: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh", url: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh", defaults: new { controller = "User", action = "UserView" });
            routes.MapRoute(name: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh/them-tai-khoan-van-hanh", url: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh/them-tai-khoan-van-hanh", defaults: new { controller = "User", action = "AddUserView" });
            routes.MapRoute(name: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh/sua-tai-khoan-van-hanh/{SearchKey}", url: "tai-khoan-van-hanh/danh-sach-tai-khoan-van-hanh/sua-tai-khoan-van-hanh/{SearchKey}", defaults: new { controller = "User", action = "EditUserView", SearchKey = UrlParameter.Optional });
            #endregion

            #region TRANSPORT_FEE_CONTROLLER - ƯỚC TÍNH CƯỚC PHÍ
            routes.MapRoute(name: "uoc-tinh-cuoc-phi", url: "uoc-tinh-cuoc-phi", defaults: new { controller = "TransportFee", action = "TransportFeeView" });
            #endregion

            #region TRANSPORT_SERVICE_CONTROLLER - DỊCH VỤ VẬN CHUYỂN
            routes.MapRoute(name: "dich-vu-van-chuyen/danh-sach-dich-vu-van-chuyen", url: "dich-vu-van-chuyen/danh-sach-dich-vu-van-chuyen", defaults: new { controller = "TransportService", action = "TransportServiceView" });
            #endregion

            #region TRANSPORT_MAIN_SERVICE_CONTROLLER - DỊCH VỤ VẬN CHUYỂN CHÍNH
            routes.MapRoute(name: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh", url: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh", defaults: new { controller = "TransportMainService", action = "FreightChargesView" });
            routes.MapRoute(name: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/them-bang-cuoc-dich-vu-chinh", url: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/them-bang-cuoc-dich-vu-chinh", defaults: new { controller = "TransportMainService", action = "AddFreightChargesView" });
            routes.MapRoute(name: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/sua-bang-cuoc-dich-vu-chinh", url: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/sua-bang-cuoc-dich-vu-chinh", defaults: new { controller = "TransportMainService", action = "EditFreightChargesView"});
            routes.MapRoute(name: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/sua-bang-cuoc-dich-vu-chinh/cau-hinh-cuoc-dich-vu-chinh", url: "bang-cuoc-dich-vu-chinh/danh-sach-bang-cuoc-dich-vu-chinh/sua-bang-cuoc-dich-vu-chinh/cau-hinh-cuoc-dich-vu-chinh", defaults: new { controller = "TransportMainService", action = "ConfigMainFeeView" });

            #endregion

            #region TRANSPORT_EXTRA_SERVICE_CONTROLLER - DỊCH VỤ VẬN CHUYỂN CỘNG THÊM
            routes.MapRoute(name: "bang-cuoc-dich-vu-cong-them/danh-sach-bang-cuoc-dich-vu-cong-them", url: "bang-cuoc-dich-vu-cong-them/danh-sach-bang-cuoc-dich-vu-cong-them", defaults: new { controller = "TransportExtraService", action = "FreightChargesView" });
            routes.MapRoute(name: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/them-bang-cuoc-dich-vu-van-chuyen-cong-them", url: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/them-bang-cuoc-dich-vu-van-chuyen-cong-them", defaults: new { controller = "TransportExtraService", action = "AddFreightChargesView" });
            routes.MapRoute(name: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/sua-bang-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", url: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/sua-bang-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", defaults: new { controller = "TransportExtraService", action = "EditFreightChargesView", TransportPostageID = UrlParameter.Optional });
            routes.MapRoute(name: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/chi-tiet-bang-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", url: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/chi-tiet-bang-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", defaults: new { controller = "TransportExtraService", action = "DetailFreightChargesView", TransportPostageID = UrlParameter.Optional });
            routes.MapRoute(name: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/sua-bang-cuoc-dich-vu-van-chuyen-cong-them/cau-hinh-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", url: "bang-cuoc-dich-vu-van-chuyen-cong-them/danh-sach-bang-cuoc-dich-vu-van-chuyen-cong-them/sua-bang-cuoc-dich-vu-van-chuyen-cong-them/cau-hinh-cuoc-dich-vu-van-chuyen-cong-them/{TransportPostageID}", defaults: new { controller = "TransportExtraService", action = "ConfigMainFeeView", TransportPostageID = UrlParameter.Optional });
            #endregion

            #region RECONCILIATION
            routes.MapRoute(name: "quan-ly-doi-soat/tien-cod", url: "quan-ly-doi-soat/tien-cod", defaults: new { controller = "Reconciliation", action = "CODMoney" });
            routes.MapRoute(name: "quan-ly-doi-soat/tien-cuoc", url: "quan-ly-doi-soat/tien-cuoc", defaults: new { controller = "Reconciliation", action = "Freight" });
            routes.MapRoute(name: "quan-ly-doi-soat/but-toan-dieu-chinh", url: "quan-ly-doi-soat/but-toan-dieu-chinh", defaults: new { controller = "Reconciliation", action = "FeeAdjustingJournalEntries" });
            routes.MapRoute(name: "quan-ly-doi-soat/chi-tiet-dieu-chinh", url: "quan-ly-doi-soat/chi-tiet-dieu-chinh", defaults: new { controller = "Reconciliation", action = "FeeDetailEditedItemCode" });
            routes.MapRoute(name: "quan-ly-doi-soat/buu-gui-hoan-cuoc", url: "quan-ly-doi-soat/buu-gui-hoan-cuoc", defaults: new { controller = "Reconciliation", action = "PostageRefund" });
            routes.MapRoute(name: "quan-ly-doi-soat/giay-bao-cuoc-phi", url: "quan-ly-doi-soat/giay-bao-cuoc-phi", defaults: new { controller = "Reconciliation", action = "DebtNotices" });
            routes.MapRoute(name: "quan-ly-doi-soat/bao-cao-dong-bo", url: "quan-ly-doi-soat/bao-cao-dong-bo", defaults: new { controller = "Reconciliation", action = "ReconciliationFeeReport" });
            #endregion

            #region ORDERREPORT
            routes.MapRoute(name: "bao-cao-san-luong/danh-sach", url: "bao-cao-san-luong/danh-sach", defaults: new { controller = "OrderReport", action = "OrderReportView" });
            #endregion

            #region PARTNERWEBHOOK
            routes.MapRoute(name: "khai-bao-doi-tac-webhook/danh-sach", url: "khai-bao-doi-tac-webhook/danh-sach", defaults: new { controller = "PartnerWebhook", action = "Index" });
            #endregion

            #region NOTIFYCONTROL
            routes.MapRoute(name: "quan-ly-thong-bao", url: "quan-ly-thong-bao", defaults: new { controller = "NotifyControl", action = "NotifyView" });
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
