using System.Collections.Generic;
using System.Data;
using JobCostReconciliation.Model;
using Newtonsoft.Json.Linq;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IJobCostActivityService
    {
        void ReconcileJobCstActRecordsAndEGMAmounts(string jobNumber, string company = "");
        void ReconcileJobCstActRecordsAndEGMAmountsByActivity(string jobNumber, string activity, string company = "");
        void ValidateEgmAmountsByJobNumber(string jobNumber);
        ReconciliationEgmTotals GetEgmAmounts(string jobNumber);
        List<ReconciliationEgmTotals> GetWorkflowJobs();
        bool EgmTotalsMatch(ReconciliationEgmTotals jobEgmTotals);
        void AuditSapphireWorkflow_HomeEstimateToApproved(int timespanDays);

        JObject FormatAsJson(ReconciliationEgmTotals objectToFormat);
        ReconciliationEgmTotals GetEgmAmountsByHomeRID(int homeRID);
    }
}
