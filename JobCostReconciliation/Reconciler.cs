using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using JobCostReconciliation.Interfaces;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Model;

namespace JobCostReconciliation
{
    public class Reconciler : IReconciler
    {
        public string dataObject = string.Empty;

        private readonly ISapphireRepository _sapphireRepository;
        private readonly IPervasiveRepository _pervasiveRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly IServiceLog _serviceLog;
        private readonly IConsoleLogger _consoleLogger;

        private readonly IImportService _importService;
        private readonly IExportService _exportService;

        private readonly IJobCostActivityService _jobCostActivityService;
        private readonly IVariancePurchaseOrderService _variancePurchaseOrderService;
        private readonly IJobService _jobService;

        public Reconciler(ISapphireRepository sapphireRepository, IPervasiveRepository pervasiveRepository, IQueueRepository queueRepository, IServiceLog serviceLog, IConsoleLogger consoleLogger, IImportService importService, IExportService exportService, IJobCostActivityService jobCostActivityService, IJobService jobService, IVariancePurchaseOrderService variancePurchaseOrderService)
        {
            _sapphireRepository = sapphireRepository;
            _pervasiveRepository = pervasiveRepository;
            _queueRepository = queueRepository;
            _serviceLog = serviceLog;
            _consoleLogger = consoleLogger;

            _importService = importService;
            _exportService = exportService;
            _jobCostActivityService = jobCostActivityService;
            _jobService = jobService;
            _variancePurchaseOrderService = variancePurchaseOrderService;
        }

        public void RunApp()
        {
            ListJobs();            
        }

        private void ListJobs()
        {
            Console.WriteLine("Please select the job you would like to run:");

            Console.WriteLine("1 - Purchase Orders: Reconcile JobPO Missing Records");
            Console.WriteLine("2 - Job Cost Activities: Reconcile JobCstActs (EGM) Records");
            Console.WriteLine("3 - VPOs: Reconcile JobVPO Record Dates");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                switch (result)
                {
                    case 1:
                        ReconcileMissingJobPORecords();
                        break;

                    case 2:
                        ReconcileMissingJobCostActivityRecords();
                        break;

                    case 3:
                        ReconcileJobVPORecords();
                        break;
                    
                    default:
                        _consoleLogger.Info("Please select a valid option.");
                        ListJobs();
                        break;
                }
            }
            else
            {
                _consoleLogger.Info("Please select a valid option.");
                ListJobs();
            }
        }

