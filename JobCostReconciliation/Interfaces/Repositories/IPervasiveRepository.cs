using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IPervasiveRepository
    {
        DataTable GetPervasiveRecords(string dataObject = "", string jobNumber = "");
        string GetCompanyByJob(string jobNumber);
    }
}
