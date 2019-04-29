using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using JobCostReconciliation.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JobCostReconciliation.Services
{
    public class JobCostActivityService : IJobCostActivityService
    {
        public string dataObject = "JobCstActs";

        private readonly ISapphireRepository _sapphireRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPurchaseOrderHeaderRepository _purchaseOrderHeaderRepository;
        private readonly IServiceLog _serviceLog;
        private readonly IExportService _exportService;
        private readonly IJobService _jobService;
        private readonly IJobCostActivityRepository _jobCostActivityRepository;
        private readonly IWorkflowService _workflowService;

        public JobCostActivityService()
        {

        }

        public JobCostActivityService(ISapphireRepository sapphireRepository, ICompanyRepository companyRepository, IPurchaseOrderHeaderRepository purchaseOrderHeaderRepository, IServiceLog serviceLog, IExportService exportService, IJobService jobService, IJobCostActivityRepository jobCostActivityRepository, IWorkflowService workflowService)
        {
            _sapphireRepository = sapphireRepository;
            _companyRepository = companyRepository;
            _purchaseOrderHeaderRepository = purchaseOrderHeaderRepository;
            _serviceLog = serviceLog;
            _exportService = exportService;
            _jobService = jobService;
            _jobCostActivityRepository = jobCostActivityRepository;
            _workflowService = workflowService;
        }

        public ReconciliationEgmTotals GetEgmAmountsByHomeRID(int homeRID)
        {
            var jobNumber = _jobService.GetJobNumberByHomeRID(homeRID);
            return GetEgmAmounts(jobNumber);
        }

        private DataTable GetSapphireEgmJobTotals(List<int> homeRIds)
        {
            JobCostActivityRepository jobCostActivityRepository = new JobCostActivityRepository();
            return jobCostActivityRepository.GetEgmJobTotals(homeRIds);
        }

        private int GetSapphireWorkflow(string jobNumber)
        {
            Workflow workflow = _workflowService.GetWorkflow(jobNumber);
            return workflow.WFlowRID;
        }

        public ReconciliationEgmTotals GetEgmAmounts(string jobNumber)
        {
            SapphireRepository sapphireRepository = new SapphireRepository();
            DataTable sapphireEgmJobTotals = sapphireRepository.GetSapphireRecords("JobCstActs_EGM", jobNumber);

            if (!(sapphireEgmJobTotals is null))
            {
                // Get pervasive egm totals by Job
                PurchaseOrderHeaderRepository purchaseOrderHeaderRepository = new PurchaseOrderHeaderRepository();
                DataTable pervasiveEgmJobTotals = purchaseOrderHeaderRepository.GetPervasiveRecords("JobCstActs_EGM", jobNumber);

                if (!(pervasiveEgmJobTotals is null) && pervasiveEgmJobTotals.AsEnumerable().Any())
                {
                    var egmJobTotalsAudit = AuditEgmJobTotals(sapphireEgmJobTotals, pervasiveEgmJobTotals);

                    ReconciliationEgmTotals egmTotals = new ReconciliationEgmTotals();
                    egmTotals.JobNumber = jobNumber;
                    egmTotals.SapphireEgmTotal = Convert.ToDecimal(egmJobTotalsAudit.ToList()[0].SapphireEgmTotal);
                    egmTotals.PervasiveEgmTotal = Convert.ToDecimal(egmJobTotalsAudit.ToList()[0].PervasiveEgmTotal);
                    egmTotals.EstimateApprovalDate = egmJobTotalsAudit.ToList()[0].EstimateApprovalDate;

                    return egmTotals;
                }
            }

            return null;
        }

        public List<ReconciliationEgmTotals> GetWorkflowJobs()
        {
            // get workflows
            WorkflowService workflowService = new WorkflowService();
            List<Workflow> workflows = workflowService.ListWorkflows();

            var HomeRIDs = workflows.AsEnumerable()
                .Select(w => w.RefObjRID).Distinct().ToList();

            // get sapphire egm totals by HomeRIDs
            var sapphireEgmJobTotals = GetCompanyByJobNumber(GetSapphireEgmJobTotals(HomeRIDs));

            if (!(sapphireEgmJobTotals is null) && sapphireEgmJobTotals.AsEnumerable().Any())
            {
                // Get pervasive egm totals by Job
                PurchaseOrderHeaderRepository purchaseOrderHeaderRepository = new PurchaseOrderHeaderRepository();
                DataTable pervasiveEgmJobTotals = purchaseOrderHeaderRepository.GetPervasiveRecords("JobCstActs_EGM");

                if (!(pervasiveEgmJobTotals is null) && pervasiveEgmJobTotals.AsEnumerable().Any())
                {
                    var egmJobTotalsAudit = AuditEgmJobTotals(sapphireEgmJobTotals, pervasiveEgmJobTotals);
                    return egmJobTotalsAudit.ToList();
                }
            }
            return null;
        }

        private IEnumerable<ReconciliationEgmTotals> AuditEgmJobTotals(DataTable sapphireEgmJobTotals, DataTable pervasiveEgmJobTotals)
        {
            try
            {
                var auditEgmJobTotals = from srow in sapphireEgmJobTotals.AsEnumerable()
                                        join prow in pervasiveEgmJobTotals.AsEnumerable().DefaultIfEmpty() on
                                            new { JobNumber = srow.Field<string>("JobNumber") }
                                            equals new { JobNumber = prow.Field<string>("job_no") }
                                            //where (prow.Field<string>("databaseName").Equals(srow.Field<string>("dbName")))
                                        select new ReconciliationEgmTotals()
                                        {
                                            HomeRID = srow.Field<int>("HomeRID"),
                                            JobNumber = srow.Field<string>("JobNumber"),
                                            DatabaseName = srow.Field<string>("dbName"),
                                            SapphireEgmTotal = (decimal)srow.Field<decimal>("JobCstActs_BudgetAmt"),
                                            PervasiveEgmTotal = (decimal)prow.Field<double>("egm_amount"),
                                            EstimateApprovalDate = srow.Field<DateTime>("EstApprDate")
                                        };

                return auditEgmJobTotals;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateMissingJobCstActs(DataTable recordsToInsert, string dbName, string sqlOperation = "INSERT",string activity = "")
        {
            try
            {
                recordsToInsert.Columns.Add("po_type");
                bool isFirstRow = true;
                foreach (DataRow row in recordsToInsert.Rows)
                {
                    if (InsertJobCstActsRecord(row))
                    {
                        if (isFirstRow)
                        {
                            _serviceLog.AppendLogMessage(String.Format("\nThe following activities have been created in {0} for job {1}:", dbName, row["NewJobNumber"]));
                            _serviceLog.AppendLogMessage("NewJobNumber\t\tActivity");
                            isFirstRow = false;
                        }

                        _serviceLog.AppendLogMessage($"{row["NewJobNumber"]}\t\t{row["activity"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.ToString());
            }
        }

        private bool InsertJobCstActsRecord(DataRow recordToInsert)
        {
            try
            {
                string company = _companyRepository.GetCompanyByJob(recordToInsert["job_no"].ToString());
                recordToInsert["po_type"] = dataObject.Equals("JobCstActs") ? "N" : "Y";

                string sqlText = _exportService.BuildSQLInsert(this.dataObject, recordToInsert, company);

                // Insert row
                _jobCostActivityRepository.InsertJobCstActsRecord(sqlText, recordToInsert["job_no"].ToString(), recordToInsert["activity"].ToString());
                return true;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog($" Failure inserting JobCstActs record for job {recordToInsert["job_no"]} activity {recordToInsert["activity"]}", "", ex);
                return false;
            }
        }

        public void AuditSapphireWorkflow_HomeEstimateToApproved(int timespanDays)
        {
            // get workflows
            List<Workflow> workflows = _workflowService.ListWorkflows();

            var HomeRIDs = workflows.AsEnumerable()
                .Select(w => w.RefObjRID).Distinct().ToList();

            // get sapphire egm totals by HomeRIDs
            var sapphireEgmJobTotals = GetCompanyByJobNumber(GetSapphireEgmJobTotals(HomeRIDs));

            if (!(sapphireEgmJobTotals is null) && sapphireEgmJobTotals.AsEnumerable().Any())
            {
                // Get pervasive egm totals by Job
                DataTable pervasiveEgmJobTotals = _purchaseOrderHeaderRepository.GetPervasiveRecords("JobCstActs_EGM");

                if (!(pervasiveEgmJobTotals is null) && pervasiveEgmJobTotals.AsEnumerable().Any())
                {
                    var egmJobTotalsAudit = AuditEgmJobTotals(sapphireEgmJobTotals, pervasiveEgmJobTotals);

                    egmJobTotalsAudit = egmJobTotalsAudit
                        .Where(w => w.EstimateApprovalDate > DateTime.Today.AddDays(-timespanDays))
                        .OrderByDescending(o => o.EstimateApprovalDate);

                    _serviceLog.AppendLogMessage($"\n");
                    _serviceLog.AppendLogMessage($"Sapphire Workflow Audit");
                    _serviceLog.AppendLogMessage($"Row # \t Job Number \t Sapphire EGM Total \t Pervasive EGM Total \t STATUS");

                    int rowNum = 0;
                    foreach (var egmJob in egmJobTotalsAudit.AsEnumerable())
                    {
                        string jobNumber = egmJob.JobNumber;
                        string sapphireEgmTotal = Convert.ToDecimal(egmJob.SapphireEgmTotal).ToString("0.00");
                        string pervasiveEgmTotal = Convert.ToDecimal(egmJob.PervasiveEgmTotal).ToString("0.00");
                        rowNum++;
                        string jobDetail = $"{rowNum} \t {jobNumber} \t {sapphireEgmTotal} \t\t {pervasiveEgmTotal}";

                        if (sapphireEgmTotal.Equals(pervasiveEgmTotal))
                        {
                            jobDetail += "\t\t SUCCESS";
                        }
                        else
                        {

                            jobDetail += $"\t\t\t FAILURE: Egm amounts do not match.";
                        }

                        _serviceLog.AppendLogMessage(jobDetail);
                    }

                    _serviceLog.AppendLogMessage("\n");
                }
            }
        }

        public void ReconcileJobCstActRecordsAndEGMAmounts(string jobNumber, string company = "")
        {
            // check to see if there is an existing workflow for this record
            int workflowRID = GetSapphireWorkflow(jobNumber);

            // Get sapphire JobCstActs data
            DataTable sapphireJobCstActs = _sapphireRepository.GetSapphireRecords("JobCstActs", jobNumber);

            if (!(sapphireJobCstActs is null))
            {
                if (!string.IsNullOrEmpty(jobNumber) && !string.IsNullOrEmpty(company))
                {
                    // Get po_header data from pervasive
                    DataTable pervasiveJobCstActs = _jobCostActivityRepository.GetPervasiveJobCstActs(jobNumber, company);

                    if (!(pervasiveJobCstActs is null) && pervasiveJobCstActs.AsEnumerable().Any())
                    {
                        // 1. Find missing Job Cost Activities (JobCstActs) & create in PO_Header
                        CreateMissingJobCstActs(sapphireJobCstActs, pervasiveJobCstActs, company, workflowRID);

                        // 2) Set EGM amounts (reset all PO_Header egm amounts to 0 for this job then sync BudgetAmt from sapphire to PO_Header)
                        ResetEgmAmounts(jobNumber, company, workflowRID);
                        SetEgmValues(jobNumber, sapphireJobCstActs, company, workflowRID);

                        // 3) Recalc EGMs (IT_RecalcEGMValues)
                        _jobCostActivityRepository.RecalculateEgmValues(jobNumber, company, workflowRID);
                    }
                }
            }
        }

        public void ReconcileJobCstActRecordsAndEGMAmountsByActivity(string jobNumber, string activity, string company = "")
        {
            // check to see if there is an existing workflow for this record
            int workflowRID = GetSapphireWorkflow(jobNumber);
            company = _companyRepository.GetCompanyByJob(jobNumber);

            DataTable sapphireJobCstActs = _sapphireRepository.GetSapphireRecords("JobCstActs", jobNumber);

            if (!(sapphireJobCstActs is null))
            {
                if (!string.IsNullOrEmpty(jobNumber) && !string.IsNullOrEmpty(company))
                {
                    // Get po_header data from pervasive
                    DataTable pervasiveJobCstActs =
                        _jobCostActivityRepository.GetPervasiveJobCstActs(jobNumber, company);

                    if (!(pervasiveJobCstActs is null) && pervasiveJobCstActs.AsEnumerable().Any())
                    {
                        // 1. Find missing Job Cost Activities (JobCstActs) & create in PO_Header
                        CreateMissingJobCstActs(sapphireJobCstActs, pervasiveJobCstActs, company, workflowRID, activity);

                        // 2) Set EGM amounts (reset all PO_Header egm amounts to 0 for this job then sync BudgetAmt from sapphire to PO_Header)
                        var jobCstActivity = sapphireJobCstActs.AsEnumerable()
                            .Where(s => s.Field<string>("activity").Equals(activity))
                            .ToList();

                        if (jobCstActivity.Any())
                        {
                            ResetEgmAmounts(jobNumber, company, workflowRID, activity);
                            SetEgmValuesByActivity(jobNumber, activity, (decimal)jobCstActivity[0]["sap_egm_amount"], company, workflowRID);
                        }

                        // 3) Recalc EGMs (IT_RecalcEGMValues)
                        _jobCostActivityRepository.RecalculateEgmValues(jobNumber, company, workflowRID);
                    }
                }
            }
        }

        private void CreateMissingJobCstActs(DataTable sapphireJobCstActs, DataTable pervasiveJobCstActs, string company = "", int workflowRID = 0, string activity = "")
        {
            try
            {
                // Get list of activities from pervasive job data 
                var idsFromPervasiveTable = new HashSet<string>(pervasiveJobCstActs.AsEnumerable()
                    .OrderBy(o => o.Field<uint>("activity").ToString())
                    .Select(tb => tb.Field<uint>("activity").ToString()));

                // identify missing activity records
                var recordsFromSapphireListNotInPervasiveList = sapphireJobCstActs.AsEnumerable()
                    .Where(s => !idsFromPervasiveTable.Contains(s.Field<string>("activity")));

                // if there are any activities missing in pervasive, create them
                if (!(recordsFromSapphireListNotInPervasiveList is null) && recordsFromSapphireListNotInPervasiveList.AsEnumerable().Any())
                {
                    foreach (DataRow row in recordsFromSapphireListNotInPervasiveList.CopyToDataTable().Rows)
                    {
                        //_serviceLog.AppendLog($" Create missing activity {row["activity"]} for job {row["job_no"]}");
                        _jobCostActivityRepository.CreateJobCstActsRecord(row, company, workflowRID, activity);
                    }
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog($" Failure creating Missing Job Cost Activity records", "", ex);
            }
        }

        private void ResetEgmAmounts(string jobNumber, string company = "", int workflowRID = 0, string activity = "")
        {
            string sql = _jobCostActivityRepository.CreateResetEgmAmountSQL(jobNumber, company, workflowRID, activity);

            _serviceLog.AppendLog($" Updating EGM Amounts to 0 for job {jobNumber}");
            _jobCostActivityRepository.UpdateEgmAmountsToZero(sql);
            _serviceLog.AppendLog($" EGM Amounts updated to 0 for job {jobNumber}");
        }

        private void SetEgmValues(string jobNumber, DataTable sapphireJobCstActs, string company = "", int workflowRID = 0)
        {
            try
            {
                foreach (var jobCstActivity in sapphireJobCstActs.AsEnumerable())
                {
                    var activity = int.TryParse(jobCstActivity["activity"].ToString(), out int result) ? result : -1;
                    var sapphireEgmAmount = jobCstActivity["sap_egm_amount"] ?? 0;

                    _serviceLog.AppendLog($" Set EGM Amount for job {jobNumber} activity {activity} to {sapphireEgmAmount}");
                    _jobCostActivityRepository.SetEgmAmounts(jobNumber, activity, (decimal)sapphireEgmAmount, company, workflowRID);
                }
            }
            catch (Exception exception)
            {
                _serviceLog.AppendLog($" Failure setting EGM values for job {jobNumber}.  {exception.ToString()}", "", null);

            }
        }

        private void SetEgmValuesByActivity(string jobNumber, string inputActivity, decimal sapEgmAmount, string company = "", int workflowRID = 0)
        {
            try
            {
                _serviceLog.AppendLog($" Set EGM Amount for job {jobNumber} activity {inputActivity} to {sapEgmAmount.ToString("0.00")}");
                _jobCostActivityRepository.SetEgmAmounts(jobNumber, Convert.ToInt32(inputActivity), sapEgmAmount, company, workflowRID);
            }
            catch (Exception exception)
            {
                _serviceLog.AppendLog($" Failure setting EGM values for job {jobNumber}.  {exception.ToString()}", "", null);
            }
        }

        private DataTable GetCompanyByJobNumber(DataTable jobs)
        {
            jobs.Columns.Add("dbName");
            foreach (var job in jobs.AsEnumerable())
            {
                CompanyRepository companyRepository = new CompanyRepository();
                job["dbName"] = companyRepository.GetCompanyByJob(job["JobNumber"].ToString());
            }
            return jobs;
        }

        public JObject FormatAsJson(ReconciliationEgmTotals objectToFormat)
        {
            return JObject.FromObject(objectToFormat);
        }

    }
}
