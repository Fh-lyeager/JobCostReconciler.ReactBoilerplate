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
    [Route("api/[controller]")]
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
        public IEnumerable<Job> JobTotals(string jobNumber)
        {
            var job = jobNumber;

            //var jobNumber = "ABN010050000";
            job = "APT015810000";
            //var jobNumber = "";

            JobCostActivityService _jobCostActivityService = new JobCostActivityService();

            ReconciliationEgmTotals egmTotals = _jobCostActivityService.GetEgmAmountsByJobNumber(job);

            return Enumerable.Range(1,1).Select(i => new Job
            {
                JobNumber = egmTotals.JobNumber,
                SapphireEgmTotal = Convert.ToDecimal(egmTotals.SapphireEgmTotal.ToString()),
                PervasiveEgmTotal = Convert.ToDecimal(egmTotals.PervasiveEgmTotal.ToString()),
            });
            
        }

        public class Job
        {
            public string JobNumber { get; set; }
            public decimal SapphireEgmTotal { get; set; }
            public decimal PervasiveEgmTotal { get; set; }

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
