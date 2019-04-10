using System.Data;

namespace JobCostReconciliation.Interfaces.Clients
{
    public interface ISqlClient
    {
        DataTable QueryADO(string queryString, string connectionString);
        void NonQueryADO(string queryString, string connectionString);
    }
}
