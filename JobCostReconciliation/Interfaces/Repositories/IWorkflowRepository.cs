using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IWorkflowRepository
    {
        DataTable GetSapphireWorkflow(string jobNumber = "");
    }
}
