using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Services;
using JobCostReconciliation.Model;
using Newtonsoft.Json.Linq;

namespace JobCostReconciler.Controllers
{
    [Route("admin/api/[controller]")]
    public class JobCostController : Controller
    {

        [HttpGet("[action]/{jobNumber}")]
        public IEnumerable<JobCostSummary> JobTotals(string jobNumber)
        {
            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmounts(jobNumber);

            return Enumerable.Range(1, 1).Select(i => new JobCostSummary
            {
                id = i,
                jobNumber = egmTotals.JobNumber,
                estimateApprovalDate = egmTotals.EstimateApprovalDate,
                sapphireEgmTotal = Convert.ToDecimal(egmTotals.SapphireEgmTotal.ToString()).ToString("0.00"),
                pervasiveEgmTotal = Convert.ToDecimal(egmTotals.PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        [HttpGet("[action]/{homeRID}")]
        public IEnumerable<JobCostSummary> JobData(int homeRID)
        {
            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmountsByHomeRID(homeRID);

            return Enumerable.Range(1, 1).Select(i => new JobCostSummary
            {
                id = i,
                jobNumber = egmTotals.JobNumber,
                estimateApprovalDate = egmTotals.EstimateApprovalDate,
                sapphireEgmTotal = Convert.ToDecimal(egmTotals.SapphireEgmTotal.ToString()).ToString("0.00"),
                pervasiveEgmTotal = Convert.ToDecimal(egmTotals.PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<JobCostSummary> WorkflowJobCostSummary()
        {
            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            List<ReconciliationEgmTotals> egmTotals = _jobCostActivityService.GetWorkflowJobs();

            return Enumerable.Range(1, egmTotals.Count-1).Select(i => new JobCostSummary
            {
                id = i,
                jobNumber = egmTotals[i].JobNumber,
                sapphireEgmTotal = Convert.ToDecimal(egmTotals[i].SapphireEgmTotal.ToString()).ToString("0.00"),
                pervasiveEgmTotal = Convert.ToDecimal(egmTotals[i].PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        [HttpGet("sentry/events")]
        public async Task<String> SentryEvents()
        {
            SentryEventService sentryEventService = new SentryEventService();
            var events = await sentryEventService.GetEvents().ConfigureAwait(true);


            return events;
        }

        public class JobCostSummary
        {
            public int id { get; set; }
            public string jobNumber { get; set; }
            public DateTime estimateApprovalDate { get; set; }
            public string sapphireEgmTotal { get; set; }
            public string pervasiveEgmTotal { get; set; }
            public bool totalsMatch
            {
                get
                {
                    return sapphireEgmTotal.Equals(pervasiveEgmTotal);
                }
            }
        }
    }
}
