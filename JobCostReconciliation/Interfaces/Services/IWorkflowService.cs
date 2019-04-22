using System.Collections.Generic;
using System.Data;
using JobCostReconciliation.Model;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IWorkflowService
    {
        Workflow GetWorkflow(string jobNumber);
        List<Workflow> ListWorkflows();
    }
}