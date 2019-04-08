using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Clients;
using System.Configuration;
using System.Data;
using System;

namespace JobCostReconciliation.Data.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly ISqlClient _sqlClient;
        private readonly IServiceLog _serviceLog;

        public QueueRepository(ISqlClient sqlClient, IServiceLog serviceLog)
        {
            _sqlClient = sqlClient;
            _serviceLog = serviceLog;
        }

        public DataTable GetQueueRecords(string dataObject = "")
        {
            var queueQuery = GetQueueQueryString();

            if (!string.IsNullOrEmpty(queueQuery))
            {
                var connString = ConfigurationManager.ConnectionStrings["QueueDbContext"].ConnectionString;

                _serviceLog.AppendLog(String.Format(" Getting Queue {0} records", dataObject));
                var queueRecords = _sqlClient.QueryADO(queueQuery, connString);
                _serviceLog.AppendLog(String.Format(" {0} Queue {1} records found", queueRecords.Rows.Count, dataObject));

                if (queueRecords != null && queueRecords.Rows.Count != 0) return queueRecords;
            }

            return null;
        }

        private string GetQueueQueryString()
        {
            return "SELECT * FROM DataIntegrationQueue.dbo.AuditPOSync";
        }

        public void TruncateQueueTable(string DataObject = "")
        {
            var sql = "TRUNCATE TABLE DataIntegrationQueue.dbo.AuditPOSync";

            var connString = ConfigurationManager.ConnectionStrings["QueueDbContext"].ConnectionString;
            _sqlClient.NonQueryADO(sql, connString);
            
        }

        public DataTable GetCompanies()
        {
            var sqlQuery = "SELECT * FROM Companies";

            if (!string.IsNullOrEmpty(sqlQuery))
            {
                var connString = ConfigurationManager.ConnectionStrings["QueueDbContext"].ConnectionString;
                var queueRecords = _sqlClient.QueryADO(sqlQuery, connString);

                if (queueRecords != null && queueRecords.Rows.Count != 0) return queueRecords;
            }

            return null;
        }


        public void InsertAuditPOSyncRecord(string sql)
        {
            var connString = ConfigurationManager.ConnectionStrings["QueueDbContext"].ConnectionString;
            _sqlClient.NonQueryADO(sql, connString);
        }

        public string CreateInsertSQL(DataRow dataRow)
        {
            try
            {

                string insertSQL = "INSERT INTO [AuditPOSync] " +
                                    "([DataSource], [SapphireObjRID], [po_no], [NewJobNumber], [activity], [p_egm_amount], [po_type], " +
                                    "[SapphireObjID], [job_no], [POHeaderRecordID], [Approval_Date], [ApprovePaymentDate], [Cancelled_Date], " +
                                    "[Check_Date], [Community], [LastModifiedDate], [POEffectiveDate], [Payment_Amount], [Payment_Date], " +
                                    "[Release_Date], [Required_Date], [SapphirePONumber], [Subtotal], [Tax], [Taxable_Amount], [Total], " +
                                    "[VPO_Yes_No], [Vendor_ID], [check_no], [eSubmittalDate], [eXmitDate], [original_po_no]) " +
                                    "VALUES( '" +
                                                dataRow["DataSource"] + "', " +
                                                dataRow["SapphireObjRID"] + ", " +
                                                dataRow["po_no"] + ", " +
                                                "'" + dataRow["NewJobNumber"] + "', " +
                                                dataRow["activity"] + ", " +
                                                dataRow["p_egm_amount"] + ", " +
                                                "'" + dataRow["po_type"] + "', " +
                                                "'" + dataRow["SapphireObjID"] + "', " +
                                                "'" + dataRow["job_no"] + "', " +
                                                dataRow["POHeaderRecordID"] + ", " +
                                                "'" + dataRow["Approval_Date"] + "'" + ", " +
                                                "'" + dataRow["ApprovePaymentDate"] + "'" + ", " +
                                                "'" + dataRow["Cancelled_Date"] + "'" + ", " +
                                                "'" + dataRow["Check_Date"] + "'" + ", " +
                                                "'" + dataRow["Community"] + "', " +
                                                "'" + dataRow["LastModifiedDate"] + "'" + ", " +
                                                "'" + dataRow["POEffectiveDate"] + "'" + ", " +
                                                dataRow["Payment_Amount"] + ", " +
                                                "'" + dataRow["Payment_Date"] + "'" + ", " +
                                                "'" + dataRow["Release_Date"] + "'" + ", " +
                                                "'" + dataRow["Required_Date"] + "'" + ", '" +
                                                dataRow["SapphirePONumber"] + "', " +
                                                dataRow["Subtotal"] + ", " +
                                                dataRow["Tax"] + ", " +
                                                dataRow["Taxable_Amount"] + ", " +
                                                dataRow["Total"] + ", " +
                                                dataRow["VPO_Yes_No"] + ", " +
                                                "'" + dataRow["Vendor_ID"] + "', " +
                                                dataRow["check_no"] + ", " +
                                                "'" + dataRow["eSubmittalDate"] + "'" + ", " +
                                                "'" + dataRow["eXmitDate"] + "'" + ", " +
                                                dataRow["original_po_no"] +
                                            ")";

                return insertSQL;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public DataTable GetAllRows()
        {
            DataIntegrationQueueDataSetTableAdapters.AuditPOSyncTableAdapter AuditPOSyncTableAdapter =
                new DataIntegrationQueueDataSetTableAdapters.AuditPOSyncTableAdapter();
            return AuditPOSyncTableAdapter.GetData().CopyToDataTable();
        }
    }
}
