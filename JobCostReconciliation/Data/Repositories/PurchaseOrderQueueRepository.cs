using JobCostReconciliation.Data.Contexts;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace JobCostReconciliation.Data.Repositories
{
    public class PurchaseOrderQueueRepository : IPurchaseOrderQueueRepository
    {
        public IList<PurchaseOrderQueue> List(QueueItemStatusType queueItemStatusType)
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderQueueItems
                    .OrderBy(o => o.Id)
                    .Where(q => q.Status == queueItemStatusType)
                    .ToList();
            }
        }
    }
}
