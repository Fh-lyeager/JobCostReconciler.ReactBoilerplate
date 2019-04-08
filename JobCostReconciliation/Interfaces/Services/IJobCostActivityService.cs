using System.Data;
using JobCostReconciliation.Model;
using Newtonsoft.Json.Linq;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IJobCostActivityService
    {
        void ConfirmJobCstActInsertByJobNumber(string jobNumber, DataTable recordsToInsert, string dataObject);
        void ReconcileJobCstActRecordsAndEGMAmounts(string jobNumber, string company = "");
        void ReconcileJobCstActRecordsAndEGMAmountsByActivity(string jobNumber, string activity, string company = "");
        void ValidateEgmAmountsByJobNumber(string jobNumber);
        ReconciliationEgmTotals GetEgmAmountsByJobNumber(string jobNumber);
        //void WriteEgmTotals(ReconciliationEgmTotals jobEgmTotals);
        bool EgmTotalsMatch(ReconciliationEgmTotals jobEgmTotals);
        void AuditSapphireWorkflow_HomeEstimateToApproved(int timespanDays);

        JObject FormatAsJson(ReconciliationEgmTotals objectToFormat);
    }
}
