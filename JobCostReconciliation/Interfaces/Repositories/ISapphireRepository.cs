using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface ISapphireRepository
    {
        DataTable GetSapphireRecords(string dataObject, string jobNumber = "");
    }
}
