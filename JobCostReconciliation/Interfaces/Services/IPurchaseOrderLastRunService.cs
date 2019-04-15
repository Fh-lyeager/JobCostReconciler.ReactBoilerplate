using System;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderLastRunService
    {
        DateTime GetLastRun();
    }
}
