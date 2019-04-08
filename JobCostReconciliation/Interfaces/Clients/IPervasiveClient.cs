using System.Collections.Generic;
using System.Data;

namespace JobCostReconciliation.Interfaces.Clients
{
    public interface IPervasiveClient
    {
        DataTable QueryPervasiveADO(string queryString);
        void NonQueryPervasiveADO(string queryString);
        void StoredProcADO(string storedProc, Dictionary<string, string> parameters);
        string ScalarStoredProcADO(string storedProc, Dictionary<string, string> parameters);
        string ScalarQueryStringADO(string queryString);
    }
}
