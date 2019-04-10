using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPurchaseOrderRepository
    {
        string CreateJobPOSQLInsert(DataRow row, string company = "");
        string CreateJobPOSQLUpdate(DataRow row, string company = "");
        string CreatePOProcessingDatesSQL(DataRow row, string company = "");
    }
}
