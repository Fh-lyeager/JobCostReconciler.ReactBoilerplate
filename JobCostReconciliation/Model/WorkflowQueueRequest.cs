using System;
using Newtonsoft.Json.Linq;
using JobCostReconciliation.Model.Enums;

namespace JobCostReconciliation.Model
{
    public class WorkflowQueueRequest 
    {
        public int Id { get; set; }
        public string RequestBody { get; set; }
        public DateTime UtcDateTime { get; set; }
        public string Response { get; set; }
        public QueueItemStatusType Status { get; set; }
        public int WFlowRID { get; set; }
        public int RefObjRID { get; set; }
        public string JobNumber { get; set; }
        public DateTime HomeEstimateApprovalDate { get; set; }
        
    }
}
