using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EMS.API.Core.Repositories
{
    public class StatisticalRepository
    {
        public static ZResponse StatisticalTransport(ZStatisticalSearchTransportRequest req)
        {

            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalTransport> list = new List<ZStatisticalTransport>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_TRANSPORT", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalTransport()
                                    {
                                        Status = dr["STATUS"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"])
                                    });
                                }
                            }
                            var listOrderStatus = XData.GetListOrderStatus().Where(m => m.Value != "N" && m.Value != "L" && m.Value != "C" && m.Value != "D");
                            List<string> listBorderColor = XData.GetStatisticalBorderColor();
                            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
                            ZStatisticalSearchTransportResponse dataResponse = new ZStatisticalSearchTransportResponse();
                            List<ZStatisticalTransportDataTable> listDataTable = new List<ZStatisticalTransportDataTable>();
                            List<ZStatisticalTransportDataChart> listDataChart = new List<ZStatisticalTransportDataChart>();
                            foreach (var item in listOrderStatus)
                            {
                                int rowindex = 0;
                                switch (item.Value)
                                {
                                    case "W":
                                        rowindex = 1;
                                        break;
                                    case "P":
                                        rowindex = 2;
                                        break;
                                    case "S":
                                        rowindex = 3;
                                        break;
                                    case "A":
                                        rowindex = 4;
                                        break;
                                    case "T":
                                        rowindex = 5;
                                        break;
                                    case "F":
                                        rowindex = 6;
                                        break;
                                    case "G":
                                        rowindex = 7;
                                        break;
                                    case "H":
                                        rowindex = 8;
                                        break;
                                    case "I":
                                        rowindex = 9;
                                        break;
                                    case "U":
                                        rowindex = 10;
                                        break;
                                    case "V":
                                        rowindex = 11;
                                        break;
                                    case "Z":
                                        rowindex = 12;
                                        break;
                                    default:
                                        break;
                                }
                                var Count = list.Where(m => m.Status == item.Value).Count();
                                var SumCOD = list.Where(m => m.Status == item.Value).Sum(m => m.TotalCOD);
                                var SumCharge = list.Where(m => m.Status == item.Value).Sum(m => m.TransportFee);
                                listDataTable.Add(new ZStatisticalTransportDataTable
                                {
                                    RowIndex = rowindex,
                                    AmountTracking = Count,
                                    Status = item.Value,
                                    StatusName = item.Text,
                                    TotalCOD = SumCOD,
                                    TotalCharge = SumCharge
                                });
                            }

                            foreach (var item in listOrderStatus.Where(m => m.Value != "W" && m.Value != "P"))
                            {
                                var TotalList = list.Where(m => m.Status != "W" && m.Status != "P").Count();
                                if (TotalList == 0)
                                {
                                    listDataChart.Add(new ZStatisticalTransportDataChart
                                    {
                                        RowIndex = 1,
                                        Status = "",
                                        StatusName = " ",
                                        Percent = 100,
                                        BorderColor = listBorderColor[0],
                                        BackgroundColor = listBackgroundColor[0]
                                    });
                                    break;
                                }
                                int rowindex = 0;
                                string borderColor = "";
                                string backgroundColor = "";
                                switch (item.Value)
                                {
                                    case "S":
                                    case "A":
                                    case "T":
                                        rowindex = 1;
                                        borderColor = listBorderColor[0];
                                        backgroundColor = listBackgroundColor[0];
                                        break;
                                    case "F":
                                        rowindex = 2;
                                        borderColor = listBorderColor[1];
                                        backgroundColor = listBackgroundColor[1];
                                        break;
                                    case "G":
                                        rowindex = 3;
                                        borderColor = listBorderColor[2];
                                        backgroundColor = listBackgroundColor[2];
                                        break;
                                    case "H":
                                        rowindex = 4;
                                        borderColor = listBorderColor[3];
                                        backgroundColor = listBackgroundColor[3];
                                        break;
                                    case "I":
                                        rowindex = 5;
                                        borderColor = listBorderColor[4];
                                        backgroundColor = listBackgroundColor[4];
                                        break;
                                    case "U":
                                    case "V":
                                    case "Z":
                                        rowindex = 6;
                                        borderColor = listBorderColor[5];
                                        backgroundColor = listBackgroundColor[5];
                                        break;
                                    default:
                                        break;
                                }

                                if (rowindex == 1 && listDataChart.Where(m => m.RowIndex == 1).Count() > 0)
                                {
                                    continue;
                                }

                                if (rowindex == 6 && listDataChart.Where(m => m.RowIndex == 6).Count() > 0)
                                {
                                    continue;
                                }

                                double CheckPercent = 0;
                                var Count = 0;
                                var CheckStatus = "";
                                var CheckStatusName = "";
                                switch (rowindex)
                                {
                                    case 1:
                                        Count = list.Where(m => m.Status == "S" || m.Status == "A" || m.Status == "T").Count();
                                        CheckPercent = Math.Round((double)(100 * Count / TotalList), 2);
                                        CheckStatus = "T";
                                        CheckStatusName = "Đang vận chuyển";
                                        break;
                                    case 6:
                                        Count = list.Where(m => m.Status == "U" || m.Status == "V" || m.Status == "Z").Count();
                                        CheckPercent = Math.Round((double)(100 * Count / TotalList), 2);
                                        CheckStatus = "U";
                                        CheckStatusName = "Chuyển hoàn";
                                        break;
                                    default:
                                        Count = list.Where(m => m.Status == item.Value).Count();
                                        CheckPercent = Math.Round((double)(100 * Count / TotalList), 2);
                                        CheckStatus = item.Value;
                                        CheckStatusName = item.Text;
                                        break;
                                }

                                listDataChart.Add(new ZStatisticalTransportDataChart
                                {
                                    RowIndex = rowindex,
                                    Status = CheckStatus,
                                    StatusName = CheckStatusName,
                                    Percent = CheckPercent,
                                    BorderColor = borderColor,
                                    BackgroundColor = backgroundColor
                                });
                            }

                            dataResponse = new ZStatisticalSearchTransportResponse()
                            {
                                DataTable = listDataTable.OrderBy(m => m.RowIndex).ToList(),
                                DataChart = listDataChart.OrderBy(m => m.RowIndex).ToList()
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalOrder(ZStatisticalSearchOrderRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchOrder> list = new List<ZStatisticalSearchOrder>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_ORDER", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalSearchOrder()
                                    {
                                        Status = dr["STATUS"].ToString(),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"]),
                                        TotalCharge = Convert.ToInt64(dr["TOTAL_CHARGE"])
                                    });
                                }
                            }
                            var listOrderStatus = XData.GetListOrderStatus().Where(m => m.Value != "C" && m.Value != "D");
                            ZStatisticalSearchOrderResponse dataResponse = new ZStatisticalSearchOrderResponse();
                            List<ZStatisticalOrderDataTable> listDataTable = new List<ZStatisticalOrderDataTable>();
                            List<ZStatisticalOrderDataChart> listDataChart = new List<ZStatisticalOrderDataChart>();
                            List<string> listBorderColor = XData.GetStatisticalBorderColor();
                            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
                            foreach (var item in listOrderStatus)
                            {

                                int rowindex = 0;
                                switch (item.Value)
                                {
                                    case "N":
                                        rowindex = 1;
                                        break;
                                    case "L":
                                        rowindex = 2;
                                        break;
                                    case "W":
                                        rowindex = 3;
                                        break;
                                    case "P":
                                        rowindex = 4;
                                        break;
                                    case "S":
                                        rowindex = 5;
                                        break;
                                    case "A":
                                        rowindex = 6;
                                        break;
                                    case "T":
                                        rowindex = 7;
                                        break;
                                    case "F":
                                        rowindex = 8;
                                        break;
                                    case "G":
                                        rowindex = 9;
                                        break;
                                    case "H":
                                        rowindex = 10;
                                        break;
                                    case "I":
                                        rowindex = 11;
                                        break;
                                    case "U":
                                        rowindex = 12;
                                        break;
                                    case "V":
                                        rowindex = 13;
                                        break;
                                    case "Z":
                                        rowindex = 14;
                                        break;
                                    default:
                                        break;
                                }
                                var Count = list.Where(m => m.Status == item.Value).Count();
                                var SumTotalOrderValue = list.Where(m => m.Status == item.Value).Sum(m => m.TotalOrderValue);
                                var SumTotalCharge = list.Where(m => m.Status == item.Value).Sum(m => m.TotalCharge);
                                listDataTable.Add(new ZStatisticalOrderDataTable
                                {
                                    RowIndex = rowindex,
                                    AmountOrder = Count,
                                    Status = item.Value,
                                    StatusName = item.Text,
                                    TotalOrderValue = SumTotalOrderValue,
                                    TotalCharge = SumTotalCharge
                                });
                            }
                            foreach (var item in listOrderStatus)
                            {
                                var TotalListNot_N = list.Where(m => m.Status != "N").Count();
                                if (TotalListNot_N == 0)
                                {
                                    listDataChart.Add(new ZStatisticalOrderDataChart
                                    {
                                        RowIndex = 1,
                                        Status = "",
                                        StatusName = " ",
                                        Percent = 100,
                                        BorderColor = listBorderColor[0],
                                        BackgroundColor = listBackgroundColor[0]
                                    });
                                    break;
                                }
                                int rowindex = 0;
                                string borderColor = "";
                                string backgroundColor = "";
                                switch (item.Value)
                                {
                                    case "N":
                                    case "W":
                                    case "P":
                                    case "S":
                                    case "A":
                                    case "T":
                                    case "F":
                                    case "G":
                                    case "H":
                                    case "I":
                                    case "U":
                                    case "V":
                                    case "Z":
                                        rowindex = 1;
                                        borderColor = listBorderColor[0];
                                        backgroundColor = listBackgroundColor[0];
                                        break;
                                    case "L":
                                        rowindex = 2;
                                        borderColor = listBorderColor[1];
                                        backgroundColor = listBackgroundColor[1];
                                        break;
                                    default:
                                        break;
                                }

                                if (rowindex == 1 && listDataChart.Where(m => m.RowIndex == 1).Count() > 0)
                                {
                                    continue;
                                }

                                if (rowindex == 2 && listDataChart.Where(m => m.RowIndex == 2).Count() > 0)
                                {
                                    continue;
                                }
                                double CheckPercent = 0;
                                var Count = 0;
                                var CheckStatus = "";
                                var CheckStatusName = "";
                                switch (rowindex)
                                {
                                    case 1:
                                        Count = list.Where(m => m.Status != "L" && m.Status != "N").Count();
                                        CheckPercent = Math.Round((double)(100 * Count / TotalListNot_N), 2);
                                        CheckStatus = "T";
                                        CheckStatusName = "Đang vận chuyển";
                                        break;
                                    case 2:
                                        Count = list.Where(m => m.Status == "L").Count();
                                        CheckPercent = Math.Round((double)(100 * Count / TotalListNot_N), 2);
                                        CheckStatus = "L";
                                        CheckStatusName = "Đơn tại cửa hàng";
                                        break;
                                    default:
                                        break;
                                }

                                listDataChart.Add(new ZStatisticalOrderDataChart
                                {
                                    RowIndex = rowindex,
                                    Status = CheckStatus,
                                    StatusName = CheckStatusName,
                                    Percent = CheckPercent,
                                    BorderColor = borderColor,
                                    BackgroundColor = backgroundColor
                                });
                            }

                            dataResponse = new ZStatisticalSearchOrderResponse()
                            {
                                DataTable = listDataTable.OrderBy(m => m.RowIndex).ToList(),
                                DataChart = listDataChart.OrderBy(m => m.RowIndex).ToList()
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalCustomer(ZStatisticalSearchCustomerRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColor();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
            List<ZStatisticalCustomerDataTable> list = new List<ZStatisticalCustomerDataTable>();
            ZStatisticalSearchCustomerResponse dataResponse = new ZStatisticalSearchCustomerResponse();
            List<ZStatisticalCustomerDataTable> listDataTable = new List<ZStatisticalCustomerDataTable>();
            List<ZStatisticalCustomerDataChart> listDataChart = new List<ZStatisticalCustomerDataChart>();
            List<ZStatisticalCustomerDataTable> listTemp = new List<ZStatisticalCustomerDataTable>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_CUSTOMER", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalCustomerDataTable()
                                    {
                                        ProvinceName = dr["NAME"].ToString(),
                                        TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"])
                                    });
                                }
                            }
                            list = list.OrderByDescending(m => m.TotalOrderProvince).ToList();
                            long SumTotalOrderValueList = list.Sum(m => m.TotalOrderValue) > 0 ? list.Sum(m => m.TotalOrderValue) : 1;
                            var i = 0;
                            long SumTotalOrderProvince = 0;
                            long SumTotalOrderValue = 0;
                            double SumPercentage = 0;
                            foreach (var item in list)
                            {
                                i = i + 1;
                                if (i <= 10)
                                {
                                    if (item.ProvinceName != "")
                                    {
                                        listDataTable.Add(new ZStatisticalCustomerDataTable()
                                        {
                                            ProvinceName = item.ProvinceName,
                                            TotalOrderProvince = item.TotalOrderProvince,
                                            TotalOrderValue = item.TotalOrderValue,
                                            Percentage = Math.Round((double)(100 * item.TotalOrderValue) / SumTotalOrderValueList, 2)
                                        });
                                    }
                                    else
                                    {
                                        listTemp.Add(new ZStatisticalCustomerDataTable()
                                        {
                                            ProvinceName = item.ProvinceName,
                                            TotalOrderProvince = item.TotalOrderProvince,
                                            TotalOrderValue = item.TotalOrderValue,
                                            Percentage = Math.Round((double)(100 * item.TotalOrderValue) / SumTotalOrderValueList, 2)
                                        });
                                    }
                                }
                                else
                                {
                                    listTemp.Add(new ZStatisticalCustomerDataTable()
                                    {
                                        ProvinceName = item.ProvinceName,
                                        TotalOrderProvince = item.TotalOrderProvince,
                                        TotalOrderValue = item.TotalOrderValue,
                                        Percentage = Math.Round((double)(100 * item.TotalOrderValue) / SumTotalOrderValueList, 2)
                                    });
                                }
                            }
                            if (list.Count > 10)
                            {
                                if (list.Where(m => m.ProvinceName != "").ToList().Count > 10)
                                {
                                    foreach (var item in listTemp)
                                    {
                                        SumTotalOrderProvince = SumTotalOrderProvince + item.TotalOrderProvince;
                                        SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                        SumPercentage = Math.Round((double)(SumPercentage + item.Percentage), 2);
                                    }
                                    listDataTable.Add(new ZStatisticalCustomerDataTable()
                                    {
                                        ProvinceName = "Khác",
                                        TotalOrderProvince = SumTotalOrderProvince,
                                        TotalOrderValue = SumTotalOrderValue,
                                        Percentage = SumPercentage
                                    });
                                }
                                else
                                {
                                    if (listDataTable.Count < 10)
                                    {
                                        for (int j = listDataTable.Count; j <= 10; j++)
                                        {
                                            if (listDataTable.Count + 1 <= 10)
                                            {
                                                listDataTable.Add(new ZStatisticalCustomerDataTable()
                                                {
                                                    ProvinceName = "...",
                                                    TotalOrderProvince = 0,
                                                    TotalOrderValue = 0,
                                                    Percentage = 0
                                                });
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        foreach (var item in listTemp)
                                        {
                                            SumTotalOrderProvince = SumTotalOrderProvince + item.TotalOrderProvince;
                                            SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                            SumPercentage = Math.Round((double)(SumPercentage + item.Percentage), 2);
                                        }
                                        listDataTable.Add(new ZStatisticalCustomerDataTable()
                                        {
                                            ProvinceName = "Khác",
                                            TotalOrderProvince = SumTotalOrderProvince,
                                            TotalOrderValue = SumTotalOrderValue,
                                            Percentage = SumPercentage
                                        });
                                    }
                                    else
                                    {
                                        foreach (var item in listTemp)
                                        {
                                            SumTotalOrderProvince = SumTotalOrderProvince + item.TotalOrderProvince;
                                            SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                            SumPercentage = Math.Round((double)(SumPercentage + item.Percentage), 2);
                                        }
                                        listDataTable.Add(new ZStatisticalCustomerDataTable()
                                        {
                                            ProvinceName = "Khác",
                                            TotalOrderProvince = SumTotalOrderProvince,
                                            TotalOrderValue = SumTotalOrderValue,
                                            Percentage = SumPercentage
                                        });
                                    }
                                }
                            }
                            else
                            {
                                if (listTemp.Where(m => m.ProvinceName != "").ToList().Count > 0)
                                {
                                    var CountList = list.Count();
                                    for (int j = CountList; j < 10; j++)
                                    {
                                        if (CountList + 1 <= 10)
                                        {
                                            listDataTable.Add(new ZStatisticalCustomerDataTable()
                                            {
                                                ProvinceName = "...",
                                                TotalOrderProvince = 0,
                                                TotalOrderValue = 0,
                                                Percentage = 0
                                            });
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    foreach (var item in listTemp)
                                    {
                                        SumTotalOrderProvince = SumTotalOrderProvince + item.TotalOrderProvince;
                                        SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                        SumPercentage = Math.Round((double)(SumPercentage + item.Percentage), 2);
                                    }
                                    listDataTable.Add(new ZStatisticalCustomerDataTable()
                                    {
                                        ProvinceName = "Khác",
                                        TotalOrderProvince = SumTotalOrderProvince,
                                        TotalOrderValue = SumTotalOrderValue,
                                        Percentage = SumPercentage
                                    });
                                }
                                else
                                {
                                    var CountList = list.Count();
                                    for (int j = CountList; j < 10; j++)
                                    {
                                        if (CountList + 1 <= 10)
                                        {
                                            listDataTable.Add(new ZStatisticalCustomerDataTable()
                                            {
                                                ProvinceName = "...",
                                                TotalOrderProvince = 0,
                                                TotalOrderValue = 0,
                                                Percentage = 0
                                            });
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    listDataTable.Add(new ZStatisticalCustomerDataTable()
                                    {
                                        ProvinceName = "Khác",
                                        TotalOrderProvince = 0,
                                        TotalOrderValue = 0,
                                        Percentage = 0
                                    });
                                }
                            }
                            if (list.Count == 0)
                            {
                                listDataChart.Add(new ZStatisticalCustomerDataChart()
                                {
                                    ProvinceName = " ",
                                    TotalOrderProvince = 0,
                                    Percentage = 100,
                                    BorderColor = listBorderColor[0],
                                    BackgroundColor = listBackgroundColor[0]
                                });
                            }
                            else
                            {
                                var iColor = -1;
                                foreach (var item in listDataTable)
                                {
                                    iColor = iColor + 1;
                                    listDataChart.Add(new ZStatisticalCustomerDataChart()
                                    {
                                        ProvinceName = item.ProvinceName,
                                        TotalOrderProvince = item.TotalOrderProvince,
                                        Percentage = item.Percentage,
                                        BorderColor = listBorderColor[iColor],
                                        BackgroundColor = listBackgroundColor[iColor]
                                    });
                                }
                            }
                            dataResponse = new ZStatisticalSearchCustomerResponse()
                            {
                                DataTable = listDataTable,
                                DataChart = listDataChart
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalCustomer: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalCashflow(ZStatisticalSearchCashflowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchCashflow> list = new List<ZStatisticalSearchCashflow>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_CASHFLOW", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalSearchCashflow()
                                    {
                                        Status = dr["STATUS"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TotalTransportFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"])
                                    });
                                }
                            }

                            var listOrderStatus = XData.GetListOrderStatus().Where(m => m.Value != "N" && m.Value != "L" && m.Value != "C" && m.Value != "D" && m.Value != "W" && m.Value != "P");
                            List<string> listBorderColor = XData.GetStatisticalBorderColor();
                            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
                            ZStatisticalSearchCashflowResponse dataResponse = new ZStatisticalSearchCashflowResponse();
                            List<ZStatisticalCashflowDataTable> listDataTable = new List<ZStatisticalCashflowDataTable>();
                            List<ZStatisticalCashflowDataChart> listDataChart = new List<ZStatisticalCashflowDataChart>();
                            foreach (var item in listOrderStatus)
                            {
                                var TotalSumCOD = list.Sum(m => m.TotalCOD) > 0 ? list.Sum(m => m.TotalCOD) : 1;
                                int rowindex = 0;
                                string borderColor = "";
                                string backgroundColor = "";
                                switch (item.Value)
                                {
                                    case "S":
                                    case "A":
                                    case "T":
                                        rowindex = 1;
                                        borderColor = listBorderColor[0];
                                        backgroundColor = listBackgroundColor[0];
                                        break;
                                    case "F":
                                        rowindex = 2;
                                        borderColor = listBorderColor[1];
                                        backgroundColor = listBackgroundColor[1];
                                        break;
                                    case "G":
                                        rowindex = 3;
                                        borderColor = listBorderColor[2];
                                        backgroundColor = listBackgroundColor[2];
                                        break;
                                    case "H":
                                        rowindex = 4;
                                        borderColor = listBorderColor[3];
                                        backgroundColor = listBackgroundColor[3];
                                        break;
                                    case "I":
                                        rowindex = 5;
                                        borderColor = listBorderColor[4];
                                        backgroundColor = listBackgroundColor[4];
                                        break;
                                    case "U":
                                    case "V":
                                    case "Z":
                                        rowindex = 6;
                                        borderColor = listBorderColor[5];
                                        backgroundColor = listBackgroundColor[5];
                                        break;
                                    default:
                                        break;
                                }

                                if (rowindex == 1 && listDataTable.Where(m => m.RowIndex == 1).Count() > 0)
                                {
                                    continue;
                                }

                                if (rowindex == 6 && listDataTable.Where(m => m.RowIndex == 6).Count() > 0)
                                {
                                    continue;
                                }

                                double CheckPercent = 0;
                                var Amount = 0;
                                var CheckStatus = "";
                                var CheckStatusName = "";
                                long SumCOD = 0;
                                long SumTransportFee = 0;
                                switch (rowindex)
                                {
                                    case 1:
                                        Amount = list.Where(m => m.Status == "S" || m.Status == "A" || m.Status == "T").Count();
                                        SumCOD = list.Where(m => m.Status == "S" || m.Status == "A" || m.Status == "T").Sum(m => m.TotalCOD);
                                        SumTransportFee = list.Where(m => m.Status == "S" || m.Status == "A" || m.Status == "T").Sum(m => m.TotalTransportFee);
                                        CheckPercent = Math.Round((double)(100 * SumCOD) / TotalSumCOD, 2);
                                        CheckStatus = "T";
                                        CheckStatusName = "Đang vận chuyển";
                                        break;
                                    case 6:
                                        Amount = list.Where(m => m.Status == "U" || m.Status == "V" || m.Status == "Z").Count();
                                        SumCOD = list.Where(m => m.Status == "U" || m.Status == "V" || m.Status == "Z").Sum(m => m.TotalCOD);
                                        SumTransportFee = list.Where(m => m.Status == "U" || m.Status == "V" || m.Status == "Z").Sum(m => m.TotalTransportFee);
                                        CheckPercent = Math.Round((double)(100 * SumCOD) / TotalSumCOD, 2);
                                        CheckStatus = "U";
                                        CheckStatusName = "Chuyển hoàn";
                                        break;
                                    default:
                                        Amount = list.Where(m => m.Status == item.Value).Count();
                                        SumCOD = list.Where(m => m.Status == item.Value).Sum(m => m.TotalCOD);
                                        SumTransportFee = list.Where(m => m.Status == item.Value).Sum(m => m.TotalTransportFee);
                                        CheckPercent = Math.Round((double)(100 * SumCOD) / TotalSumCOD, 2);
                                        CheckStatus = item.Value;
                                        CheckStatusName = XData.GetNameOrderStatusCashflow(item.Value);
                                        break;
                                }
                                listDataTable.Add(new ZStatisticalCashflowDataTable
                                {
                                    RowIndex = rowindex,
                                    Status = CheckStatus,
                                    StatusName = CheckStatusName,
                                    Amount = Amount,
                                    TotalCOD = SumCOD,
                                    TotalTransportFee = SumTransportFee,
                                    PercentCOD = CheckPercent
                                });

                                listDataChart.Add(new ZStatisticalCashflowDataChart
                                {
                                    RowIndex = rowindex,
                                    Status = CheckStatus,
                                    StatusName = CheckStatusName,
                                    PercentCOD = CheckPercent,
                                    TotalCOD = SumCOD,
                                    BorderColor = borderColor,
                                    BackgroundColor = backgroundColor
                                });
                            }
                            if (listDataChart.Sum(m => m.PercentCOD) == 0)
                            {
                                listDataChart = new List<ZStatisticalCashflowDataChart>();
                                listDataChart.Add(new ZStatisticalCashflowDataChart
                                {
                                    RowIndex = 0,
                                    Status = " ",
                                    StatusName = " ",
                                    PercentCOD = 100,
                                    TotalCOD = 0,
                                    BorderColor = listBorderColor[0],
                                    BackgroundColor = listBackgroundColor[0]
                                });
                            }
                            dataResponse = new ZStatisticalSearchCashflowResponse()
                            {
                                DataTable = listDataTable.OrderBy(m => m.RowIndex).ToList(),
                                DataChart = listDataChart.OrderBy(m => m.RowIndex).ToList()
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalCashflow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalProduct(ZStatisticalSearchProductRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColor();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
            List<ZStatisticalProductDataTable> list = new List<ZStatisticalProductDataTable>();
            ZStatisticalSearchProductResponse dataResponse = new ZStatisticalSearchProductResponse();
            List<ZStatisticalProductDataTable> listDataTable = new List<ZStatisticalProductDataTable>();
            List<ZStatisticalProductDataChart> listDataChart = new List<ZStatisticalProductDataChart>();
            List<ZStatisticalProductDataTable> listTemp = new List<ZStatisticalProductDataTable>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_PRODUCT", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalProductDataTable()
                                    {
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"])
                                    });
                                }
                            }
                            long SumTotalOrderValueList = list.Sum(m => m.TotalOrderValue) > 0 ? list.Sum(m => m.TotalOrderValue) : 1;
                            var i = 0;
                            long SumTotalProduct = 0;
                            long SumTotalOrderValue = 0;
                            double SumPercentage = 0;
                            list = list.OrderByDescending(m => m.TotalProduct).ToList();
                            foreach (var item in list)
                            {
                                if (item.ProductCode != "")
                                {
                                    i = i + 1;
                                    if (i <= 10)
                                    {
                                        listDataTable.Add(new ZStatisticalProductDataTable()
                                        {
                                            ProductCode = item.ProductCode,
                                            TotalProduct = item.TotalProduct,
                                            TotalOrderValue = item.TotalOrderValue,
                                            Percentage = Math.Round((double)(100 * item.TotalOrderValue) / SumTotalOrderValueList, 2)
                                        });
                                    }
                                    else
                                    {
                                        listTemp.Add(new ZStatisticalProductDataTable()
                                        {
                                            ProductCode = item.ProductCode,
                                            TotalProduct = item.TotalProduct,
                                            TotalOrderValue = item.TotalOrderValue,
                                            Percentage = Math.Round((double)(100 * item.TotalOrderValue) / SumTotalOrderValueList, 2)
                                        });
                                    }
                                }
                                else
                                {
                                    SumTotalProduct = SumTotalProduct + item.TotalProduct;
                                    SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                    SumPercentage = SumPercentage + item.Percentage;
                                }
                            }

                            if (listTemp.Count > 0)
                            {
                                foreach (var item in listTemp)
                                {
                                    SumTotalProduct = SumTotalProduct + item.TotalProduct;
                                    SumTotalOrderValue = SumTotalOrderValue + item.TotalOrderValue;
                                    SumPercentage = Math.Round((double)SumPercentage + item.Percentage, 2);
                                }
                                list = list.OrderByDescending(m => m.TotalProduct).ToList();
                                listDataTable.Add(new ZStatisticalProductDataTable()
                                {
                                    ProductCode = "Khác",
                                    TotalProduct = SumTotalProduct,
                                    TotalOrderValue = SumTotalOrderValue,
                                    Percentage = SumPercentage
                                });
                            }
                            else
                            {
                                if (list.Count > 0)
                                {
                                    list = list.OrderByDescending(m => m.TotalProduct).ToList();
                                    var CountList = list.Count();
                                    for (int j = CountList; j < 10; j++)
                                    {
                                        if (CountList + 1 <= 10)
                                        {
                                            listDataTable.Add(new ZStatisticalProductDataTable()
                                            {
                                                ProductCode = "...",
                                                TotalProduct = 0,
                                                TotalOrderValue = 0,
                                                Percentage = 0
                                            });
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (SumTotalProduct != 0 && SumTotalOrderValue != 0 && SumPercentage != 0)
                                    {
                                        listDataTable.Add(new ZStatisticalProductDataTable()
                                        {
                                            ProductCode = "Khác",
                                            TotalProduct = SumTotalProduct,
                                            TotalOrderValue = SumTotalOrderValue,
                                            Percentage = SumPercentage
                                        });
                                    }
                                    else
                                    {
                                        listDataTable.Add(new ZStatisticalProductDataTable()
                                        {
                                            ProductCode = "Khác",
                                            TotalProduct = 0,
                                            TotalOrderValue = 0,
                                            Percentage = 0
                                        });
                                    }
                                    var iColor = -1;
                                    foreach (var item in listDataTable)
                                    {
                                        iColor = iColor + 1;
                                        listDataChart.Add(new ZStatisticalProductDataChart()
                                        {
                                            ProductCode = item.ProductCode,
                                            TotalProduct = item.TotalProduct,
                                            Percentage = item.Percentage,
                                            BorderColor = listBorderColor[iColor],
                                            BackgroundColor = listBackgroundColor[iColor]
                                        });
                                    }
                                    if (listDataTable.Count == 0)
                                    {
                                        listDataChart.Add(new ZStatisticalProductDataChart()
                                        {
                                            ProductCode = " ",
                                            TotalProduct = 0,
                                            Percentage = 100,
                                            BorderColor = listBorderColor[0],
                                            BackgroundColor = listBackgroundColor[0]
                                        });
                                    }
                                }
                                else
                                {
                                    var CountList = list.Count();
                                    for (int j = CountList; j < 10; j++)
                                    {
                                        if (CountList + 1 <= 10)
                                        {
                                            listDataTable.Add(new ZStatisticalProductDataTable()
                                            {
                                                ProductCode = "...",
                                                TotalProduct = 0,
                                                TotalOrderValue = 0,
                                                Percentage = 0
                                            });
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    listDataTable.Add(new ZStatisticalProductDataTable()
                                    {
                                        ProductCode = "Khác",
                                        TotalProduct = 0,
                                        TotalOrderValue = 0,
                                        Percentage = 0
                                    });
                                    listDataChart.Add(new ZStatisticalProductDataChart()
                                    {
                                        ProductCode = " ",
                                        TotalProduct = 0,
                                        Percentage = 100,
                                        BorderColor = listBorderColor[0],
                                        BackgroundColor = listBackgroundColor[0]
                                    });
                                }
                            }

                            dataResponse = new ZStatisticalSearchProductResponse()
                            {
                                DataTable = listDataTable,
                                DataChart = listDataChart
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalCustomer: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse TurnoverLable(ZTurnoverRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZTurnover data = new ZTurnover();
            List<ZTurnoverResponse> dataResponse = new List<ZTurnoverResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_TURNOVER", new object[]
                {
                    req.Date,
                    req.PPCRMCode,
                    req.ShopID,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    data = new ZTurnover()
                                    {
                                        TurnoverToday = Convert.ToInt64(dr["TURNOVER_TODAY"]),
                                        TurnoverWeek = Convert.ToInt64(dr["TURNOVER_WEEK"]),
                                        TurnoverMonth = Convert.ToInt64(dr["TURNOVER_MONTH"])
                                    };
                                }
                            }
                            dataResponse.Add(new ZTurnoverResponse()
                            {
                                Type = "TURNOVER_TODAY",
                                Lable = DateTime.Now.ToString("dd/MM/yyyy"),
                                Value = data.TurnoverToday
                            });
                            string fromDate = "";
                            string toDate = "";
                            DateTime today = DateTime.Now;
                            string checkToday = today.DayOfWeek.ToString();
                            if (checkToday == "Sunday")
                            {
                                toDate = DateTime.Now.ToString("dd/MM/yyyy");
                                fromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                List<string> listofWeek = new List<string>();
                                DayOfWeek day = DateTime.Now.DayOfWeek;
                                int days = day - DayOfWeek.Monday;
                                DateTime start = DateTime.Now.AddDays(-days);
                                DateTime end = start.AddDays(6);
                                for (int i = 0; i < 7; i++)
                                {
                                    if (i == 0)
                                    {
                                        fromDate = start.AddDays(i).ToString("dd/MM/yyyy");
                                    }
                                    else if (i == 6)
                                    {
                                        toDate = start.AddDays(i).ToString("dd/MM/yyyy");
                                    }
                                    listofWeek.Add(start.AddDays(i).ToString("dd/MM/yyyy"));
                                }
                            }
                            dataResponse.Add(new ZTurnoverResponse()
                            {
                                Type = "TURNOVER_WEEK",
                                Lable = fromDate + " - " + toDate,
                                Value = data.TurnoverWeek
                            });
                            int Month = DateTime.Now.Month;
                            dataResponse.Add(new ZTurnoverResponse()
                            {
                                Type = "TURNOVER_MONTH",
                                Lable = Month + "",
                                Value = data.TurnoverMonth
                            });
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TurnoverLable: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalTurnover(ZStatisticalSearchTurnoverRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColor();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColor();
            List<ZStatisticalTurnoverDataChart> list = new List<ZStatisticalTurnoverDataChart>();
            ZStatisticalSearchTurnoverResponse dataResponse = new ZStatisticalSearchTurnoverResponse();
            List<ZStatisticalTurnoverDataChart> listDataChart = new List<ZStatisticalTurnoverDataChart>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_TURNOVER", new object[]
                {
                    req.PPCRMCode,
                    req.ShopID,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZStatisticalTurnoverDataChart()
                                    {
                                        Date = dr["CREATED_DATE"].ToString(),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"])
                                    });
                                }
                            }
                            List<string> listofWeek = new List<string>();
                            DayOfWeek day = DateTime.Now.DayOfWeek;
                            int days = day - DayOfWeek.Monday;
                            DateTime start = DateTime.Now.AddDays(-days);
                            DateTime end = start.AddDays(6);
                            string isCheckFromDate = "";
                            string isCheckToDate = "";
                            for (int i = 0; i < 7; i++)
                            {
                                if (i == 0)
                                {
                                    isCheckFromDate = start.AddDays(i).ToString("dd/MM/yyyy");
                                }
                                else if (i == 6)
                                {
                                    isCheckToDate = start.AddDays(i).ToString("dd/MM/yyyy");
                                }
                                listofWeek.Add(start.AddDays(i).ToString("dd/MM/yyyy"));
                            }
                            List<ZStatisticalTurnoverDataChart> ListA = new List<ZStatisticalTurnoverDataChart>();
                            if (isCheckFromDate == req.FromDate && req.ToDate == isCheckToDate)
                            {
                                int iColor = -1;
                                foreach (var item in list)
                                {
                                    iColor = iColor + 1;
                                    listDataChart.Add(new ZStatisticalTurnoverDataChart()
                                    {
                                        Date = item.Date,
                                        TotalOrderValue = item.TotalOrderValue,
                                        BorderColor = listBorderColor[iColor],
                                        BackgroundColor = listBackgroundColor[iColor]
                                    });
                                }
                                if (list.Count == 0)
                                {
                                    iColor = iColor + 1;
                                }
                                int rowindex = 0;
                                foreach (var i in listofWeek)
                                {
                                    rowindex = rowindex + 1;
                                    var check = list.Where(m => m.Date == i).Count();
                                    if (check == 0)
                                    {
                                        listDataChart.Add(new ZStatisticalTurnoverDataChart()
                                        {
                                            RowIndex = rowindex,
                                            Date = i,
                                            TotalOrderValue = 0,
                                            BorderColor = listBorderColor[iColor],
                                            BackgroundColor = listBackgroundColor[iColor]
                                        });
                                    }
                                    else
                                    {
                                        for (int j = 0; j < listDataChart.Count; j++)
                                        {
                                            if (i == listDataChart[j].Date)
                                            {
                                                listDataChart[j].RowIndex = rowindex;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                List<string> allDates = new List<string>();
                                for (DateTime date = Convert.ToDateTime(req.FromDate, new CultureInfo("fr-FR")); date <= Convert.ToDateTime(req.ToDate, new CultureInfo("fr-FR")); date = date.AddDays(1))
                                {
                                    allDates.Add(date.ToString("dd/MM/yyyy"));
                                }
                                int iColor = -1;
                                foreach (var item in list)
                                {
                                    iColor = iColor + 1;
                                    listDataChart.Add(new ZStatisticalTurnoverDataChart()
                                    {
                                        Date = item.Date,
                                        TotalOrderValue = item.TotalOrderValue,
                                        BorderColor = listBorderColor[iColor],
                                        BackgroundColor = listBackgroundColor[iColor]
                                    });
                                }
                                if (list.Count == 0)
                                {
                                    iColor = iColor + 1;
                                }
                                int rowindex = 0;
                                foreach (var i in allDates)
                                {
                                    rowindex = rowindex + 1;
                                    var check = list.Where(m => m.Date == i).Count();
                                    if (check == 0)
                                    {
                                        listDataChart.Add(new ZStatisticalTurnoverDataChart()
                                        {
                                            RowIndex = rowindex,
                                            Date = i,
                                            TotalOrderValue = 0,
                                            BorderColor = listBorderColor[iColor],
                                            BackgroundColor = listBackgroundColor[iColor]
                                        });
                                    }
                                    else
                                    {
                                        for (int j = 0; j < listDataChart.Count; j++)
                                        {
                                            if (i == listDataChart[j].Date)
                                            {
                                                listDataChart[j].RowIndex = rowindex;
                                            }
                                        }
                                    }
                                }
                            }

                            List<ZTurnoverResponse> turnoverData = new List<ZTurnoverResponse>();
                            var turnoverRes = TurnoverLable(new ZTurnoverRequest()
                            {
                                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                                PPCRMCode = req.PPCRMCode,
                                ShopID = req.ShopID
                            });
                            if (turnoverRes.Code == "00")
                            {
                                var data = JsonConvert.DeserializeObject<List<ZTurnoverResponse>>(turnoverRes.Data);
                                foreach (var item in data)
                                {
                                    turnoverData.Add(new ZTurnoverResponse()
                                    {
                                        Type = item.Type,
                                        Lable = item.Lable,
                                        Value = item.Value
                                    });
                                }
                            }
                            else
                            {
                                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalTurnover.turnoverData: " + turnoverRes.Message);
                            }
                            dataResponse = new ZStatisticalSearchTurnoverResponse()
                            {
                                DataLable = turnoverData,
                                DataChart = listDataChart.OrderBy(m => m.RowIndex).ToList()
                            };
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(dataResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalTurnover: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse StatisticalRequestOrder(ZRequestOrderDashboardRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZRequestOrderDashboard> list = new List<ZRequestOrderDashboard>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_NEW_PKG.LIST_STATISTICAL_REQ_ORDER", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    list.Add(new ZRequestOrderDashboard()
                                    {
                                        Status = Convert.ToInt32(dr["STATUS"])
                                    });
                                }
                            }
                            var listRequestOrderStatus = XData.GetListRequestOrderStatus();
                            var Count = list.Count() == 0 ? 1 : list.Count();
                            List<string> listBorderColor = XData.GetStatisticalReqOrderBorderColor();
                            List<string> listBackgroundColor = XData.GetStatisticalReqOrderBackgroundColor();
                            List<ZRequestOrderDashboardResponse> listProcess = new List<ZRequestOrderDashboardResponse>();
                            List<ZRequestOrderDashboardResponse> listResponse = new List<ZRequestOrderDashboardResponse>();
                            foreach (var item in listRequestOrderStatus)
                            {
                                var Amount = list.Where(m => m.Status == Convert.ToInt32(item.Value)).Count();
                                double percentComplete = Math.Round((double)(100 * Amount) / Count, 2);
                                var temp = Convert.ToDouble(Amount / Count) * 100;
                                listProcess.Add(new ZRequestOrderDashboardResponse
                                {
                                    Lable = item.Text,
                                    Status = Convert.ToInt32(item.Value),
                                    Amount = Amount,
                                    AmountPerCent = percentComplete
                                });
                            }
                            int check_a = 0;
                            double percent_plus = 0;
                            int length = listProcess.Count();
                            foreach (var itemProcess in listProcess)
                            {
                                check_a = check_a + 1;
                                if (check_a < length)
                                {
                                    if (itemProcess.Status == 0)
                                    {
                                        percent_plus = percent_plus + itemProcess.AmountPerCent;
                                        listResponse.Add(new ZRequestOrderDashboardResponse()
                                        {
                                            Lable = itemProcess.Lable,
                                            Status = itemProcess.Status,
                                            Amount = itemProcess.Amount,
                                            AmountPerCent = itemProcess.AmountPerCent,
                                            BorderColor = listBorderColor[0],
                                            BackgroundColor = listBackgroundColor[0]
                                        });
                                    }
                                    else if (itemProcess.Status == 1)
                                    {
                                        percent_plus = percent_plus + itemProcess.AmountPerCent;
                                        listResponse.Add(new ZRequestOrderDashboardResponse()
                                        {
                                            Lable = itemProcess.Lable,
                                            Status = itemProcess.Status,
                                            Amount = itemProcess.Amount,
                                            AmountPerCent = itemProcess.AmountPerCent,
                                            BorderColor = listBorderColor[1],
                                            BackgroundColor = listBackgroundColor[1]
                                        });
                                    }
                                }
                                else if (check_a == length)
                                {
                                    if (itemProcess.Status == 2)
                                    {
                                        if (percent_plus == 0)
                                        {
                                            listResponse.Add(new ZRequestOrderDashboardResponse()
                                            {
                                                Lable = itemProcess.Lable,
                                                Status = itemProcess.Status,
                                                Amount = itemProcess.Amount,
                                                AmountPerCent = itemProcess.AmountPerCent,
                                                BorderColor = listBorderColor[2],
                                                BackgroundColor = listBackgroundColor[2]
                                            });
                                        }
                                        else if (percent_plus != 0)
                                        {
                                            listResponse.Add(new ZRequestOrderDashboardResponse()
                                            {
                                                Lable = itemProcess.Lable,
                                                Status = itemProcess.Status,
                                                Amount = itemProcess.Amount,
                                                AmountPerCent = Math.Round(100 - (percent_plus), 2),
                                                BorderColor = listBorderColor[2],
                                                BackgroundColor = listBackgroundColor[2]
                                            });
                                        }
                                    }
                                }
                            }

                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(listResponse);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalRequestOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        // Version 2

        #region StatisticalWeb
        public static ZResponse StatisticalWeb(ZStatisticalDashboardWebRequest req)
        {
            ZResponse res = new ZResponse();
            ZStatisticalDashboardWebResponse DashboardWeb = new ZStatisticalDashboardWebResponse();
            try
            {
                DashboardWeb = new ZStatisticalDashboardWebResponse()
                {
                    TotalTurnover = TotalTurnoverWeb(req),
                    Cashflow = CashflowWeb(req),
                    Order = OrderWeb(req),
                    Product = new ZStatisticalProductWeb()
                    {
                        ProductSelling = ProductSellingWeb(req),
                        ProductRefund = ProductRefundWeb(req)
                    },
                    Customer = new ZStatisticalCustomerWeb()
                    {
                        CustomerSelling = CustomerSellingWeb(req),
                        CustomerRefund = CustomerRefundWeb(req)
                    },
                    DeliveryTransport = DeliveryTransportWeb(req)
                };
                res.Code = "00";
                res.Message = "Lấy dữ liệu thành công";
                res.Data = JsonConvert.SerializeObject(DashboardWeb);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalWeb: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static long TotalTurnoverWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            long totalTurnover = 0;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_TURNOVER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                totalTurnover = dr["TOTAL_TURNOVER"].ToString() == "" ? 0 : Convert.ToInt64(dr["TOTAL_TURNOVER"]);
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverWeb: " + ex.Message);
            }
            return totalTurnover;
        }
        public static List<ZStatisticalSearchCashflowWeb> CashflowWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchCashflowWeb> listCashflow = new List<ZStatisticalSearchCashflowWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_CASHFLOW", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCashflow.Add(new ZStatisticalSearchCashflowWeb()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatusCashflow(dr["STATUS"].ToString()),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                    TotalTransportFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"])
                                });
                            }
                        }
                        listCashflow = listCashflow.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowWeb: " + ex.Message);
            }
            return listCashflow;
        }
        public static List<ZStatisticalSearchOrderWeb> OrderWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchOrderWeb> listOrder = new List<ZStatisticalSearchOrderWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_ORDER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listOrder.Add(new ZStatisticalSearchOrderWeb()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                    Quantity = Convert.ToInt64(dr["QUANTITY"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                });
                            }
                        }
                        listOrder = listOrder.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderWeb: " + ex.Message);
            }
            return listOrder;
        }
        public static List<ZStatisticalSearchProductSellingWeb> ProductSellingWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductSellingWeb> listProductSelling = new List<ZStatisticalSearchProductSellingWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_PRODUCT_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductSelling.Add(new ZStatisticalSearchProductSellingWeb()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductSelling = listProductSelling.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingWeb: " + ex.Message);
            }
            return listProductSelling;
        }
        public static List<ZStatisticalSearchProductRefundWeb> ProductRefundWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductRefundWeb> listProductRefund = new List<ZStatisticalSearchProductRefundWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_PRODUCT_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductRefund.Add(new ZStatisticalSearchProductRefundWeb()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductRefund = listProductRefund.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundWeb: " + ex.Message);
            }
            return listProductRefund;
        }
        public static List<ZStatisticalSearchCustomerSellingWeb> CustomerSellingWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerSellingWeb> listCustomerSelling = new List<ZStatisticalSearchCustomerSellingWeb>();
            List<ZStatisticalSearchCustomerSellingWeb> listCustomerSellingAll = new List<ZStatisticalSearchCustomerSellingWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_CUSTOMER_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerSelling.Add(new ZStatisticalSearchCustomerSellingWeb()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerSelling.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerSelling.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerSelling)
                        {
                            iColor = iColor + 1;
                            listCustomerSellingAll.Add(new ZStatisticalSearchCustomerSellingWeb()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerSellingAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerSellingAll[0].Percentage = 100;
                        }
                        listCustomerSellingAll = listCustomerSellingAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingWeb: " + ex.Message);
            }
            return listCustomerSellingAll;
        }
        public static List<ZStatisticalSearchCustomerRefundWeb> CustomerRefundWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerRefundWeb> listCustomerRefund = new List<ZStatisticalSearchCustomerRefundWeb>();
            List<ZStatisticalSearchCustomerRefundWeb> listCustomerRefundAll = new List<ZStatisticalSearchCustomerRefundWeb>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_CUSTOMER_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerRefund.Add(new ZStatisticalSearchCustomerRefundWeb()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerRefund.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerRefund.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerRefund)
                        {
                            iColor = iColor + 1;
                            listCustomerRefundAll.Add(new ZStatisticalSearchCustomerRefundWeb()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerRefundAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerRefundAll[0].Percentage = 100;
                        }
                        listCustomerRefundAll = listCustomerRefundAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundWeb: " + ex.Message);
            }
            return listCustomerRefundAll;
        }
        public static List<ZStatisticalDeliveryTransportWeb> DeliveryTransportWeb(ZStatisticalDashboardWebRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalDeliveryTransportWeb> list = new List<ZStatisticalDeliveryTransportWeb>();
            List<ZStatisticalDeliveryTransportWeb> listDeliveryTransport = new List<ZStatisticalDeliveryTransportWeb>();
            List<string> allDates = new List<string>();
            for (DateTime date = Convert.ToDateTime(req.FromDate, new CultureInfo("fr-FR")); date <= Convert.ToDateTime(req.ToDate, new CultureInfo("fr-FR")); date = date.AddDays(1))
            {
                allDates.Add(date.ToString("dd/MM/yyyy"));
            }
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_WEB_PKG.STATISTICAL_DELIVERY_TRANSPORT", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                list.Add(new ZStatisticalDeliveryTransportWeb()
                                {
                                    CreatedDate = dr["CREATED_DATE"].ToString(),
                                    Status_G = "G",
                                    StatusName_G = "Giao thành công",
                                    TotalOrder_G = Convert.ToInt64(dr["QUANTITY_G"]),
                                    TotalCOD_G = Convert.ToInt64(dr["TOTAL_COD_G"]),
                                    Status_F = "F",
                                    StatusName_F = "Giao thất bại",
                                    TotalOrder_F = Convert.ToInt32(dr["QUANTITY_F"]),
                                    TotalCOD_F = Convert.ToInt64(dr["TOTAL_COD_F"])
                                });
                            }
                        }
                        foreach (var d in allDates)
                        {
                            if (listDeliveryTransport.Where(m => m.CreatedDate == d).ToList().Count == 0)
                            {
                                if (list.Where(m => m.CreatedDate == d).ToList().Count > 0)
                                {
                                    var listTemp = list.Where(m => m.CreatedDate == d).ToList();
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportWeb
                                    {
                                        CreatedDate = d,
                                        Status_G = listTemp[0].Status_G,
                                        StatusName_G = listTemp[0].StatusName_G,
                                        TotalOrder_G = listTemp[0].TotalOrder_G,
                                        TotalCOD_G = listTemp[0].TotalCOD_G,
                                        Status_F = listTemp[0].Status_F,
                                        StatusName_F = listTemp[0].StatusName_F,
                                        TotalOrder_F = listTemp[0].TotalOrder_F,
                                        TotalCOD_F = listTemp[0].TotalCOD_F
                                    });
                                }
                                else
                                {
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportWeb
                                    {
                                        CreatedDate = d,
                                        Status_G = "G",
                                        StatusName_G = "Giao thành công",
                                        TotalOrder_G = 0,
                                        TotalCOD_G = 0,
                                        Status_F = "F",
                                        StatusName_F = "Giao thất bại",
                                        TotalOrder_F = 0,
                                        TotalCOD_F = 0
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportWeb: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportWeb: " + ex.Message);
            }
            return listDeliveryTransport;
        }
        #endregion

        #region StatisticalAndroid
        public static ZResponse StatisticalAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            ZResponse res = new ZResponse();
            ZStatisticalDashboardAndroidResponse DashboardAndroid = new ZStatisticalDashboardAndroidResponse();
            try
            {
                DashboardAndroid = new ZStatisticalDashboardAndroidResponse()
                {
                    TotalTurnover = TotalTurnoverAndroid(req),
                    Cashflow = CashflowAndroid(req),
                    Order = OrderAndroid(req),
                    Product = new ZStatisticalProductAndroid()
                    {
                        ProductSelling = ProductSellingAndroid(req),
                        ProductRefund = ProductRefundAndroid(req)
                    },
                    Customer = new ZStatisticalCustomerAndroid()
                    {
                        CustomerSelling = CustomerSellingAndroid(req),
                        CustomerRefund = CustomerRefundAndroid(req)
                    },
                    DeliveryTransport = DeliveryTransportAndroid(req)
                };
                res.Code = "00";
                res.Message = "Lấy dữ liệu thành công";
                res.Data = JsonConvert.SerializeObject(DashboardAndroid);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalAndroid: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static long TotalTurnoverAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            long totalTurnover = 0;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_TURNOVER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                totalTurnover = dr["TOTAL_TURNOVER"].ToString() == "" ? 0 : Convert.ToInt64(dr["TOTAL_TURNOVER"]);
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverAndroid: " + ex.Message);
            }
            return totalTurnover;
        }
        public static List<ZStatisticalSearchCashflowAndroid> CashflowAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchCashflowAndroid> listCashflow = new List<ZStatisticalSearchCashflowAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_CASHFLOW", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCashflow.Add(new ZStatisticalSearchCashflowAndroid()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatusCashflow(dr["STATUS"].ToString()),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                    TotalTransportFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"])
                                });
                            }
                        }
                        listCashflow = listCashflow.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowAndroid: " + ex.Message);
            }
            return listCashflow;
        }
        public static List<ZStatisticalSearchOrderAndroid> OrderAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchOrderAndroid> listOrder = new List<ZStatisticalSearchOrderAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_ORDER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listOrder.Add(new ZStatisticalSearchOrderAndroid()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                    Quantity = Convert.ToInt64(dr["QUANTITY"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                });
                            }
                        }
                        listOrder = listOrder.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderAndroid: " + ex.Message);
            }
            return listOrder;
        }
        public static List<ZStatisticalSearchProductSellingAndroid> ProductSellingAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductSellingAndroid> listProductSelling = new List<ZStatisticalSearchProductSellingAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_PRODUCT_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductSelling.Add(new ZStatisticalSearchProductSellingAndroid()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductSelling = listProductSelling.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingAndroid: " + ex.Message);
            }
            return listProductSelling;
        }
        public static List<ZStatisticalSearchProductRefundAndroid> ProductRefundAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductRefundAndroid> listProductRefund = new List<ZStatisticalSearchProductRefundAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_PRODUCT_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductRefund.Add(new ZStatisticalSearchProductRefundAndroid()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductRefund = listProductRefund.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundAndroid: " + ex.Message);
            }
            return listProductRefund;
        }
        public static List<ZStatisticalSearchCustomerSellingAndroid> CustomerSellingAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerSellingAndroid> listCustomerSelling = new List<ZStatisticalSearchCustomerSellingAndroid>();
            List<ZStatisticalSearchCustomerSellingAndroid> listCustomerSellingAll = new List<ZStatisticalSearchCustomerSellingAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_CUSTOMER_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerSelling.Add(new ZStatisticalSearchCustomerSellingAndroid()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerSelling.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerSelling.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerSelling)
                        {
                            iColor = iColor + 1;
                            listCustomerSellingAll.Add(new ZStatisticalSearchCustomerSellingAndroid()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerSellingAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerSellingAll[0].Percentage = 100;
                        }
                        listCustomerSellingAll = listCustomerSellingAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingAndroid: " + ex.Message);
            }
            return listCustomerSellingAll;
        }
        public static List<ZStatisticalSearchCustomerRefundAndroid> CustomerRefundAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerRefundAndroid> listCustomerRefund = new List<ZStatisticalSearchCustomerRefundAndroid>();
            List<ZStatisticalSearchCustomerRefundAndroid> listCustomerRefundAll = new List<ZStatisticalSearchCustomerRefundAndroid>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_CUSTOMER_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerRefund.Add(new ZStatisticalSearchCustomerRefundAndroid()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerRefund.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerRefund.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerRefund)
                        {
                            iColor = iColor + 1;
                            listCustomerRefundAll.Add(new ZStatisticalSearchCustomerRefundAndroid()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerRefundAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerRefundAll[0].Percentage = 100;
                        }
                        listCustomerRefundAll = listCustomerRefundAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundAndroid: " + ex.Message);
            }
            return listCustomerRefundAll;
        }
        public static List<ZStatisticalDeliveryTransportAndroid> DeliveryTransportAndroid(ZStatisticalDashboardAndroidRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalDeliveryTransportAndroid> list = new List<ZStatisticalDeliveryTransportAndroid>();
            List<ZStatisticalDeliveryTransportAndroid> listDeliveryTransport = new List<ZStatisticalDeliveryTransportAndroid>();
            List<string> allDates = new List<string>();
            for (DateTime date = Convert.ToDateTime(req.FromDate, new CultureInfo("fr-FR")); date <= Convert.ToDateTime(req.ToDate, new CultureInfo("fr-FR")); date = date.AddDays(1))
            {
                allDates.Add(date.ToString("dd/MM/yyyy"));
            }
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_ANDROID_PKG.STATISTICAL_DELIVERY_TRANSPORT", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                list.Add(new ZStatisticalDeliveryTransportAndroid()
                                {
                                    CreatedDate = dr["CREATED_DATE"].ToString(),
                                    Status_G = "G",
                                    StatusName_G = "Giao thành công",
                                    TotalOrder_G = Convert.ToInt64(dr["QUANTITY_G"]),
                                    TotalCOD_G = Convert.ToInt64(dr["TOTAL_COD_G"]),
                                    Status_F = "F",
                                    StatusName_F = "Giao thất bại",
                                    TotalOrder_F = Convert.ToInt32(dr["QUANTITY_F"]),
                                    TotalCOD_F = Convert.ToInt64(dr["TOTAL_COD_F"])
                                });
                            }
                        }
                        foreach (var d in allDates)
                        {
                            if (listDeliveryTransport.Where(m => m.CreatedDate == d).ToList().Count == 0)
                            {
                                if (list.Where(m => m.CreatedDate == d).ToList().Count > 0)
                                {
                                    var listTemp = list.Where(m => m.CreatedDate == d).ToList();
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportAndroid
                                    {
                                        CreatedDate = d,
                                        Status_G = listTemp[0].Status_G,
                                        StatusName_G = listTemp[0].StatusName_G,
                                        TotalOrder_G = listTemp[0].TotalOrder_G,
                                        TotalCOD_G = listTemp[0].TotalCOD_G,
                                        Status_F = listTemp[0].Status_F,
                                        StatusName_F = listTemp[0].StatusName_F,
                                        TotalOrder_F = listTemp[0].TotalOrder_F,
                                        TotalCOD_F = listTemp[0].TotalCOD_F
                                    });
                                }
                                else
                                {
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportAndroid
                                    {
                                        CreatedDate = d,
                                        Status_G = "G",
                                        StatusName_G = "Giao thành công",
                                        TotalOrder_G = 0,
                                        TotalCOD_G = 0,
                                        Status_F = "F",
                                        StatusName_F = "Giao thất bại",
                                        TotalOrder_F = 0,
                                        TotalCOD_F = 0
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportAndroid: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportAndroid: " + ex.Message);
            }
            return listDeliveryTransport;
        }
        #endregion

        #region StatisticalIOS
        public static ZResponse StatisticalIOS(ZStatisticalDashboardIOSRequest req)
        {
            ZResponse res = new ZResponse();
            ZStatisticalDashboardIOSResponse DashboardIOS = new ZStatisticalDashboardIOSResponse();
            try
            {
                DashboardIOS = new ZStatisticalDashboardIOSResponse()
                {
                    TotalTurnover = TotalTurnoverIOS(req),
                    Cashflow = CashflowIOS(req),
                    Order = OrderIOS(req),
                    Product = new ZStatisticalProductIOS()
                    {
                        ProductSelling = ProductSellingIOS(req),
                        ProductRefund = ProductRefundIOS(req)
                    },
                    Customer = new ZStatisticalCustomerIOS()
                    {
                        CustomerSelling = CustomerSellingIOS(req),
                        CustomerRefund = CustomerRefundIOS(req)
                    },
                    DeliveryTransport = DeliveryTransportIOS(req)
                };
                res.Code = "00";
                res.Message = "Lấy dữ liệu thành công";
                res.Data = JsonConvert.SerializeObject(DashboardIOS);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.StatisticalIOS: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static long TotalTurnoverIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            long totalTurnover = 0;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_TURNOVER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                totalTurnover = dr["TOTAL_TURNOVER"].ToString() == "" ? 0 : Convert.ToInt64(dr["TOTAL_TURNOVER"]);
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.TotalTurnoverIOS: " + ex.Message);
            }
            return totalTurnover;
        }
        public static List<ZStatisticalSearchCashflowIOS> CashflowIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchCashflowIOS> listCashflow = new List<ZStatisticalSearchCashflowIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_CASHFLOW", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCashflow.Add(new ZStatisticalSearchCashflowIOS()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatusCashflow(dr["STATUS"].ToString()),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                    TotalTransportFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"])
                                });
                            }
                        }
                        listCashflow = listCashflow.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CashflowIOS: " + ex.Message);
            }
            return listCashflow;
        }
        public static List<ZStatisticalSearchOrderIOS> OrderIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchOrderIOS> listOrder = new List<ZStatisticalSearchOrderIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_ORDER", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listOrder.Add(new ZStatisticalSearchOrderIOS()
                                {
                                    RowIndex = Convert.ToInt32(dr["ROW_INDEX"]),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                    Quantity = Convert.ToInt64(dr["QUANTITY"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                });
                            }
                        }
                        listOrder = listOrder.OrderBy(m => m.RowIndex).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.OrderIOS: " + ex.Message);
            }
            return listOrder;
        }
        public static List<ZStatisticalSearchProductSellingIOS> ProductSellingIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductSellingIOS> listProductSelling = new List<ZStatisticalSearchProductSellingIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_PRODUCT_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductSelling.Add(new ZStatisticalSearchProductSellingIOS()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductSelling = listProductSelling.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductSellingIOS: " + ex.Message);
            }
            return listProductSelling;
        }
        public static List<ZStatisticalSearchProductRefundIOS> ProductRefundIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalSearchProductRefundIOS> listProductRefund = new List<ZStatisticalSearchProductRefundIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_PRODUCT_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listProductRefund.Add(new ZStatisticalSearchProductRefundIOS()
                                {
                                    ProductCode = dr["PRODUCT_CODE"].ToString(),
                                    ProductName = dr["PRODUCT_NAME"].ToString(),
                                    ProductImage = dr["PRODUCT_IMAGE"].ToString(),
                                    TotalProduct = Convert.ToInt64(dr["TOTAL_PRODUCT"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        listProductRefund = listProductRefund.OrderByDescending(m => m.TotalProduct).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.ProductRefundIOS: " + ex.Message);
            }
            return listProductRefund;
        }
        public static List<ZStatisticalSearchCustomerSellingIOS> CustomerSellingIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerSellingIOS> listCustomerSelling = new List<ZStatisticalSearchCustomerSellingIOS>();
            List<ZStatisticalSearchCustomerSellingIOS> listCustomerSellingAll = new List<ZStatisticalSearchCustomerSellingIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_CUSTOMER_SELLING", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerSelling.Add(new ZStatisticalSearchCustomerSellingIOS()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerSelling.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerSelling.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerSelling)
                        {
                            iColor = iColor + 1;
                            listCustomerSellingAll.Add(new ZStatisticalSearchCustomerSellingIOS()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerSellingAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerSellingAll[0].Percentage = 100;
                        }
                        listCustomerSellingAll = listCustomerSellingAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerSellingIOS: " + ex.Message);
            }
            return listCustomerSellingAll;
        }
        public static List<ZStatisticalSearchCustomerRefundIOS> CustomerRefundIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<string> listBorderColor = XData.GetStatisticalBorderColorV2();
            List<string> listBackgroundColor = XData.GetStatisticalBackgroundColorV2();
            List<ZStatisticalSearchCustomerRefundIOS> listCustomerRefund = new List<ZStatisticalSearchCustomerRefundIOS>();
            List<ZStatisticalSearchCustomerRefundIOS> listCustomerRefundAll = new List<ZStatisticalSearchCustomerRefundIOS>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_CUSTOMER_REFUND", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                listCustomerRefund.Add(new ZStatisticalSearchCustomerRefundIOS()
                                {
                                    ProvinceName = dr["NAME"].ToString(),
                                    TotalOrderProvince = Convert.ToInt64(dr["TOTAL_ORDER_PROVINCE"]),
                                    TotalCOD = Convert.ToInt64(dr["TOTAL_COD"])
                                });
                            }
                        }
                        long SumTotalOrderProvince = listCustomerRefund.Sum(m => m.TotalOrderProvince) > 0 ? listCustomerRefund.Sum(m => m.TotalOrderProvince) : 1;
                        var iColor = -1;
                        foreach (var item in listCustomerRefund)
                        {
                            iColor = iColor + 1;
                            listCustomerRefundAll.Add(new ZStatisticalSearchCustomerRefundIOS()
                            {
                                ProvinceName = item.ProvinceName,
                                TotalOrderProvince = item.TotalOrderProvince,
                                Percentage = Math.Round((double)(100 * item.TotalOrderProvince) / SumTotalOrderProvince, 2),
                                BorderColor = listBorderColor[iColor],
                                BackgroundColor = listBackgroundColor[iColor]
                            });
                        }
                        if (listCustomerRefundAll.Where(m => m.ProvinceName == "...").Count() == 5)
                        {
                            listCustomerRefundAll[0].Percentage = 100;
                        }
                        listCustomerRefundAll = listCustomerRefundAll.OrderByDescending(m => m.TotalOrderProvince).ToList();
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.CustomerRefundIOS: " + ex.Message);
            }
            return listCustomerRefundAll;
        }
        public static List<ZStatisticalDeliveryTransportIOS> DeliveryTransportIOS(ZStatisticalDashboardIOSRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZStatisticalDeliveryTransportIOS> list = new List<ZStatisticalDeliveryTransportIOS>();
            List<ZStatisticalDeliveryTransportIOS> listDeliveryTransport = new List<ZStatisticalDeliveryTransportIOS>();
            List<string> allDates = new List<string>();
            for (DateTime date = Convert.ToDateTime(req.FromDate, new CultureInfo("fr-FR")); date <= Convert.ToDateTime(req.ToDate, new CultureInfo("fr-FR")); date = date.AddDays(1))
            {
                allDates.Add(date.ToString("dd/MM/yyyy"));
            }
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STATISTICAL_IOS_PKG.STATISTICAL_DELIVERY_TRANSPORT", new object[]
                {
                    req.PPCRMCode,
                    req.FromDate,
                    req.ToDate,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                        {
                            while (dr.Read())
                            {
                                list.Add(new ZStatisticalDeliveryTransportIOS()
                                {
                                    CreatedDate = dr["CREATED_DATE"].ToString(),
                                    Status_G = "G",
                                    StatusName_G = "Giao thành công",
                                    TotalOrder_G = Convert.ToInt64(dr["QUANTITY_G"]),
                                    TotalCOD_G = Convert.ToInt64(dr["TOTAL_COD_G"]),
                                    Status_F = "F",
                                    StatusName_F = "Giao thất bại",
                                    TotalOrder_F = Convert.ToInt32(dr["QUANTITY_F"]),
                                    TotalCOD_F = Convert.ToInt64(dr["TOTAL_COD_F"])
                                });
                            }
                        }
                        foreach (var d in allDates)
                        {
                            if (listDeliveryTransport.Where(m => m.CreatedDate == d).ToList().Count == 0)
                            {
                                if (list.Where(m => m.CreatedDate == d).ToList().Count > 0)
                                {
                                    var listTemp = list.Where(m => m.CreatedDate == d).ToList();
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportIOS
                                    {
                                        CreatedDate = d,
                                        Status_G = listTemp[0].Status_G,
                                        StatusName_G = listTemp[0].StatusName_G,
                                        TotalOrder_G = listTemp[0].TotalOrder_G,
                                        TotalCOD_G = listTemp[0].TotalCOD_G,
                                        Status_F = listTemp[0].Status_F,
                                        StatusName_F = listTemp[0].StatusName_F,
                                        TotalOrder_F = listTemp[0].TotalOrder_F,
                                        TotalCOD_F = listTemp[0].TotalCOD_F
                                    });
                                }
                                else
                                {
                                    listDeliveryTransport.Add(new ZStatisticalDeliveryTransportIOS
                                    {
                                        CreatedDate = d,
                                        Status_G = "G",
                                        StatusName_G = "Giao thành công",
                                        TotalOrder_G = 0,
                                        TotalCOD_G = 0,
                                        Status_F = "F",
                                        StatusName_F = "Giao thất bại",
                                        TotalOrder_F = 0,
                                        TotalCOD_F = 0
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportIOS: " + res.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StatisticalRepository.DeliveryTransportIOS: " + ex.Message);
            }
            return listDeliveryTransport;
        }
        #endregion

    }
}