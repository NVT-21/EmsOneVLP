using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Statistical")]
    public class StatisticalController : ApiController
    {
        [Route("StatisticalDashboard")]
        public ZResponse StatisticalDashboard(ZStatisticalDashboardRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                switch (req.Type)
                {
                    case "Transport":
                        res = StatisticalRepository.StatisticalTransport(new ZStatisticalSearchTransportRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "Order":
                        res = StatisticalRepository.StatisticalOrder(new ZStatisticalSearchOrderRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "Customer":
                        res = StatisticalRepository.StatisticalCustomer(new ZStatisticalSearchCustomerRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "Cashflow":
                        res = StatisticalRepository.StatisticalCashflow(new ZStatisticalSearchCashflowRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "Product":
                        res = StatisticalRepository.StatisticalProduct(new ZStatisticalSearchProductRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "Turnover":
                        res = StatisticalRepository.StatisticalTurnover(new ZStatisticalSearchTurnoverRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    case "RequestOrder":
                        res = StatisticalRepository.StatisticalRequestOrder(new ZRequestOrderDashboardRequest()
                        {
                            ShopID = req.ShopID,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        }); ;
                        break;
                    default:
                        res.Code = "99";
                        res.Message = "Không tìm thấy loại báo cáo, thống kê: " + req.Type;
                        break;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalController.StatisticalDashboard: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("StatisticalDashboardV2")]
        public ZResponse StatisticalDashboardV2(ZStatisticalDashboardV2Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                switch (req.DeviceName)
                {
                    case "WEB":
                        res = StatisticalRepository.StatisticalWeb(new ZStatisticalDashboardWebRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        });
                        break;
                    case "ANDROID":
                        res = StatisticalRepository.StatisticalAndroid(new ZStatisticalDashboardAndroidRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        });
                        break;
                    case "IOS":
                        res = StatisticalRepository.StatisticalIOS(new ZStatisticalDashboardIOSRequest()
                        {
                            PPCRMCode = req.PPCRMCode,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate
                        });
                        break;
                    default:
                        res.Code = "99";
                        res.Message = "Không tìm thấy loại báo cáo, thống kê theo DeviceName: " + req.DeviceName;
                        break;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalController.StatisticalDashboardV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
