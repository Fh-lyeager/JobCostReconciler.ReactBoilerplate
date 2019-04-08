using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Services
{
    public class ServiceLog : IServiceLog
    {
        private readonly ISqlClient _sqlClient;

        public ServiceLog(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        private string Date = string.Format("{0}-{1}-{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        public void AppendLog(string Message = "", string Filename = "", Exception Ex = null)
        {
            if (String.IsNullOrEmpty(Filename)) Filename = string.Format("Job Cost Reconciliation {0}.txt", Date.ToString());
            string MessageText = String.Format("{0} {1}", DateTime.Now.ToString(), String.IsNullOrEmpty(Message) ? Ex.Message : Message);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(MessageText);
            File.AppendAllText(Filename, stringBuilder.ToString());

            Console.WriteLine(MessageText);
        }

        public void AppendLogMessage(string Message)
        {
            Console.WriteLine(Message);

            string Filename = string.Format("Job Cost Reconciliation {0}.txt", Date.ToString());
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Message);
            File.AppendAllText(Filename, stringBuilder.ToString());
        }

        public string WriteToFile(DataTable dataTable, string Filename = "")
        {
            var lines = new List<string>();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                Select(column => column.ColumnName).
                ToArray();

            var header = String.Join(",", columnNames);
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                .Select(row => String.Join(",", row.ItemArray));
            lines.AddRange(valueLines);

            if (String.IsNullOrEmpty(Filename))
            {
                Filename = "ReconciliationOutput.csv";
            }
            else
            {
                Filename += "_ReconciliationOutput.csv";
            }

            File.WriteAllLines(Filename, lines);
            Process.Start(Filename);

            return Filename;
        }

        public string WriteToFile(List<Reconciliation> reconciliationRecords, string Filename = "")
        {
            var lines = new List<string>();

            string[] columnNames = new string[] {"DatabaseName", "JobCstActRID", "JobNumber", "Activity", "SEgmAmount", "PEgmAmount", "POHeaderRecordID", "po_no" };
                
            var header = String.Join(",", columnNames);
            lines.Add(header);

            var valueLines = reconciliationRecords
                .Select(row => String.Join(",", row.DatabaseName, row.JobCstActRID.ToString(), row.JobNumber, row.Activity, row.SEgmAmount.ToString(), row.PEgmAmount.ToString(), row.POHeaderRecordID.ToString(),row.po_no.ToString()));
            lines.AddRange(valueLines);

            if (String.IsNullOrEmpty(Filename))
            {
                Filename = "ReconciliationOutput.csv";
            }
            else
            {
                Filename += "_ReconciliationOutput.csv";
            }

            File.WriteAllLines(Filename, lines);
            Process.Start(Filename);

            return Filename;
        }

        public void InsertLog(int workflowRID, string sqlOperation, string sqlQuery, string status)
        {
            try
            {
                string sql = CreateLogTransactionSQL(workflowRID, sqlOperation, sqlQuery, status);
                string connection = ConfigurationManager.ConnectionStrings["SWAPI"].ConnectionString.ToString();
                _sqlClient.NonQueryADO(sql, connection);
            }
            catch (Exception ex)
            {
                throw new Exception("Failure inserting API call into the log. " + ex.InnerException);
            }
        }

        private string CreateLogTransactionSQL(int workflowRID, string sqlOperation, string sqlQuery, string status)
        {
            return "INSERT INTO SWAPI_Log (WorkflowRID, SQLOperation, SQLQuery, [Status], Created)" +
                   "VALUES (" +
                   workflowRID + ", " +
                   "'" + sqlOperation + "', " +
                   "'" + sqlQuery.Replace("'", "''") + "', " +
                   "'" + status + "', " +
                   "GETDATE()" +
                   ")";
        }
    }
}
