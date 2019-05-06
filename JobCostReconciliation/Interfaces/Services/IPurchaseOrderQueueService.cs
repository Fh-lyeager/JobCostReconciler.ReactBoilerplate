using System.Collections.Generic;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderQueueService
    {
        List<PurchaseOrderQueue> GetErroredItems();
        List<PurchaseOrderQueue> GetNewItems();
    }
}
