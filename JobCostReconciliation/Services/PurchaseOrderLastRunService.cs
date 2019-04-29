using System;
using System.Collections.Generic;
using System.Linq;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Data.Repositories;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Services
{
    public class PurchaseOrderLastRunService : IPurchaseOrderLastRunService
    {
        private readonly IPurchaseOrderLastRunRepository _purchaseOrderLastRunRepository;

        public PurchaseOrderLastRunService()
        {
            _purchaseOrderLastRunRepository = new PurchaseOrderLastRunRepository();
        }

        public PurchaseOrderLastRunService(IPurchaseOrderLastRunRepository purchaseOrderLastRunRepository)
        {
            _purchaseOrderLastRunRepository = purchaseOrderLastRunRepository;
        }

        public DateTime GetLastRun()
        {
            return _purchaseOrderLastRunRepository.GetLastRuns().AsEnumerable()
                .OrderByDescending(o => o.NextRunId)
                .Select(d => d.RunComplete)
                .First();
        }

        public List<PurchaseOrderLastRun> List()
        {
            return _purchaseOrderLastRunRepository.GetLastRuns().ToList();
        }
    }
}
