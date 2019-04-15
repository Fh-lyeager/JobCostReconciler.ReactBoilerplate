using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using System;
using System.Data;
using System.Linq;

namespace JobCostReconciliation.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ISapphireRepository _sapphireRepository;
        private readonly IPurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;
        private readonly IServiceLog _serviceLog;
        private readonly IExportService _exportService;
        private readonly IDateService _dateService;

        public PurchaseOrderService(ISapphireRepository sapphireRepository, IPurchaseOrderHeaderRepository purchaseOrderHeaderRepository, IServiceLog serviceLog, IExportService exportService, IDateService dateService)
        {
            _sapphireRepository = sapphireRepository;
            _purchaseOrderHeaderRepository = purchaseOrderHeaderRepository;
            _serviceLog = serviceLog;
            _exportService = exportService;
            _dateService = dateService;
        }

        public DataTable GetSapphireJobPORecords()
        {
            return _sapphireRepository.GetSapphireRecords("JobPOs");
        }

        public void ReconcilePurchaseOrderDates(string dataObject = "JobPOs")
        {
            DataTable recordsToReconcile = new DataTable();
            recordsToReconcile.Columns.Add("dbName");
            recordsToReconcile.Columns.Add("JobPOID");
            recordsToReconcile.Columns.Add("DateComplByVnd");
            recordsToReconcile.Columns.Add("DateApproved");
            recordsToReconcile.Columns.Add("eSubmittalDate");
            recordsToReconcile.Columns.Add("ApprovePaymentDate");

            bool UnmatchedValue = false;
            var sapphireRecords = _sapphireRepository.GetSapphireRecords(dataObject);
            var pervasiveRecords = _purchaseOrderHeaderRepository.GetPervasiveRecords(dataObject);

            try
            {
                var poCompareDataSet = from srow in sapphireRecords.AsEnumerable()
                                       join prow in pervasiveRecords.AsEnumerable().DefaultIfEmpty() on
                                       new { JobPOID = srow.Field<string>("SapphirePONumber") }
                                       equals new { JobPOID = prow.Field<string>("SapphirePONumber") }
                                       select new
                                       {
                                           DbName = srow.Field<string>("dbName"),
                                           JobPOID = srow.Field<string>("SapphirePONumber"),
                                           DateComplByVnd = srow.Field<DateTime>("DateComplByVnd"),
                                           DateApproved = srow.Field<DateTime>("DateApproved"),
                                           eSubmittalDate = prow.Field<DateTime>("eSubmittalDate"),
                                           ApprovePaymentDate = prow.Field<DateTime>("ApprovePaymentDate"),
                                           SapphirePONumber = prow.Field<string>("SapphirePONumber")
                                       };

                if (poCompareDataSet.Any())
                {
                    // Find matching pervasive record, compare date fields
                    foreach(var po in poCompareDataSet)
                    {
                        // sapphire fields
                        var jobPOID = po.GetType().GetProperty("SapphirePONumber").GetValue(po, null).ToString() ?? "";

                        // get matching pervasive record where SapphirePONumber = jobPOID
                        var pervasiveRows = pervasiveRecords.AsEnumerable()
                        .Where(p => p.Field<string>("SapphirePONumber").Equals(jobPOID));

                        if (pervasiveRows.Any())
                        {
                            DataTable dt = pervasiveRows.CopyToDataTable();
                            if (dt.Rows.Count > 1)
                            {
                                _serviceLog.AppendLog(String.Format("Multiple pervasive records found for SapphirePONumber {0}", jobPOID));
                                continue;
                            }

                            if (dt.Rows.Count == 1)
                            {
                                // eSubmittalDate (JobSchActs.DateComplByVnd) 
                                //      also, JobPOs.DateCompleted - verify which field appears to be currently used in production
                                object dateComplByVnd = po.GetType().GetProperty("DateComplByVnd").GetValue(po, null);
                                object eSubmittalDate = dt.Rows[0]["eSubmittalDate"];
                                if (!_dateService.DatesAreEqual(dateComplByVnd, eSubmittalDate)) UnmatchedValue = true;

                                // ApprovePaymentDate (DateApproved)
                                object dateApproved = po.GetType().GetProperty("DateApproved").GetValue(po, null);
                                object approvePaymentDate = dt.Rows[0]["ApprovePaymentDate"];
                                if (!_dateService.DatesAreEqual(dateApproved, approvePaymentDate)) UnmatchedValue = true;

                                if (UnmatchedValue)
                                {
                                    DataRow recordToUpdate = recordsToReconcile.NewRow();

                                    recordToUpdate["dbName"] = po.GetType().GetProperty("dbName").GetValue(po, null).ToString() ?? "";
                                    recordToUpdate["JobPOID"] = jobPOID;
                                    recordToUpdate["DateComplByVnd"] = _dateService.FormatDateForPervasive(dateComplByVnd);
                                    recordToUpdate["DateApproved"] = _dateService.FormatDateForPervasive(dateApproved);

                                    recordToUpdate["eSubmittalDate"] = String.IsNullOrEmpty(eSubmittalDate.ToString()) ? null : ((DateTime)eSubmittalDate).ToString("yyyy-MM-dd");
                                    recordToUpdate["ApprovePaymentDate"] = String.IsNullOrEmpty(approvePaymentDate.ToString()) ? null : ((DateTime)approvePaymentDate).ToString("yyyy-MM-dd");

                                    recordsToReconcile.Rows.Add(recordToUpdate);
                                    UnmatchedValue = false;
                                }
                            }
                        }
                    }
                }

                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "poProcessingDates", "UPDATE");
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);

                // write export data to file anyway
                if (recordsToReconcile.Rows.Count > 0)
                {
                    _exportService.PrepareDataForExport(recordsToReconcile, "poProcessingDates", "UPDATE");
                }
            }
        }

    }
}
