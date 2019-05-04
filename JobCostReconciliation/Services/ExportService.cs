using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Model;
using JobCostReconciliation.Services;
using JobCostReconciliation.Data.Repositories;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace JobCostReconciliation
{
    public class ExportService : IExportService
    {
        private readonly IJobCostActivityRepository _jobCostActivityRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IServiceLog _serviceLog;

        public ExportService()
        {
            _jobCostActivityRepository = new JobCostActivityRepository();
            _purchaseOrderRepository = new PurchaseOrderRepository();
            _serviceLog = new ServiceLog();
        }

        public ExportService(IJobCostActivityRepository jobCostActivityRepository, IPurchaseOrderRepository purchaseOrderRepository, IServiceLog serviceLog)
        {
            _jobCostActivityRepository = jobCostActivityRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _serviceLog = serviceLog;
        }

        public void PrepareDataForExport(DataTable dataTable, string dataObject, string sqlOperation = "INSERT")
        {
            try
            {
                // don't get po_no for "JobPOs" list - wait until list is distilled to items matching JobCstActs, etc
                if (!dataObject.Equals("JobPOs"))
                {
                    if (!dataObject.Equals("JobVPOs"))
                    {
                        dataTable.Columns.Add("po_type");
                        dataTable.Columns.Add("po_no");
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        PrepareDataRowForExport(row, dataObject, sqlOperation);
                    }

                    // write final output to csv file
                    string Filename = _serviceLog.WriteToFile(dataTable, dataObject);

                    _serviceLog.AppendLog(String.Format("{0} Sapphire {1} records found missing in Pervasive", dataTable.Rows.Count, dataObject));
                    _serviceLog.AppendLog(String.Format("{0} {1} records written to file {2}", dataTable.Rows.Count, dataObject, Filename));
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.ToString());
            }
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public void PrepareDataRowForExport(DataRow row, string dataObject, string sqlOperation = "INSERT")
        {
            try
            {
                string company = String.IsNullOrEmpty(row["dbName"].ToString()) ? row["dbName"].ToString() : String.Empty;
                if (!dataObject.Equals("JobVPOs")) 
                {
                    row["po_type"] = dataObject.Equals("JobCstActs") ? "N" : "Y";
                }

                string sqlText = BuildSQLInsert(dataObject, row, company);

                // append prepared sql statements to text file
                if (!string.IsNullOrEmpty(sqlText.Trim()))
                {
                    WriteSQLToFile(sqlText.Trim(), dataObject, sqlOperation);
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(String.Format("Unable to prepare datarow for export. Job Number: {0}", row["job_no"]), "", ex);
            }
        }

        public void WriteSQLToFile(string sqlText, string dataObject, string sqlOperation = "INSERT")
        {
            sqlText += "\r\n";
            string Filename = String.Format("{0}_{1}.sql.txt", dataObject, sqlOperation);
            File.AppendAllText(Filename, sqlText);
        }

        public string BuildSQLInsert(string DataObject, DataRow row, string Company = "")
        {
            if (DataObject.Equals("JobCstActs")) { return _jobCostActivityRepository.CreateJobCostSQLInsert(row, Company); }

            if (DataObject.Equals("jobPORecordsNotFoundInPervasive")) { return _purchaseOrderRepository.CreateJobPOSQLInsert(row, Company); }
            if (DataObject.Equals("jobPORecordsMatchingPervasiveJobCstActRecords")) { return _purchaseOrderRepository.CreateJobPOSQLUpdate(row, Company); }

            if (DataObject.Equals("poProcessingDates")) { return _purchaseOrderRepository.CreatePOProcessingDatesSQL(row, Company); }

            if (DataObject.Equals("jobCostEGMAmounts")) { return _jobCostActivityRepository.CreateJobCostEGMAmountsUpdateSQL(row, Company); }

            return string.Empty;
        }

    }
}
