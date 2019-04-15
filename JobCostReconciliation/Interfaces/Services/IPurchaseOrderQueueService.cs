using System.Linq;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderQueueService
    {
        IQueryable<PurchaseOrderQueue> GetErroredItems();
        int CountErroredItems();
        int ItemsInQueue();
    }
}
