using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderHeaderRepository
    {
        DataTable GetPervasiveRecords(string dataObject = "", string jobNumber = "");
    }
}