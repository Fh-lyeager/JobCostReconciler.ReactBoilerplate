using System.Collections.Generic;
using System.Data;

namespace JobCostReconciliation.Interfaces.Repositories
{
    public interface IJobCostActivityRepository
    {
        string CreateJobCostSQLInsert(DataRow row, string company = "", string activity = "");
        string CreateJobCostEGMAmountsUpdateSQL(DataRow row, string company = "");

        void CreateJobCstActsRecord(DataRow row, string company = "", int workflowRID = 0, string activity = "");
        void InsertJobCstActsRecord(string query, string jobNumber, string activity, int workflowRID = 0);

        string CreateResetEgmAmountSQL(string jobNumber, string company = "", int workflowRID = 0, string activity = "");

        void UpdateEgmAmountsToZero(string sql, int workflowRID = 0);
        void SetEgmAmounts(string jobNumber, int activity, decimal sapEgmAmount, string company = "", int workflowRID = 0);
        void RecalculateEgmValues(string jobNumber, string company = "", int workflowRID = 0);

        DataTable GetPervasiveJobCstActs(string jobNumber, string company = "");
        DataTable GetEgmJobTotals(List<int> homeRIds);
    }
}
