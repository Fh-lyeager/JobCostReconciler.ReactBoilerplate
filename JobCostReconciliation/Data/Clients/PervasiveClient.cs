using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Pervasive.Data.SqlClient;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Services;


namespace JobCostReconciliation.Data.Clients
{
    public class PervasiveClient : IPervasiveClient
    {
        private readonly string _pervasiveDbContext = ConfigurationManager.ConnectionStrings["PervasiveDbContext"].ConnectionString;

        public PervasiveClient()
        {

        }

        public DataTable QueryPervasiveADO(string queryString)
        {
            var dataTable = new DataTable();
            try
            {
                using (var oConnection = new PsqlConnection(_pervasiveDbContext))
                {
                    oConnection.Open();
                    var oCommand = new PsqlCommand(queryString, oConnection);
                    var oAdapter = new PsqlDataAdapter(oCommand);
                    oAdapter.Fill(dataTable);
                }
            }
            catch (Exception exception)
            {
                //_logger.Error($"Database Failure Pervasive ADO.NET {exception.InnerException}");
                return null;
            }

            return dataTable;
        }

        public void NonQueryPervasiveADO(string queryString)
        {
            try
            {
                using (var oConnection = new PsqlConnection(_pervasiveDbContext))
                {
                    oConnection.Open();
                    var oCommand = new PsqlCommand(queryString, oConnection);
                    oCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
            catch (Exception exception)
            {
                //_logger.Error($"Database Failure Pervasive ADO.NET {queryString} {exception.InnerException}");
                throw exception;
            }
        }

        public void StoredProcADO(string storedProc, Dictionary<string, string> parameters)
        {
            using (PsqlConnection oConnection = new PsqlConnection(_pervasiveDbContext))
            {
                PsqlCommand oCommand = new PsqlCommand(storedProc, oConnection);
                oCommand.CommandType = CommandType.StoredProcedure;
                foreach (String sKey in parameters.Keys) { oCommand.Parameters.AddWithValue(sKey, parameters[sKey]); }

                try
                {
                    oConnection.Open();
                    oCommand.ExecuteNonQuery();
                }
                catch (PsqlException pSqlException)
                {
                    //_logger.Error($"Database Failure Pervasive ADO.NET {pSqlException}");
                    throw pSqlException;
                }
            }
        }

        public string ScalarStoredProcADO(string storedProc, Dictionary<string, string> parameters)
        {
            string result;
            using (var oConnection = new PsqlConnection(_pervasiveDbContext))
            {
                var oCommand = new PsqlCommand(storedProc, oConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                foreach (var sKey in parameters.Keys) { oCommand.Parameters.AddWithValue(sKey, parameters[sKey]); }
                var oAdaptor = new PsqlDataAdapter(oCommand);
                var dt = new DataTable();
                try
                {
                    oConnection.Open();
                    oAdaptor.Fill(dt);
                    oConnection.Close();
                    result = dt.Rows.Count > 0 ? Convert.ToString(dt.Rows[0][0]) : null;
                }
                catch (Exception exception)
                {
                    //_logger.Error($"Database Failure Pervasive ADO.NET {exception}");
                    throw exception;
                }
            }
            return result;
        }

        public string ScalarQueryStringADO(string queryString)
        {
            var result = "";
            using (var oConnection = new PsqlConnection(_pervasiveDbContext))
            {
                oConnection.Open();
                var oCommand = new PsqlCommand(queryString, oConnection);
                result = Convert.ToString(oCommand.ExecuteScalar());
                oConnection.Close();
            }
            return result;
        }
    }
}
