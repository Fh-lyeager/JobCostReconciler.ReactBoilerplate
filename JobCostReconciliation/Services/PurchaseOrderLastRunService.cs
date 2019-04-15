using System;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;

namespace JobCostReconciliation.Services
{
    public class PurchaseOrderLastRunService : IPurchaseOrderLastRunService
    {
        private readonly IPurchaseOrderLastRunRepository _purchaseOrderLastRunRepository;

        public PurchaseOrderLastRunService()
        {

        }

        public PurchaseOrderLastRunService(IPurchaseOrderLastRunRepository purchaseOrderLastRunRepository)
        {
            _purchaseOrderLastRunRepository = purchaseOrderLastRunRepository;
        }

        public DateTime GetLastRun()
        {
            PurchaseOrderLastRunRepository purchaseOrderLastRunRepository = new PurchaseOrderLastRunRepository();
            return purchaseOrderLastRunRepository.GetLastRunTime();
        }
    }
}
