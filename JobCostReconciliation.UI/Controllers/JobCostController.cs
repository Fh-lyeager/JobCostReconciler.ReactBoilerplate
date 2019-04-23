using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<Job> JobTotals(string jobNumber)
        {
            var job = jobNumber;

            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmountsByJobNumber(job);

            return Enumerable.Range(1, 1).Select(i => new Job
            {
                id = i,
                jobNumber = egmTotals.JobNumber,
                estimateApprovalDate = egmTotals.EstimateApprovalDate,
                sapphireEgmTotal = Convert.ToDecimal(egmTotals.SapphireEgmTotal.ToString()).ToString("0.00"),
                pervasiveEgmTotal = Convert.ToDecimal(egmTotals.PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<Job> WorkflowJobsEgmTotals()
        {
            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            List<ReconciliationEgmTotals> egmTotals = _jobCostActivityService.GetWorkflowJobs();

            return Enumerable.Range(1, egmTotals.Count-1).Select(i => new Job
            {
                id = i,
                jobNumber = egmTotals[i].JobNumber,
                sapphireEgmTotal = Convert.ToDecimal(egmTotals[i].SapphireEgmTotal.ToString()).ToString("0.00"),
                pervasiveEgmTotal = Convert.ToDecimal(egmTotals[i].PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        public class Job
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
