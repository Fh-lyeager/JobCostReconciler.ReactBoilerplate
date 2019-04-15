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
    public class JobController : Controller
    {
        private string[] JobSummary = new[] 
        {
            "ABC011230000",
            "123456.12",
            "123456.12"
        };

        public JobController()
        {
        }

        [HttpGet("[action]/{jobNumber}")]
        //[HttpGet("[action]")]
        public IEnumerable<Job> JobTotals(string jobNumber)
        {
            var job = jobNumber;

            //job = "APT015810000";

            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmountsByJobNumber(job);

            return Enumerable.Range(1,1).Select(i => new Job
            {
                JobNumber = egmTotals.JobNumber,
                EstimateApprovalDate = egmTotals.EstimateApprovalDate,
                SapphireEgmTotal = Convert.ToDecimal(egmTotals.SapphireEgmTotal.ToString()).ToString("0.00"),
                PervasiveEgmTotal = Convert.ToDecimal(egmTotals.PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<Job> WorkflowJobsEgmTotals()
        {
            JobCostActivityService _jobCostActivityService = new JobCostActivityService();
            List<ReconciliationEgmTotals> egmTotals = _jobCostActivityService.GetWorkflowJobs();

            return Enumerable.Range(1, egmTotals.Count-1).Select(i => new Job
            {
                JobNumber = egmTotals[i].JobNumber,
                SapphireEgmTotal = Convert.ToDecimal(egmTotals[i].SapphireEgmTotal.ToString()).ToString("0.00"),
                PervasiveEgmTotal = Convert.ToDecimal(egmTotals[i].PervasiveEgmTotal.ToString()).ToString("0.00")
            });
        }

        public class Job
        {
            public string JobNumber { get; set; }
            public DateTime EstimateApprovalDate { get; set; }
            public string SapphireEgmTotal { get; set; }
            public string PervasiveEgmTotal { get; set; }
            public bool TotalsMatch
            {
                get
                {
                    return SapphireEgmTotal.Equals(PervasiveEgmTotal);
                }
            }
        }
    }
}
