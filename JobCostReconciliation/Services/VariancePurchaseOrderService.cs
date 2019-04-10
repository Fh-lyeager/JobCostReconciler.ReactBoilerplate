using System;
using System.Data;
using System.Linq;

using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Services
{
    public class VariancePurchaseOrderService : IVariancePurchaseOrderService
    {
        private readonly ISapphireRepository _sapphireRepository;
        private readonly IPervasiveRepository _pervasiveRepository;
        private readonly IServiceLog _serviceLog;
        private readonly IExportService _exportService;
        private readonly IDateService _dateService;

        public VariancePurchaseOrderService(ISapphireRepository sapphireRepository, IPervasiveRepository pervasiveRepository, IServiceLog serviceLog, IExportService exportService, IDateService dateService)
        {
            _sapphireRepository = sapphireRepository;
            _pervasiveRepository = pervasiveRepository;
            _serviceLog = serviceLog;
            _exportService = exportService;
            _dateService = dateService;
        }

        // JobVPO Approval Dates Business Logic
        //  If JobVPOs.Total < 0 and JobVPOs.Status is "Approved" or "Authorized", set po_header.approval_date = JobVPOs.DateAuthorized
        //  Otherwise, set po_header.approval_date = JobVPOs.DateApproved    
        public void ReconcileJobVPOApprovalDates(string dataObject = "JobVPOsBySapphirePONumber")
        {
            DataTable recordsToReconcile = new DataTable();
            recordsToReconcile.Columns.Add("dbName");
            recordsToReconcile.Columns.Add("JobVPORID");
            recordsToReconcile.Columns.Add("JobVPOID");
            recordsToReconcile.Columns.Add("SapphireAmtTotal");
            recordsToReconcile.Columns.Add("JobVPOStatus");
            recordsToReconcile.Columns.Add("DateAuthorized");
            recordsToReconcile.Columns.Add("DateApproved");
            recordsToReconcile.Columns.Add("approval_date");
            recordsToReconcile.Columns.Add("ApprovePaymentDate");
            recordsToReconcile.Columns.Add("DateCompleted");
            recordsToReconcile.Columns.Add("eSubmittalDate");
            recordsToReconcile.Columns.Add("SapphirePONumber");
            recordsToReconcile.Columns.Add("SQLUpdate");

            try
            {
                var sapphireRecords = _sapphireRepository.GetSapphireRecords("JobVPOs");
                var pervasiveRecords = _pervasiveRepository.GetPervasiveRecords("JobVPOs");

                if ((!(sapphireRecords is null) && sapphireRecords.AsEnumerable().Any())
                        && (!(pervasiveRecords is null) && pervasiveRecords.AsEnumerable().Any()))
                {
                    // (left) join sapphire and pervasive data together on JobVPOs.JobVPOID = po_header.SapphirePONumber
                    var vpoCompareDataSet = from srow in sapphireRecords.AsEnumerable()
                                            join prow in pervasiveRecords.AsEnumerable().DefaultIfEmpty() on
                                            new { JobVPOID = srow.Field<string>("JobVPOID") }
                                            equals new { JobVPOID = prow.Field<string>("SapphirePONumber") }
                                            where srow.Field<decimal>("AmtTotal") < 0
                                            && (srow.Field<string>("Status").Equals("Approved") ||
                                                srow.Field<string>("Status").Equals("Authorized"))
                                            select new
                                            {
                                                DbName = prow.Field<string>("datasource"),
                                                JobVPORID = srow.Field<int>("JobVPORID"),
                                                JobVPOID = srow.Field<string>("JobVPOID"),
                                                SapphireAmtTotal = srow.Field<decimal>("AmtTotal"),
                                                JobVPOStatus = srow.Field<string>("Status"),
                                                DateAuthorized = srow.Field<DateTime?>("DateAuthorized"),
                                                DateApproved = srow.Field<DateTime?>("DateApproved"),
                                                Approval_Date = prow.Field<DateTime?>("approval_date"),
                                                SapphirePONumber = prow.Field<string>("SapphirePONumber")
                                              
                                            };

                    bool UnmatchedValue = false;
                    string updateSQL = string.Empty;

                    if (vpoCompareDataSet.Any())
                    {
                        // Find matching pervasive record, compare date fields
                        foreach (var vpo in vpoCompareDataSet)
                        {
                            // if JobVPOs.AmtTotal is negative (< 0) and status is either Approved or Authorized,
                            if (vpo.SapphireAmtTotal < 0 && (vpo.JobVPOStatus == "Approved" || vpo.JobVPOStatus == "Authorized"))
                            {
                                // set po_header.approval_date = JobVPOs.DateAuthorized
                                UnmatchedValue = !_dateService.DatesAreEqual(vpo.DateAuthorized, vpo.Approval_Date, false);
                                if (UnmatchedValue && !String.IsNullOrEmpty(vpo.DbName))
                                {
                                    updateSQL = CreateSQLUpdateSetDateValuesBySapphirePONumber(vpo.JobVPOID, "approval_date", _dateService.FormatDateForPervasive((object)vpo.DateAuthorized), vpo.DbName);
                                }
                            }

                            if (UnmatchedValue)
                            {
                                DataRow recordToUpdate = recordsToReconcile.NewRow();

                                recordToUpdate["dbName"] = vpo.DbName;
                                recordToUpdate["JobVPORID"] = vpo.JobVPORID;
                                recordToUpdate["JobVPOID"] = vpo.JobVPOID;
                                recordToUpdate["SapphireAmtTotal"] = vpo.SapphireAmtTotal;
                                recordToUpdate["JobVPOStatus"] = vpo.JobVPOStatus;
                                recordToUpdate["DateAuthorized"] = _dateService.FormatDateForPervasive(vpo.DateAuthorized);
                                recordToUpdate["DateApproved"] = _dateService.FormatDateForPervasive(vpo.DateApproved);
                                recordToUpdate["approval_date"] = String.IsNullOrEmpty(vpo.Approval_Date.ToString()) ? null : _dateService.FormatDateForPervasive((DateTime)vpo.Approval_Date);
                                recordToUpdate["SapphirePONumber"] = vpo.SapphirePONumber;
                                recordToUpdate["SQLUpdate"] = updateSQL;

                                recordsToReconcile.Rows.Add(recordToUpdate);
                                UnmatchedValue = false;
                            }
                        }
                    }
                }

                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "JobVPOsBySapphirePONumber", "UPDATE");
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);

                // write export data to file anyway
                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "JobVPOsBySapphirePONumber", "UPDATE");
                }
            }
        }


        private string CreateSQLUpdateSetDateValuesBySapphirePONumber(string SapphirePONumber, string columnName, object dateValueToSet, string dbName)
        {
            string updateSQL = string.Empty;
            try
            {
                if (dbName is null) return null;
                updateSQL = " UPDATE \"" + dbName + "\".PO_HEADER" +
                            " SET " + columnName + " = " + (String.IsNullOrEmpty(dateValueToSet.ToString()) ? "NULL" : "'" + dateValueToSet + "'") +
                            " WHERE SapphirePONumber = '" + SapphirePONumber + "'";
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog($"{ex.Message} SapphirePONumber: {SapphirePONumber} columnName: {columnName} dateValueToSet: {dateValueToSet} dbName: {dbName}", "", ex);
                return null;
            }
            return updateSQL;
        }

        private string CreateSQLUpdateSetDateBySapphirePONumber(string SapphirePONumber, string dbName, string dateSql)
        {
            try
            {
                if (dbName is null) return null;
                
                return " UPDATE \"" + dbName + "\".PO_HEADER" +
                        " SET " + dateSql + 
                        " WHERE SapphirePONumber = '" + SapphirePONumber + "'";
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog($"{ex.Message} SapphirePONumber: {SapphirePONumber} dbName: {dbName} dateSql: {dateSql}", "", ex);
                return null;
            }
        }

        private string BuildUpdateDateSql(object dateValueToSet, string columnName, string dateSql = "")
        {
            try
            {
                string prepend = String.Empty;

                if (!String.IsNullOrEmpty(dateSql))
                {
                    prepend = dateSql + ", ";
                }

                if (dateValueToSet is null)
                {
                    return prepend + columnName + " = NULL";
                }

                string dateValue;
                dateValue = String.IsNullOrEmpty(dateValueToSet.ToString()) ? "NULL" : "'" + dateValueToSet + "'";

                return prepend + columnName + " = " + dateValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReconcileJobVPOApprovedAndCompletedDates(string dataObject = "JobVPOsBySapphirePONumber")
         {
            DataTable recordsToReconcile = new DataTable();
            recordsToReconcile.Columns.Add("dbName");
            recordsToReconcile.Columns.Add("JobVPORID");
            recordsToReconcile.Columns.Add("JobVPOID");
            recordsToReconcile.Columns.Add("JobVPOStatus");
            recordsToReconcile.Columns.Add("DateApproved");
            recordsToReconcile.Columns.Add("ApprovePaymentDate");
            recordsToReconcile.Columns.Add("DateCompleted");
            recordsToReconcile.Columns.Add("eSubmittalDate");
            recordsToReconcile.Columns.Add("DateCancelled");
            recordsToReconcile.Columns.Add("cancelled_date");
            recordsToReconcile.Columns.Add("SapphirePONumber");
            recordsToReconcile.Columns.Add("SQLUpdate");

            try
            {
                var sapphireRecords = _sapphireRepository.GetSapphireRecords("JobVPOs");
                var pervasiveRecords = _pervasiveRepository.GetPervasiveRecords("JobVPOs");

                if ((!(sapphireRecords is null) && sapphireRecords.AsEnumerable().Any())
                        && (!(pervasiveRecords is null) && pervasiveRecords.AsEnumerable().Any()))
                { 
                    // ** Testing ** /	
                   //var sapVPORecord = sapphireRecords.AsEnumerable().Where(s => s.Field<string>("JobVPOID").Equals("VPO-000350"));	
                   //s var perVPORecord = pervasiveRecords.AsEnumerable().Where(p => p.Field<string>("SapphirePONumber").Equals("VPO-000350"));	


                    // (left) join sapphire and pervasive data together on JobVPOs.JobVPOID = po_header.SapphirePONumber
                   var vpoCompareDataSet = from srow in sapphireRecords.AsEnumerable()
                                            join prow in pervasiveRecords.AsEnumerable().DefaultIfEmpty() on
                                            new { JobVPOID = srow.Field<string>("JobVPOID") }
                                            equals new { JobVPOID = prow.Field<string>("SapphirePONumber") }
                                            //where srow.Field<DateTime?>("DateApproved").Equals( prow.Field<DateTime?>("ApprovePaymentDate"))
                                              //    || (srow.Field<DateTime?>("DateCompleted").Equals(prow.Field<DateTime?>("eSubmittalDate")) )
                                            select new
                                            {
                                                DbName = prow.Field<string>("datasource"),
                                                JobVPORID = srow.Field<int>("JobVPORID"),
                                                JobVPOID = srow.Field<string>("JobVPOID"),
                                                JobVPOStatus = srow.Field<string>("Status"),
                                                DateApproved = srow.Field<DateTime?>("DateApproved"),
                                                ApprovePaymentDate = prow.Field<DateTime?>("ApprovePaymentDate"),
                                                DateCompleted = srow.Field<DateTime?>("DateCompleted"),
                                                eSubmittalDate = prow.Field<DateTime?>("eSubmittalDate"),
                                                SapphirePONumber = prow.Field<string>("SapphirePONumber"),
                                                DateCancelled = srow.Field<DateTime?>("DateCancelled"),
                                                Cancelled_Date = prow.Field<DateTime?>("cancelled_date")
                                            };

                    bool unmatchedValue = false;
                    string updateSQL = string.Empty;

                    if (vpoCompareDataSet.Any())
                    {
                        // Find matching pervasive record, compare date fields
                        foreach (var vpo in vpoCompareDataSet)
                        {
                            string dateSql = string.Empty;

                            // set po_header.ApprovePaymentDate = JobVPOs.DateApproved
                            if (!_dateService.DatesAreEqual(vpo.DateApproved, vpo.ApprovePaymentDate, false))
                            {
                                dateSql = BuildUpdateDateSql(_dateService.FormatDateForPervasive((object)vpo.DateApproved), "ApprovePaymentDate");
                                unmatchedValue = true;
                            }

                            // set po_header.eSubmittalDate = JobVPOs.DateCompleted
                            if (!_dateService.DatesAreEqual(vpo.DateCompleted, vpo.eSubmittalDate, false))
                            {
                                dateSql = BuildUpdateDateSql(_dateService.FormatDateForPervasive((object)vpo.DateCompleted), "eSubmittalDate", dateSql);
                                unmatchedValue = true;
                            }

                            //set po_header.cancelled_date = JovVPOs.DateCancelled
                            if (!_dateService.DatesAreEqual(vpo.DateCancelled, vpo.Cancelled_Date, false))
                            {
                                dateSql = BuildUpdateDateSql(_dateService.FormatDateForPervasive((object)vpo.DateCancelled), "cancelled_date");
                                unmatchedValue = true;
                            }

                            if (unmatchedValue && !String.IsNullOrEmpty(vpo.DbName))
                            {
                                updateSQL = CreateSQLUpdateSetDateBySapphirePONumber(vpo.JobVPOID, vpo.DbName, dateSql);
                            }

                            if (unmatchedValue)
                            {
                                DataRow recordToUpdate = recordsToReconcile.NewRow();

                                recordToUpdate["dbName"] = vpo.DbName;
                                recordToUpdate["JobVPORID"] = vpo.JobVPORID;
                                recordToUpdate["JobVPOID"] = vpo.JobVPOID;
                                recordToUpdate["JobVPOStatus"] = vpo.JobVPOStatus;
                                recordToUpdate["DateApproved"] = _dateService.FormatDateForPervasive(vpo.DateApproved);
                                recordToUpdate["ApprovePaymentDate"] = String.IsNullOrEmpty(vpo.ApprovePaymentDate.ToString()) ? null : _dateService.FormatDateForPervasive((DateTime)vpo.ApprovePaymentDate);
                                recordToUpdate["DateCompleted"] = _dateService.FormatDateForPervasive(vpo.DateCompleted);
                                recordToUpdate["eSubmittalDate"] = String.IsNullOrEmpty(vpo.eSubmittalDate.ToString()) ? null : _dateService.FormatDateForPervasive((DateTime)vpo.eSubmittalDate);
                                recordToUpdate["SapphirePONumber"] = vpo.SapphirePONumber;
                                recordToUpdate["DateCancelled"] = _dateService.FormatDateForPervasive(vpo.DateCancelled);
                                recordToUpdate["cancelled_date"] = String.IsNullOrEmpty(vpo.Cancelled_Date.ToString()) ? null : _dateService.FormatDateForPervasive((DateTime)vpo.Cancelled_Date);
                                recordToUpdate["SQLUpdate"] = updateSQL;

                                recordsToReconcile.Rows.Add(recordToUpdate);
                                unmatchedValue = false;
                            }
                        }
                    }
                }

                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "JobVPOsBySapphirePONumber", "UPDATE");
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);

                // write export data to file anyway
                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "JobVPOsBySapphirePONumber", "UPDATE");
                }
            }
        }
    }
}
