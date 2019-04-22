using System.Linq;
using JobCostReconciliation.Data.Contexts;
using JobCostReconciliation.Interfaces.Repositories;

namespace JobCostReconciliation.Data.Repositories
{
    public class JobRepository : IJobRepository
    {
        public int GetHomeRIDByJobNumber(string jobNumber)
        {
            using (var _db = new SapphireDbContext())
            {
                return _db.Jobs
                    .Where(j => j.JobID.Substring(0, 12).Equals(jobNumber))
                    .Select(h => h.HomeRID)
                    .FirstOrDefault();
            }
        }
    }
}
