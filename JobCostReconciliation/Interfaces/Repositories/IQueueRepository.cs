using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IQueueRepository
    {
        DataTable GetQueueRecords(string DataObject = "");
        void TruncateQueueTable(string DataObject = "");
        DataTable GetCompanies();
        void InsertAuditPOSyncRecord(string sql);
        string CreateInsertSQL(DataRow dataRow);
        DataTable GetAllRows();
    }
}
