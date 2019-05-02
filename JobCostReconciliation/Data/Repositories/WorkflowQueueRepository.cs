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
    public class WorkflowQueueRepository : IWorkflowQueueRepository
    {
        public WorkflowQueueRepository()
        {

        }

        public List<WorkflowQueue> List()
        {
            using (var _db = new SwapiDbContext())
            {
                return _db.WorkflowQueueItems
                    .OrderBy(o => o.Id)
                    .ToList();
            }
        }

        public List<WorkflowQueue> List(QueueItemStatusType queueItemStatusType)
        {
            using (var _db = new SwapiDbContext())
            {
                return _db.WorkflowQueueItems
                    .OrderBy(o => o.Id)
                    .Where(q => q.Status == queueItemStatusType)
                    .ToList();
            }
        }

    }
}

