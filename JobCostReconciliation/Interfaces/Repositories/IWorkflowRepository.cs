using System.Collections.Generic;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IWorkflowRepository
    {
        List<Workflow> GetSapphireWorkflow(int homeRID = 0);
    }
}
