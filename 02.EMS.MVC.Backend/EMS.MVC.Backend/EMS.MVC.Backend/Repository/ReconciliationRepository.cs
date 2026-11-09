using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class ReconciliationRepository
    {
        public static AdjustingJournalEntriesViewModel DataFeeEditedItemCode(ZReconciliationFeeAdjustingJournalEntriesRequest req)
        {
            ZResponse res = new ZResponse();
            AdjustingJournalEntriesViewModel model = null;
            List<AdjustingJournalEntriesDataViewModel> ListItem = new List<AdjustingJournalEntriesDataViewModel>();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RECONCILIATION_ADJUSTING_JOURNAL_ENTRIES_LIST",
                    Data = JsonConvert.SerializeObject(new ZReconciliationFeeAdjustingJournalEntriesRequest
                    {
                        FromDate = req.FromDate,
                        ToDate = req.ToDate,
                        CustomerPPCRM = req.CustomerPPCRM,
                        AdjustingPostOffice = req.AdjustingPostOffice
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZReconciliationFeeAdjustingJournalEntriesResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new AdjustingJournalEntriesDataViewModel()
                        {
                            AdjustingID = item.AdjustingID,
                            AdjustingDate = item.AdjustingDate,
                            AdjustingPostOffice = item.AdjustingPostOffice,
                            ItemCode = item.ItemCode,
                            TimeRelease = item.TimeRelease,
                            ProvisionalVolume = item.ProvisionalVolume,
                            ChargeableVolume = item.ChargeableVolume,
                            MainFee = item.MainFee,
                            ServiceFee = item.ServiceFee,
                            TransportFeePublic = item.TransportFeePublic,
                            FeePublic = item.FeePublic,
                            Reason = item.Reason
                        });
                    }
                    model = new AdjustingJournalEntriesViewModel()
                    {
                        SumMainFee = ListItem.Sum(m => m.MainFee),
                        SumServiceFee = ListItem.Sum(m => m.ServiceFee),
                        SumTransportFeePublic = ListItem.Sum(m => m.TransportFeePublic),
                        SumFeePublic = ListItem.Sum(m => m.FeePublic),
                        SumProvisionalVolume = ListItem.Sum(m => m.ProvisionalVolume),
                        SumChargeableVolume = ListItem.Sum(m => m.ChargeableVolume),
                        ListItem = ListItem
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationRepository.DataFeeEditedItemCode: " + ex.Message);
            }
            return model;
        }
        public static List<DetailEditedItemCodeViewModel> DataFeeDetailEditedItemCode(ZReconciliationListDetailEditedItemCodeRequest req)
        {
            ZResponse res = new ZResponse();
            List<DetailEditedItemCodeViewModel> model = new List<DetailEditedItemCodeViewModel>();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RECONCILIATION_DETAIL_EDITED_ITEMCODE_LIST",
                    Data = JsonConvert.SerializeObject(new ZReconciliationListDetailEditedItemCodeRequest
                    {
                        FromDate = req.FromDate,
                        ToDate = req.ToDate,
                        PostOffice = req.PostOffice,
                        CustomerPPCRM = req.CustomerPPCRM
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZReconciliationListDetailEditedItemCodeResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        model.Add(new DetailEditedItemCodeViewModel()
                        {
                            NumRow = item.NumRow,
                            ItemCode = item.ItemCode,
                            AdjBalanceProvisionalVolume = item.AdjBalanceProvisionalVolume,
                            AdjBalanceChargeableVolume = item.AdjBalanceChargeableVolume,
                            AdjBalanceMainFee = item.AdjBalanceMainFee,
                            AdjBalanceServiceFee = item.AdjBalanceServiceFee,
                            AdjBalanceTransportFeePublic = item.AdjBalanceTransportFeePublic,
                            AdjBalanceFeePublic = item.AdjBalanceFeePublic,
                            ItemCodeDetail = item.ItemCodeDetail
                        });
                    }
                    model = model.OrderBy(m => m.NumRow).ToList();
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationRepository.DataFeeDetailEditedItemCode: " + ex.Message);
            }
            return model;
        }
        public static PostageRefundViewModel DataPostageRefund(ZReconciliationSearchPostageRefundV1Request req)
        {
            ZResponse res = new ZResponse();
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            PostageRefundViewModel model = null;
            List<PostageRefundDetailsViewModel> modelDetail = new List<PostageRefundDetailsViewModel>();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RECONCILIATION_POSTAGE_REFUND",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<ZReconciliationSearchPostageRefundData>(res.Data);
                    foreach (var item in list.DataDetails)
                    {
                        modelDetail.Add(new PostageRefundDetailsViewModel()
                        {
                            RowNumber = item.RowNumber,
                            CustomerCode = item.CustomerCode,
                            ItemCode = item.ItemCode,
                            ReleaseDate = item.ReleaseDate == "" ? "" : DateTime.ParseExact(item.ReleaseDate, "yyyy-MM-ddThh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss"),
                            RefundDate = item.RefundDate == "" ? "" : DateTime.ParseExact(item.RefundDate, "yyyy-MM-ddThh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss"),
                            MoneyforCollection = item.MoneyforCollection,
                            CODFee = item.CODFee,
                            CODFeeRefund = item.CODFeeRefund,
                            DiscountRate = item.DiscountRate,
                            FeeE1TransferRefund = item.FeeE1TransferRefund,
                            RefundAmount = item.RefundAmount,
                            Reference = item.Reference,
                            Note = item.Note,
                        });
                    }
                    model = new PostageRefundViewModel()
                    {
                        CustomerCode = list.CustomerCode,
                        MonthYear = list.MonthYear,
                        DataDetails = modelDetail,
                        SumMoneyforCollection = modelDetail.Sum(m => m.MoneyforCollection),
                        SumCODFee = modelDetail.Sum(m => m.CODFee),
                        SumCODFeeRefund = modelDetail.Sum(m => m.CODFeeRefund),
                        SumFeeE1TransferRefund = modelDetail.Sum(m => m.FeeE1TransferRefund),
                        SumRefundAmount = modelDetail.Sum(m => m.RefundAmount)
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationRepository.DataPostageRefund: " + ex.Message);
            }
            return model;
        }
        public static DebtNoticesViewModel DataDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ZResponse res = new ZResponse();
            DebtNoticesViewModel model = null;
            DebtNoticesDataInfo modelDataInfo = new DebtNoticesDataInfo();
            List<DebtNoticesDataInfoDetails> modelDataInfoDetail = new List<DebtNoticesDataInfoDetails>();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RECONCILIATION_DEBT_NOTICES",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<ZReconciliationSearchDebtNoticesData>(res.Data);
                    foreach (var item in list.DataInfo)
                    {
                        modelDataInfo = new DebtNoticesDataInfo()
                        {
                            CustomerCode = item.CustomerCode,
                            CustomerName = item.CustomerName,
                            Address = item.Address,
                            District = item.District,
                            Date = item.Date == "" ? "" : DateTime.ParseExact(item.Date, "yyyy-MM-ddThh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss"),
                            EcommerceRate = item.EcommerceRate,
                            MobileNumber = item.MobileNumber,
                            DomesticRate = item.DomesticRate,
                            InternationalRate = item.InternationalRate,
                            InvoiceNumber = item.InvoiceNumber,
                            ReturnRate = item.ReturnRate,
                            OtherCharges = item.OtherCharges,
                            Payments = item.Payments,
                            PaymentAddress = item.PaymentAddress,
                            AgreementRate = item.AgreementRate,
                            PostOfficeAccountantName = item.PostOfficeAccountantName,
                            PostOfficeAccountantPhone = item.PostOfficeAccountantPhone,
                            PriorityRate = item.PriorityRate,
                            TaxCode = item.TaxCode,
                            ParcelRevenue = item.ParcelRevenue,
                            RegisteredParcelRevenue = item.RegisteredParcelRevenue,
                            RegularParcelRevenue = item.RegularParcelRevenue,
                            PermuteRevenue = item.PermuteRevenue,
                            AdjustmentFeeRevenue = item.AdjustmentFeeRevenue,
                            OtherAdjustmentChargesRevenue = item.OtherAdjustmentChargesRevenue,
                            EMSExtraServiceRevenue = item.EMSExtraServiceRevenue,
                            RefundRevenue = item.RefundRevenue,
                            PromotionCVRevenue = item.PromotionCVRevenue,
                            InternationalRevenue = item.InternationalRevenue,
                            InternationalPriorityRevenue = item.InternationalPriorityRevenue,
                            NegotiableRevenue = item.NegotiableRevenue,
                            EcommerceRevenue = item.EcommerceRevenue,
                            DomesticRevenue = item.DomesticRevenue,
                            ArrearsRevenue = item.ArrearsRevenue
                        };
                    }
                    foreach (var itemDetail in list.DataInfoDetails)
                    {
                        modelDataInfoDetail.Add(new DebtNoticesDataInfoDetails()
                        {
                            RowNumber = itemDetail.RowNumber,
                            CustomerCode = itemDetail.CustomerCode,
                            CustomerName = itemDetail.CustomerName,
                            Address = itemDetail.Address,
                            ItemCode = itemDetail.ItemCode,
                            ReleaseDate = itemDetail.ReleaseDate == "" ? "" : DateTime.ParseExact(itemDetail.ReleaseDate, "yyyy-MM-ddThh:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss"),
                            PostOfficeExploitCode = itemDetail.PostOfficeExploitCode,
                            PostOfficeExploit = itemDetail.PostOfficeExploit,
                            PostOfficeReturn = itemDetail.PostOfficeReturn,
                            Receiver = itemDetail.Receiver,
                            ReceiverTel = itemDetail.ReceiverTel,
                            ReceiverAddress = itemDetail.ReceiverAddress,
                            ReferenceNumber = itemDetail.ReferenceNumber,
                            CODFee = itemDetail.CODFee,
                            MainFee = itemDetail.MainFee,
                            ServiceFee = itemDetail.ServiceFee,
                            AdjustmentFee = itemDetail.AdjustmentFee,
                            Internal = itemDetail.Internal,
                            Value = itemDetail.Value,
                            ConversionVolume = itemDetail.ConversionVolume,
                            Weight = itemDetail.Weight,
                            Content = itemDetail.Content
                        });
                    }
                    model = new DebtNoticesViewModel()
                    {
                        CustomerCode = list.CustomerCode,
                        MonthYear = list.MonthYear,
                        DataInfo = modelDataInfo,
                        DataInfoDetails = modelDataInfoDetail.OrderBy(m => m.RowNumber).ToList(),
                        SumMainFee = modelDataInfoDetail.Sum(m => m.MainFee),
                        SumCODFee = modelDataInfoDetail.Sum(m => m.CODFee),
                        SumServiceFee = modelDataInfoDetail.Sum(m => m.ServiceFee),
                        SumAdjustmentFee = modelDataInfoDetail.Sum(m => m.AdjustmentFee),
                        SumTotalCharge = modelDataInfoDetail.Sum(m => m.CODFee + m.MainFee + m.ServiceFee + m.AdjustmentFee)
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationRepository.DataDebtNotices: " + ex.Message);
            }
            return model;
        }
        public static SummaryOfDebtNoticesViewModel DataSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ZResponse res = new ZResponse();
            SummaryOfDebtNoticesViewModel model = null;
            SummaryOfDebtNoticesDataInfoModel modelDataInfo = new SummaryOfDebtNoticesDataInfoModel();
            SummaryOfDebtNoticesDataInfoDetailsModel modelDataInfoDetails = new SummaryOfDebtNoticesDataInfoDetailsModel();
            List<SummaryOfDebtNoticesDataInfoDetailsItemModel> modelDataInfoDetailsListItem = new List<SummaryOfDebtNoticesDataInfoDetailsItemModel>();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RECONCILIATION_SUMMARY_DEBT_NOTICES",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<ZReconciliationSearchSummaryOfDebtNoticesData>(res.Data);
                    foreach (var item in list.DataInfoDetails)
                    {
                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 1,
                            NumRow = "1",
                            Explain = "EMS Quốc tế",
                            NumberOfParcels = item.InterParcelQuantity,
                            FeeBeforeVAT = item.InterEBT,
                            FeeVAT = item.InterVAT,
                            FeeAfterVAT = item.InterPAT
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 2,
                            NumRow = "2",
                            Explain = "EMS Trong nước",
                            NumberOfParcels = item.DomesticParcelQuantity,
                            FeeBeforeVAT = item.DomesticEBT,
                            FeeVAT = item.DomesticVAT,
                            FeeAfterVAT = item.DomesticRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 3,
                            NumRow = "3",
                            Explain = "Dịch vụ EMS cộng thêm",
                            NumberOfParcels = item.EmsServiceQuantity,
                            FeeBeforeVAT = item.EmsServiceEBT,
                            FeeVAT = item.EmsServiceVAT,
                            FeeAfterVAT = item.RevenueEmsService
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 4,
                            NumRow = "4",
                            Explain = "Chuyển hoàn",
                            NumberOfParcels = item.ReturneParcelQuantity,
                            FeeBeforeVAT = item.ReturnedParcelEBT,
                            FeeVAT = item.ReturnedParcelVAT,
                            FeeAfterVAT = item.ReturnedParcelRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 5,
                            NumRow = "5A",
                            Explain = "Cước điều chỉnh",
                            NumberOfParcels = item.AdjustChargeQuantity,
                            FeeBeforeVAT = item.AdjustChargeEBT,
                            FeeVAT = item.AdjustChargeVAT,
                            FeeAfterVAT = item.AdjustChargeRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 6,
                            NumRow = "5B",
                            Explain = "Cước điều chỉnh VAT 10%",
                            NumberOfParcels = item.AdjustChargeQuantityExtend,
                            FeeBeforeVAT = item.AdjustChargeQuantityExtendEBT,
                            FeeVAT = item.AdjustChargeQuantityExtendVAT,
                            FeeAfterVAT = item.AdjustChargeQuantityExtendRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 7,
                            NumRow = "6",
                            Explain = "Bưu kiện",
                            NumberOfParcels = item.ParcelQuantity,
                            FeeBeforeVAT = item.ParcelEBT,
                            FeeVAT = item.ParcelVAT,
                            FeeAfterVAT = item.ParcelRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 8,
                            NumRow = "7",
                            Explain = "Bưu phẩm thường",
                            NumberOfParcels = item.StdParcelQuantity,
                            FeeBeforeVAT = item.StdParcelEBT,
                            FeeVAT = item.StdParcelVAT,
                            FeeAfterVAT = item.StdParcelRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 9,
                            NumRow = "8",
                            Explain = "Bưu phẩm GS",
                            NumberOfParcels = item.GSParcelQuantity,
                            FeeBeforeVAT = item.GSParcelEBT,
                            FeeVAT = item.GSParcelVAT,
                            FeeAfterVAT = item.GSParcelRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 10,
                            NumRow = "9A",
                            Explain = "Hoàn cước",
                            NumberOfParcels = item.ReCODQuantity,
                            FeeBeforeVAT = item.ReCODEBT,
                            FeeVAT = item.ReCODVAT,
                            FeeAfterVAT = item.ReCODRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 11,
                            NumRow = "9B",
                            Explain = "Hoàn cước VAT 10%",
                            NumberOfParcels = item.ReCODExtendQuantity,
                            FeeBeforeVAT = item.ReCODExtendEBT,
                            FeeVAT = item.ReCODExtendVAT,
                            FeeAfterVAT = item.ReCODExtendRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 12,
                            NumRow = "10A",
                            Explain = "Truy thu",
                            NumberOfParcels = item.ArrearsQuantity,
                            FeeBeforeVAT = item.ArrearsEBT,
                            FeeVAT = item.ArrearsVAT,
                            FeeAfterVAT = item.ArrearsRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 13,
                            NumRow = "10B",
                            Explain = "Truy thu VAT 10%",
                            NumberOfParcels = item.ArrearsExtendQuantity,
                            FeeBeforeVAT = item.ArrearsExtendEBT,
                            FeeVAT = item.ArrearsExtendVAT,
                            FeeAfterVAT = item.ArrearsExtendRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 14,
                            NumRow = "11",
                            Explain = "Chiết khấu thương mại",
                            NumberOfParcels = item.TradeDiscontQuantity,
                            FeeBeforeVAT = item.TradeDiscountEBT,
                            FeeVAT = item.TradeDiscountVAT,
                            FeeAfterVAT = item.TradeDiscountRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 15,
                            NumRow = "12",
                            Explain = "Giảm cước chương trình KM",
                            NumberOfParcels = item.KM_Theo_CV_SL,
                            FeeBeforeVAT = item.KM_Theo_CV_DT_Truoc_VAT,
                            FeeVAT = item.KM_Theo_CV_DT_VAT,
                            FeeAfterVAT = item.KM_Theo_CV_DT
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 16,
                            NumRow = "13",
                            Explain = "Doanh thu khác",
                            NumberOfParcels = item.OtherRevenueQuantity,
                            FeeBeforeVAT = item.OtherRevenueEBT,
                            FeeVAT = item.OtherRevenueVAT,
                            FeeAfterVAT = item.OtherRevenueIncomes
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 17,
                            NumRow = "14",
                            Explain = "Doanh thu chi nhánh khác",
                            NumberOfParcels = item.OtherRevenueQuantity,
                            FeeBeforeVAT = item.OtherRevenueEBT,
                            FeeVAT = item.OtherRevenueVAT,
                            FeeAfterVAT = item.OtherRevenueIncomes
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 18,
                            NumRow = "15",
                            Explain = "Tổng cộng",
                            NumberOfParcels = item.TotalQuantity,
                            FeeBeforeVAT = item.TotalEBT,
                            FeeVAT = item.TotalVAT,
                            FeeAfterVAT = item.TotalRevenue
                        });

                        modelDataInfoDetailsListItem.Add(new SummaryOfDebtNoticesDataInfoDetailsItemModel()
                        {
                            RowIndex = 19,
                            NumRow = "",
                            Explain = "Tổng thanh toán",
                            FeeAfterVAT = item.TotalRevenue
                        });

                        modelDataInfoDetails = new SummaryOfDebtNoticesDataInfoDetailsModel()
                        {
                            CustomerCode = item.CustomerCode,
                            CustomerName = item.CustomerName,
                            AccountantName = item.AccountantName,
                            AccountantPhone = item.AccountantPhone,
                            InvoiceNumber = item.InvoiceNumber,
                            PaymentPostCode = item.PaymentPostCode,
                            PaymentPostAddress = item.PaymentPostAddress,
                            ListItem = modelDataInfoDetailsListItem.OrderBy(m => m.RowIndex).ToList()
                        };
                    }
                    modelDataInfo = new SummaryOfDebtNoticesDataInfoModel()
                    {
                        BranchNameMenu = list.DataInfo.BranchNameMenu,
                        PaymentInformationMenu = list.DataInfo.PaymentInformationMenu.Replace("\r\n", " "),
                        CustomerServiceCenterNameMenu = list.DataInfo.CustomerServiceCenterNameMenu
                    };
                    model = new SummaryOfDebtNoticesViewModel()
                    {
                        CustomerCode = list.CustomerCode,
                        MonthYear = list.MonthYear,
                        DataInfo = modelDataInfo,
                        DataInfoDetails = modelDataInfoDetails,
                        TotalRevenueToText = modelDataInfoDetails.ListItem[modelDataInfoDetails.ListItem.Count - 1].FeeAfterVAT == 0 ? "" : XData.NumberToText(modelDataInfoDetails.ListItem[modelDataInfoDetails.ListItem.Count - 1].FeeAfterVAT)
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationRepository.DataSummaryOfDebtNotices: " + ex.Message);
            }
            return model;
        }

    }
}