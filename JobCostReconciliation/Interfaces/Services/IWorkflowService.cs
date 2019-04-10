using System.Collections.Generic;
using System.Data;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IWorkflowService
    {
        Workflow GetSapphireWorkflow(string jobNumber);
        List<Workflow> ListSapphireWorkflows();
    }
}