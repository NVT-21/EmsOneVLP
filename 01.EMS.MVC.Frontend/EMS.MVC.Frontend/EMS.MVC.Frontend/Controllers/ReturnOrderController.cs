using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Controllers
{
    public class ReturnOrderController : Controller
    {
        // GET: ReturnOrder
        public ActionResult AddReturnOrder3TView()
        {
            var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());

            AddReturnOrder3TViewModel model = new AddReturnOrder3TViewModel()
            {
                ListProductName = ReceiverDataStore.ListProduct3T,
                ProductName = "",
                OrderValue = 1,
                Weight = 100,
                Quantity = 1,
                Sender = new CustomerModel()
                {
                    ListProvince = ListProvince,
                    ListDistrict = new List<DistrictModel>(),
                    ListWard = new List<WardModel>()
                },
                Receiver = new CustomerModel(),
                ListReceiver3T = ReceiverDataStore.ListReceiver3T
            };
            return PartialView(model);
        }

        #region Tính cước
        public ActionResult TemporaryTransportServiceFeeView(AddOrderViewModel req)
        {
            TemporaryTransportServiceFeeViewModel model = new TemporaryTransportServiceFeeViewModel();
            if(req.IsLoad == "Y")
            {
                var res = XCommon.TransportPostageCharged(new ZTransportPostageChargedRequest()
                {
                    MainServiceID = req.MainTransportServiceID,
                    ExtraServiceIDs = req.ExtraTransportServiceID,
                    FromProvinceID = req.Sender.Province.ID,
                    FromDistrictID = req.Sender.District.ID,
                    FromWardID = req.Sender.Ward.ID,
                    ToProvinceID = req.Receiver.Province.ID,
                    ToDistrictID = req.Receiver.District.ID,
                    ToWardID = req.Receiver.Ward.ID,
                    Weight = req.Weight,
                    TransportSizeLength = req.Length,
                    TransportSizeWidth = req.Width,
                    TransportSizeHeight = req.Height,
                    Quantity = req.Quantity,
                    Amount = req.OrderValue,
                    COD = req.TotalCOD,
                    CustomerCode = req.CrmOrPayPostCode,
                });
                if(res.Code == "00")
                {
                    var Item = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(res.Data);
                    model.ExtraServiceTotalAmount = Item.ExtraFee;
                    model.MainServiceTotalAmount = Item.MainFee + Item.Surcharge;
                    model.TotalAmount = Item.TotalAmount;
                }
                else
                {
                    model.Message = res.Message;
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Tạo đơn return
        public JsonResult JAddOrder3T(AddOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JAddOrder3TDataConvertRequest(req);
                res = OrderRepository.AddReturnOrderAndSendTransport(OrderAddRequest);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReutrnOrderController.JAddOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ZPartnerOrderAddRequest JAddOrder3TDataConvertRequest(AddOrderViewModel req)
        {
            ZPartnerOrderAddRequest OrderAddDataRequest = null;
            if( req.CollectionType.ID == 1)
            {
                req.Note = "Thu tiền người gửi - " + req.Note;
            }
            var ReceiverFullAddress = ReceiverDataStore.ListReceiver3T.Where(x => x.MobileNumber == req.Receiver.MobileNumber).FirstOrDefault().FullAddress;

            OrderAddDataRequest = new ZPartnerOrderAddRequest
            {
                 CrmOrPaypostCode = "C017777814",
                 OrderCode = "",
                 OrderName = req.OrderName,
                 OrderValue = req.OrderValue,
                 OrderQuantity = req.Quantity,
                 IsTransport = "Y",
                 IsSendTransport = "Y",
                 Note = req.Note,
                 Message = req.Message,
                 BuyerInfo = new ZPartnerOrderAddBuyerInfoRequest
                 {
                     FullName = req.Receiver.FullName,
                     MobileNumber = req.Receiver.MobileNumber,
                     ProvinceID = req.Receiver.Province.ID,
                     DistrictID = req.Receiver.District.ID,
                     WardID = req.Receiver.Ward.ID,
                     Street = req.Receiver.FullAddress       
                 },
                 SenderInfo = new ZPartnerOrderAddSenderInfoRequest
                 {
                     FullName = req.Sender.FullName,
                     MobileNumber = req.Sender.MobileNumber,
                     ProvinceID = req.Sender.Province.ID,
                     DistrictID = req.Sender.District.ID,
                     WardID = req.Sender.Ward.ID,
                     Street = req.Sender.Street
                 },
                 ReceiverInfo = new ZPartnerOrderAddReceiverInfoRequest
                 {
                     FullName = req.Receiver.FullName,
                     MobileNumber = req.Receiver.MobileNumber,
                     ProvinceID = req.Receiver.Province.ID,
                     DistrictID = req.Receiver.District.ID,
                     WardID = req.Receiver.Ward.ID,
                     Street = ReceiverFullAddress,             
                 },
                 TransportInfo = new ZPartnerOrderAddTransportInfoRequest
                 {
                     TransportMainServiceID = 141,
                     TransportExtraServiceID = null,
                     TransportWeight = req.Weight,
                     TransportSizeHeight = req.Height,
                     TransportSizeWidth = req.Width,
                     TransportSizeLength = req.Length,
                     CollectionType = req.CollectionType.ID,
                     TransportPayer = 1
                 }
            };
            return OrderAddDataRequest;
        }
        #endregion
    }
}