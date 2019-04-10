using System;

namespace JobCostReconciliation.Model
{
    public class Reconciliation
    {
        public string DatabaseName { get; set; }
        public int JobCstActRID { get; set; }
        public string JobNumber { get; set; }
        public string Activity { get; set; }
        public decimal SEgmAmount { get; set; }
        public double PEgmAmount { get; set; }
        public int POHeaderRecordID { get; set; }
        public uint po_no { get; set; }
    }

    public class ReconciliationEgmTotals
    {
        public int HomeRID { get; set; }
        public string JobNumber { get; set; }
        public string DatabaseName { get; set; }
        public decimal SapphireEgmTotal { get; set; }
        public decimal PervasiveEgmTotal { get; set; }
        public DateTime EstimateApprovalDate { get; set; }
    }
}
