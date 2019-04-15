using JobCostReconciliation.Data.Contexts;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Model.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JobCostReconciliation.Data.Repositories
{
    public class PurchaseOrderLastRunRepository : IPurchaseOrderLastRunRepository
    {
        public DateTime GetLastRunTime()
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderLastRuns
                    .OrderByDescending(o => o.RunComplete)
                    .Where(run => run.Status == QueueItemStatusType.Processed)
                    .Select(run => run.RunComplete)
                    .FirstOrDefault();
            }
        }
    }
}
