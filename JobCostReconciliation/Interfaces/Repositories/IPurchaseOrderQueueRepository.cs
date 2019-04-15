using JobCostReconciliation.Model;
using System.Collections.Generic;
using System.Linq;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderQueueRepository
    {
        IQueryable<PurchaseOrderQueue> GetErroredItems();
        int CountErroredItems();
        IList<PurchaseOrderQueue> ListNew(int numberToLoad);
        int ItemsInQueue();
    }
}
