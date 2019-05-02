using System;
using System.Collections.Generic;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderLastRunService
    {
        DateTime GetLastRun();
        List<PurchaseOrderLastRun> List();
    }
}