        private void ReconcileJobVPORecords()
        {
            Console.WriteLine("1 - Reconcile JobVPO Approval Dates");
            Console.WriteLine("2 - Reconcile JobVPO Approval/eSubmittal/Cancel Dates");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                switch (result)
                {
                    case 1:
                        // Check that pervasive data matches sapphire data based on the following logic:
                        //  If JobVPOs.Total < 0 and JobVPOs.Status is "Approved" or "Authorized", set po_header.approval_date = JobVPOs.DateAuthorized
                        _serviceLog.AppendLog("Reconcile Job VPO Approval Dates");
                        this.dataObject = "JobVPOs";
                        _variancePurchaseOrderService.ReconcileJobVPOApprovalDates();
                        break;

                    case 2:
                        _serviceLog.AppendLog("Reconcile Job VPO Approval/eSubmittal/Cancel Dates");
                        this.dataObject = "JobVPOs";
                        _variancePurchaseOrderService.ReconcileJobVPOApprovedAndCompletedDates();
                        break;

                    default:
                        _consoleLogger.Info("Please select a valid option.");
                        ReconcileJobVPORecords();
                        break;
                }
            }
            else
            {
                _consoleLogger.Info("Please select a valid option.");
                ReconcileJobVPORecords();
            }
        }

        private void ReconcileMissingJobPORecords()
        {
            _serviceLog.AppendLog("Reconciling JobPO records");
            this.dataObject = "JobPOs";
            GetSapphireRecordsNotFoundInPervasive("JobPOs", "JobPOReconcilerID");
        }

        private void ReconcileMissingJobCostActivityRecords()
        {
            Console.WriteLine("1 - Reconcile JobCstActs (EGM) Amounts By Job");
            Console.WriteLine("2 - Reconcile JobCstActs (EGM) Amounts By Job and Activity");
            Console.WriteLine("3 - Audit HomeEstimateToApproved Sapphire Workflow EGM Amounts");
            
            string jobNumber = String.Empty;

            if (int.TryParse(Console.ReadLine(), out int result2))
            {
                switch (result2)
                {
                   
                    case 1:
                        _serviceLog.AppendLogMessage("Reconciling JobCstActs (EGM) Missing Records and update EGM Amounts By Job");
                        this.dataObject = "JobCstActs";
                        ReconcileJobCstActRecordsAndEGMAmounts();
                        break;

                    case 2:
                        _serviceLog.AppendLogMessage("Validate JobCstActs (EGM) Amounts by Job and Activity");
                        this.dataObject = "JobCstActs";
                        ReconcileJobCstActByJobAndActivity();
                        break;

                    case 3:
                        _serviceLog.AppendLogMessage("Audit HomeEstimateToApproved Sapphire Workflow EGM Amounts");
                        this.dataObject = "JobCstActs";
                        AuditSapphireWorkflow(Model.Enums.Workflow.HomeEstimateStatusToApproved);
                        break;

                    default:
                        _consoleLogger.Info("Please select a valid option.");
                        ReconcileMissingJobCostActivityRecords();
                        break;
                }
            }
            else
            {
                _consoleLogger.Info("Please select a valid option.");
                ReconcileMissingJobCostActivityRecords();
            }
        }

        private void ReconcileJobCstActByJobAndActivity()
        {
            string jobNumber = RequestJobNumberInput();
            string activity = RequestActivityInput();
            string company = _pervasiveRepository.GetCompanyByJob(jobNumber);
            _jobCostActivityService.ReconcileJobCstActRecordsAndEGMAmountsByActivity(jobNumber, activity, company);

            _jobCostActivityService.ValidateEgmAmountsByJobNumber(jobNumber);

            // Loop through process again, allowing entry of new job number
            ReconcileMissingJobCostActivityRecords();
        }

        private void ReconcileJobCostActivityRecordsByJobNumber(string dataObject = "JobCstActs", string filterColumn = "JobCstReconcilerID")
        {
            try
            {
                string jobNumber = RequestJobNumberInput();
                GetSapphireRecordsNotFoundInPervasive(dataObject, filterColumn, jobNumber);
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
            }
        }

        private void AuditSapphireWorkflow(Model.Enums.Workflow workflowName)
        {
            Console.WriteLine("Enter number of days to go back.");

            string jobNumber = String.Empty;

            if (int.TryParse(Console.ReadLine(), out int result2))
            {
                switch (workflowName)
                {
                    case Model.Enums.Workflow.HomeEstimateStatusToApproved:
                        _jobCostActivityService.AuditSapphireWorkflow_HomeEstimateToApproved(result2);
                        break;

                    default:
                        break;
                }
            }

            // Loop through process again, allowing entry of new job number
            ReconcileMissingJobCostActivityRecords();
        }

        private void ReconcileJobCstActRecordsAndEGMAmounts()
        {
            string jobNumber = RequestJobNumberInput();

            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmountsByJobNumber(jobNumber);

            //_jobCostActivityService.WriteEgmTotals(egmTotals);

            if (!_jobCostActivityService.EgmTotalsMatch(egmTotals))
            {
                // Confirm update
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    string company = _pervasiveRepository.GetCompanyByJob(jobNumber);
                    _jobCostActivityService.ReconcileJobCstActRecordsAndEGMAmounts(jobNumber, company);

                    _jobCostActivityService.ValidateEgmAmountsByJobNumber(jobNumber);
                }
            }

            // Loop through process again, allowing entry of new job number
            ReconcileMissingJobCostActivityRecords();
        }

        private string RequestJobNumberInput()
        {
            try
            {
                Console.Write("\nEnter job number (ex: ABC01/000/0000 or ABC010000000): ");
                string input = Console.ReadLine().ToString();
                CleanJobNumberInput(input);

                string jobNumber = input.ToUpper().Replace("/", "");
                return jobNumber;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                throw ex;
            }
        }

        private void CleanJobNumberInput(string jobNumber)
        {
            // if input has slashes, remove them
            // job number should now be 12 characters
            if (jobNumber.Replace("/", "").Length != 12)
            {
                Console.WriteLine("Please ensure job number has been entered correctly.  Unable to validate job number.");
                RequestJobNumberInput();
            }
        }

        private string RequestActivityInput()
        {
            try
            {
                Console.Write("\nEnter activity (ex: 10100): ");
                string input = Console.ReadLine().ToString();
                CleanActivityInput(input);

                return input;
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.Message, "", ex);
                throw ex;
            }
        } 

        private void CleanActivityInput(string activity)
        {
            if (activity.Length != 5)
            {
                Console.WriteLine("Please ensure activity has been entered correctly.  Unable to validate activity.");
                RequestActivityInput();
            }

            if (!int.TryParse(activity, out int result))
            {
                Console.WriteLine("Please ensure activity has been entered correctly.  Unable to validate activity.");
                RequestActivityInput();
            } 
        }

        private void GetSapphireRecordsNotFoundInPervasive(string dataObject, string filterColumn, string jobNumber = "")
        {
            try
            {
                var sapphireRecords = _sapphireRepository.GetSapphireRecords(dataObject, jobNumber);
                var pervasiveRecords = _pervasiveRepository.GetPervasiveRecords(dataObject, jobNumber);

                // if no records returned from sapphire
                if (sapphireRecords is null || !(sapphireRecords.AsEnumerable().Any())) 
                {
                    Console.WriteLine("No sapphire records found for this job.  Please ensure you have entered the job number correctly and have configured the correct database connection.");
                    return;
                }

                if (!(pervasiveRecords is null) && pervasiveRecords.AsEnumerable().Any())
                {
                    // Get list of ids from pervasive data based on filter column passed in by object this task is currently attempting to reconcile
                    //    e.g. if SapphireObjID = JobPOs, then pervasive JobPOReconcilerID will be "JobPOID-JobID-ActID"
                    //         if SapphireObjID = JobCstActs, then pervasive JobCstReconcilerID will be "JobID-ActID"
                    var idsFromPervasiveTable = new HashSet<string>(pervasiveRecords.AsEnumerable()
                       .OrderBy(o => o.Field<string>(filterColumn))
                       .Select(tb => tb.Field<string>(filterColumn)));

                    var recordsFromSapphireListNotInPervasiveList = sapphireRecords.AsEnumerable()
                        .Where(ta => !idsFromPervasiveTable.Contains(ta.Field<string>(filterColumn)));

                    // No missing records identified
                    if (!recordsFromSapphireListNotInPervasiveList.Any() && !String.IsNullOrEmpty(jobNumber))
                    {
                        Console.WriteLine(String.Format("\nNo missing activities were found for job {0}.", _jobService.FormatJobNumber(jobNumber)));

                        // Loop through process again, allow entry of new job number for another run
                        if (!String.IsNullOrEmpty(jobNumber)) ReconcileJobCostActivityRecordsByJobNumber();
                    }

                    // If any missing records are identified
                    if (recordsFromSapphireListNotInPervasiveList.Any())
                    {
                        DataTable dataTable = recordsFromSapphireListNotInPervasiveList.CopyToDataTable();

                        // Prepare data for export 
                        //  1 - recordsFromSapphireListNotInPervasiveList (do not build SQL insert for JobPOs, do not export files for specific job number)
                        if (String.IsNullOrEmpty(jobNumber)) { _exportService.PrepareDataForExport(dataTable, dataObject); }

                        // If JobNumber has been set, continue JobCstActs insert procedure
                        if (!String.IsNullOrEmpty(jobNumber))
                        {
                            _jobCostActivityService.ConfirmJobCstActInsertByJobNumber(jobNumber, dataTable, dataObject);

                            // Loop through process again, allow entry of new job number for another run
                            if (!String.IsNullOrEmpty(jobNumber)) ReconcileJobCostActivityRecordsByJobNumber();
                        }

                        //  Note: When reconciling JobPOs, after receiving list back of Sapphire JobPOs which do not match  on "JobPOID-JobID-ActID" from pervasive,
                        //      remaining list must be checked against "JobID-ActID" from pervasive to identify matching JobCstActs records.  
                        //      if matching record is found, new po_no should be generated and existing JobCstAct record in PO_Header should be updated.
                        //       Any remaining items that do not match JobPOs or JobCstActs records in PO_Header will be INSERTs.
                        if (dataObject == "JobPOs" && dataTable.Rows.Count > 0)
                        {
                            // this list will become a list of PO_Header records to update with new po_no and JobPOs data from Sapphire
                            var idsFromPervasiveTableWherePoNoIs0 = new HashSet<string>(pervasiveRecords.AsEnumerable()
                                .OrderBy(o => o.Field<string>(filterColumn))
                                .Where(p => p.Field<UInt32>("po_no").Equals(0))
                                .Select(tb => tb.Field<string>(filterColumn)));

                            if (idsFromPervasiveTableWherePoNoIs0.Any())
                            {
                                var jobPORecordsMatchingPervasiveJobCstActRecords = recordsFromSapphireListNotInPervasiveList.AsEnumerable()
                                .Where(jp => idsFromPervasiveTableWherePoNoIs0.Contains(jp.Field<string>("JobCstReconcilerID")));

                                // 2 - jobPORecordsMatchingPervasiveJobCstActRecords (generate pervasive po_no and build SQL Update statement)
                                _exportService.PrepareDataForExport(jobPORecordsMatchingPervasiveJobCstActRecords.CopyToDataTable(), "jobPORecordsMatchingPervasiveJobCstActRecords", "UPDATE");

                                // this list will be a list of records to INSERT to PO_Header since no match was found on JobPORID-JobID-Activity or JobID-Activity combinations
                                var jobPORecordsNotFoundInPervasive = recordsFromSapphireListNotInPervasiveList.AsEnumerable()
                                    .Where(jp => !idsFromPervasiveTable.Contains(jp.Field<string>("JobCstReconcilerID")));

                                if (jobPORecordsNotFoundInPervasive.Any())
                                {
                                    // 3 - jobPORecordsNotFoundInPervasive (generate pervasive po_no and build SQL INSERT statement)
                                    _exportService.PrepareDataForExport(jobPORecordsNotFoundInPervasive.CopyToDataTable(), "jobPORecordsNotFoundInPervasive");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _serviceLog.AppendLog(ex.ToString());
            }
        }
    }
}