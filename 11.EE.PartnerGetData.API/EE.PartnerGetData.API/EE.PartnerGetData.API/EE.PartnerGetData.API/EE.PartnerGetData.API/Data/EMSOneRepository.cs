using EE.PartnerGetData.API.Models;
using EE.PartnerGetData.API.Models.EMSOne;
using EE.PartnerGetData.API.Models.EMSOneTracking;
using EE.PartnerGetData.API.Utility;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;

namespace EE.PartnerGetData.API.Data
{
    public class EMSOneRepository
    {
        #region GetDataForControlFreight
        public static ResponseEntity GetDataForControlFreight(EMSOneGetDataForControlFreightRequest request)
        {
            var _response = new ResponseEntity();
            var _values = new List<EMSOneGetDataForControlFreightResponse>();

            try
            {
                string sp = "sp_EMSOneGetDataForControlFreight";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter() { ParameterName = "@CustomerCode ", Value = request.CustomerCode, },
                    new SqlParameter() { ParameterName = "@FromDate ", Value = request.FromDate, },
                    new SqlParameter() { ParameterName = "@ToDate ", Value = request.ToDate, },
                };

                var _dt = Helper.GetDataFromEE(sp, parameters);

                if (_dt != null)
                {
                    foreach (DataRow row in _dt.Rows)
                    {
                        var _value = new EMSOneGetDataForControlFreightResponse()
                        {
                            LadingCode = row["LadingCode"].ToString(),
                            Code = row["Code"].ToString(),
                            Freight = Convert.ToInt64(row["Freight"]),
                            AdditionService = row["AdditionService"].ToString(),
                            CustomerCode = row["CustomerCode"].ToString(),
                            ReleaseTime = row["ReleaseTime"].ToString(),
                            SpecialService = row["SpecialService"].ToString(),
                        };

                        var _releaseDate = row["ReleaseDate"].ToString();
                        var _releaseTime = row["ReleaseTime"].ToString();

                        var _dReleaseTime = DateTime.Now;

                        var _isDate = DateTime.TryParseExact(_releaseDate + " " + _releaseTime.PadLeft(4, '0'), "yyyyMMdd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _dReleaseTime);

                        if (!_isDate) _dReleaseTime = DateTime.Now;

                        _value.ReleaseTime = _dReleaseTime.ToString("dd/MM/yyyy HH:mm:ss");

                        _values.Add(_value);
                    }

                    if (_values.Count > 0)
                    {
                        _response.Code = "00";
                        _response.Message = "Lấy dữ liệu thành công.";
                        _response.Data = JsonConvert.SerializeObject(_values);
                    }
                    else
                    {
                        _response.Code = "01";
                        _response.Message = "Không tìm thấy dữ liệu phù hợp.";
                    }
                }
                else
                {
                    _response.Code = "98";
                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.GetDataForControlFreight", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống, vui lòng liên hệ ban quản trị.";
            }

            return _response;
        }
        #endregion

        #region SyncData
        public static ResponseEntity SyncData(DataTable dt)
        {
            ResponseEntity _response = new ResponseEntity();

            try
            {
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                    new OracleParameter() { ParameterName = "P_TABLE_NAME", Direction = ParameterDirection.Input, OracleDbType = OracleDbType.Varchar2, Value = dt.TableName },
                    new OracleParameter() { ParameterName = "P_DATA", Direction = ParameterDirection.Input, Value = dt.ToXML(), OracleDbType = OracleDbType.Clob },
                };

                Helper.ExecuteNonQuery("EE_SYNC_DATA_PKG.EE_SYNC_DATA", _parameters, CommandType.StoredProcedure);

                var _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                if (_result > 0)
                {
                    _response.Code = "00";
                    var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                    _response.Message = "Cập nhật dữ liệu thành công.";
                }
                else
                {
                    switch (_result)
                    {
                        default:
                            _response.Code = "99";
                            _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.SyncData", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống.";
            }

            return _response;
        }
        #endregion

        #region SyncDataV2
        public static ResponseEntity SyncDataV2(DataTable dt)
        {
            ResponseEntity _response = new ResponseEntity();

            try
            {
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                    new OracleParameter() { ParameterName = "P_TABLE_NAME", Direction = ParameterDirection.Input, OracleDbType = OracleDbType.Varchar2, Value = dt.TableName },
                    new OracleParameter() { ParameterName = "P_DATA", Direction = ParameterDirection.Input, Value = dt.ToXML(), OracleDbType = OracleDbType.Clob },
                };

                Helper.ExecuteNonQuery("EMS_EE_SYNC_ORDER_PKG.EE_SYNC_DATA_V2", _parameters, CommandType.StoredProcedure);

                var _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                if (_result > 0)
                {
                    _response.Code = "00";
                    var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                    _response.Message = "Cập nhật dữ liệu thành công.";
                }
                else
                {
                    switch (_result)
                    {
                        default:
                            _response.Code = "99";
                            _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.SyncDataV2", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống.";
            }

            return _response;
        }
        #endregion

        #region DataEMSOneCustomer
        public static ResponseEntity DataEMSOneCustomer(EMSOneTrackingDataEMSOneCustomerRequest request)
        {
            ResponseEntity _response = new ResponseEntity();
            List<EMSOneTrackingDataEMSOneCustomerResponse> _oListDataCustomer = new List<EMSOneTrackingDataEMSOneCustomerResponse>();
            try
            {
                var _values = JsonConvert.DeserializeObject<EMSOneTrackingDataEMSOneCustomerModel>(request.Data);
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_FROM_DATE", Direction = ParameterDirection.Input, Value = _values.FromDate, OracleDbType = OracleDbType.Varchar2},
                    new OracleParameter() { ParameterName = "P_TO_DATE", Direction = ParameterDirection.Input, Value = _values.ToDate, OracleDbType = OracleDbType.Varchar2},
                    new OracleParameter() { ParameterName = "P_STATUS", Direction = ParameterDirection.Input, Value = _values.Status, OracleDbType = OracleDbType.Varchar2},
                    new OracleParameter() { ParameterName = "P_MOBILE_NUMBER", Direction = ParameterDirection.Input, Value = _values.MobileNumber, OracleDbType = OracleDbType.Varchar2},
                    new OracleParameter() { ParameterName = "P_CUSTOMER_CODE", Direction = ParameterDirection.Input, Value = _values.CustomerCode, OracleDbType = OracleDbType.Varchar2},
                    new OracleParameter() { ParameterName = "P_CRM_OR_PAYPOST_CODE", Direction = ParameterDirection.Input, Value = _values.CrmOrPaypostCode, OracleDbType = OracleDbType.Varchar2 },
                    new OracleParameter() { ParameterName = "P_IS_LOCK", Direction = ParameterDirection.Input, Value = _values.IsLock, OracleDbType = OracleDbType.Varchar2 },
                    new OracleParameter() { ParameterName = "P_DATE_TYPE", Direction = ParameterDirection.Input, Value = _values.DateType, OracleDbType = OracleDbType.Varchar2 },
                    new OracleParameter() { ParameterName = "P_RETURN_CODE", Direction = ParameterDirection.Output, OracleDbType = OracleDbType.Int32 },
                    new OracleParameter() { ParameterName = "P_OUT_CURSOR", Direction = ParameterDirection.Output, OracleDbType = OracleDbType.RefCursor },
                };

                Helper.ExecuteNonQuery("EMS_SHOP_PKG.SEARCH_SHOP", _parameters, CommandType.StoredProcedure);
                var _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[08].Value).Value;
                if (_result > 0)
                {
                    using (var dr = ((Oracle.ManagedDataAccess.Types.OracleRefCursor)_parameters[09].Value).GetDataReader())
                    {
                        while (dr.Read())
                        {
                            _oListDataCustomer.Add(new EMSOneTrackingDataEMSOneCustomerResponse()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                FullName = Convert.ToString(dr["FULL_NAME"]),
                                StoreName = Convert.ToString(dr["STORE_NAME"]),
                                Email = Convert.ToString(dr["EMAIL"]),
                                MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                Code = Convert.ToString(dr["CODE"]),
                                CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                Status = Convert.ToString(dr["STATUS"]),
                                IsLock = Convert.ToString(dr["IS_LOCK"]),
                            });
                        }
                    }
                    if (_oListDataCustomer.Count > 0)
                    {
                        _response.Code = "00";
                        _response.Message = "Thành công.";
                        _response.Data = JsonConvert.SerializeObject(_oListDataCustomer);
                    }
                    else
                    {
                        _response.Code = "01";
                        _response.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    switch (_result)
                    {
                        default:
                            _response.Code = "99";
                            _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.DataEMSOneCustomer", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống.";
            }
            return _response;
        }
        #endregion

        #region TrackingSyncData
        public static ResponseEntity TrackingSyncData(EMSOneTrackingSyncDataRequest request)
        {
            ResponseEntity _response = new ResponseEntity();
            try
            {
                int _result = -1;
                var _values = JsonConvert.DeserializeObject<EMSOneTrackingSyncHistoryModel>((request.Data));
                switch (_values.ActionName)
                {
                    case "ADD_XML_DATA_ITEM":
                        var _parameters_01 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataItemInfo.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataItemInfo.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_01, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_01[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_01[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    case "ADD_XML_DATA_ADDEDSERVICEITEM":
                        var _parameters_02 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataValueSerrviceCode.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataValueSerrviceCode.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_02, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_02[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_02[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    case "ADD_XML_DATA_STATUS":
                        var _parameters_03 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataItemStatus.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataItemStatus.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_03, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_03[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_03[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    case "ADD_XML_DATA_STATUS_PICKUP":
                        var _parameters_04 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataItemStatusPickup.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataItemStatusPickup.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_04, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_04[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_04[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    case "ADD_XML_DATA_DELIVERY":
                        var _parameters_05 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataDelivery.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataDelivery.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_05, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_05[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_05[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    case "ADD_XML_DATA_COD":
                        var _parameters_06 = new OracleParameter[]
                        {
                            new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32 },
                            new OracleParameter() { ParameterName = "P_TRACKING_ID", Direction = ParameterDirection.Input, Value = _values.TrackingID, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_TRACKING_CODE", Direction = ParameterDirection.Input, Value = _values.TrackingCode, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_FROM_TIME", Direction = ParameterDirection.Input, Value = _values.FromTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_TO_TIME", Direction = ParameterDirection.Input, Value = _values.ToTime, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_AMOUNT", Direction = ParameterDirection.Input, Value = _values.ListDataCOD.Count, OracleDbType = OracleDbType.Int32},
                            new OracleParameter() { ParameterName = "P_ACTION_NAME", Direction = ParameterDirection.Input, Value = _values.ActionName, OracleDbType = OracleDbType.Varchar2},
                            new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = _values.ListDataCOD.ToXML(), OracleDbType = OracleDbType.Clob }
                        };
                        Helper.ExecuteNonQuery("EMS_TRACKING_SYNC_DATA_PKG.TRACKING_SYNC_DATA", _parameters_06, CommandType.StoredProcedure);
                        _result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_06[00].Value).Value;
                        if (_result > 0)
                        {
                            _response.Code = "00";
                            var _success = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters_06[00].Value).Value;
                            _response.Message = "Cập nhật dữ liệu thành công.";
                        }
                        else
                        {
                            switch (_result)
                            {
                                default:
                                    _response.Code = "99";
                                    _response.Message = "Có lỗi trong quá trình xử lí dữ liệu, vui lòng liên hệ ban quản trị.";
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.TrackingSyncData", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống.";
            }
            return _response;
        }
        #endregion

        #region GetFilesUpdate
        public static ResponseEntity GetFilesUpdate()
        {
            ResponseEntity _response = new ResponseEntity();
            try
            {
                string _filePath = HttpContext.Current.Server.MapPath("~/Files/");

                DirectoryInfo d = new DirectoryInfo(_filePath);
                FileInfo[] Files = d.GetFiles();

                var _files = new List<string>();

                foreach (var file in Files)
                {
                    _files.Add(file.Name);
                }

                _response.Code = "00";
                _response.Message = "Lấy dữ liệu thành công.";
                _response.Data = JsonConvert.SerializeObject(_files);
            }
            catch (Exception ex)
            {
                Log.WriteLog("EMSOneRepository.GetFilesUpdate", ex.GetException(), LogFileType.Exception);
                _response.Code = "99";
                _response.Message = "Lỗi xử lí hệ thống.";
            }
            return _response;
        }
        #endregion
    }
}