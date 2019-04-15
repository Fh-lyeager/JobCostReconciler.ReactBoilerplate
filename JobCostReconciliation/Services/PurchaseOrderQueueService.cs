using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Services
{
    public class PurchaseOrderQueueService : IPurchaseOrderQueueService
    {
        public PurchaseOrderQueueService()
        {

        }

        public IQueryable<PurchaseOrderQueue> GetErroredItems()
        {
            PurchaseOrderQueueRepository purchaseOrderQueueRepository = new PurchaseOrderQueueRepository();
            return purchaseOrderQueueRepository.GetErroredItems();
        }

        public int CountErroredItems()
        {
            PurchaseOrderQueueRepository purchaseOrderQueueRepository = new PurchaseOrderQueueRepository();
            return purchaseOrderQueueRepository.CountErroredItems();
        }

        public int ItemsInQueue()
        {
            PurchaseOrderQueueRepository purchaseOrderQueueRepository = new PurchaseOrderQueueRepository();
            return purchaseOrderQueueRepository.ItemsInQueue();
        }

    }
}
