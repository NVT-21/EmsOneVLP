using EMS.Library;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;

namespace EMS.API.PayPost.Core
{
    public static class XHelper
    {
        private static string _logPath = string.Empty;
        private static string _connectionString = string.Empty;
        private static Database _database = null;
        private static string _schema = string.Empty;
        static string _imageDefault = string.Empty;
        private static OracleConnection _oracleConnection = null;

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

        static OracleConnection OracleConnection
        {
            get
            {
                try
                {
                    if (_oracleConnection == null)
                        _oracleConnection = new OracleConnection(XHelper.ConnectionString);
                    if (_oracleConnection.State != ConnectionState.Open)
                        _oracleConnection.Open();
                }
                catch (Exception ex)
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "XHelper.OracleConnection: " + ex.Message);
                    _oracleConnection = null;
                }
                return _oracleConnection;
            }

            set
            {
                _oracleConnection = value;
            }
        }

        public static Database Database
        {
            get
            {
                if (_database == null)
                    _database = new OracleDatabase(XHelper.ConnectionString);
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

        public static int ExecuteNonQuery(DbCommand dbCommand)
        {
            int result = -1;
            try
            {
                OracleConnection connection = XHelper.OracleConnection;
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbCommand.Connection = connection;
                    return dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XHelper.ExecuteNonQuery: " + ex.Message);
                result = -99;
            }
            return result;
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
    }
}