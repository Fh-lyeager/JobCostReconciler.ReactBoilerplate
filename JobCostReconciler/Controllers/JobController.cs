using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("[action]")]
        public IEnumerable<Job> JobTotals()
        {
            return Enumerable.Range(1,1).Select(i => new Job
            {
                JobNumber = JobSummary[0],
                SapphireEgmTotal = Convert.ToDecimal(JobSummary[1].ToString()),
                PervasiveEgmTotal = Convert.ToDecimal(JobSummary[2].ToString()),
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
