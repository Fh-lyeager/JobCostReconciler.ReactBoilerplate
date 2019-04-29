using System;
using System.Collections.Generic;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderLastRunRepository
    {
        IList<PurchaseOrderLastRun> GetLastRuns();

    }
}
