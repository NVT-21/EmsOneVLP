using EMS.Library;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.IO;
using EMS.Entity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Linq;
using System.Data.SqlClient;

namespace EMS.Core
{
    public static class XHelper
    {
        #region API SYNC_GATEWAY
        static string _syncGatewayURL = string.Empty;

        //static HttpClient _clientSyncGateway = null;

        public static string SyncGatewayUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_syncGatewayURL))
                    _syncGatewayURL = ConfigurationManager.AppSettings["SYNC_GATEWAY_URL"];
                return _syncGatewayURL;
            }

            set
            {
                _syncGatewayURL = value;
            }
        }
        public static ZResponse InvokeSyncGateway(ZRequest requestBody)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientSyncGateway = new HttpClient())
                {
                    ClientSyncGateway.BaseAddress = new Uri(SyncGatewayUrl);
                    ClientSyncGateway.DefaultRequestHeaders.Accept.Clear();
                    ClientSyncGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientSyncGateway.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var req = new ZRequest()
                    {
                        Code = requestBody.Code,
                        Data = requestBody.Data,
                        Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    };

                    HttpResponseMessage response = ClientSyncGateway.PostAsJsonAsync("Execute", req).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        res = response.Content.ReadAsAsync<ZResponse>().Result;
                    }
                    else
                    {
                        LogToFile(LogFileType.TRACE, string.Format("XHelper.InvokeSyncGateway: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }

                //if (_clientSyncGateway == null)
                //{
                //    _clientSyncGateway = new HttpClient();
                //    _clientSyncGateway.BaseAddress = new Uri(SyncGatewayUrl);
                //    _clientSyncGateway.DefaultRequestHeaders.Accept.Clear();
                //    _clientSyncGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    ServicePointManager.FindServicePoint(_clientSyncGateway.BaseAddress).ConnectionLeaseTimeout = 60 * 1000; //1 minute     
                //    //_clientSyncGateway.Timeout = TimeSpan.FromMinutes(2);
                //}

                //var req = new ZRequest()
                //{
                //    Code = requestBody.Code,
                //    Data = requestBody.Data,
                //    Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                //};

                //HttpResponseMessage response = _clientSyncGateway.PostAsJsonAsync("Execute", req).Result;

                //if (response.IsSuccessStatusCode)
                //{
                //    res = response.Content.ReadAsAsync<ZResponse>().Result;
                //}
                //else
                //{
                //    LogToFile(LogFileType.TRACE, string.Format("XHelper.InvokeSyncGateway: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                //    switch (response.StatusCode)
                //    {
                //        default:
                //            res.Code = "99";
                //            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                //            break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.InvokeSyncGateway: request={0}||exception={1}", JsonConvert.SerializeObject(requestBody), ex.Message));
            }
            return res;
        }
        #endregion

        #region Kết nối Label
        static HttpClient _clientLabel = null;

        static string _labelURL = string.Empty;

        static string LabelUrl
        {
            get
            {
                if(string.IsNullOrEmpty(_labelURL))
                    _labelURL = ConfigurationManager.AppSettings["LABEL_URL"];
                return _labelURL;
            }

            set
            {
                _labelURL = value;
            }
        }

        public static ZResponse InvokeLabel(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                if(_clientLabel == null)
                {
                    _clientLabel = new HttpClient();
                    _clientLabel.BaseAddress = new Uri(LabelUrl);
                    _clientLabel.DefaultRequestHeaders.Accept.Clear();
                    _clientLabel.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("APIKey", APIKey);
                    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64("lms:lms")));
                    //ServicePointManager.FindServicePoint(BaseAddress).ConnectionLeaseTimeout = 5 * 60 * 1000;
                }
                
                HttpResponseMessage response = null;
                switch(method)
                {
                    case "GET":
                        response = _clientLabel.GetAsync(requestUri).Result;
                        break;
                    case "POST":
                        response = _clientLabel.PostAsJsonAsync(requestUri, value).Result;
                        break;
                    default:
                        break;
                }

                if(response.IsSuccessStatusCode)
                {
                    res.Data = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogToFile(LogFileType.MESSAGE, string.Format("XHelper.Invoke: request={0} || response={1} || content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                    switch(response.StatusCode)
                    {
                        default:
                            res.Code = "99";
                            res.Message = "Lỗi " + response.StatusCode + ", vui lòng liên hệ quản trị.";
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.InvokeLabel: request={0} || exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }
        #endregion

        #region EMS_TRANSPORT_KEY   
        static string _emsTransportKey = string.Empty;

        public static string EMSTransportKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsTransportKey))
                    _emsTransportKey = ConfigurationManager.AppSettings["EMS_TRANSPORT_KEY"];
                return _emsTransportKey;
            }

            set
            {
                _emsTransportKey = value;
            }
        }
        #endregion

        #region Lấy giá trị DeviceName ở Header
        public static string _deviceName = string.Empty;

        public static string DeviceName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    _deviceName = "";
                }
                else
                {
                    _deviceName = HttpContext.Current.Request.Headers["DeviceName"];
                }
                return _deviceName;
            }
            set
            {
                _deviceName = value;
            }
        }
        #endregion

        #region Đường dẫn chứa ảnh
        public static string _imageURL = string.Empty;

        public static string ImageURL
        {
            get
            {
                if (string.IsNullOrEmpty(_imageURL))
                    _imageURL = ConfigurationManager.AppSettings["IMAGE_URL"];
                return _imageURL;
            }

            set
            {
                _imageURL = value;
            }
        }
        #endregion

        #region Đường dẫn chứa ảnh mặc định
        static string _imageDefault = string.Empty;


        public static string ImageDefault
        {
            get
            {
                if (string.IsNullOrEmpty(_imageDefault))
                    _imageDefault = ConfigurationManager.AppSettings["IMAGE_DEFAULT"];
                return _imageDefault;
            }

            set
            {
                _imageDefault = value;
            }
        }
        #endregion

        #region Đường dẫn chứa file của Yêu cầu hỗ trợ
        static string _pathRequestSupport = string.Empty;


        public static string PathRequestSupport
        {
            get
            {
                if (string.IsNullOrEmpty(_pathRequestSupport))
                    _pathRequestSupport = ConfigurationManager.AppSettings["IMAGE_URL"] + "Assets/RequestSupport/";
                return _pathRequestSupport;
            }

            set
            {
                _pathRequestSupport = value;
            }
        }
        #endregion

        #region Đường dẫn đến Frontend
        static string _frontendURL = string.Empty;

        public static string FrontEndURL
        {
            get
            {
                if (string.IsNullOrEmpty(_frontendURL))
                    _frontendURL = ConfigurationManager.AppSettings["FRONT_END_URL"];
                return _frontendURL;
            }

            set
            {
                _frontendURL = value;
            }
        }
        #endregion

        #region Temp
        public static string UrlAvatarDefault = FrontEndURL + "Assets/images/avatar_default.jpg";
        public static string DisplayNameDefault = "Ẩn danh";
        #endregion

        #region Kết nối DB Version 1.0
        private static string _connectionString = string.Empty;

        private static string _schema = string.Empty;

        private static Database _database = null;

        static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString = ConfigurationManager.AppSettings["CONNECTION_STRING"];
                return _connectionString;
            }

            set
            {
                _connectionString = value;
            }
        }

        public static Database Database
        {
            get
            {
                if (_database == null)
                    _database = new OracleDatabase(ConnectionString);
                return _database;
            }

            set
            {
                _database = value;
            }
        }
        public static string Schema
        {
            get
            {
                if (string.IsNullOrEmpty(_schema))
                    _schema = ConfigurationManager.AppSettings["SCHEMA"];
                return _schema;
            }

            set
            {
                _schema = value;
            }
        }

        private static OracleConnection _oracleConnection = null;

        static OracleConnection OracleConnection
        {
            get
            {
                try
                {
                    if (_oracleConnection == null)
                        _oracleConnection = new OracleConnection(ConnectionString);
                    if (_oracleConnection.State != ConnectionState.Open)
                        _oracleConnection.Open();
                }
                catch (Exception ex)
                {
                    LogToFile(LogFileType.EXCEPTION, "XHelper.OracleConnection: " + ex.Message);
                    _oracleConnection = null;
                }
                return _oracleConnection;
            }

            set
            {
                _oracleConnection = value;
            }
        }

        public static int ExecuteNonQuery(DbCommand dbCommand)
        {
            int result = -1;
            try
            {
                OracleConnection connection = OracleConnection;
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbCommand.Connection = connection;
                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogToFile(LogFileType.EXCEPTION, "XHelper.ExecuteNonQuery: " + ex.Message);
                result = -99;
            }
            return result;
        }
        #endregion

        #region Kết nối DB Version 2.0
        private static string QueryConnString = ConfigurationManager.AppSettings["CONNECTION_STRING"];
        public static Oracle.ManagedDataAccess.Client.OracleConnection QueryConnection2 = null;
        private static Oracle.ManagedDataAccess.Client.OracleConnection PrepareConn2()
        {
            try
            {
                if (QueryConnection2 == null)
                    QueryConnection2 = new Oracle.ManagedDataAccess.Client.OracleConnection(QueryConnString);

                if (QueryConnection2.State != ConnectionState.Open)
                    QueryConnection2.Open();

                return QueryConnection2;
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XHelper.PrepareConn2: " + ex.Message);
            }
            return null;
        }
        public static void ExecuteNonQueryV2(string commandText, Oracle.ManagedDataAccess.Client.OracleParameter[] parameters, CommandType commandType)
        {
            try
            {
                Oracle.ManagedDataAccess.Client.OracleConnection connection = PrepareConn2();

                if ((connection != null) && (connection.State == ConnectionState.Open))
                {
                    using (Oracle.ManagedDataAccess.Client.OracleCommand _cmd = new Oracle.ManagedDataAccess.Client.OracleCommand())
                    {
                        _cmd.Connection = connection;
                        _cmd.CommandType = commandType;
                        _cmd.Parameters.AddRange(parameters);
                        _cmd.CommandText = commandText;
                        _cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XHelper.ExecuteNonQueryV2: " + ex.Message);
            }
        }
        #endregion ConnectionV2


        #region SQL Connect Fee
        private static string _connectionStringFee = string.Empty;
        private static string _schemaFee = string.Empty;
        private static Database _databaseFee = null;
        private static SqlConnection _sqlConnectionFee = null;

        public static string ConnectionStringFee
        {
            get
            {
                if(string.IsNullOrEmpty(_connectionStringFee))
                    _connectionStringFee = ConfigurationManager.AppSettings["CONNECTION_STRING_FEE"];
                return _connectionStringFee;
            }
            set
            {
                _connectionStringFee = value;
            }
        }

        public static Database DatabaseFee
        {
            get
            {
                if(_databaseFee == null)
                    _databaseFee = new SqlDatabase(ConnectionStringFee);
                return _databaseFee;
            }
            set
            {
                _databaseFee = value;
            }
        }

        public static string SchemaFee
        {
            get
            {
                if(string.IsNullOrEmpty(_schemaFee))
                    _schemaFee = ConfigurationManager.AppSettings["SCHEMA_FEE"];
                return _schemaFee;
            }
            set
            {
                _schema = value;
            }
        }

        public static SqlConnection SqlConnectionFee
        {
            get
            {
                try
                {
                    if(_sqlConnectionFee == null || _sqlConnectionFee.ConnectionString == "")
                        _sqlConnectionFee = new SqlConnection(ConnectionStringFee);
                    if(_sqlConnectionFee.State != ConnectionState.Open)
                        _sqlConnectionFee.Open();
                }
                catch(Exception ex)
                {
                    LogToFile(LogFileType.EXCEPTION, "XHelper.SqlConnection: " + ex.Message);
                    _sqlConnectionFee = null;
                }
                return _sqlConnectionFee;
            }
            set
            {
                _sqlConnectionFee = value;
            }
        }

        public static int SqlExecuteNonQuery(DbCommand dbCommand)
        {
            int result = -1;
            try
            {
                SqlConnection connection = SqlConnectionFee;
                if(connection != null && connection.State == ConnectionState.Open)
                {
                    dbCommand.Connection = connection;
                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                LogToFile(LogFileType.EXCEPTION, "XHelper.SqlExecuteNonQuery: " + ex.Message);
                result = -99;
            }
            return result;
        }

        public static IDataReader SqlExecuteReader(DbCommand dbCommand, SqlConnection connection)
        {
            try
            {
                dbCommand.Connection = connection;
                return dbCommand.ExecuteReader();
            }
            catch( Exception ex )
            {
                LogToFile(LogFileType.EXCEPTION, "SqlHelper.SqlExecuteReader: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Ghi log
        private static string _logPath = string.Empty;

        static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = ConfigurationManager.AppSettings["LOG_PATH"];
                return _logPath;
            }

            set
            {
                _logPath = value;
            }
        }

        public static void LogToFile(LogFileType logFileType, string message)
        {
            try
            {
                LoggerGeneral logger = new LoggerGeneral();
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                FormatterGeneral formatter = new FormatterGeneral(message);
                string logPath = XHelper.LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                switch (logFileType)
                {
                    case LogFileType.TRACE:
                        logPath = logPath + @"\TRACE.log";
                        break;
                    case LogFileType.MESSAGE:
                        logPath = logPath + @"\MESSAGE.log";
                        break;
                    default:
                        logPath = logPath + @"\EXCEPTION.log";
                        break;
                }
                logger.Write(formatter, logPath);
            }
            catch
            {

            }
        }
        #endregion

        #region Function
        public static int ToInt(this object data)
        {
            var _value = 0;

            try
            {
                _value = Convert.ToInt32(data);
            }
            catch (Exception)
            {

            }

            return _value;
        }

        public static long ToLong(this object data)
        {
            long _value = 0;

            try
            {
                _value = Convert.ToInt64(data);
            }
            catch (Exception)
            {

            }

            return _value;
        }
        #endregion

       
    }
}