using System.Linq;
using System.Collections.Generic;
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

        public List<PurchaseOrderQueue> GetErroredItems()
        {
            return _purchaseOrderQueueRepository.List(Model.Enums.QueueItemStatusType.Error).ToList();
        }

        public List<PurchaseOrderQueue> GetNewItems()
        {
            return _purchaseOrderQueueRepository.List(Model.Enums.QueueItemStatusType.New).ToList();
        }
    }
}
