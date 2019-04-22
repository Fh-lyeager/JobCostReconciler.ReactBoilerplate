using System.Linq;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Services
{
    public class PurchaseOrderQueueService : IPurchaseOrderQueueService
    {
        private readonly IPurchaseOrderQueueRepository _purchaseOrderQueueRepository;
        public PurchaseOrderQueueService()
        {
            _purchaseOrderQueueRepository = new PurchaseOrderQueueRepository();
        }

        public PurchaseOrderQueueService(IPurchaseOrderQueueRepository purchaseOrderQueueRepository)
        {
            _purchaseOrderQueueRepository = purchaseOrderQueueRepository;
        }

        public IQueryable<PurchaseOrderQueue> GetErroredItems()
        {
            return _purchaseOrderQueueRepository.GetErroredItems();
        }

        public int CountErroredItems()
        {
            return _purchaseOrderQueueRepository.CountErroredItems();
        }

        public int ItemsInQueue()
        {
            return _purchaseOrderQueueRepository.ItemsInQueue();
        }

    }
}
