using System;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderLastRunRepository
    {
        DateTime GetLastRunTime();

    }
}
