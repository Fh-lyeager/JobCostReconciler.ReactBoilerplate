using System.Collections.Generic;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderQueueService
    {
        IList<PurchaseOrderQueue> GetErroredItems();
        IList<PurchaseOrderQueue> GetNewItems();
    }
}
