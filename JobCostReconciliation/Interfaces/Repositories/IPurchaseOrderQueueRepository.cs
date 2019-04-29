using JobCostReconciliation.Model;
using JobCostReconciliation.Model.Enums;
using System.Collections.Generic;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderQueueRepository
    {
        IList<PurchaseOrderQueue> List(QueueItemStatusType queueItemStatusType);
    }
}
