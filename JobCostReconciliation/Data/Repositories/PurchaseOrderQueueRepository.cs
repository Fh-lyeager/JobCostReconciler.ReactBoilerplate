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
        public IQueryable<PurchaseOrderQueue> GetErroredItems()
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderQueueItems
                    .Where(q => q.Status == QueueItemStatusType.Error);
            }
        }

        public int CountErroredItems()
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderQueueItems
                    .Where(q => q.Status == QueueItemStatusType.Error)
                    .Count();
            }
        }

        public IList<PurchaseOrderQueue> ListNew(int numberToLoad)
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderQueueItems
                    .OrderBy(o => o.Id)
                    .Where(q => q.Status == QueueItemStatusType.New)
                    //.Take(numberToLoad)
                    .ToList();
            }
        }

        public int ItemsInQueue()
        {
            using (var _db = new ProcessorDbContext())
            {
                return _db.PurchaseOrderQueueItems.Count();
            }
        }
    }
}
