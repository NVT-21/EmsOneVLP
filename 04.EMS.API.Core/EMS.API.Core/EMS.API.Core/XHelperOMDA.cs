using EMS.Core;
using EMS.Library;
using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace EMS.API.Core
{
    public class XHelperOMDA
    {
        private static string _connectionStringOMDA = string.Empty;
        private static OracleConnection _oracleConnectionOMDA = null;

        #region OracleConnection OMDA
        static string ConnectionStringOMDA
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionStringOMDA))
                    _connectionStringOMDA = ConfigurationManager.AppSettings["CONNECTION_STRING_OMDA"];
                return _connectionStringOMDA;
            }

            set
            {
                _connectionStringOMDA = value;
            }
        }
        static OracleConnection OracleConnectionOMDA
        {
            get
            {
                try
                {
                    if (_oracleConnectionOMDA == null)
                        _oracleConnectionOMDA = new OracleConnection(XHelperOMDA.ConnectionStringOMDA);
                    if (_oracleConnectionOMDA.State != ConnectionState.Open)
                        _oracleConnectionOMDA.Open();
                }
                catch (Exception ex)
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "XHelperOMDA.OracleConnectionOMDA: " + ex.Message);
                    _oracleConnectionOMDA = null;
                }
                return _oracleConnectionOMDA;
            }

            set
            {
                _oracleConnectionOMDA = value;
            }
        }
        #endregion

        #region ExecuteNonQuery OMDA
        public static void ExecuteNonQuery(string commandText, Oracle.ManagedDataAccess.Client.OracleParameter[] parameters, CommandType commandType)
        {
            try
            {
                Oracle.ManagedDataAccess.Client.OracleConnection connection = XHelperOMDA.OracleConnectionOMDA;

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "XHelperOMDA.ExecuteNonQuery: " + ex.Message);
            }
        }
        #endregion
    }
}