using System;

namespace JobCostReconciliation.Model
{
    public class Workflow
    {
        public int WFlowRID { get; set; }
        public int WFTempRID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RefObjType { get; set; }
        public int RefObjRID { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreationDate { get; set; }
        public int InitiatorRID { get; set; }
        public int RetryCount { get; set; }
    }
}
