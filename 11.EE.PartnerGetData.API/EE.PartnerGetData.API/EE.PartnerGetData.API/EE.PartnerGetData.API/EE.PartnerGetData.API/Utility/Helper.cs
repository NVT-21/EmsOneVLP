using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace EE.PartnerGetData.API.Utility
{
    public class Helper
    {
        private static string _sqlConnection = string.Empty;
        private static string SqlConnection { get { if (string.IsNullOrEmpty(_sqlConnection)) _sqlConnection = ConfigurationManager.AppSettings["SQLConnection"]; return _sqlConnection; } }

        private static string _timeOut = string.Empty;
        private static string SqlTimeOut
        {
            get
            {
                if (string.IsNullOrEmpty(_timeOut))
                    _timeOut = ConfigurationManager.AppSettings["TimeOut"];

                if (string.IsNullOrEmpty(_timeOut))
                    _timeOut = "120";

                return _timeOut;
            }
        }

        public static DataTable GetDataFromEE(string storeProcedure, SqlParameter[] parameters)
        {
            DataTable tblData = new DataTable("Data");

            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnection))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (var command = new SqlCommand(storeProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = Convert.ToInt32(SqlTimeOut);
                        command.Parameters.AddRange(parameters);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(tblData);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tblData;
        }
        //---------------------------------------------------------------------------

        private static string _connectionString = string.Empty;
        private static OracleConnection _oracleConnection = null;

        #region OracleConnection
        static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString = ConfigurationManager.AppSettings["EMS_CONNECTION_STRING"];
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
                        _oracleConnection = new OracleConnection(Helper.ConnectionString);
                    if (_oracleConnection.State != ConnectionState.Open)
                        _oracleConnection.Open();
                }
                catch (Exception exception)
                {
                    Log.WriteLog("Helper.OracleConnection", exception.GetException(), LogFileType.Exception);
                    _oracleConnection = null;
                }
                return _oracleConnection;
            }

            set
            {
                _oracleConnection = value;
            }
        }
        #endregion

        #region ExecuteNonQuery
        public static void ExecuteNonQuery(string commandText, Oracle.ManagedDataAccess.Client.OracleParameter[] parameters, CommandType commandType)
        {
            try
            {
                Oracle.ManagedDataAccess.Client.OracleConnection connection = Helper.OracleConnection;

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
            catch (Exception exception)
            {
                Log.WriteLog("Helper.ExecuteNonQuery - " + commandText, exception.GetException(), LogFileType.Exception);
            }
        }
        #endregion
    }
}