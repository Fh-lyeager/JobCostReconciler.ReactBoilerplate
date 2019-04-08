using System;
using System.Data;
using System.Data.SqlClient;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Services;

namespace JobCostReconciliation.Data.Clients
{
    public class SqlClient : ISqlClient
    {
        public SqlClient()
        {
        
        }

        public DataTable QueryADO(string queryString, string connectionString)
        {
            var dt = new DataTable();
            try
            {
                using (var oConnection = new SqlConnection(connectionString))
                {
                    oConnection.Open();
                    var oCommand = new SqlCommand(queryString, oConnection);
                    oCommand.CommandTimeout = 300;
                    var oAdapter = new SqlDataAdapter(oCommand);
                    oAdapter.Fill(dt);
                }
            }
            catch (SqlException sqlException)
            {
                //_logger.Error($"Database Failure ADO.NET: {sqlException}");
            }
            catch (Exception exception)
            {
                //_logger.Error($"Database Failure ADO.NET {exception.ToString()}");
            }

            return dt;
        }

        public void NonQueryADO(string queryString, string connectionString)
        {
            try
            {
                using (var oConnection = new SqlConnection(connectionString))
                {
                    oConnection.Open();
                    var oCommand = new SqlCommand(queryString, oConnection);
                    oCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
            catch (Exception exception)
            {
                //_logger.Error($"Database Failure Queue ADO.NET {exception.ToString()}");
            }
        }
    }
}
